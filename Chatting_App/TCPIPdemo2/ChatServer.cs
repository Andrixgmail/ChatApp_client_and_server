using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using ChatShared;

namespace TCPIPdemo2;

// This is the actual chat server. I keep it completely separate from the window so the networking
// code never touches any UI controls. The form only listens to the two events below.
public sealed class ChatServer
{
    // Every connected and named user lives in here, keyed by a unique id I give them.
    private readonly ConcurrentDictionary<Guid, ConnectedUser> _users = new();

    // I lock around name checks so two people joining at the same moment cannot end up with the same name.
    private readonly Lock _nameLock = new();

    private TcpListener? _listener;
    private CancellationTokenSource? _cts;
    private Task? _acceptLoop;
    private volatile bool _stopping;

    // I raise this with a line of text whenever something worth showing in the activity log happens.
    public event Action<string>? Log;

    // I raise this with the latest list of names whenever someone joins or leaves.
    public event Action<IReadOnlyList<string>>? UsersChanged;

    public bool IsRunning => _listener is not null;

    // Here I open the listening socket and kick off the loop that accepts new clients.
    // If the port is already taken, the SocketException bubbles up so the form can tell the user.
    public void Start(IPAddress address, int port)
    {
        if (IsRunning)
        {
            throw new InvalidOperationException("The server is already running.");
        }

        var listener = new TcpListener(address, port);
        listener.Start();

        _listener = listener;
        _cts = new CancellationTokenSource();
        _stopping = false;
        _acceptLoop = AcceptLoopAsync(listener, _cts.Token);

        Log?.Invoke($"Listening on {address}:{port}");
    }

    // Here I shut everything down in a friendly order: warn the users, stop accepting, then drop every connection.
    public async Task StopAsync()
    {
        if (_listener is null || _cts is null)
        {
            return;
        }

        _stopping = true;
        await BroadcastAsync(ChatPacket.Notice("The server is shutting down.")).ConfigureAwait(false);

        _cts.Cancel();
        _listener.Stop();

        foreach (ConnectedUser user in _users.Values)
        {
            user.Connection.Dispose();
        }
        _users.Clear();

        try
        {
            if (_acceptLoop is not null)
            {
                await _acceptLoop.ConfigureAwait(false);
            }
        }
        catch (Exception)
        {
            // The accept loop only ends because I cancelled it, so whatever it threw on the way out does not matter.
        }

        _cts.Dispose();
        _cts = null;
        _listener = null;
        _acceptLoop = null;

        Log?.Invoke("Server stopped.");
        UsersChanged?.Invoke([]);
    }

    // This loop just keeps accepting sockets. Each new client gets its own task so one slow user never blocks the rest.
    private async Task AcceptLoopAsync(TcpListener listener, CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            TcpClient tcp;
            try
            {
                tcp = await listener.AcceptTcpClientAsync(cancellationToken).ConfigureAwait(false);
            }
            catch (Exception ex) when (ex is OperationCanceledException or ObjectDisposedException or SocketException)
            {
                break;
            }

            _ = HandleClientAsync(tcp, cancellationToken);
        }
    }

    // This is the whole life of one client: wait for the Join packet, register the name, then relay messages until they leave.
    private async Task HandleClientAsync(TcpClient tcp, CancellationToken cancellationToken)
    {
        var connection = new ChatConnection(tcp);
        ConnectedUser? user = null;

        try
        {
            // I give a new connection ten seconds to introduce itself, otherwise I drop it.
            using var joinTimeout = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            joinTimeout.CancelAfter(TimeSpan.FromSeconds(10));

            ChatPacket? hello = await connection.ReceiveAsync(joinTimeout.Token).ConfigureAwait(false);
            if (hello is not { Type: PacketType.Join })
            {
                Log?.Invoke($"Dropped {connection.RemoteEndPoint} because it never sent a join request.");
                return;
            }

            user = Register(hello.Sender, connection);
            Log?.Invoke($"{user.Name} joined from {connection.RemoteEndPoint}");

            await connection.SendAsync(ChatPacket.Welcome(user.Name), cancellationToken).ConfigureAwait(false);
            await BroadcastAsync(ChatPacket.Join(user.Name)).ConfigureAwait(false);
            await PublishUserListAsync().ConfigureAwait(false);

            while (await connection.ReceiveAsync(cancellationToken).ConfigureAwait(false) is { } packet)
            {
                // I only relay chat messages. I always stamp them with the name I assigned, so nobody can pretend to be someone else.
                if (packet.Type != PacketType.Message)
                {
                    continue;
                }

                string text = ChatPacket.CleanText(packet.Text);
                if (text.Length == 0)
                {
                    continue;
                }

                // I flatten line breaks for the log only, so one message stays on one line in the activity view.
                Log?.Invoke($"{user.Name}: {text.ReplaceLineEndings(" ")}");
                await BroadcastAsync(ChatPacket.Message(user.Name, text)).ConfigureAwait(false);
            }
        }
        catch (Exception ex) when (ex is OperationCanceledException or IOException or SocketException or ObjectDisposedException)
        {
            // These all just mean the connection went away, which I handle the same way as a clean disconnect below.
        }
        finally
        {
            connection.Dispose();

            if (user is not null && _users.TryRemove(user.Id, out _) && !_stopping)
            {
                Log?.Invoke($"{user.Name} left.");
                await BroadcastAsync(ChatPacket.Leave(user.Name)).ConfigureAwait(false);
                await PublishUserListAsync().ConfigureAwait(false);
            }
        }
    }

    // Here I pick a final display name. If the name is taken I add a number, so two "Andrea"s become "Andrea" and "Andrea 2".
    private ConnectedUser Register(string requestedName, ChatConnection connection)
    {
        string baseName = ChatPacket.CleanName(requestedName);
        if (baseName.Length == 0)
        {
            baseName = "Guest";
        }

        lock (_nameLock)
        {
            var taken = _users.Values.Select(u => u.Name).ToHashSet(StringComparer.OrdinalIgnoreCase);
            string name = baseName;
            for (int i = 2; taken.Contains(name); i++)
            {
                name = $"{baseName} {i}";
            }

            var user = new ConnectedUser(Guid.NewGuid(), name, connection);
            _users[user.Id] = user;
            return user;
        }
    }

    private async Task PublishUserListAsync()
    {
        List<string> names = _users.Values.Select(u => u.Name).Order(StringComparer.OrdinalIgnoreCase).ToList();
        UsersChanged?.Invoke(names);
        await BroadcastAsync(ChatPacket.UserList(names)).ConfigureAwait(false);
    }

    // Here I send one packet to everybody at the same time. If one user has a dead connection I simply skip them,
    // because their own read loop will notice and clean them up.
    private Task BroadcastAsync(ChatPacket packet)
    {
        IEnumerable<Task> sends = _users.Values.Select(async user =>
        {
            try
            {
                using var timeout = new CancellationTokenSource(TimeSpan.FromSeconds(5));
                await user.Connection.SendAsync(packet, timeout.Token).ConfigureAwait(false);
            }
            catch (Exception)
            {
                // A failed send to one user should never stop the message reaching everyone else.
            }
        });

        return Task.WhenAll(sends);
    }

    private sealed record ConnectedUser(Guid Id, string Name, ChatConnection Connection);
}

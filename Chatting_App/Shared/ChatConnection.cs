using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ChatShared;

// This class wraps one TCP connection and lets me send and receive whole ChatPacket objects
// instead of worrying about raw bytes. Both the client and the server use it.
public sealed class ChatConnection : IDisposable, IAsyncDisposable
{
    // I refuse lines longer than this so a broken or hostile peer cannot make me hold a huge string.
    public const int MaxLineLength = 16 * 1024;

    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web)
    {
        Converters = { new JsonStringEnumConverter() }
    };

    private readonly TcpClient _client;
    private readonly StreamReader _reader;
    private readonly StreamWriter _writer;

    // Several tasks can try to send at the same moment (for example two broadcasts), so I only let one write at a time.
    private readonly SemaphoreSlim _writeLock = new(1, 1);
    private int _disposed;

    public ChatConnection(TcpClient client)
    {
        _client = client;

        // I turn off Nagle so short chat messages go out right away instead of waiting to be batched.
        _client.NoDelay = true;

        NetworkStream stream = client.GetStream();
        var utf8 = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false);
        _reader = new StreamReader(stream, utf8, detectEncodingFromByteOrderMarks: false, bufferSize: 4096, leaveOpen: true);
        _writer = new StreamWriter(stream, utf8, bufferSize: 4096, leaveOpen: true) { NewLine = "\n" };

        RemoteEndPoint = client.Client.RemoteEndPoint?.ToString() ?? "unknown";
    }

    // I keep the remote address around so I can show it in the server log even after the socket closes.
    public string RemoteEndPoint { get; }

    // Here I turn the packet into one line of JSON, write it and flush it straight away.
    public async Task SendAsync(ChatPacket packet, CancellationToken cancellationToken = default)
    {
        ObjectDisposedException.ThrowIf(Volatile.Read(ref _disposed) == 1, this);

        string json = JsonSerializer.Serialize(packet, JsonOptions);

        await _writeLock.WaitAsync(cancellationToken).ConfigureAwait(false);
        try
        {
            await _writer.WriteLineAsync(json.AsMemory(), cancellationToken).ConfigureAwait(false);
            await _writer.FlushAsync(cancellationToken).ConfigureAwait(false);
        }
        finally
        {
            _writeLock.Release();
        }
    }

    // Here I wait for the next valid packet. I return null when the other side closes the connection cleanly.
    // Blank lines, oversized lines and anything that is not valid JSON get skipped instead of crashing the app.
    public async Task<ChatPacket?> ReceiveAsync(CancellationToken cancellationToken = default)
    {
        while (true)
        {
            string? line = await _reader.ReadLineAsync(cancellationToken).ConfigureAwait(false);
            if (line is null)
            {
                return null;
            }

            if (line.Length == 0 || line.Length > MaxLineLength)
            {
                continue;
            }

            try
            {
                ChatPacket? packet = JsonSerializer.Deserialize<ChatPacket>(line, JsonOptions);
                if (packet is not null)
                {
                    return packet;
                }
            }
            catch (JsonException)
            {
                // Somebody sent me something that is not a ChatPacket, so I just ignore that line.
            }
        }
    }

    // I make disposing safe to call more than once because the client, the server and the read loop might all try to do it.
    public void Dispose()
    {
        if (Interlocked.Exchange(ref _disposed, 1) == 1)
        {
            return;
        }

        // Closing the socket first wakes up any pending read so the read loop can finish.
        try { _client.Client.Shutdown(SocketShutdown.Both); } catch (SocketException) { } catch (ObjectDisposedException) { }
        _client.Dispose();

        // The writer may try to flush into a dead socket, which is fine to ignore at this point.
        try { _writer.Dispose(); } catch (IOException) { } catch (ObjectDisposedException) { }
        _reader.Dispose();
        _writeLock.Dispose();
    }

    public ValueTask DisposeAsync()
    {
        Dispose();
        return ValueTask.CompletedTask;
    }
}

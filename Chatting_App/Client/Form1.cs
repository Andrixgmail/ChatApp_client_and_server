using System.Net.Sockets;
using ChatShared;

namespace Client;

// This is the chat client window. I connect with a name, then everything the server sends me
// shows up in the chat area, and anything I type gets sent to everyone else.
public partial class Form1 : Form
{
    // A few friendly colours I hand out to other people's names, so a busy chat is easier to follow.
    private static readonly Color[] NameColours =
    [
        Color.FromArgb(240, 178, 92),
        Color.FromArgb(214, 128, 214),
        Color.FromArgb(120, 200, 120),
        Color.FromArgb(240, 120, 120),
        Color.FromArgb(90, 200, 220),
        Color.FromArgb(200, 180, 255)
    ];

    private ChatConnection? _connection;
    private CancellationTokenSource? _sessionCts;
    private string _myName = string.Empty;

    // I create the bold and italic fonts once and reuse them, instead of making a new Font for every single line of chat.
    private readonly Font _boldFont;
    private readonly Font _italicFont;

    public Form1()
    {
        InitializeComponent();
        _boldFont = new Font(rtbChat.Font, FontStyle.Bold);
        _italicFont = new Font(rtbChat.Font, FontStyle.Italic);
        Disposed += (_, _) =>
        {
            _boldFont.Dispose();
            _italicFont.Dispose();
        };
    }

    private void Form1_Load(object sender, EventArgs e)
    {
        ClientSettings settings = ClientSettings.Load();
        txtName.Text = settings.UserName;
        txtHost.Text = settings.Host;
        txtPort.Text = settings.Port.ToString();

        txtName.MaxLength = ChatPacket.MaxNameLength;
        txtMessage.MaxLength = ChatPacket.MaxMessageLength;

        SetConnectedState(false);
        AppendNotice("Enter your name and the server address, then press Connect.");
    }

    // The one button flips between Connect and Disconnect depending on where I am.
    private async void btnConnect_Click(object sender, EventArgs e)
    {
        if (_connection is null)
        {
            await ConnectAsync();
        }
        else
        {
            Disconnect("You left the chat.");
        }
    }

    private async Task ConnectAsync()
    {
        string name = ChatPacket.CleanName(txtName.Text);
        string host = txtHost.Text.Trim();

        if (name.Length == 0)
        {
            ShowWarning("Please choose a display name first.");
            txtName.Focus();
            return;
        }

        if (host.Length == 0)
        {
            ShowWarning("Please enter the server address, for example 127.0.0.1.");
            txtHost.Focus();
            return;
        }

        if (!int.TryParse(txtPort.Text.Trim(), out int port) || port is < 1 or > 65535)
        {
            ShowWarning("Please enter a port number between 1 and 65535.");
            txtPort.Focus();
            return;
        }

        btnConnect.Enabled = false;
        SetStatus($"Connecting to {host}:{port}...", Theme.MutedText);

        // I give the connection five seconds. Without a timeout the app would just sit there if the server is not running.
        var tcp = new TcpClient();
        try
        {
            using var timeout = new CancellationTokenSource(TimeSpan.FromSeconds(5));
            await tcp.ConnectAsync(host, port, timeout.Token);
        }
        catch (Exception ex) when (ex is SocketException or OperationCanceledException)
        {
            tcp.Dispose();
            string reason = ex is OperationCanceledException ? "the server did not answer in time" : ex.Message;
            SetStatus("Not connected", Theme.MutedText);
            AppendNotice($"Could not connect to {host}:{port} because {reason}.");
            btnConnect.Enabled = true;
            return;
        }

        var connection = new ChatConnection(tcp);
        try
        {
            await connection.SendAsync(ChatPacket.Join(name));
        }
        catch (Exception ex) when (ex is IOException or SocketException)
        {
            connection.Dispose();
            SetStatus("Not connected", Theme.MutedText);
            AppendNotice("The server dropped the connection straight away.");
            btnConnect.Enabled = true;
            return;
        }

        _connection = connection;
        _sessionCts = new CancellationTokenSource();
        _myName = name;

        new ClientSettings(name, host, port).Save();

        SetConnectedState(true);
        SetStatus($"Connected to {host}:{port}", Theme.Success);

        // I deliberately do not await this. It runs for as long as I stay connected, and because I start it from the
        // UI thread every packet comes back on the UI thread too, so I can update controls without Invoke.
        _ = ReceiveLoopAsync(connection, _sessionCts.Token);
    }

    private async Task ReceiveLoopAsync(ChatConnection connection, CancellationToken cancellationToken)
    {
        string reason = "The server closed the connection.";
        try
        {
            while (await connection.ReceiveAsync(cancellationToken) is { } packet)
            {
                HandlePacket(packet);
            }
        }
        catch (OperationCanceledException)
        {
            // I cancel this myself when I disconnect, so there is nothing more to do.
            return;
        }
        catch (Exception ex) when (ex is IOException or SocketException or ObjectDisposedException)
        {
            reason = "The connection to the server was lost.";
        }

        // I only tidy up if this is still the active connection and not one I already replaced or closed.
        if (ReferenceEquals(connection, _connection))
        {
            Disconnect(reason);
        }
    }

    // Here I decide what to show for each kind of packet the server sends me.
    private void HandlePacket(ChatPacket packet)
    {
        switch (packet.Type)
        {
            case PacketType.Welcome:
                _myName = packet.Sender;
                Text = $"Chat Client ({_myName})";
                AppendNotice($"You are in the chat as {_myName}.");
                break;

            case PacketType.Join:
                AppendNotice(packet.Sender == _myName ? "You joined the chat." : $"{packet.Sender} joined the chat.");
                break;

            case PacketType.Leave:
                AppendNotice($"{packet.Sender} left the chat.");
                break;

            case PacketType.Message:
                AppendMessage(packet);
                break;

            case PacketType.UserList:
                ShowUsers(packet.Users ?? []);
                break;

            case PacketType.Notice:
                AppendNotice(packet.Text);
                break;
        }
    }

    private async void btnSend_Click(object sender, EventArgs e)
    {
        if (_connection is null)
        {
            return;
        }

        string text = ChatPacket.CleanText(txtMessage.Text);
        if (text.Length == 0)
        {
            return;
        }

        try
        {
            await _connection.SendAsync(ChatPacket.Message(_myName, text));
            txtMessage.Clear();
        }
        catch (Exception ex) when (ex is IOException or SocketException or ObjectDisposedException)
        {
            Disconnect("I could not send that message because the connection dropped.");
        }

        txtMessage.Focus();
    }

    // Enter sends the message, and Shift+Enter adds a new line, just like most chat apps.
    private void txtMessage_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Enter && !e.Shift)
        {
            e.SuppressKeyPress = true;
            btnSend.PerformClick();
        }
    }

    // Here I close the connection and reset the window. I call this for a normal disconnect and when something goes wrong.
    private void Disconnect(string reason)
    {
        _sessionCts?.Cancel();
        _sessionCts?.Dispose();
        _sessionCts = null;

        _connection?.Dispose();
        _connection = null;

        ShowUsers([]);
        Text = "Chat Client";
        SetConnectedState(false);
        SetStatus("Not connected", Theme.MutedText);
        AppendNotice(reason);
    }

    private void Form1_FormClosing(object? sender, FormClosingEventArgs e)
    {
        // I close the socket on the way out so the server sees me leave straight away instead of timing out.
        _sessionCts?.Cancel();
        _connection?.Dispose();
        _connection = null;
    }

    private void SetConnectedState(bool connected)
    {
        btnConnect.Enabled = true;
        btnConnect.Text = connected ? "Disconnect" : "Connect";
        btnConnect.BackColor = connected ? Theme.Danger : Theme.Accent;

        txtName.Enabled = !connected;
        txtHost.Enabled = !connected;
        txtPort.Enabled = !connected;

        txtMessage.Enabled = connected;
        btnSend.Enabled = connected;
        AcceptButton = connected ? null : btnConnect;

        if (connected)
        {
            txtMessage.Focus();
        }
    }

    private void SetStatus(string text, Color colour)
    {
        lblStatus.Text = text;
        lblStatus.ForeColor = colour;
    }

    private void ShowUsers(IReadOnlyList<string> users)
    {
        lstUsers.BeginUpdate();
        lstUsers.Items.Clear();
        foreach (string name in users)
        {
            lstUsers.Items.Add(name == _myName ? $"{name} (you)" : name);
        }
        lstUsers.EndUpdate();
        lblUsers.Text = $"Online ({users.Count})";
    }

    // Each message line looks like "[14:05] Andrea: hello", with the time dimmed and the name coloured.
    private void AppendMessage(ChatPacket packet)
    {
        bool isMine = packet.Sender == _myName;
        string time = packet.Timestamp.ToLocalTime().ToString("HH:mm");
        string body = packet.Text.Replace("\n", Environment.NewLine + "        ");

        AppendColoured($"[{time}] ", Theme.MutedText);
        AppendColoured(isMine ? "You" : packet.Sender, isMine ? Theme.Self : ColourFor(packet.Sender), bold: true);
        AppendColoured($": {body}{Environment.NewLine}", Theme.Text);
        rtbChat.ScrollToCaret();
    }

    private void AppendNotice(string text)
    {
        AppendColoured($"[{DateTime.Now:HH:mm}] {text}{Environment.NewLine}", Theme.MutedText, italic: true);
        rtbChat.ScrollToCaret();
    }

    // I move the caret to the very end, set the colour and font, then add the text, which gives each piece its own style.
    private void AppendColoured(string text, Color colour, bool bold = false, bool italic = false)
    {
        rtbChat.SelectionStart = rtbChat.TextLength;
        rtbChat.SelectionLength = 0;
        rtbChat.SelectionColor = colour;

        rtbChat.SelectionFont = bold ? _boldFont : italic ? _italicFont : rtbChat.Font;
        rtbChat.AppendText(text);
    }

    // I need the same person to always get the same colour, so I use a simple stable hash instead of GetHashCode,
    // which changes every time the app starts.
    private static Color ColourFor(string name)
    {
        int hash = 0;
        foreach (char c in name)
        {
            hash = unchecked(hash * 31 + c);
        }
        return NameColours[(hash & int.MaxValue) % NameColours.Length];
    }

    private void ShowWarning(string text) =>
        MessageBox.Show(this, text, "Chat Client", MessageBoxButtons.OK, MessageBoxIcon.Warning);
}

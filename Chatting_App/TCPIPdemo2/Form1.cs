using System.Net;
using System.Net.Sockets;
using ChatShared;

namespace TCPIPdemo2;

// This is the server window. All it does is collect the address and port, start and stop the
// ChatServer, and show what is going on. The real work happens inside ChatServer.
public partial class Form1 : Form
{
    private readonly ChatServer _server = new();
    private bool _closeAfterStop;

    public Form1()
    {
        InitializeComponent();

        // The server raises its events on background threads, so I hop back onto the UI thread before touching any control.
        _server.Log += message => RunOnUi(() => AppendLog(message));
        _server.UsersChanged += users => RunOnUi(() => ShowUsers(users));
    }

    private void Server_Load(object sender, EventArgs e)
    {
        txtHost.Text = ChatDefaults.ServerBindAddress;
        txtPort.Text = ChatDefaults.Port.ToString();
        SetRunningState(false);
        AppendLog("Ready. Use 0.0.0.0 to accept connections on every network adapter.");
    }

    private void btnStart_Click(object sender, EventArgs e)
    {
        // I treat an empty address box as "listen everywhere" so the user can just press Start.
        string hostText = txtHost.Text.Trim();
        IPAddress? address = IPAddress.Any;
        if (hostText.Length > 0 && !IPAddress.TryParse(hostText, out address))
        {
            ShowWarning("Please enter a valid IP address, for example 0.0.0.0 or 127.0.0.1.");
            txtHost.Focus();
            return;
        }

        if (!TryReadPort(out int port))
        {
            ShowWarning("Please enter a port number between 1 and 65535.");
            txtPort.Focus();
            return;
        }

        try
        {
            _server.Start(address!, port);
        }
        catch (SocketException ex)
        {
            ShowWarning($"I could not start the server on that address and port.\n\n{ex.Message}");
            return;
        }

        SetRunningState(true);
        AnnounceReachableAddresses(address!, port);
    }

    private async void btnStop_Click(object sender, EventArgs e)
    {
        btnStop.Enabled = false;
        await _server.StopAsync();
        SetRunningState(false);
    }

    // When the window is closed while the server is running, I cancel the close, stop the server properly, then close for real.
    private async void Form1_FormClosing(object? sender, FormClosingEventArgs e)
    {
        if (_closeAfterStop || !_server.IsRunning)
        {
            return;
        }

        e.Cancel = true;
        _closeAfterStop = true;
        await _server.StopAsync();
        Close();
    }

    // Here I print the addresses other computers can actually type into the client, which saves a trip to ipconfig.
    private void AnnounceReachableAddresses(IPAddress boundAddress, int port)
    {
        if (!boundAddress.Equals(IPAddress.Any))
        {
            AppendLog($"Clients should connect to {boundAddress}:{port}");
            return;
        }

        try
        {
            IEnumerable<string> lanAddresses = Dns.GetHostAddresses(Dns.GetHostName())
                .Where(ip => ip.AddressFamily == AddressFamily.InterNetwork && !IPAddress.IsLoopback(ip))
                .Select(ip => ip.ToString());

            AppendLog($"On this PC, clients can use 127.0.0.1:{port}");
            foreach (string ip in lanAddresses)
            {
                AppendLog($"On your network, clients can use {ip}:{port}");
            }
        }
        catch (SocketException)
        {
            // Looking up my own addresses is only a convenience, so if it fails I just skip it.
        }
    }

    private void SetRunningState(bool running)
    {
        btnStart.Enabled = !running;
        btnStop.Enabled = running;
        txtHost.Enabled = !running;
        txtPort.Enabled = !running;
        lblStatus.Text = running ? "Server is running" : "Server is stopped";
        lblStatus.ForeColor = running ? Theme.Success : Theme.MutedText;
    }

    private void ShowUsers(IReadOnlyList<string> users)
    {
        lstUsers.BeginUpdate();
        lstUsers.Items.Clear();
        foreach (string name in users)
        {
            lstUsers.Items.Add(name);
        }
        lstUsers.EndUpdate();
        lblUsers.Text = $"Online ({users.Count})";
    }

    private void AppendLog(string message)
    {
        txtLog.AppendText($"[{DateTime.Now:HH:mm:ss}] {message}{Environment.NewLine}");
    }

    private bool TryReadPort(out int port) =>
        int.TryParse(txtPort.Text.Trim(), out port) && port is > 0 and <= 65535;

    private void ShowWarning(string text) =>
        MessageBox.Show(this, text, "Server", MessageBoxButtons.OK, MessageBoxIcon.Warning);

    // A tiny helper I use so background events can safely update the window, even while it is closing.
    private void RunOnUi(Action action)
    {
        if (IsDisposed || Disposing)
        {
            return;
        }

        if (!InvokeRequired)
        {
            action();
            return;
        }

        try
        {
            BeginInvoke(action);
        }
        catch (InvalidOperationException)
        {
            // The window handle is already gone, so there is nothing left to update.
        }
    }
}

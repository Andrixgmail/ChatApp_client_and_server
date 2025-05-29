using SimpleTCP;
using System.Text;

namespace Client
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        SimpleTcpClient client;


        private void Form1_Load(object sender, EventArgs e)
        {
            client = new SimpleTcpClient();
            client.StringEncoder = Encoding.UTF8;
            client.DataReceived += Client_DataReceived;
        }

        private void Client_DataReceived(object? sender, SimpleTCP.Message e)
        {
            txtStatus.Invoke((MethodInvoker)delegate ()
            {
                txtStatus.Text += e.MessageString;

            });
        }

        private void btnConnect_Click_1(object sender, EventArgs e)
        {
            btnConnect.Enabled = false;
            //string ipaddress = ToString
            // Get server address from textbox or other source
            client.Connect(txtHost.Text, int.Parse(txtPort.Text));
            
            
        }

        private void btnSend_Click_1(object sender, EventArgs e)
        {
            client.WriteLineAndGetReply(txtMessage.Text, TimeSpan.FromSeconds(3));
        }
    }
}

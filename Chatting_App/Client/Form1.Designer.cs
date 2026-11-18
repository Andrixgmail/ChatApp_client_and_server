namespace Client
{
    partial class Form1
    {
        /// <summary>
        ///  I keep the designer container here so any components get cleaned up with the form.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Here I release the designer components when the window is closed.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  This is where I build the client window: connection details along the top, the chat in the middle,
        ///  the online list on the right and the message box along the bottom.
        ///  The Visual Studio designer can still open and edit this method.
        /// </summary>
        private void InitializeComponent()
        {
            lblName = new Label();
            txtName = new TextBox();
            label1 = new Label();
            txtHost = new TextBox();
            label2 = new Label();
            txtPort = new TextBox();
            btnConnect = new Button();
            lblStatus = new Label();
            rtbChat = new RichTextBox();
            lblUsers = new Label();
            lstUsers = new ListBox();
            txtMessage = new TextBox();
            btnSend = new Button();
            SuspendLayout();
            //
            // lblName
            //
            lblName.AutoSize = true;
            lblName.ForeColor = Color.FromArgb(148, 155, 168);
            lblName.Location = new Point(16, 21);
            lblName.Name = "lblName";
            lblName.Size = new Size(45, 19);
            lblName.TabIndex = 0;
            lblName.Text = "Name";
            //
            // txtName
            //
            txtName.BackColor = Color.FromArgb(34, 37, 44);
            txtName.BorderStyle = BorderStyle.FixedSingle;
            txtName.ForeColor = Color.FromArgb(232, 234, 239);
            txtName.Location = new Point(68, 18);
            txtName.Name = "txtName";
            txtName.PlaceholderText = "Your name";
            txtName.Size = new Size(140, 25);
            txtName.TabIndex = 1;
            //
            // label1
            //
            label1.AutoSize = true;
            label1.ForeColor = Color.FromArgb(148, 155, 168);
            label1.Location = new Point(224, 21);
            label1.Name = "label1";
            label1.Size = new Size(38, 19);
            label1.TabIndex = 2;
            label1.Text = "Host";
            //
            // txtHost
            //
            txtHost.BackColor = Color.FromArgb(34, 37, 44);
            txtHost.BorderStyle = BorderStyle.FixedSingle;
            txtHost.ForeColor = Color.FromArgb(232, 234, 239);
            txtHost.Location = new Point(270, 18);
            txtHost.Name = "txtHost";
            txtHost.PlaceholderText = "127.0.0.1";
            txtHost.Size = new Size(160, 25);
            txtHost.TabIndex = 3;
            //
            // label2
            //
            label2.AutoSize = true;
            label2.ForeColor = Color.FromArgb(148, 155, 168);
            label2.Location = new Point(446, 21);
            label2.Name = "label2";
            label2.Size = new Size(34, 19);
            label2.TabIndex = 4;
            label2.Text = "Port";
            //
            // txtPort
            //
            txtPort.BackColor = Color.FromArgb(34, 37, 44);
            txtPort.BorderStyle = BorderStyle.FixedSingle;
            txtPort.ForeColor = Color.FromArgb(232, 234, 239);
            txtPort.Location = new Point(488, 18);
            txtPort.MaxLength = 5;
            txtPort.Name = "txtPort";
            txtPort.PlaceholderText = "8910";
            txtPort.Size = new Size(72, 25);
            txtPort.TabIndex = 5;
            //
            // btnConnect
            //
            btnConnect.BackColor = Color.FromArgb(47, 158, 143);
            btnConnect.Cursor = Cursors.Hand;
            btnConnect.FlatAppearance.BorderSize = 0;
            btnConnect.FlatStyle = FlatStyle.Flat;
            btnConnect.ForeColor = Color.White;
            btnConnect.Location = new Point(576, 15);
            btnConnect.Name = "btnConnect";
            btnConnect.Size = new Size(120, 32);
            btnConnect.TabIndex = 6;
            btnConnect.Text = "Connect";
            btnConnect.UseVisualStyleBackColor = false;
            btnConnect.Click += btnConnect_Click;
            //
            // lblStatus
            //
            lblStatus.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            lblStatus.ForeColor = Color.FromArgb(148, 155, 168);
            lblStatus.Location = new Point(16, 56);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(868, 20);
            lblStatus.TabIndex = 7;
            lblStatus.Text = "Not connected";
            //
            // rtbChat
            //
            rtbChat.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            rtbChat.BackColor = Color.FromArgb(34, 37, 44);
            rtbChat.BorderStyle = BorderStyle.None;
            rtbChat.DetectUrls = true;
            rtbChat.ForeColor = Color.FromArgb(232, 234, 239);
            rtbChat.Location = new Point(16, 84);
            rtbChat.Name = "rtbChat";
            rtbChat.ReadOnly = true;
            rtbChat.ScrollBars = RichTextBoxScrollBars.Vertical;
            rtbChat.Size = new Size(688, 436);
            rtbChat.TabIndex = 8;
            rtbChat.TabStop = false;
            rtbChat.Text = "";
            //
            // lblUsers
            //
            lblUsers.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblUsers.AutoSize = true;
            lblUsers.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            lblUsers.Location = new Point(720, 84);
            lblUsers.Name = "lblUsers";
            lblUsers.Size = new Size(75, 19);
            lblUsers.TabIndex = 9;
            lblUsers.Text = "Online (0)";
            //
            // lstUsers
            //
            lstUsers.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            lstUsers.BackColor = Color.FromArgb(34, 37, 44);
            lstUsers.BorderStyle = BorderStyle.None;
            lstUsers.ForeColor = Color.FromArgb(232, 234, 239);
            lstUsers.IntegralHeight = false;
            lstUsers.ItemHeight = 17;
            lstUsers.Location = new Point(720, 110);
            lstUsers.Name = "lstUsers";
            lstUsers.SelectionMode = SelectionMode.None;
            lstUsers.Size = new Size(164, 410);
            lstUsers.TabIndex = 10;
            lstUsers.TabStop = false;
            //
            // txtMessage
            //
            txtMessage.AcceptsReturn = true;
            txtMessage.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            txtMessage.BackColor = Color.FromArgb(34, 37, 44);
            txtMessage.BorderStyle = BorderStyle.FixedSingle;
            txtMessage.ForeColor = Color.FromArgb(232, 234, 239);
            txtMessage.Location = new Point(16, 532);
            txtMessage.Multiline = true;
            txtMessage.Name = "txtMessage";
            txtMessage.PlaceholderText = "Type a message. Enter sends, Shift+Enter adds a new line.";
            txtMessage.ScrollBars = ScrollBars.Vertical;
            txtMessage.Size = new Size(580, 52);
            txtMessage.TabIndex = 11;
            txtMessage.KeyDown += txtMessage_KeyDown;
            //
            // btnSend
            //
            btnSend.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnSend.BackColor = Color.FromArgb(47, 158, 143);
            btnSend.Cursor = Cursors.Hand;
            btnSend.FlatAppearance.BorderSize = 0;
            btnSend.FlatStyle = FlatStyle.Flat;
            btnSend.ForeColor = Color.White;
            btnSend.Location = new Point(604, 532);
            btnSend.Name = "btnSend";
            btnSend.Size = new Size(100, 52);
            btnSend.TabIndex = 12;
            btnSend.Text = "Send";
            btnSend.UseVisualStyleBackColor = false;
            btnSend.Click += btnSend_Click;
            //
            // Form1
            //
            AcceptButton = btnConnect;
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            BackColor = Color.FromArgb(24, 26, 31);
            ClientSize = new Size(900, 600);
            Controls.Add(btnSend);
            Controls.Add(txtMessage);
            Controls.Add(lstUsers);
            Controls.Add(lblUsers);
            Controls.Add(rtbChat);
            Controls.Add(lblStatus);
            Controls.Add(btnConnect);
            Controls.Add(txtPort);
            Controls.Add(label2);
            Controls.Add(txtHost);
            Controls.Add(label1);
            Controls.Add(txtName);
            Controls.Add(lblName);
            Font = new Font("Segoe UI", 10F);
            ForeColor = Color.FromArgb(232, 234, 239);
            MinimumSize = new Size(760, 460);
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Chat Client";
            FormClosing += Form1_FormClosing;
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblName;
        private TextBox txtName;
        private Label label1;
        private TextBox txtHost;
        private Label label2;
        private TextBox txtPort;
        private Button btnConnect;
        private Label lblStatus;
        private RichTextBox rtbChat;
        private Label lblUsers;
        private ListBox lstUsers;
        private TextBox txtMessage;
        private Button btnSend;
    }
}

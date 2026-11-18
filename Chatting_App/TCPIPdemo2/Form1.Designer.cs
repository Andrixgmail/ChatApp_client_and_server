namespace TCPIPdemo2
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
        ///  This is where I build the server window: a dark top bar for the address and port,
        ///  an activity log on the left and the list of online users on the right.
        ///  The Visual Studio designer can still open and edit this method.
        /// </summary>
        private void InitializeComponent()
        {
            label1 = new Label();
            label2 = new Label();
            txtHost = new TextBox();
            txtPort = new TextBox();
            btnStart = new Button();
            btnStop = new Button();
            lblStatus = new Label();
            lblLog = new Label();
            txtLog = new TextBox();
            lblUsers = new Label();
            lstUsers = new ListBox();
            SuspendLayout();
            //
            // label1
            //
            label1.AutoSize = true;
            label1.ForeColor = Color.FromArgb(148, 155, 168);
            label1.Location = new Point(16, 21);
            label1.Name = "label1";
            label1.Size = new Size(66, 19);
            label1.TabIndex = 0;
            label1.Text = "IP address";
            //
            // txtHost
            //
            txtHost.BackColor = Color.FromArgb(34, 37, 44);
            txtHost.BorderStyle = BorderStyle.FixedSingle;
            txtHost.ForeColor = Color.FromArgb(232, 234, 239);
            txtHost.Location = new Point(92, 18);
            txtHost.Name = "txtHost";
            txtHost.PlaceholderText = "0.0.0.0";
            txtHost.Size = new Size(160, 25);
            txtHost.TabIndex = 1;
            //
            // label2
            //
            label2.AutoSize = true;
            label2.ForeColor = Color.FromArgb(148, 155, 168);
            label2.Location = new Point(268, 21);
            label2.Name = "label2";
            label2.Size = new Size(34, 19);
            label2.TabIndex = 2;
            label2.Text = "Port";
            //
            // txtPort
            //
            txtPort.BackColor = Color.FromArgb(34, 37, 44);
            txtPort.BorderStyle = BorderStyle.FixedSingle;
            txtPort.ForeColor = Color.FromArgb(232, 234, 239);
            txtPort.Location = new Point(310, 18);
            txtPort.MaxLength = 5;
            txtPort.Name = "txtPort";
            txtPort.PlaceholderText = "8910";
            txtPort.Size = new Size(80, 25);
            txtPort.TabIndex = 3;
            //
            // btnStart
            //
            btnStart.BackColor = Color.FromArgb(47, 158, 143);
            btnStart.Cursor = Cursors.Hand;
            btnStart.FlatAppearance.BorderSize = 0;
            btnStart.FlatStyle = FlatStyle.Flat;
            btnStart.ForeColor = Color.White;
            btnStart.Location = new Point(406, 15);
            btnStart.Name = "btnStart";
            btnStart.Size = new Size(100, 32);
            btnStart.TabIndex = 4;
            btnStart.Text = "Start";
            btnStart.UseVisualStyleBackColor = false;
            btnStart.Click += btnStart_Click;
            //
            // btnStop
            //
            btnStop.BackColor = Color.FromArgb(196, 72, 72);
            btnStop.Cursor = Cursors.Hand;
            btnStop.FlatAppearance.BorderSize = 0;
            btnStop.FlatStyle = FlatStyle.Flat;
            btnStop.ForeColor = Color.White;
            btnStop.Location = new Point(514, 15);
            btnStop.Name = "btnStop";
            btnStop.Size = new Size(100, 32);
            btnStop.TabIndex = 5;
            btnStop.Text = "Stop";
            btnStop.UseVisualStyleBackColor = false;
            btnStop.Click += btnStop_Click;
            //
            // lblStatus
            //
            lblStatus.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            lblStatus.ForeColor = Color.FromArgb(148, 155, 168);
            lblStatus.Location = new Point(16, 56);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(788, 20);
            lblStatus.TabIndex = 6;
            lblStatus.Text = "Server is stopped";
            //
            // lblLog
            //
            lblLog.AutoSize = true;
            lblLog.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            lblLog.Location = new Point(16, 84);
            lblLog.Name = "lblLog";
            lblLog.Size = new Size(57, 19);
            lblLog.TabIndex = 7;
            lblLog.Text = "Activity";
            //
            // txtLog
            //
            txtLog.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            txtLog.BackColor = Color.FromArgb(34, 37, 44);
            txtLog.BorderStyle = BorderStyle.None;
            txtLog.Font = new Font("Consolas", 10F);
            txtLog.ForeColor = Color.FromArgb(232, 234, 239);
            txtLog.Location = new Point(16, 110);
            txtLog.Multiline = true;
            txtLog.Name = "txtLog";
            txtLog.ReadOnly = true;
            txtLog.ScrollBars = ScrollBars.Vertical;
            txtLog.Size = new Size(600, 414);
            txtLog.TabIndex = 8;
            txtLog.TabStop = false;
            //
            // lblUsers
            //
            lblUsers.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblUsers.AutoSize = true;
            lblUsers.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            lblUsers.Location = new Point(632, 84);
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
            lstUsers.Location = new Point(632, 110);
            lstUsers.Name = "lstUsers";
            lstUsers.SelectionMode = SelectionMode.None;
            lstUsers.Size = new Size(172, 414);
            lstUsers.TabIndex = 10;
            lstUsers.TabStop = false;
            //
            // Form1
            //
            AcceptButton = btnStart;
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            BackColor = Color.FromArgb(24, 26, 31);
            ClientSize = new Size(820, 540);
            Controls.Add(lstUsers);
            Controls.Add(lblUsers);
            Controls.Add(txtLog);
            Controls.Add(lblLog);
            Controls.Add(lblStatus);
            Controls.Add(btnStop);
            Controls.Add(btnStart);
            Controls.Add(txtPort);
            Controls.Add(label2);
            Controls.Add(txtHost);
            Controls.Add(label1);
            Font = new Font("Segoe UI", 10F);
            ForeColor = Color.FromArgb(232, 234, 239);
            MinimumSize = new Size(700, 400);
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Chat Server";
            FormClosing += Form1_FormClosing;
            Load += Server_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private TextBox txtHost;
        private TextBox txtPort;
        private Button btnStart;
        private Button btnStop;
        private Label lblStatus;
        private Label lblLog;
        private TextBox txtLog;
        private Label lblUsers;
        private ListBox lstUsers;
    }
}

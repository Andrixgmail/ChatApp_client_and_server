namespace Client
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            btnConnect = new Button();
            txtPort = new TextBox();
            txtHost = new TextBox();
            label2 = new Label();
            label1 = new Label();
            txtMessage = new TextBox();
            btnSend = new Button();
            txtStatus = new TextBox();
            SuspendLayout();
            // 
            // btnConnect
            // 
            btnConnect.Location = new Point(387, 44);
            btnConnect.Name = "btnConnect";
            btnConnect.Size = new Size(94, 29);
            btnConnect.TabIndex = 9;
            btnConnect.Text = "Connect";
            btnConnect.UseVisualStyleBackColor = true;
            btnConnect.Click += btnConnect_Click_1;
            // 
            // txtPort
            // 
            txtPort.Location = new Point(171, 92);
            txtPort.Name = "txtPort";
            txtPort.Size = new Size(159, 27);
            txtPort.TabIndex = 8;
            // 
            // txtHost
            // 
            txtHost.Location = new Point(171, 46);
            txtHost.Name = "txtHost";
            txtHost.Size = new Size(159, 27);
            txtHost.TabIndex = 7;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(56, 92);
            label2.Name = "label2";
            label2.Size = new Size(35, 20);
            label2.TabIndex = 6;
            label2.Text = "Port";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(56, 49);
            label1.Name = "label1";
            label1.Size = new Size(76, 20);
            label1.TabIndex = 5;
            label1.Text = "IP address";
            // 
            // txtMessage
            // 
            txtMessage.Location = new Point(56, 143);
            txtMessage.Multiline = true;
            txtMessage.Name = "txtMessage";
            txtMessage.Size = new Size(425, 50);
            txtMessage.TabIndex = 10;
            // 
            // btnSend
            // 
            btnSend.Location = new Point(387, 211);
            btnSend.Name = "btnSend";
            btnSend.Size = new Size(94, 29);
            btnSend.TabIndex = 11;
            btnSend.Text = "Send";
            btnSend.UseVisualStyleBackColor = true;
            btnSend.Click += btnSend_Click_1;
            // 
            // txtStatus
            // 
            txtStatus.Location = new Point(56, 246);
            txtStatus.Multiline = true;
            txtStatus.Name = "txtStatus";
            txtStatus.Size = new Size(425, 116);
            txtStatus.TabIndex = 12;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(txtStatus);
            Controls.Add(btnSend);
            Controls.Add(txtMessage);
            Controls.Add(btnConnect);
            Controls.Add(txtPort);
            Controls.Add(txtHost);
            Controls.Add(label2);
            Controls.Add(label1);
            Name = "Form1";
            Text = "Client";
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnConnect;
        private TextBox txtPort;
        private TextBox txtHost;
        private Label label2;
        private Label label1;
        private TextBox txtMessage;
        private Button btnSend;
        private TextBox txtStatus;
    }
}

namespace TCPIPdemo2
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
            label1 = new Label();
            label2 = new Label();
            txtHost = new TextBox();
            txtPort = new TextBox();
            btnStart = new Button();
            btnStop = new Button();
            txtStatus = new TextBox();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(52, 32);
            label1.Name = "label1";
            label1.Size = new Size(76, 20);
            label1.TabIndex = 0;
            label1.Text = "IP address";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(52, 75);
            label2.Name = "label2";
            label2.Size = new Size(35, 20);
            label2.TabIndex = 1;
            label2.Text = "Port";
            // 
            // txtHost
            // 
            txtHost.Location = new Point(167, 29);
            txtHost.Name = "txtHost";
            txtHost.Size = new Size(159, 27);
            txtHost.TabIndex = 2;
            // 
            // txtPort
            // 
            txtPort.Location = new Point(167, 75);
            txtPort.Name = "txtPort";
            txtPort.Size = new Size(159, 27);
            txtPort.TabIndex = 3;
            // 
            // btnStart
            // 
            btnStart.Location = new Point(383, 27);
            btnStart.Name = "btnStart";
            btnStart.Size = new Size(94, 29);
            btnStart.TabIndex = 4;
            btnStart.Text = "Start";
            btnStart.UseVisualStyleBackColor = true;
            btnStart.Click += btnStart_Click_1;
            // 
            // btnStop
            // 
            btnStop.Location = new Point(504, 27);
            btnStop.Name = "btnStop";
            btnStop.Size = new Size(94, 29);
            btnStop.TabIndex = 5;
            btnStop.Text = "Stop";
            btnStop.UseVisualStyleBackColor = true;
            // 
            // txtStatus
            // 
            txtStatus.Location = new Point(170, 134);
            txtStatus.Multiline = true;
            txtStatus.Name = "txtStatus";
            txtStatus.Size = new Size(428, 203);
            txtStatus.TabIndex = 6;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(txtStatus);
            Controls.Add(btnStop);
            Controls.Add(btnStart);
            Controls.Add(txtPort);
            Controls.Add(txtHost);
            Controls.Add(label2);
            Controls.Add(label1);
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Server";
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
        private TextBox txtStatus;
    }
}

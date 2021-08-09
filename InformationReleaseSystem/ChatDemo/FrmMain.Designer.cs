namespace ChatDemo
{
    partial class FrmMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lbIP = new System.Windows.Forms.Label();
            this.txtIP = new System.Windows.Forms.TextBox();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.lbPort = new System.Windows.Forms.Label();
            this.btnStart = new System.Windows.Forms.Button();
            this.txtMsg = new System.Windows.Forms.TextBox();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.btnSendMsg = new System.Windows.Forms.Button();
            this.btnSendShake = new System.Windows.Forms.Button();
            this.btnSendFile = new System.Windows.Forms.Button();
            this.btmHtml = new System.Windows.Forms.Button();
            this.btnSendZip = new System.Windows.Forms.Button();
            this.btnSendMsg2 = new System.Windows.Forms.Button();
            this.cbClient = new System.Windows.Forms.ComboBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lbIP
            // 
            this.lbIP.AutoSize = true;
            this.lbIP.Location = new System.Drawing.Point(66, 30);
            this.lbIP.Name = "lbIP";
            this.lbIP.Size = new System.Drawing.Size(17, 12);
            this.lbIP.TabIndex = 0;
            this.lbIP.Text = "ip";
            // 
            // txtIP
            // 
            this.txtIP.Location = new System.Drawing.Point(102, 27);
            this.txtIP.Name = "txtIP";
            this.txtIP.Size = new System.Drawing.Size(165, 21);
            this.txtIP.TabIndex = 1;
            this.txtIP.Text = "192.168.2.188";
            // 
            // txtPort
            // 
            this.txtPort.Location = new System.Drawing.Point(102, 66);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(100, 21);
            this.txtPort.TabIndex = 2;
            this.txtPort.Text = "11211";
            // 
            // lbPort
            // 
            this.lbPort.AutoSize = true;
            this.lbPort.Location = new System.Drawing.Point(55, 69);
            this.lbPort.Name = "lbPort";
            this.lbPort.Size = new System.Drawing.Size(29, 12);
            this.lbPort.TabIndex = 3;
            this.lbPort.Text = "端口";
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(102, 93);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 23);
            this.btnStart.TabIndex = 4;
            this.btnStart.Text = "开始";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // txtMsg
            // 
            this.txtMsg.Location = new System.Drawing.Point(68, 137);
            this.txtMsg.Multiline = true;
            this.txtMsg.Name = "txtMsg";
            this.txtMsg.Size = new System.Drawing.Size(213, 261);
            this.txtMsg.TabIndex = 5;
            this.txtMsg.Text = "{\"type\": \"pong\"}\r\n";
            this.txtMsg.TextChanged += new System.EventHandler(this.TxtMsg_TextChanged);
            // 
            // txtLog
            // 
            this.txtLog.Location = new System.Drawing.Point(299, 184);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.Size = new System.Drawing.Size(317, 214);
            this.txtLog.TabIndex = 6;
            // 
            // btnSendMsg
            // 
            this.btnSendMsg.Location = new System.Drawing.Point(299, 135);
            this.btnSendMsg.Name = "btnSendMsg";
            this.btnSendMsg.Size = new System.Drawing.Size(75, 23);
            this.btnSendMsg.TabIndex = 7;
            this.btnSendMsg.Text = "btnSendMsg";
            this.btnSendMsg.UseVisualStyleBackColor = true;
            this.btnSendMsg.Click += new System.EventHandler(this.btnSendMsg_Click);
            // 
            // btnSendShake
            // 
            this.btnSendShake.Location = new System.Drawing.Point(532, 134);
            this.btnSendShake.Name = "btnSendShake";
            this.btnSendShake.Size = new System.Drawing.Size(105, 23);
            this.btnSendShake.TabIndex = 8;
            this.btnSendShake.Text = "btnSendShake";
            this.btnSendShake.UseVisualStyleBackColor = true;
            this.btnSendShake.Click += new System.EventHandler(this.btnSendShake_Click);
            // 
            // btnSendFile
            // 
            this.btnSendFile.Location = new System.Drawing.Point(532, 10);
            this.btnSendFile.Name = "btnSendFile";
            this.btnSendFile.Size = new System.Drawing.Size(119, 23);
            this.btnSendFile.TabIndex = 9;
            this.btnSendFile.Text = "btnSendFile";
            this.btnSendFile.UseVisualStyleBackColor = true;
            this.btnSendFile.Click += new System.EventHandler(this.btnSendFile_Click);
            // 
            // btmHtml
            // 
            this.btmHtml.Location = new System.Drawing.Point(532, 48);
            this.btmHtml.Name = "btmHtml";
            this.btmHtml.Size = new System.Drawing.Size(119, 23);
            this.btmHtml.TabIndex = 10;
            this.btmHtml.Text = "btmHtml";
            this.btmHtml.UseVisualStyleBackColor = true;
            this.btmHtml.Click += new System.EventHandler(this.btmHtml_Click);
            // 
            // btnSendZip
            // 
            this.btnSendZip.Location = new System.Drawing.Point(532, 86);
            this.btnSendZip.Name = "btnSendZip";
            this.btnSendZip.Size = new System.Drawing.Size(119, 23);
            this.btnSendZip.TabIndex = 11;
            this.btnSendZip.Text = "btnZip";
            this.btnSendZip.UseVisualStyleBackColor = true;
            this.btnSendZip.Click += new System.EventHandler(this.BtnSendZip_Click);
            // 
            // btnSendMsg2
            // 
            this.btnSendMsg2.Location = new System.Drawing.Point(402, 135);
            this.btnSendMsg2.Name = "btnSendMsg2";
            this.btnSendMsg2.Size = new System.Drawing.Size(110, 23);
            this.btnSendMsg2.TabIndex = 12;
            this.btnSendMsg2.Text = "btnSendMsg2";
            this.btnSendMsg2.UseVisualStyleBackColor = true;
            this.btnSendMsg2.Click += new System.EventHandler(this.btnSendMsg2_Click);
            // 
            // cbClient
            // 
            this.cbClient.FormattingEnabled = true;
            this.cbClient.Location = new System.Drawing.Point(299, 27);
            this.cbClient.Name = "cbClient";
            this.cbClient.Size = new System.Drawing.Size(178, 20);
            this.cbClient.TabIndex = 13;
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(402, 58);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 14;
            this.btnClose.Text = "关闭连接";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(878, 407);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.cbClient);
            this.Controls.Add(this.btnSendMsg2);
            this.Controls.Add(this.btnSendZip);
            this.Controls.Add(this.btmHtml);
            this.Controls.Add(this.btnSendFile);
            this.Controls.Add(this.btnSendShake);
            this.Controls.Add(this.btnSendMsg);
            this.Controls.Add(this.txtLog);
            this.Controls.Add(this.txtMsg);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.lbPort);
            this.Controls.Add(this.txtPort);
            this.Controls.Add(this.txtIP);
            this.Controls.Add(this.lbIP);
            this.Name = "FrmMain";
            this.Text = "FrmMain";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainFrm_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbIP;
        private System.Windows.Forms.TextBox txtIP;
        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.Label lbPort;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.TextBox txtMsg;
        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.Button btnSendMsg;
        private System.Windows.Forms.Button btnSendShake;
        private System.Windows.Forms.Button btnSendFile;
        private System.Windows.Forms.Button btmHtml;
        private System.Windows.Forms.Button btnSendZip;
        private System.Windows.Forms.Button btnSendMsg2;
        private System.Windows.Forms.ComboBox cbClient;
        private System.Windows.Forms.Button btnClose;
    }
}
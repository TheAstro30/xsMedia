namespace xsSettings.Controls
{
    partial class OptionProxy
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblInfo1 = new System.Windows.Forms.Label();
            this.lblHost = new System.Windows.Forms.Label();
            this.txtHost = new System.Windows.Forms.TextBox();
            this.lblPort = new System.Windows.Forms.Label();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.lblPassword = new System.Windows.Forms.Label();
            this.txtUserName = new System.Windows.Forms.TextBox();
            this.lblUserName = new System.Windows.Forms.Label();
            this.lblInfo2 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblInfo1
            // 
            this.lblInfo1.Location = new System.Drawing.Point(8, 36);
            this.lblInfo1.Name = "lblInfo1";
            this.lblInfo1.Size = new System.Drawing.Size(337, 51);
            this.lblInfo1.TabIndex = 0;
            this.lblInfo1.Text = "If you are behind an HTTP Proxy firewall, you can have xsMedia connect to various" +
                " network streams by entering the proxy details below:";
            // 
            // lblHost
            // 
            this.lblHost.AutoSize = true;
            this.lblHost.Location = new System.Drawing.Point(45, 97);
            this.lblHost.Name = "lblHost";
            this.lblHost.Size = new System.Drawing.Size(65, 15);
            this.lblHost.TabIndex = 1;
            this.lblHost.Text = "Proxy host:";
            // 
            // txtHost
            // 
            this.txtHost.Location = new System.Drawing.Point(48, 115);
            this.txtHost.Name = "txtHost";
            this.txtHost.Size = new System.Drawing.Size(179, 23);
            this.txtHost.TabIndex = 2;
            this.txtHost.Tag = "HOST";
            // 
            // lblPort
            // 
            this.lblPort.AutoSize = true;
            this.lblPort.Location = new System.Drawing.Point(230, 97);
            this.lblPort.Name = "lblPort";
            this.lblPort.Size = new System.Drawing.Size(64, 15);
            this.lblPort.TabIndex = 3;
            this.lblPort.Text = "Proxy port:";
            // 
            // txtPort
            // 
            this.txtPort.Location = new System.Drawing.Point(233, 115);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(70, 23);
            this.txtPort.TabIndex = 4;
            this.txtPort.Tag = "PORT";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtPassword);
            this.groupBox1.Controls.Add(this.lblPassword);
            this.groupBox1.Controls.Add(this.txtUserName);
            this.groupBox1.Controls.Add(this.lblUserName);
            this.groupBox1.Controls.Add(this.lblInfo2);
            this.groupBox1.Location = new System.Drawing.Point(48, 144);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(255, 161);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Credentials:";
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(13, 124);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '●';
            this.txtPassword.Size = new System.Drawing.Size(229, 23);
            this.txtPassword.TabIndex = 4;
            this.txtPassword.Tag = "PASS";
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.Location = new System.Drawing.Point(10, 106);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(60, 15);
            this.lblPassword.TabIndex = 3;
            this.lblPassword.Text = "Password:";
            // 
            // txtUserName
            // 
            this.txtUserName.Location = new System.Drawing.Point(13, 80);
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.Size = new System.Drawing.Size(229, 23);
            this.txtUserName.TabIndex = 2;
            this.txtUserName.Tag = "USER";
            // 
            // lblUserName
            // 
            this.lblUserName.AutoSize = true;
            this.lblUserName.Location = new System.Drawing.Point(10, 62);
            this.lblUserName.Name = "lblUserName";
            this.lblUserName.Size = new System.Drawing.Size(66, 15);
            this.lblUserName.TabIndex = 1;
            this.lblUserName.Text = "User name:";
            // 
            // lblInfo2
            // 
            this.lblInfo2.Location = new System.Drawing.Point(10, 19);
            this.lblInfo2.Name = "lblInfo2";
            this.lblInfo2.Size = new System.Drawing.Size(236, 35);
            this.lblInfo2.TabIndex = 0;
            this.lblInfo2.Text = "Some HTTP proxy servers may require authentication; enter your details here:";
            // 
            // OptionProxy
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.txtPort);
            this.Controls.Add(this.lblPort);
            this.Controls.Add(this.txtHost);
            this.Controls.Add(this.lblHost);
            this.Controls.Add(this.lblInfo1);
            this.Name = "OptionProxy";
            this.Size = new System.Drawing.Size(348, 324);
            this.Controls.SetChildIndex(this.lblInfo1, 0);
            this.Controls.SetChildIndex(this.lblHost, 0);
            this.Controls.SetChildIndex(this.txtHost, 0);
            this.Controls.SetChildIndex(this.lblPort, 0);
            this.Controls.SetChildIndex(this.txtPort, 0);
            this.Controls.SetChildIndex(this.groupBox1, 0);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblInfo1;
        private System.Windows.Forms.Label lblHost;
        private System.Windows.Forms.TextBox txtHost;
        private System.Windows.Forms.Label lblPort;
        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.TextBox txtUserName;
        private System.Windows.Forms.Label lblUserName;
        private System.Windows.Forms.Label lblInfo2;
    }
}

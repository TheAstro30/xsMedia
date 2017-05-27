namespace xsMedia.Forms
{
    partial class FrmAbout
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
            this.btnOk = new System.Windows.Forms.Button();
            this.pnlIcon = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblAuthor = new System.Windows.Forms.Label();
            this.lblVersion = new System.Windows.Forms.Label();
            this.lblPlugins = new System.Windows.Forms.Label();
            this.txtPlugins = new System.Windows.Forms.TextBox();
            this.lblCopyright = new System.Windows.Forms.Label();
            this.linkVlc = new System.Windows.Forms.LinkLabel();
            this.lblCodeName = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Location = new System.Drawing.Point(374, 347);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 0;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            // 
            // pnlIcon
            // 
            this.pnlIcon.BackColor = System.Drawing.Color.Transparent;
            this.pnlIcon.Location = new System.Drawing.Point(12, 12);
            this.pnlIcon.Name = "pnlIcon";
            this.pnlIcon.Size = new System.Drawing.Size(64, 64);
            this.pnlIcon.TabIndex = 1;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.Location = new System.Drawing.Point(82, 9);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(161, 30);
            this.lblTitle.TabIndex = 2;
            this.lblTitle.Text = "xsMedia Player";
            // 
            // lblAuthor
            // 
            this.lblAuthor.AutoSize = true;
            this.lblAuthor.BackColor = System.Drawing.Color.Transparent;
            this.lblAuthor.Location = new System.Drawing.Point(84, 61);
            this.lblAuthor.Name = "lblAuthor";
            this.lblAuthor.Size = new System.Drawing.Size(234, 15);
            this.lblAuthor.TabIndex = 3;
            this.lblAuthor.Text = "Written by: Jason James Newland && Ryan J.";
            // 
            // lblVersion
            // 
            this.lblVersion.AutoSize = true;
            this.lblVersion.BackColor = System.Drawing.Color.Transparent;
            this.lblVersion.Location = new System.Drawing.Point(84, 89);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(49, 15);
            this.lblVersion.TabIndex = 4;
            this.lblVersion.Text = "Version:";
            // 
            // lblPlugins
            // 
            this.lblPlugins.AutoSize = true;
            this.lblPlugins.BackColor = System.Drawing.Color.Transparent;
            this.lblPlugins.Location = new System.Drawing.Point(84, 148);
            this.lblPlugins.Name = "lblPlugins";
            this.lblPlugins.Size = new System.Drawing.Size(107, 15);
            this.lblPlugins.TabIndex = 5;
            this.lblPlugins.Text = "Assembly versions:";
            // 
            // txtPlugins
            // 
            this.txtPlugins.BackColor = System.Drawing.Color.White;
            this.txtPlugins.Location = new System.Drawing.Point(87, 166);
            this.txtPlugins.Multiline = true;
            this.txtPlugins.Name = "txtPlugins";
            this.txtPlugins.ReadOnly = true;
            this.txtPlugins.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtPlugins.Size = new System.Drawing.Size(362, 162);
            this.txtPlugins.TabIndex = 1;
            // 
            // lblCopyright
            // 
            this.lblCopyright.BackColor = System.Drawing.Color.Transparent;
            this.lblCopyright.Location = new System.Drawing.Point(84, 116);
            this.lblCopyright.Name = "lblCopyright";
            this.lblCopyright.Size = new System.Drawing.Size(365, 23);
            this.lblCopyright.TabIndex = 7;
            this.lblCopyright.Text = "Copyright ©2013 - 2017, KangaSoft Software. All Rights Reserved";
            // 
            // linkVlc
            // 
            this.linkVlc.AutoSize = true;
            this.linkVlc.Location = new System.Drawing.Point(9, 351);
            this.linkVlc.Name = "linkVlc";
            this.linkVlc.Size = new System.Drawing.Size(100, 15);
            this.linkVlc.TabIndex = 2;
            this.linkVlc.TabStop = true;
            this.linkVlc.Text = "VideoLAN project";
            this.linkVlc.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkVlcLinkClicked);
            // 
            // lblCodeName
            // 
            this.lblCodeName.AutoSize = true;
            this.lblCodeName.BackColor = System.Drawing.Color.Transparent;
            this.lblCodeName.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCodeName.Location = new System.Drawing.Point(84, 39);
            this.lblCodeName.Name = "lblCodeName";
            this.lblCodeName.Size = new System.Drawing.Size(138, 15);
            this.lblCodeName.TabIndex = 8;
            this.lblCodeName.Text = "Code Name - \"Pegasus\"";
            // 
            // FrmAbout
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(461, 379);
            this.ControlBox = false;
            this.Controls.Add(this.lblCodeName);
            this.Controls.Add(this.linkVlc);
            this.Controls.Add(this.lblCopyright);
            this.Controls.Add(this.txtPlugins);
            this.Controls.Add(this.lblPlugins);
            this.Controls.Add(this.lblVersion);
            this.Controls.Add(this.lblAuthor);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.pnlIcon);
            this.Controls.Add(this.btnOk);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmAbout";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "About xsMedia";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Panel pnlIcon;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblAuthor;
        private System.Windows.Forms.Label lblVersion;
        private System.Windows.Forms.Label lblPlugins;
        private System.Windows.Forms.TextBox txtPlugins;
        private System.Windows.Forms.Label lblCopyright;
        private System.Windows.Forms.LinkLabel linkVlc;
        private System.Windows.Forms.Label lblCodeName;

    }
}
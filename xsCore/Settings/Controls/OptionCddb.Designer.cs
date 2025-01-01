namespace xsCore.Settings.Controls
{
    partial class OptionCddb
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
            this.lblHost = new System.Windows.Forms.Label();
            this.txtHost = new System.Windows.Forms.TextBox();
            this.chkCache = new System.Windows.Forms.CheckBox();
            this.chkEnable = new System.Windows.Forms.CheckBox();
            this.lblInfo = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblHost
            // 
            this.lblHost.AutoSize = true;
            this.lblHost.Location = new System.Drawing.Point(30, 121);
            this.lblHost.Name = "lblHost";
            this.lblHost.Size = new System.Drawing.Size(76, 15);
            this.lblHost.TabIndex = 2;
            this.lblHost.Text = "CDDB Server:";
            // 
            // txtHost
            // 
            this.txtHost.Location = new System.Drawing.Point(33, 139);
            this.txtHost.Name = "txtHost";
            this.txtHost.Size = new System.Drawing.Size(303, 23);
            this.txtHost.TabIndex = 3;
            this.txtHost.Tag = "HOST";
            // 
            // chkCache
            // 
            this.chkCache.AutoSize = true;
            this.chkCache.Location = new System.Drawing.Point(33, 185);
            this.chkCache.Name = "chkCache";
            this.chkCache.Size = new System.Drawing.Size(263, 19);
            this.chkCache.TabIndex = 4;
            this.chkCache.Tag = "CACHE";
            this.chkCache.Text = "Always cache CDDB lookups for faster access";
            this.chkCache.UseVisualStyleBackColor = true;
            // 
            // chkEnable
            // 
            this.chkEnable.AutoSize = true;
            this.chkEnable.Location = new System.Drawing.Point(11, 84);
            this.chkEnable.Name = "chkEnable";
            this.chkEnable.Size = new System.Drawing.Size(140, 19);
            this.chkEnable.TabIndex = 1;
            this.chkEnable.Tag = "ENABLE";
            this.chkEnable.Text = "Enable CDDB lookups";
            this.chkEnable.UseVisualStyleBackColor = true;
            // 
            // lblInfo
            // 
            this.lblInfo.Location = new System.Drawing.Point(8, 39);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(337, 37);
            this.lblInfo.TabIndex = 0;
            this.lblInfo.Text = "Compact Disc Database (CDDB) lookups enable xsMedia to get the track and artist n" +
                "ames of a currently playing CD";
            // 
            // OptionCddb
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.lblInfo);
            this.Controls.Add(this.chkEnable);
            this.Controls.Add(this.lblHost);
            this.Controls.Add(this.chkCache);
            this.Controls.Add(this.txtHost);
            this.Name = "OptionCddb";
            this.Size = new System.Drawing.Size(348, 324);
            this.Controls.SetChildIndex(this.txtHost, 0);
            this.Controls.SetChildIndex(this.chkCache, 0);
            this.Controls.SetChildIndex(this.lblHost, 0);
            this.Controls.SetChildIndex(this.chkEnable, 0);
            this.Controls.SetChildIndex(this.lblInfo, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblHost;
        private System.Windows.Forms.TextBox txtHost;
        private System.Windows.Forms.CheckBox chkCache;
        private System.Windows.Forms.CheckBox chkEnable;
        private System.Windows.Forms.Label lblInfo;

    }
}

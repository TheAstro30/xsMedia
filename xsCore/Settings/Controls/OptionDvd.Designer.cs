namespace xsCore.Settings.Controls
{
    partial class OptionDvd
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
            this.lblCaching = new System.Windows.Forms.Label();
            this.txtCaching = new System.Windows.Forms.TextBox();
            this.lblCachingMs = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblCaching
            // 
            this.lblCaching.AutoSize = true;
            this.lblCaching.Location = new System.Drawing.Point(8, 44);
            this.lblCaching.Name = "lblCaching";
            this.lblCaching.Size = new System.Drawing.Size(54, 15);
            this.lblCaching.TabIndex = 0;
            this.lblCaching.Text = "Caching:";
            // 
            // txtCaching
            // 
            this.txtCaching.Location = new System.Drawing.Point(68, 41);
            this.txtCaching.Name = "txtCaching";
            this.txtCaching.Size = new System.Drawing.Size(56, 23);
            this.txtCaching.TabIndex = 1;
            this.txtCaching.Tag = "CACHE";
            // 
            // lblCachingMs
            // 
            this.lblCachingMs.AutoSize = true;
            this.lblCachingMs.Location = new System.Drawing.Point(130, 44);
            this.lblCachingMs.Name = "lblCachingMs";
            this.lblCachingMs.Size = new System.Drawing.Size(100, 15);
            this.lblCachingMs.TabIndex = 2;
            this.lblCachingMs.Text = "milliseconds (ms)";
            // 
            // OptionDvd
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.lblCaching);
            this.Controls.Add(this.txtCaching);
            this.Controls.Add(this.lblCachingMs);
            this.Name = "OptionDvd";
            this.Size = new System.Drawing.Size(348, 324);
            this.Controls.SetChildIndex(this.lblCachingMs, 0);
            this.Controls.SetChildIndex(this.txtCaching, 0);
            this.Controls.SetChildIndex(this.lblCaching, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblCaching;
        private System.Windows.Forms.TextBox txtCaching;
        private System.Windows.Forms.Label lblCachingMs;

    }
}

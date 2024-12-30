namespace xsCore.Settings.Controls
{
    partial class OptionVideo
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
            this.cmbResize = new System.Windows.Forms.ComboBox();
            this.lblOpening = new System.Windows.Forms.Label();
            this.lblResize = new System.Windows.Forms.Label();
            this.chkShowTitle = new System.Windows.Forms.CheckBox();
            this.lblTimeOut = new System.Windows.Forms.Label();
            this.txtTimeOut = new System.Windows.Forms.TextBox();
            this.lblSeconds = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // cmbResize
            // 
            this.cmbResize.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbResize.FormattingEnabled = true;
            this.cmbResize.Location = new System.Drawing.Point(75, 61);
            this.cmbResize.Name = "cmbResize";
            this.cmbResize.Size = new System.Drawing.Size(237, 23);
            this.cmbResize.TabIndex = 1;
            this.cmbResize.Tag = "RESIZE";
            // 
            // lblOpening
            // 
            this.lblOpening.AutoSize = true;
            this.lblOpening.Location = new System.Drawing.Point(8, 38);
            this.lblOpening.Name = "lblOpening";
            this.lblOpening.Size = new System.Drawing.Size(141, 15);
            this.lblOpening.TabIndex = 2;
            this.lblOpening.Text = "When opening video files";
            // 
            // lblResize
            // 
            this.lblResize.AutoSize = true;
            this.lblResize.Location = new System.Drawing.Point(27, 64);
            this.lblResize.Name = "lblResize";
            this.lblResize.Size = new System.Drawing.Size(42, 15);
            this.lblResize.TabIndex = 3;
            this.lblResize.Text = "Resize:";
            // 
            // chkShowTitle
            // 
            this.chkShowTitle.AutoSize = true;
            this.chkShowTitle.Location = new System.Drawing.Point(11, 106);
            this.chkShowTitle.Name = "chkShowTitle";
            this.chkShowTitle.Size = new System.Drawing.Size(204, 19);
            this.chkShowTitle.TabIndex = 4;
            this.chkShowTitle.Tag = "VIDEOTITLE";
            this.chkShowTitle.Text = "Show video title on video window";
            this.chkShowTitle.UseVisualStyleBackColor = true;
            // 
            // lblTimeOut
            // 
            this.lblTimeOut.AutoSize = true;
            this.lblTimeOut.Enabled = false;
            this.lblTimeOut.Location = new System.Drawing.Point(27, 133);
            this.lblTimeOut.Name = "lblTimeOut";
            this.lblTimeOut.Size = new System.Drawing.Size(86, 15);
            this.lblTimeOut.TabIndex = 5;
            this.lblTimeOut.Text = "Time-out after:";
            // 
            // txtTimeOut
            // 
            this.txtTimeOut.Enabled = false;
            this.txtTimeOut.Location = new System.Drawing.Point(115, 130);
            this.txtTimeOut.MaxLength = 2;
            this.txtTimeOut.Name = "txtTimeOut";
            this.txtTimeOut.Size = new System.Drawing.Size(56, 23);
            this.txtTimeOut.TabIndex = 6;
            this.txtTimeOut.Tag = "TIMEOUT";
            this.txtTimeOut.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblSeconds
            // 
            this.lblSeconds.AutoSize = true;
            this.lblSeconds.Enabled = false;
            this.lblSeconds.Location = new System.Drawing.Point(177, 133);
            this.lblSeconds.Name = "lblSeconds";
            this.lblSeconds.Size = new System.Drawing.Size(50, 15);
            this.lblSeconds.TabIndex = 7;
            this.lblSeconds.Text = "seconds";
            // 
            // OptionVideo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblSeconds);
            this.Controls.Add(this.txtTimeOut);
            this.Controls.Add(this.lblTimeOut);
            this.Controls.Add(this.chkShowTitle);
            this.Controls.Add(this.lblResize);
            this.Controls.Add(this.lblOpening);
            this.Controls.Add(this.cmbResize);
            this.Name = "OptionVideo";
            this.Size = new System.Drawing.Size(348, 324);
            this.Controls.SetChildIndex(this.cmbResize, 0);
            this.Controls.SetChildIndex(this.lblOpening, 0);
            this.Controls.SetChildIndex(this.lblResize, 0);
            this.Controls.SetChildIndex(this.chkShowTitle, 0);
            this.Controls.SetChildIndex(this.lblTimeOut, 0);
            this.Controls.SetChildIndex(this.txtTimeOut, 0);
            this.Controls.SetChildIndex(this.lblSeconds, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbResize;
        private System.Windows.Forms.Label lblOpening;
        private System.Windows.Forms.Label lblResize;
        private System.Windows.Forms.CheckBox chkShowTitle;
        private System.Windows.Forms.Label lblTimeOut;
        private System.Windows.Forms.TextBox txtTimeOut;
        private System.Windows.Forms.Label lblSeconds;
    }
}

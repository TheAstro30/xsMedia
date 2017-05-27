namespace xsMedia.Controls.Effects
{
    partial class EffectMarquee
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
            this.trOpacity = new xsTrackBar.TrackBarEx();
            this.lblOpacity = new System.Windows.Forms.Label();
            this.chkVMarqEnable = new System.Windows.Forms.CheckBox();
            this.lblText = new System.Windows.Forms.Label();
            this.txtText = new System.Windows.Forms.TextBox();
            this.lblPosition = new System.Windows.Forms.Label();
            this.cmbPosition = new System.Windows.Forms.ComboBox();
            this.lblColor = new System.Windows.Forms.Label();
            this.cmbColor = new System.Windows.Forms.ComboBox();
            this.lblTimeOut = new System.Windows.Forms.Label();
            this.txtTimeOut = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.trOpacity)).BeginInit();
            this.SuspendLayout();
            // 
            // trOpacity
            // 
            this.trOpacity.BackColor = System.Drawing.Color.Transparent;
            this.trOpacity.Enabled = false;
            this.trOpacity.LargeChange = 1;
            this.trOpacity.Location = new System.Drawing.Point(256, 117);
            this.trOpacity.Maximum = 255;
            this.trOpacity.Name = "trOpacity";
            this.trOpacity.Size = new System.Drawing.Size(137, 45);
            this.trOpacity.TabIndex = 10;
            this.trOpacity.Tag = "OPACITY";
            this.trOpacity.TickFrequency = 64;
            // 
            // lblOpacity
            // 
            this.lblOpacity.AutoSize = true;
            this.lblOpacity.Enabled = false;
            this.lblOpacity.Location = new System.Drawing.Point(202, 120);
            this.lblOpacity.Name = "lblOpacity";
            this.lblOpacity.Size = new System.Drawing.Size(48, 15);
            this.lblOpacity.TabIndex = 9;
            this.lblOpacity.Text = "Opacity";
            // 
            // chkVMarqEnable
            // 
            this.chkVMarqEnable.AutoSize = true;
            this.chkVMarqEnable.Location = new System.Drawing.Point(12, 7);
            this.chkVMarqEnable.Name = "chkVMarqEnable";
            this.chkVMarqEnable.Size = new System.Drawing.Size(124, 19);
            this.chkVMarqEnable.TabIndex = 0;
            this.chkVMarqEnable.Text = "Enable text overlay";
            this.chkVMarqEnable.UseVisualStyleBackColor = true;
            // 
            // lblText
            // 
            this.lblText.AutoSize = true;
            this.lblText.Location = new System.Drawing.Point(9, 44);
            this.lblText.Name = "lblText";
            this.lblText.Size = new System.Drawing.Size(29, 15);
            this.lblText.TabIndex = 1;
            this.lblText.Text = "Text";
            // 
            // txtText
            // 
            this.txtText.Location = new System.Drawing.Point(44, 41);
            this.txtText.Name = "txtText";
            this.txtText.Size = new System.Drawing.Size(378, 23);
            this.txtText.TabIndex = 2;
            this.txtText.Tag = "TEXT";
            // 
            // lblPosition
            // 
            this.lblPosition.AutoSize = true;
            this.lblPosition.Location = new System.Drawing.Point(9, 82);
            this.lblPosition.Name = "lblPosition";
            this.lblPosition.Size = new System.Drawing.Size(50, 15);
            this.lblPosition.TabIndex = 3;
            this.lblPosition.Text = "Position";
            // 
            // cmbPosition
            // 
            this.cmbPosition.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPosition.FormattingEnabled = true;
            this.cmbPosition.Location = new System.Drawing.Point(65, 79);
            this.cmbPosition.Name = "cmbPosition";
            this.cmbPosition.Size = new System.Drawing.Size(121, 23);
            this.cmbPosition.TabIndex = 4;
            this.cmbPosition.Tag = "POSITION";
            // 
            // lblColor
            // 
            this.lblColor.AutoSize = true;
            this.lblColor.Location = new System.Drawing.Point(202, 82);
            this.lblColor.Name = "lblColor";
            this.lblColor.Size = new System.Drawing.Size(36, 15);
            this.lblColor.TabIndex = 5;
            this.lblColor.Text = "Color";
            // 
            // cmbColor
            // 
            this.cmbColor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbColor.FormattingEnabled = true;
            this.cmbColor.Location = new System.Drawing.Point(244, 79);
            this.cmbColor.Name = "cmbColor";
            this.cmbColor.Size = new System.Drawing.Size(121, 23);
            this.cmbColor.TabIndex = 6;
            this.cmbColor.Tag = "COLOR";
            // 
            // lblTimeOut
            // 
            this.lblTimeOut.AutoSize = true;
            this.lblTimeOut.Location = new System.Drawing.Point(9, 120);
            this.lblTimeOut.Name = "lblTimeOut";
            this.lblTimeOut.Size = new System.Drawing.Size(85, 15);
            this.lblTimeOut.TabIndex = 7;
            this.lblTimeOut.Text = "Timeout (secs)";
            // 
            // txtTimeOut
            // 
            this.txtTimeOut.Location = new System.Drawing.Point(100, 117);
            this.txtTimeOut.MaxLength = 5;
            this.txtTimeOut.Name = "txtTimeOut";
            this.txtTimeOut.Size = new System.Drawing.Size(63, 23);
            this.txtTimeOut.TabIndex = 8;
            this.txtTimeOut.Tag = "TIMEOUT";
            this.txtTimeOut.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // EffectMarquee
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.txtTimeOut);
            this.Controls.Add(this.lblTimeOut);
            this.Controls.Add(this.cmbColor);
            this.Controls.Add(this.lblColor);
            this.Controls.Add(this.cmbPosition);
            this.Controls.Add(this.lblPosition);
            this.Controls.Add(this.txtText);
            this.Controls.Add(this.lblText);
            this.Controls.Add(this.trOpacity);
            this.Controls.Add(this.lblOpacity);
            this.Controls.Add(this.chkVMarqEnable);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "EffectMarquee";
            this.Size = new System.Drawing.Size(431, 199);
            ((System.ComponentModel.ISupportInitialize)(this.trOpacity)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private xsTrackBar.TrackBarEx trOpacity;
        private System.Windows.Forms.Label lblOpacity;
        private System.Windows.Forms.CheckBox chkVMarqEnable;
        private System.Windows.Forms.Label lblText;
        private System.Windows.Forms.TextBox txtText;
        private System.Windows.Forms.Label lblPosition;
        private System.Windows.Forms.ComboBox cmbPosition;
        private System.Windows.Forms.Label lblColor;
        private System.Windows.Forms.ComboBox cmbColor;
        private System.Windows.Forms.Label lblTimeOut;
        private System.Windows.Forms.TextBox txtTimeOut;

    }
}

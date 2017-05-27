using xsCore.Controls;

namespace xsMedia.Controls.Effects
{
    sealed partial class EffectAdjust
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
            this.trSaturation = new xsTrackBar.TrackBarEx();
            this.lblSaturation = new System.Windows.Forms.Label();
            this.trContrast = new xsTrackBar.TrackBarEx();
            this.trBright = new xsTrackBar.TrackBarEx();
            this.trHue = new xsTrackBar.TrackBarEx();
            this.lblContrast = new System.Windows.Forms.Label();
            this.lblBright = new System.Windows.Forms.Label();
            this.lblHue = new System.Windows.Forms.Label();
            this.trGamma = new xsTrackBar.TrackBarEx();
            this.lblGamma = new System.Windows.Forms.Label();
            this.chkVAdjEnable = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.trSaturation)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trContrast)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trBright)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trHue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trGamma)).BeginInit();
            this.SuspendLayout();
            // 
            // trSaturation
            // 
            this.trSaturation.BackColor = System.Drawing.Color.Transparent;
            this.trSaturation.Enabled = false;
            this.trSaturation.LargeChange = 1;
            this.trSaturation.Location = new System.Drawing.Point(285, 41);
            this.trSaturation.Maximum = 500;
            this.trSaturation.Name = "trSaturation";
            this.trSaturation.Size = new System.Drawing.Size(137, 42);
            this.trSaturation.TabIndex = 8;
            this.trSaturation.Tag = "SATURATION";
            this.trSaturation.TickFrequency = 100;
            // 
            // lblSaturation
            // 
            this.lblSaturation.AutoSize = true;
            this.lblSaturation.Enabled = false;
            this.lblSaturation.Location = new System.Drawing.Point(218, 43);
            this.lblSaturation.Name = "lblSaturation";
            this.lblSaturation.Size = new System.Drawing.Size(61, 15);
            this.lblSaturation.TabIndex = 7;
            this.lblSaturation.Text = "Saturation";
            // 
            // trContrast
            // 
            this.trContrast.BackColor = System.Drawing.Color.Transparent;
            this.trContrast.Enabled = false;
            this.trContrast.LargeChange = 1;
            this.trContrast.Location = new System.Drawing.Point(75, 143);
            this.trContrast.Maximum = 200;
            this.trContrast.Name = "trContrast";
            this.trContrast.Size = new System.Drawing.Size(137, 42);
            this.trContrast.TabIndex = 6;
            this.trContrast.Tag = "CONTRAST";
            this.trContrast.TickFrequency = 100;
            // 
            // trBright
            // 
            this.trBright.BackColor = System.Drawing.Color.Transparent;
            this.trBright.Enabled = false;
            this.trBright.LargeChange = 1;
            this.trBright.Location = new System.Drawing.Point(75, 92);
            this.trBright.Maximum = 200;
            this.trBright.Name = "trBright";
            this.trBright.Size = new System.Drawing.Size(137, 42);
            this.trBright.TabIndex = 4;
            this.trBright.Tag = "BRIGHT";
            this.trBright.TickFrequency = 100;
            // 
            // trHue
            // 
            this.trHue.BackColor = System.Drawing.Color.Transparent;
            this.trHue.Enabled = false;
            this.trHue.LargeChange = 1;
            this.trHue.Location = new System.Drawing.Point(75, 41);
            this.trHue.Maximum = 255;
            this.trHue.Name = "trHue";
            this.trHue.Size = new System.Drawing.Size(137, 42);
            this.trHue.TabIndex = 2;
            this.trHue.Tag = "HUE";
            this.trHue.TickFrequency = 65;
            // 
            // lblContrast
            // 
            this.lblContrast.AutoSize = true;
            this.lblContrast.Enabled = false;
            this.lblContrast.Location = new System.Drawing.Point(9, 145);
            this.lblContrast.Name = "lblContrast";
            this.lblContrast.Size = new System.Drawing.Size(52, 15);
            this.lblContrast.TabIndex = 5;
            this.lblContrast.Text = "Contrast";
            // 
            // lblBright
            // 
            this.lblBright.AutoSize = true;
            this.lblBright.Enabled = false;
            this.lblBright.Location = new System.Drawing.Point(9, 94);
            this.lblBright.Name = "lblBright";
            this.lblBright.Size = new System.Drawing.Size(62, 15);
            this.lblBright.TabIndex = 3;
            this.lblBright.Text = "Brightness";
            // 
            // lblHue
            // 
            this.lblHue.AutoSize = true;
            this.lblHue.Enabled = false;
            this.lblHue.Location = new System.Drawing.Point(9, 43);
            this.lblHue.Name = "lblHue";
            this.lblHue.Size = new System.Drawing.Size(29, 15);
            this.lblHue.TabIndex = 1;
            this.lblHue.Text = "Hue";
            // 
            // trGamma
            // 
            this.trGamma.BackColor = System.Drawing.Color.Transparent;
            this.trGamma.Enabled = false;
            this.trGamma.LargeChange = 1;
            this.trGamma.Location = new System.Drawing.Point(284, 92);
            this.trGamma.Maximum = 500;
            this.trGamma.Name = "trGamma";
            this.trGamma.Size = new System.Drawing.Size(137, 42);
            this.trGamma.TabIndex = 10;
            this.trGamma.Tag = "BRIGHT";
            this.trGamma.TickFrequency = 100;
            // 
            // lblGamma
            // 
            this.lblGamma.AutoSize = true;
            this.lblGamma.Enabled = false;
            this.lblGamma.Location = new System.Drawing.Point(218, 94);
            this.lblGamma.Name = "lblGamma";
            this.lblGamma.Size = new System.Drawing.Size(49, 15);
            this.lblGamma.TabIndex = 9;
            this.lblGamma.Text = "Gamma";
            // 
            // chkVAdjEnable
            // 
            this.chkVAdjEnable.AutoSize = true;
            this.chkVAdjEnable.Location = new System.Drawing.Point(12, 7);
            this.chkVAdjEnable.Name = "chkVAdjEnable";
            this.chkVAdjEnable.Size = new System.Drawing.Size(132, 19);
            this.chkVAdjEnable.TabIndex = 0;
            this.chkVAdjEnable.Text = "Enable image adjust";
            this.chkVAdjEnable.UseVisualStyleBackColor = true;
            // 
            // EffectAdjust
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.chkVAdjEnable);
            this.Controls.Add(this.trGamma);
            this.Controls.Add(this.lblGamma);
            this.Controls.Add(this.trSaturation);
            this.Controls.Add(this.lblSaturation);
            this.Controls.Add(this.trContrast);
            this.Controls.Add(this.trBright);
            this.Controls.Add(this.trHue);
            this.Controls.Add(this.lblContrast);
            this.Controls.Add(this.lblBright);
            this.Controls.Add(this.lblHue);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "EffectAdjust";
            this.Size = new System.Drawing.Size(431, 199);
            ((System.ComponentModel.ISupportInitialize)(this.trSaturation)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trContrast)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trBright)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trHue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trGamma)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private xsTrackBar.TrackBarEx trSaturation;
        private System.Windows.Forms.Label lblSaturation;
        private xsTrackBar.TrackBarEx trContrast;
        private xsTrackBar.TrackBarEx trBright;
        private xsTrackBar.TrackBarEx trHue;
        private System.Windows.Forms.Label lblContrast;
        private System.Windows.Forms.Label lblBright;
        private System.Windows.Forms.Label lblHue;
        private xsTrackBar.TrackBarEx trGamma;
        private System.Windows.Forms.Label lblGamma;
        private System.Windows.Forms.CheckBox chkVAdjEnable;

    }
}

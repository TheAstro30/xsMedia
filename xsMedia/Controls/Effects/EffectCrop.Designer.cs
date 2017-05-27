namespace xsMedia.Controls.Effects
{
    partial class EffectCrop
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
            this.chkVCropEnable = new System.Windows.Forms.CheckBox();
            this.lblPx1 = new System.Windows.Forms.Label();
            this.noTop = new System.Windows.Forms.NumericUpDown();
            this.lblTop = new System.Windows.Forms.Label();
            this.lblPx2 = new System.Windows.Forms.Label();
            this.noLeft = new System.Windows.Forms.NumericUpDown();
            this.lblLeft = new System.Windows.Forms.Label();
            this.lblPx3 = new System.Windows.Forms.Label();
            this.noBottom = new System.Windows.Forms.NumericUpDown();
            this.lblBottom = new System.Windows.Forms.Label();
            this.lblPx4 = new System.Windows.Forms.Label();
            this.noRight = new System.Windows.Forms.NumericUpDown();
            this.lblRight = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.noTop)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.noLeft)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.noBottom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.noRight)).BeginInit();
            this.SuspendLayout();
            // 
            // chkVCropEnable
            // 
            this.chkVCropEnable.AutoSize = true;
            this.chkVCropEnable.Location = new System.Drawing.Point(12, 7);
            this.chkVCropEnable.Name = "chkVCropEnable";
            this.chkVCropEnable.Size = new System.Drawing.Size(144, 19);
            this.chkVCropEnable.TabIndex = 0;
            this.chkVCropEnable.Text = "Enable video cropping";
            this.chkVCropEnable.UseVisualStyleBackColor = true;
            // 
            // lblPx1
            // 
            this.lblPx1.AutoSize = true;
            this.lblPx1.Location = new System.Drawing.Point(243, 42);
            this.lblPx1.Name = "lblPx1";
            this.lblPx1.Size = new System.Drawing.Size(36, 15);
            this.lblPx1.TabIndex = 17;
            this.lblPx1.Text = "pixels";
            // 
            // noTop
            // 
            this.noTop.Location = new System.Drawing.Point(185, 40);
            this.noTop.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.noTop.Name = "noTop";
            this.noTop.Size = new System.Drawing.Size(52, 23);
            this.noTop.TabIndex = 16;
            this.noTop.Tag = "TOP";
            // 
            // lblTop
            // 
            this.lblTop.AutoSize = true;
            this.lblTop.Location = new System.Drawing.Point(151, 42);
            this.lblTop.Name = "lblTop";
            this.lblTop.Size = new System.Drawing.Size(28, 15);
            this.lblTop.TabIndex = 15;
            this.lblTop.Text = "Top";
            // 
            // lblPx2
            // 
            this.lblPx2.AutoSize = true;
            this.lblPx2.Location = new System.Drawing.Point(134, 93);
            this.lblPx2.Name = "lblPx2";
            this.lblPx2.Size = new System.Drawing.Size(36, 15);
            this.lblPx2.TabIndex = 20;
            this.lblPx2.Text = "pixels";
            // 
            // noLeft
            // 
            this.noLeft.Location = new System.Drawing.Point(76, 91);
            this.noLeft.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.noLeft.Name = "noLeft";
            this.noLeft.Size = new System.Drawing.Size(52, 23);
            this.noLeft.TabIndex = 19;
            this.noLeft.Tag = "LEFT";
            // 
            // lblLeft
            // 
            this.lblLeft.AutoSize = true;
            this.lblLeft.Location = new System.Drawing.Point(43, 93);
            this.lblLeft.Name = "lblLeft";
            this.lblLeft.Size = new System.Drawing.Size(27, 15);
            this.lblLeft.TabIndex = 18;
            this.lblLeft.Text = "Left";
            // 
            // lblPx3
            // 
            this.lblPx3.AutoSize = true;
            this.lblPx3.Location = new System.Drawing.Point(243, 154);
            this.lblPx3.Name = "lblPx3";
            this.lblPx3.Size = new System.Drawing.Size(36, 15);
            this.lblPx3.TabIndex = 23;
            this.lblPx3.Text = "pixels";
            // 
            // noBottom
            // 
            this.noBottom.Location = new System.Drawing.Point(185, 152);
            this.noBottom.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.noBottom.Name = "noBottom";
            this.noBottom.Size = new System.Drawing.Size(52, 23);
            this.noBottom.TabIndex = 22;
            this.noBottom.Tag = "BOTTOM";
            // 
            // lblBottom
            // 
            this.lblBottom.AutoSize = true;
            this.lblBottom.Location = new System.Drawing.Point(132, 154);
            this.lblBottom.Name = "lblBottom";
            this.lblBottom.Size = new System.Drawing.Size(47, 15);
            this.lblBottom.TabIndex = 21;
            this.lblBottom.Text = "Bottom";
            // 
            // lblPx4
            // 
            this.lblPx4.AutoSize = true;
            this.lblPx4.Location = new System.Drawing.Point(363, 93);
            this.lblPx4.Name = "lblPx4";
            this.lblPx4.Size = new System.Drawing.Size(36, 15);
            this.lblPx4.TabIndex = 26;
            this.lblPx4.Text = "pixels";
            // 
            // noRight
            // 
            this.noRight.Location = new System.Drawing.Point(305, 91);
            this.noRight.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.noRight.Name = "noRight";
            this.noRight.Size = new System.Drawing.Size(52, 23);
            this.noRight.TabIndex = 25;
            this.noRight.Tag = "RIGHT";
            // 
            // lblRight
            // 
            this.lblRight.AutoSize = true;
            this.lblRight.Location = new System.Drawing.Point(264, 93);
            this.lblRight.Name = "lblRight";
            this.lblRight.Size = new System.Drawing.Size(35, 15);
            this.lblRight.TabIndex = 24;
            this.lblRight.Text = "Right";
            // 
            // EffectCrop
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.lblPx4);
            this.Controls.Add(this.noRight);
            this.Controls.Add(this.lblRight);
            this.Controls.Add(this.lblPx3);
            this.Controls.Add(this.noBottom);
            this.Controls.Add(this.lblBottom);
            this.Controls.Add(this.lblPx2);
            this.Controls.Add(this.noLeft);
            this.Controls.Add(this.lblLeft);
            this.Controls.Add(this.lblPx1);
            this.Controls.Add(this.noTop);
            this.Controls.Add(this.lblTop);
            this.Controls.Add(this.chkVCropEnable);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "EffectCrop";
            this.Size = new System.Drawing.Size(431, 199);
            ((System.ComponentModel.ISupportInitialize)(this.noTop)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.noLeft)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.noBottom)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.noRight)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox chkVCropEnable;
        private System.Windows.Forms.Label lblPx1;
        private System.Windows.Forms.NumericUpDown noTop;
        private System.Windows.Forms.Label lblTop;
        private System.Windows.Forms.Label lblPx2;
        private System.Windows.Forms.NumericUpDown noLeft;
        private System.Windows.Forms.Label lblLeft;
        private System.Windows.Forms.Label lblPx3;
        private System.Windows.Forms.NumericUpDown noBottom;
        private System.Windows.Forms.Label lblBottom;
        private System.Windows.Forms.Label lblPx4;
        private System.Windows.Forms.NumericUpDown noRight;
        private System.Windows.Forms.Label lblRight;

    }
}

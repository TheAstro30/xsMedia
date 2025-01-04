using xsCore.Controls.TrackBar;

namespace xsMedia.Controls.Effects
{
    partial class EffectLogo
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
            this.lblOpacity = new System.Windows.Forms.Label();
            this.chkVLogoEnable = new System.Windows.Forms.CheckBox();
            this.lblFile = new System.Windows.Forms.Label();
            this.txtFile = new System.Windows.Forms.TextBox();
            this.btnAdd = new System.Windows.Forms.Button();
            this.lblTop = new System.Windows.Forms.Label();
            this.noTop = new System.Windows.Forms.NumericUpDown();
            this.lblPx1 = new System.Windows.Forms.Label();
            this.lblPx2 = new System.Windows.Forms.Label();
            this.noLeft = new System.Windows.Forms.NumericUpDown();
            this.lblLeft = new System.Windows.Forms.Label();
            this.btnRemove = new System.Windows.Forms.Button();
            this.trOpacity = new xsCore.Controls.TrackBar.TrackBarEx();
            ((System.ComponentModel.ISupportInitialize)(this.noTop)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.noLeft)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trOpacity)).BeginInit();
            this.SuspendLayout();
            // 
            // lblOpacity
            // 
            this.lblOpacity.AutoSize = true;
            this.lblOpacity.Enabled = false;
            this.lblOpacity.Location = new System.Drawing.Point(9, 127);
            this.lblOpacity.Name = "lblOpacity";
            this.lblOpacity.Size = new System.Drawing.Size(48, 15);
            this.lblOpacity.TabIndex = 9;
            this.lblOpacity.Text = "Opacity";
            // 
            // chkVLogoEnable
            // 
            this.chkVLogoEnable.AutoSize = true;
            this.chkVLogoEnable.Location = new System.Drawing.Point(12, 7);
            this.chkVLogoEnable.Name = "chkVLogoEnable";
            this.chkVLogoEnable.Size = new System.Drawing.Size(129, 19);
            this.chkVLogoEnable.TabIndex = 0;
            this.chkVLogoEnable.Text = "Enable logo overlay";
            this.chkVLogoEnable.UseVisualStyleBackColor = true;
            // 
            // lblFile
            // 
            this.lblFile.AutoSize = true;
            this.lblFile.Location = new System.Drawing.Point(9, 44);
            this.lblFile.Name = "lblFile";
            this.lblFile.Size = new System.Drawing.Size(25, 15);
            this.lblFile.TabIndex = 1;
            this.lblFile.Text = "File";
            // 
            // txtFile
            // 
            this.txtFile.Location = new System.Drawing.Point(44, 41);
            this.txtFile.Name = "txtFile";
            this.txtFile.ReadOnly = true;
            this.txtFile.Size = new System.Drawing.Size(316, 23);
            this.txtFile.TabIndex = 2;
            this.txtFile.Tag = "TEXT";
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(366, 41);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(23, 23);
            this.btnAdd.TabIndex = 3;
            this.btnAdd.Tag = "ADD";
            this.btnAdd.UseVisualStyleBackColor = true;
            // 
            // lblTop
            // 
            this.lblTop.AutoSize = true;
            this.lblTop.Location = new System.Drawing.Point(9, 83);
            this.lblTop.Name = "lblTop";
            this.lblTop.Size = new System.Drawing.Size(59, 15);
            this.lblTop.TabIndex = 11;
            this.lblTop.Text = "Top offset";
            // 
            // noTop
            // 
            this.noTop.Location = new System.Drawing.Point(76, 81);
            this.noTop.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.noTop.Name = "noTop";
            this.noTop.Size = new System.Drawing.Size(52, 23);
            this.noTop.TabIndex = 13;
            this.noTop.Tag = "TOP";
            // 
            // lblPx1
            // 
            this.lblPx1.AutoSize = true;
            this.lblPx1.Location = new System.Drawing.Point(134, 83);
            this.lblPx1.Name = "lblPx1";
            this.lblPx1.Size = new System.Drawing.Size(37, 15);
            this.lblPx1.TabIndex = 14;
            this.lblPx1.Text = "pixels";
            // 
            // lblPx2
            // 
            this.lblPx2.AutoSize = true;
            this.lblPx2.Location = new System.Drawing.Point(302, 83);
            this.lblPx2.Name = "lblPx2";
            this.lblPx2.Size = new System.Drawing.Size(37, 15);
            this.lblPx2.TabIndex = 17;
            this.lblPx2.Text = "pixels";
            // 
            // noLeft
            // 
            this.noLeft.Location = new System.Drawing.Point(244, 81);
            this.noLeft.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.noLeft.Name = "noLeft";
            this.noLeft.Size = new System.Drawing.Size(52, 23);
            this.noLeft.TabIndex = 16;
            this.noLeft.Tag = "LEFT";
            // 
            // lblLeft
            // 
            this.lblLeft.AutoSize = true;
            this.lblLeft.Location = new System.Drawing.Point(178, 83);
            this.lblLeft.Name = "lblLeft";
            this.lblLeft.Size = new System.Drawing.Size(60, 15);
            this.lblLeft.TabIndex = 15;
            this.lblLeft.Text = "Left offset";
            // 
            // btnRemove
            // 
            this.btnRemove.Location = new System.Drawing.Point(395, 41);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(23, 23);
            this.btnRemove.TabIndex = 18;
            this.btnRemove.Tag = "REMOVE";
            this.btnRemove.UseVisualStyleBackColor = true;
            // 
            // trOpacity
            // 
            this.trOpacity.BackColor = System.Drawing.Color.Transparent;
            this.trOpacity.Enabled = false;
            this.trOpacity.LargeChange = 1;
            this.trOpacity.Location = new System.Drawing.Point(63, 124);
            this.trOpacity.Maximum = 255;
            this.trOpacity.Name = "trOpacity";
            this.trOpacity.Size = new System.Drawing.Size(137, 45);
            this.trOpacity.TabIndex = 10;
            this.trOpacity.Tag = "OPACITY";
            this.trOpacity.TickFrequency = 64;
            // 
            // EffectLogo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.btnRemove);
            this.Controls.Add(this.lblPx2);
            this.Controls.Add(this.noLeft);
            this.Controls.Add(this.lblLeft);
            this.Controls.Add(this.lblPx1);
            this.Controls.Add(this.noTop);
            this.Controls.Add(this.lblTop);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.txtFile);
            this.Controls.Add(this.lblFile);
            this.Controls.Add(this.trOpacity);
            this.Controls.Add(this.lblOpacity);
            this.Controls.Add(this.chkVLogoEnable);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "EffectLogo";
            this.Size = new System.Drawing.Size(431, 199);
            ((System.ComponentModel.ISupportInitialize)(this.noTop)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.noLeft)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trOpacity)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TrackBarEx trOpacity;
        private System.Windows.Forms.Label lblOpacity;
        private System.Windows.Forms.CheckBox chkVLogoEnable;
        private System.Windows.Forms.Label lblFile;
        private System.Windows.Forms.TextBox txtFile;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Label lblTop;
        private System.Windows.Forms.NumericUpDown noTop;
        private System.Windows.Forms.Label lblPx1;
        private System.Windows.Forms.Label lblPx2;
        private System.Windows.Forms.NumericUpDown noLeft;
        private System.Windows.Forms.Label lblLeft;
        private System.Windows.Forms.Button btnRemove;

    }
}

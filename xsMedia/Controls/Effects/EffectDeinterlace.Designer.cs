namespace xsMedia.Controls.Effects
{
    partial class EffectDeinterlace
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
            this.chkVDeintEnable = new System.Windows.Forms.CheckBox();
            this.lblMode = new System.Windows.Forms.Label();
            this.cmbMode = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // chkVDeintEnable
            // 
            this.chkVDeintEnable.AutoSize = true;
            this.chkVDeintEnable.Location = new System.Drawing.Point(12, 7);
            this.chkVDeintEnable.Name = "chkVDeintEnable";
            this.chkVDeintEnable.Size = new System.Drawing.Size(154, 19);
            this.chkVDeintEnable.TabIndex = 0;
            this.chkVDeintEnable.Text = "Enable video deinterlace";
            this.chkVDeintEnable.UseVisualStyleBackColor = true;
            // 
            // lblMode
            // 
            this.lblMode.AutoSize = true;
            this.lblMode.Location = new System.Drawing.Point(9, 43);
            this.lblMode.Name = "lblMode";
            this.lblMode.Size = new System.Drawing.Size(38, 15);
            this.lblMode.TabIndex = 3;
            this.lblMode.Text = "Mode";
            // 
            // cmbMode
            // 
            this.cmbMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMode.FormattingEnabled = true;
            this.cmbMode.Location = new System.Drawing.Point(65, 40);
            this.cmbMode.Name = "cmbMode";
            this.cmbMode.Size = new System.Drawing.Size(121, 23);
            this.cmbMode.TabIndex = 4;
            this.cmbMode.Tag = "MODE";
            // 
            // EffectDeinterlace
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.cmbMode);
            this.Controls.Add(this.lblMode);
            this.Controls.Add(this.chkVDeintEnable);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "EffectDeinterlace";
            this.Size = new System.Drawing.Size(431, 199);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox chkVDeintEnable;
        private System.Windows.Forms.Label lblMode;
        private System.Windows.Forms.ComboBox cmbMode;

    }
}

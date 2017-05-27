namespace xsMedia.Forms
{
    partial class FrmEffects
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
            this.tbMain = new System.Windows.Forms.TabControl();
            this.tbAudio = new System.Windows.Forms.TabPage();
            this.tbAudioEffects = new System.Windows.Forms.TabControl();
            this.tbEqualizer = new System.Windows.Forms.TabPage();
            this.tbVideo = new System.Windows.Forms.TabPage();
            this.tbVideoEffects = new System.Windows.Forms.TabControl();
            this.tbVAdj = new System.Windows.Forms.TabPage();
            this.tbVMarq = new System.Windows.Forms.TabPage();
            this.tbVLogo = new System.Windows.Forms.TabPage();
            this.tbVCrop = new System.Windows.Forms.TabPage();
            this.tbVDeint = new System.Windows.Forms.TabPage();
            this.tbMain.SuspendLayout();
            this.tbAudio.SuspendLayout();
            this.tbAudioEffects.SuspendLayout();
            this.tbVideo.SuspendLayout();
            this.tbVideoEffects.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(392, 283);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 1;
            this.btnOk.Text = "&Done";
            this.btnOk.UseVisualStyleBackColor = true;
            // 
            // tbMain
            // 
            this.tbMain.Controls.Add(this.tbAudio);
            this.tbMain.Controls.Add(this.tbVideo);
            this.tbMain.Location = new System.Drawing.Point(7, 10);
            this.tbMain.Name = "tbMain";
            this.tbMain.SelectedIndex = 0;
            this.tbMain.Size = new System.Drawing.Size(460, 267);
            this.tbMain.TabIndex = 2;
            // 
            // tbAudio
            // 
            this.tbAudio.Controls.Add(this.tbAudioEffects);
            this.tbAudio.Location = new System.Drawing.Point(4, 24);
            this.tbAudio.Name = "tbAudio";
            this.tbAudio.Padding = new System.Windows.Forms.Padding(3);
            this.tbAudio.Size = new System.Drawing.Size(452, 239);
            this.tbAudio.TabIndex = 0;
            this.tbAudio.Tag = "AUDIO";
            this.tbAudio.Text = "Audio";
            this.tbAudio.UseVisualStyleBackColor = true;
            // 
            // tbAudioEffects
            // 
            this.tbAudioEffects.Controls.Add(this.tbEqualizer);
            this.tbAudioEffects.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbAudioEffects.Location = new System.Drawing.Point(3, 3);
            this.tbAudioEffects.Name = "tbAudioEffects";
            this.tbAudioEffects.SelectedIndex = 0;
            this.tbAudioEffects.Size = new System.Drawing.Size(446, 233);
            this.tbAudioEffects.TabIndex = 0;
            // 
            // tbEqualizer
            // 
            this.tbEqualizer.Location = new System.Drawing.Point(4, 24);
            this.tbEqualizer.Name = "tbEqualizer";
            this.tbEqualizer.Padding = new System.Windows.Forms.Padding(3);
            this.tbEqualizer.Size = new System.Drawing.Size(438, 205);
            this.tbEqualizer.TabIndex = 0;
            this.tbEqualizer.Tag = "EQ";
            this.tbEqualizer.Text = "Equalizer";
            this.tbEqualizer.UseVisualStyleBackColor = true;
            // 
            // tbVideo
            // 
            this.tbVideo.Controls.Add(this.tbVideoEffects);
            this.tbVideo.Location = new System.Drawing.Point(4, 24);
            this.tbVideo.Name = "tbVideo";
            this.tbVideo.Padding = new System.Windows.Forms.Padding(3);
            this.tbVideo.Size = new System.Drawing.Size(452, 239);
            this.tbVideo.TabIndex = 1;
            this.tbVideo.Tag = "VIDEO";
            this.tbVideo.Text = "Video";
            this.tbVideo.UseVisualStyleBackColor = true;
            // 
            // tbVideoEffects
            // 
            this.tbVideoEffects.Controls.Add(this.tbVAdj);
            this.tbVideoEffects.Controls.Add(this.tbVMarq);
            this.tbVideoEffects.Controls.Add(this.tbVLogo);
            this.tbVideoEffects.Controls.Add(this.tbVCrop);
            this.tbVideoEffects.Controls.Add(this.tbVDeint);
            this.tbVideoEffects.Location = new System.Drawing.Point(6, 6);
            this.tbVideoEffects.Name = "tbVideoEffects";
            this.tbVideoEffects.SelectedIndex = 0;
            this.tbVideoEffects.Size = new System.Drawing.Size(439, 227);
            this.tbVideoEffects.TabIndex = 0;
            // 
            // tbVAdj
            // 
            this.tbVAdj.Location = new System.Drawing.Point(4, 24);
            this.tbVAdj.Name = "tbVAdj";
            this.tbVAdj.Padding = new System.Windows.Forms.Padding(3);
            this.tbVAdj.Size = new System.Drawing.Size(431, 199);
            this.tbVAdj.TabIndex = 0;
            this.tbVAdj.Tag = "ADJUST";
            this.tbVAdj.Text = "Adjust";
            this.tbVAdj.UseVisualStyleBackColor = true;
            // 
            // tbVMarq
            // 
            this.tbVMarq.Location = new System.Drawing.Point(4, 24);
            this.tbVMarq.Name = "tbVMarq";
            this.tbVMarq.Padding = new System.Windows.Forms.Padding(3);
            this.tbVMarq.Size = new System.Drawing.Size(431, 199);
            this.tbVMarq.TabIndex = 1;
            this.tbVMarq.Tag = "MARQUEE";
            this.tbVMarq.Text = "Marquee";
            this.tbVMarq.UseVisualStyleBackColor = true;
            // 
            // tbVLogo
            // 
            this.tbVLogo.Location = new System.Drawing.Point(4, 24);
            this.tbVLogo.Name = "tbVLogo";
            this.tbVLogo.Size = new System.Drawing.Size(431, 199);
            this.tbVLogo.TabIndex = 2;
            this.tbVLogo.Tag = "LOGO";
            this.tbVLogo.Text = "Logo";
            this.tbVLogo.UseVisualStyleBackColor = true;
            // 
            // tbVCrop
            // 
            this.tbVCrop.Location = new System.Drawing.Point(4, 24);
            this.tbVCrop.Name = "tbVCrop";
            this.tbVCrop.Size = new System.Drawing.Size(431, 199);
            this.tbVCrop.TabIndex = 3;
            this.tbVCrop.Tag = "CROP";
            this.tbVCrop.Text = "Crop";
            this.tbVCrop.UseVisualStyleBackColor = true;
            // 
            // tbVDeint
            // 
            this.tbVDeint.Location = new System.Drawing.Point(4, 24);
            this.tbVDeint.Name = "tbVDeint";
            this.tbVDeint.Size = new System.Drawing.Size(431, 199);
            this.tbVDeint.TabIndex = 4;
            this.tbVDeint.Tag = "DEINTERLACE";
            this.tbVDeint.Text = "Deinterlace";
            this.tbVDeint.UseVisualStyleBackColor = true;
            // 
            // FrmEffects
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(472, 315);
            this.Controls.Add(this.tbMain);
            this.Controls.Add(this.btnOk);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmEffects";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Effects";
            this.tbMain.ResumeLayout(false);
            this.tbAudio.ResumeLayout(false);
            this.tbAudioEffects.ResumeLayout(false);
            this.tbVideo.ResumeLayout(false);
            this.tbVideoEffects.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.TabControl tbMain;
        private System.Windows.Forms.TabPage tbAudio;
        private System.Windows.Forms.TabPage tbVideo;
        private System.Windows.Forms.TabControl tbVideoEffects;
        private System.Windows.Forms.TabPage tbVAdj;
        private System.Windows.Forms.TabPage tbVMarq;
        private System.Windows.Forms.TabPage tbVLogo;
        private System.Windows.Forms.TabPage tbVCrop;
        private System.Windows.Forms.TabPage tbVDeint;
        private System.Windows.Forms.TabControl tbAudioEffects;
        private System.Windows.Forms.TabPage tbEqualizer;
    }
}
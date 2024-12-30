namespace xsCore.Controls.Forms
{
    partial class FrmMediaMeta
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
            this.components = new System.ComponentModel.Container();
            this.cmdClose = new System.Windows.Forms.Button();
            this.cmdSave = new System.Windows.Forms.Button();
            this.gbArt = new System.Windows.Forms.GroupBox();
            this.cmdNew = new System.Windows.Forms.Button();
            this.pnlArt = new System.Windows.Forms.Panel();
            this.txtComments = new System.Windows.Forms.TextBox();
            this.lblComments = new System.Windows.Forms.Label();
            this.txtEncoded = new System.Windows.Forms.TextBox();
            this.lblEncoded = new System.Windows.Forms.Label();
            this.lblCopyright = new System.Windows.Forms.Label();
            this.txtCopyright = new System.Windows.Forms.TextBox();
            this.txtLanguage = new System.Windows.Forms.TextBox();
            this.lblLanguage = new System.Windows.Forms.Label();
            this.txtPublisher = new System.Windows.Forms.TextBox();
            this.lblPublisher = new System.Windows.Forms.Label();
            this.txtTrack = new System.Windows.Forms.TextBox();
            this.lblGenre = new System.Windows.Forms.Label();
            this.txtDate = new System.Windows.Forms.TextBox();
            this.tmrMeta = new System.Windows.Forms.Timer(this.components);
            this.lblTrack = new System.Windows.Forms.Label();
            this.txtGenre = new System.Windows.Forms.TextBox();
            this.lblDate = new System.Windows.Forms.Label();
            this.txtAlbum = new System.Windows.Forms.TextBox();
            this.lblAlbum = new System.Windows.Forms.Label();
            this.txtArtist = new System.Windows.Forms.TextBox();
            this.lblArtist = new System.Windows.Forms.Label();
            this.txtTitle = new System.Windows.Forms.TextBox();
            this.lblTitle = new System.Windows.Forms.Label();
            this.gbArt.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmdClose
            // 
            this.cmdClose.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.cmdClose.Location = new System.Drawing.Point(340, 408);
            this.cmdClose.Name = "cmdClose";
            this.cmdClose.Size = new System.Drawing.Size(75, 23);
            this.cmdClose.TabIndex = 13;
            this.cmdClose.Text = "Done";
            this.cmdClose.UseVisualStyleBackColor = true;
            // 
            // cmdSave
            // 
            this.cmdSave.Location = new System.Drawing.Point(259, 408);
            this.cmdSave.Name = "cmdSave";
            this.cmdSave.Size = new System.Drawing.Size(75, 23);
            this.cmdSave.TabIndex = 14;
            this.cmdSave.Text = "Save";
            this.cmdSave.UseVisualStyleBackColor = true;
            this.cmdSave.Visible = false;
            this.cmdSave.Click += new System.EventHandler(this.CmdSaveClick);
            // 
            // gbArt
            // 
            this.gbArt.BackColor = System.Drawing.Color.Transparent;
            this.gbArt.Controls.Add(this.cmdNew);
            this.gbArt.Controls.Add(this.pnlArt);
            this.gbArt.Location = new System.Drawing.Point(309, 233);
            this.gbArt.Name = "gbArt";
            this.gbArt.Size = new System.Drawing.Size(112, 161);
            this.gbArt.TabIndex = 11;
            this.gbArt.TabStop = false;
            this.gbArt.Text = "Cover art:";
            // 
            // cmdNew
            // 
            this.cmdNew.Location = new System.Drawing.Point(31, 128);
            this.cmdNew.Name = "cmdNew";
            this.cmdNew.Size = new System.Drawing.Size(75, 23);
            this.cmdNew.TabIndex = 12;
            this.cmdNew.Text = "New";
            this.cmdNew.UseVisualStyleBackColor = true;
            this.cmdNew.Click += new System.EventHandler(this.CmdNewClick);
            // 
            // pnlArt
            // 
            this.pnlArt.Location = new System.Drawing.Point(6, 22);
            this.pnlArt.Name = "pnlArt";
            this.pnlArt.Size = new System.Drawing.Size(100, 100);
            this.pnlArt.TabIndex = 0;
            // 
            // txtComments
            // 
            this.txtComments.Location = new System.Drawing.Point(15, 336);
            this.txtComments.Multiline = true;
            this.txtComments.Name = "txtComments";
            this.txtComments.Size = new System.Drawing.Size(288, 58);
            this.txtComments.TabIndex = 10;
            // 
            // lblComments
            // 
            this.lblComments.AutoSize = true;
            this.lblComments.BackColor = System.Drawing.Color.Transparent;
            this.lblComments.Location = new System.Drawing.Point(12, 318);
            this.lblComments.Name = "lblComments";
            this.lblComments.Size = new System.Drawing.Size(77, 15);
            this.lblComments.TabIndex = 47;
            this.lblComments.Text = "Comment(s):";
            // 
            // txtEncoded
            // 
            this.txtEncoded.Location = new System.Drawing.Point(15, 292);
            this.txtEncoded.Name = "txtEncoded";
            this.txtEncoded.Size = new System.Drawing.Size(288, 23);
            this.txtEncoded.TabIndex = 9;
            // 
            // lblEncoded
            // 
            this.lblEncoded.AutoSize = true;
            this.lblEncoded.BackColor = System.Drawing.Color.Transparent;
            this.lblEncoded.Location = new System.Drawing.Point(12, 274);
            this.lblEncoded.Name = "lblEncoded";
            this.lblEncoded.Size = new System.Drawing.Size(72, 15);
            this.lblEncoded.TabIndex = 45;
            this.lblEncoded.Text = "Encoded by:";
            // 
            // lblCopyright
            // 
            this.lblCopyright.AutoSize = true;
            this.lblCopyright.BackColor = System.Drawing.Color.Transparent;
            this.lblCopyright.Location = new System.Drawing.Point(12, 230);
            this.lblCopyright.Name = "lblCopyright";
            this.lblCopyright.Size = new System.Drawing.Size(63, 15);
            this.lblCopyright.TabIndex = 43;
            this.lblCopyright.Text = "Copyright:";
            // 
            // txtCopyright
            // 
            this.txtCopyright.Location = new System.Drawing.Point(15, 248);
            this.txtCopyright.Name = "txtCopyright";
            this.txtCopyright.Size = new System.Drawing.Size(288, 23);
            this.txtCopyright.TabIndex = 8;
            // 
            // txtLanguage
            // 
            this.txtLanguage.Location = new System.Drawing.Point(310, 204);
            this.txtLanguage.Name = "txtLanguage";
            this.txtLanguage.Size = new System.Drawing.Size(111, 23);
            this.txtLanguage.TabIndex = 7;
            // 
            // lblLanguage
            // 
            this.lblLanguage.AutoSize = true;
            this.lblLanguage.BackColor = System.Drawing.Color.Transparent;
            this.lblLanguage.Location = new System.Drawing.Point(307, 186);
            this.lblLanguage.Name = "lblLanguage";
            this.lblLanguage.Size = new System.Drawing.Size(62, 15);
            this.lblLanguage.TabIndex = 41;
            this.lblLanguage.Text = "Language:";
            // 
            // txtPublisher
            // 
            this.txtPublisher.Location = new System.Drawing.Point(15, 204);
            this.txtPublisher.Name = "txtPublisher";
            this.txtPublisher.Size = new System.Drawing.Size(288, 23);
            this.txtPublisher.TabIndex = 6;
            // 
            // lblPublisher
            // 
            this.lblPublisher.AutoSize = true;
            this.lblPublisher.BackColor = System.Drawing.Color.Transparent;
            this.lblPublisher.Location = new System.Drawing.Point(12, 186);
            this.lblPublisher.Name = "lblPublisher";
            this.lblPublisher.Size = new System.Drawing.Size(59, 15);
            this.lblPublisher.TabIndex = 39;
            this.lblPublisher.Text = "Publisher:";
            // 
            // txtTrack
            // 
            this.txtTrack.Location = new System.Drawing.Point(366, 160);
            this.txtTrack.Name = "txtTrack";
            this.txtTrack.Size = new System.Drawing.Size(55, 23);
            this.txtTrack.TabIndex = 5;
            this.txtTrack.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // lblGenre
            // 
            this.lblGenre.AutoSize = true;
            this.lblGenre.BackColor = System.Drawing.Color.Transparent;
            this.lblGenre.Location = new System.Drawing.Point(12, 142);
            this.lblGenre.Name = "lblGenre";
            this.lblGenre.Size = new System.Drawing.Size(41, 15);
            this.lblGenre.TabIndex = 35;
            this.lblGenre.Text = "Genre:";
            // 
            // txtDate
            // 
            this.txtDate.Location = new System.Drawing.Point(310, 116);
            this.txtDate.Name = "txtDate";
            this.txtDate.Size = new System.Drawing.Size(111, 23);
            this.txtDate.TabIndex = 3;
            // 
            // tmrMeta
            // 
            this.tmrMeta.Interval = 5000;
            this.tmrMeta.Tick += new System.EventHandler(this.TmrMetaTick);
            // 
            // lblTrack
            // 
            this.lblTrack.AutoSize = true;
            this.lblTrack.BackColor = System.Drawing.Color.Transparent;
            this.lblTrack.Location = new System.Drawing.Point(363, 142);
            this.lblTrack.Name = "lblTrack";
            this.lblTrack.Size = new System.Drawing.Size(49, 15);
            this.lblTrack.TabIndex = 37;
            this.lblTrack.Text = "Track #:";
            // 
            // txtGenre
            // 
            this.txtGenre.Location = new System.Drawing.Point(15, 160);
            this.txtGenre.Name = "txtGenre";
            this.txtGenre.Size = new System.Drawing.Size(345, 23);
            this.txtGenre.TabIndex = 4;
            // 
            // lblDate
            // 
            this.lblDate.AutoSize = true;
            this.lblDate.BackColor = System.Drawing.Color.Transparent;
            this.lblDate.Location = new System.Drawing.Point(307, 98);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(76, 15);
            this.lblDate.TabIndex = 33;
            this.lblDate.Text = "Release Date:";
            // 
            // txtAlbum
            // 
            this.txtAlbum.Location = new System.Drawing.Point(15, 116);
            this.txtAlbum.Name = "txtAlbum";
            this.txtAlbum.Size = new System.Drawing.Size(288, 23);
            this.txtAlbum.TabIndex = 2;
            // 
            // lblAlbum
            // 
            this.lblAlbum.AutoSize = true;
            this.lblAlbum.BackColor = System.Drawing.Color.Transparent;
            this.lblAlbum.Location = new System.Drawing.Point(12, 98);
            this.lblAlbum.Name = "lblAlbum";
            this.lblAlbum.Size = new System.Drawing.Size(46, 15);
            this.lblAlbum.TabIndex = 31;
            this.lblAlbum.Text = "Album:";
            // 
            // txtArtist
            // 
            this.txtArtist.Location = new System.Drawing.Point(15, 72);
            this.txtArtist.Name = "txtArtist";
            this.txtArtist.Size = new System.Drawing.Size(406, 23);
            this.txtArtist.TabIndex = 1;
            // 
            // lblArtist
            // 
            this.lblArtist.AutoSize = true;
            this.lblArtist.BackColor = System.Drawing.Color.Transparent;
            this.lblArtist.Location = new System.Drawing.Point(12, 54);
            this.lblArtist.Name = "lblArtist";
            this.lblArtist.Size = new System.Drawing.Size(38, 15);
            this.lblArtist.TabIndex = 29;
            this.lblArtist.Text = "Artist:";
            // 
            // txtTitle
            // 
            this.txtTitle.Location = new System.Drawing.Point(15, 28);
            this.txtTitle.Name = "txtTitle";
            this.txtTitle.Size = new System.Drawing.Size(406, 23);
            this.txtTitle.TabIndex = 0;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblTitle.Location = new System.Drawing.Point(12, 9);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(33, 15);
            this.lblTitle.TabIndex = 27;
            this.lblTitle.Text = "Title:";
            // 
            // FrmMediaMeta
            // 
            this.AcceptButton = this.cmdClose;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(434, 440);
            this.Controls.Add(this.cmdClose);
            this.Controls.Add(this.cmdSave);
            this.Controls.Add(this.gbArt);
            this.Controls.Add(this.txtComments);
            this.Controls.Add(this.lblComments);
            this.Controls.Add(this.txtEncoded);
            this.Controls.Add(this.lblEncoded);
            this.Controls.Add(this.lblCopyright);
            this.Controls.Add(this.txtCopyright);
            this.Controls.Add(this.txtLanguage);
            this.Controls.Add(this.lblLanguage);
            this.Controls.Add(this.txtPublisher);
            this.Controls.Add(this.lblPublisher);
            this.Controls.Add(this.txtTrack);
            this.Controls.Add(this.lblGenre);
            this.Controls.Add(this.txtDate);
            this.Controls.Add(this.lblTrack);
            this.Controls.Add(this.txtGenre);
            this.Controls.Add(this.lblDate);
            this.Controls.Add(this.txtAlbum);
            this.Controls.Add(this.lblAlbum);
            this.Controls.Add(this.txtArtist);
            this.Controls.Add(this.lblArtist);
            this.Controls.Add(this.txtTitle);
            this.Controls.Add(this.lblTitle);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmMediaMeta";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Media Info:";
            this.gbArt.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button cmdClose;
        private System.Windows.Forms.Button cmdSave;
        private System.Windows.Forms.GroupBox gbArt;
        private System.Windows.Forms.Button cmdNew;
        private System.Windows.Forms.Panel pnlArt;
        private System.Windows.Forms.TextBox txtComments;
        private System.Windows.Forms.Label lblComments;
        private System.Windows.Forms.TextBox txtEncoded;
        private System.Windows.Forms.Label lblEncoded;
        private System.Windows.Forms.Label lblCopyright;
        private System.Windows.Forms.TextBox txtCopyright;
        private System.Windows.Forms.TextBox txtLanguage;
        private System.Windows.Forms.Label lblLanguage;
        private System.Windows.Forms.TextBox txtPublisher;
        private System.Windows.Forms.Label lblPublisher;
        private System.Windows.Forms.TextBox txtTrack;
        private System.Windows.Forms.Label lblGenre;
        private System.Windows.Forms.TextBox txtDate;
        private System.Windows.Forms.Timer tmrMeta;
        private System.Windows.Forms.Label lblTrack;
        private System.Windows.Forms.TextBox txtGenre;
        private System.Windows.Forms.Label lblDate;
        private System.Windows.Forms.TextBox txtAlbum;
        private System.Windows.Forms.Label lblAlbum;
        private System.Windows.Forms.TextBox txtArtist;
        private System.Windows.Forms.Label lblArtist;
        private System.Windows.Forms.TextBox txtTitle;
        private System.Windows.Forms.Label lblTitle;
    }
}
namespace xsCore.Settings.Controls
{
    partial class OptionPlayback
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
            this.lblCounter = new System.Windows.Forms.Label();
            this.cmbCounter = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtJump = new System.Windows.Forms.TextBox();
            this.lblJumpSeconds = new System.Windows.Forms.Label();
            this.lblVol = new System.Windows.Forms.Label();
            this.txtVol = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbSpeed = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbLoop = new System.Windows.Forms.ComboBox();
            this.gbMidi = new System.Windows.Forms.GroupBox();
            this.btnRemove = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.txtSoundFont = new System.Windows.Forms.TextBox();
            this.lblSoundFont = new System.Windows.Forms.Label();
            this.gbMidi.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblCounter
            // 
            this.lblCounter.AutoSize = true;
            this.lblCounter.Location = new System.Drawing.Point(11, 45);
            this.lblCounter.Name = "lblCounter";
            this.lblCounter.Size = new System.Drawing.Size(93, 15);
            this.lblCounter.TabIndex = 1;
            this.lblCounter.Text = "Counter display:";
            // 
            // cmbCounter
            // 
            this.cmbCounter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCounter.FormattingEnabled = true;
            this.cmbCounter.Location = new System.Drawing.Point(110, 42);
            this.cmbCounter.Name = "cmbCounter";
            this.cmbCounter.Size = new System.Drawing.Size(146, 23);
            this.cmbCounter.TabIndex = 2;
            this.cmbCounter.Tag = "COUNTER";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 94);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(198, 15);
            this.label1.TabIndex = 3;
            this.label1.Text = "Step forward/backward in media by:";
            // 
            // txtJump
            // 
            this.txtJump.Location = new System.Drawing.Point(218, 91);
            this.txtJump.MaxLength = 2;
            this.txtJump.Name = "txtJump";
            this.txtJump.Size = new System.Drawing.Size(56, 23);
            this.txtJump.TabIndex = 7;
            this.txtJump.Tag = "SECONDS";
            this.txtJump.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblJumpSeconds
            // 
            this.lblJumpSeconds.AutoSize = true;
            this.lblJumpSeconds.Location = new System.Drawing.Point(280, 94);
            this.lblJumpSeconds.Name = "lblJumpSeconds";
            this.lblJumpSeconds.Size = new System.Drawing.Size(50, 15);
            this.lblJumpSeconds.TabIndex = 8;
            this.lblJumpSeconds.Text = "seconds";
            // 
            // lblVol
            // 
            this.lblVol.AutoSize = true;
            this.lblVol.Location = new System.Drawing.Point(11, 123);
            this.lblVol.Name = "lblVol";
            this.lblVol.Size = new System.Drawing.Size(204, 15);
            this.lblVol.TabIndex = 9;
            this.lblVol.Text = "Increase/decrease volume in steps of:";
            // 
            // txtVol
            // 
            this.txtVol.Location = new System.Drawing.Point(218, 120);
            this.txtVol.MaxLength = 2;
            this.txtVol.Name = "txtVol";
            this.txtVol.Size = new System.Drawing.Size(56, 23);
            this.txtVol.TabIndex = 10;
            this.txtVol.Tag = "STEPS";
            this.txtVol.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 170);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(91, 15);
            this.label2.TabIndex = 11;
            this.label2.Text = "Playback speed:";
            // 
            // cmbSpeed
            // 
            this.cmbSpeed.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSpeed.FormattingEnabled = true;
            this.cmbSpeed.Location = new System.Drawing.Point(110, 167);
            this.cmbSpeed.Name = "cmbSpeed";
            this.cmbSpeed.Size = new System.Drawing.Size(95, 23);
            this.cmbSpeed.TabIndex = 12;
            this.cmbSpeed.Tag = "SPEED";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(11, 201);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(87, 15);
            this.label3.TabIndex = 13;
            this.label3.Text = "Media looping:";
            // 
            // cmbLoop
            // 
            this.cmbLoop.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLoop.FormattingEnabled = true;
            this.cmbLoop.Location = new System.Drawing.Point(110, 198);
            this.cmbLoop.Name = "cmbLoop";
            this.cmbLoop.Size = new System.Drawing.Size(95, 23);
            this.cmbLoop.TabIndex = 14;
            this.cmbLoop.Tag = "LOOP";
            // 
            // gbMidi
            // 
            this.gbMidi.Controls.Add(this.btnRemove);
            this.gbMidi.Controls.Add(this.btnAdd);
            this.gbMidi.Controls.Add(this.txtSoundFont);
            this.gbMidi.Controls.Add(this.lblSoundFont);
            this.gbMidi.Location = new System.Drawing.Point(14, 232);
            this.gbMidi.Name = "gbMidi";
            this.gbMidi.Size = new System.Drawing.Size(320, 76);
            this.gbMidi.TabIndex = 15;
            this.gbMidi.TabStop = false;
            this.gbMidi.Text = "MIDI support:";
            // 
            // btnRemove
            // 
            this.btnRemove.Location = new System.Drawing.Point(291, 37);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(23, 23);
            this.btnRemove.TabIndex = 3;
            this.btnRemove.Tag = "REMOVE";
            this.btnRemove.UseVisualStyleBackColor = true;
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(262, 37);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(23, 23);
            this.btnAdd.TabIndex = 2;
            this.btnAdd.Tag = "ADD";
            this.btnAdd.UseVisualStyleBackColor = true;
            // 
            // txtSoundFont
            // 
            this.txtSoundFont.Location = new System.Drawing.Point(9, 37);
            this.txtSoundFont.Name = "txtSoundFont";
            this.txtSoundFont.ReadOnly = true;
            this.txtSoundFont.Size = new System.Drawing.Size(247, 23);
            this.txtSoundFont.TabIndex = 1;
            // 
            // lblSoundFont
            // 
            this.lblSoundFont.AutoSize = true;
            this.lblSoundFont.Location = new System.Drawing.Point(6, 19);
            this.lblSoundFont.Name = "lblSoundFont";
            this.lblSoundFont.Size = new System.Drawing.Size(111, 15);
            this.lblSoundFont.TabIndex = 0;
            this.lblSoundFont.Text = "Current sound font:";
            // 
            // OptionPlayback
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gbMidi);
            this.Controls.Add(this.cmbLoop);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cmbSpeed);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtVol);
            this.Controls.Add(this.lblVol);
            this.Controls.Add(this.lblJumpSeconds);
            this.Controls.Add(this.txtJump);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbCounter);
            this.Controls.Add(this.lblCounter);
            this.Name = "OptionPlayback";
            this.Size = new System.Drawing.Size(348, 324);
            this.Controls.SetChildIndex(this.lblCounter, 0);
            this.Controls.SetChildIndex(this.cmbCounter, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.txtJump, 0);
            this.Controls.SetChildIndex(this.lblJumpSeconds, 0);
            this.Controls.SetChildIndex(this.lblVol, 0);
            this.Controls.SetChildIndex(this.txtVol, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.cmbSpeed, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.cmbLoop, 0);
            this.Controls.SetChildIndex(this.gbMidi, 0);
            this.gbMidi.ResumeLayout(false);
            this.gbMidi.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblCounter;
        private System.Windows.Forms.ComboBox cmbCounter;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtJump;
        private System.Windows.Forms.Label lblJumpSeconds;
        private System.Windows.Forms.Label lblVol;
        private System.Windows.Forms.TextBox txtVol;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbSpeed;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cmbLoop;
        private System.Windows.Forms.GroupBox gbMidi;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.TextBox txtSoundFont;
        private System.Windows.Forms.Label lblSoundFont;


    }
}

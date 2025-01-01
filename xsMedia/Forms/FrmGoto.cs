/* xsMedia - Media Player
 * (c)2013 - 2025
 * Jason James Newland
 * KangaSoft Software, All Rights Reserved
 * Licenced under the GNU public licence */
using System;
using System.Drawing;
using System.Windows.Forms;
using xsCore.Controls.Forms;
using xsMedia.Properties;

namespace xsMedia.Forms
{
    /* This was really soooo hard to write, I might have to go for worker's compensation for injured fingers...
     * Of course, I'm joking. Only took me an hour - now that I realize how VLC's position code works :P.
     * The DateTimePicker control is probably the worst fucking thing to use for this type of implementation,
     * as you cannot use AcceptButton on the form, as the ValueChanged event isn't fired; so we could end up with
     * an incorrect value as the final result. Until I find a better solution, this will have to do. */
    public sealed class FrmGoto : FormEx
    {
        private readonly DateTimePicker _dtTimeCode;
        private readonly Button _btnOk;

        private readonly ToolTip _toolTipProvider = new ToolTip();

        public float MediaCurrentPosition { get; set; }
        public long MediaLength { get; set; }

        /* The main property that gives us the value to scroll playback to */
        public float MediaNewPosition { get; private set; }

        public FrmGoto()
        {
            ClientSize = new Size(206, 83);
            Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            MaximizeBox = false;
            MinimizeBox = false;
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = @"Go to time";

            var lblGoto = new Label
            {
                AutoSize = true,
                BackColor = Color.Transparent,
                Location = new Point(8, 16),
                Size = new Size(102, 15),
                Text = @"Go to this time:"
            };

            _dtTimeCode = new DateTimePicker
            {
                CustomFormat = @"HH:mm:ss",
                Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0),
                Format = DateTimePickerFormat.Custom,
                Location = new Point(97, 12),
                ShowUpDown = true,
                Size = new Size(75, 23),
                TabIndex = 0
            };
           
            var btnReset = new Button
            {
                Location = new Point(177, 12),
                Size = new Size(23, 23),
                TabIndex = 1,
                Image = Resources.dlgReset.ToBitmap(),
                UseVisualStyleBackColor = true
            };

            var toolTip = new ToolTip();
            toolTip.SetToolTip(btnReset, "Reset time");

            _btnOk = new Button
            {
                Location = new Point(44, 51),
                Size = new Size(75, 23),
                TabIndex = 2,
                Text = @"Go",
                UseVisualStyleBackColor = true
            };

            var btnCancel = new Button
            {
                DialogResult = DialogResult.Cancel,
                Location = new Point(125, 51),
                Size = new Size(75, 23),
                TabIndex = 3,
                Text = @"Cancel",
                UseVisualStyleBackColor = true
            };

            Controls.AddRange(new Control[]
            {
                lblGoto, _dtTimeCode, btnReset, _btnOk, btnCancel
            });

            _toolTipProvider.SetToolTip(_dtTimeCode, "Time format in hours, minutes, seconds (HH:MM:SS)");

            _dtTimeCode.KeyPress += OnKeyPress;
            _dtTimeCode.KeyDown += OnKeyDown;
            btnReset.Click += ButtonResetClick;
            _btnOk.Click += ButtonOkClick;
        }

        /* Overrides */
        protected override void OnLoad(EventArgs e)
        {
            /* This DateTimePicker control is such a pain in the ass... */
            var seconds = (int) (MediaCurrentPosition*MediaLength)/1000;
            var ts = TimeSpan.FromSeconds(seconds);
            _dtTimeCode.Value = Convert.ToDateTime(ts.ToString());
            base.OnLoad(e);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            _toolTipProvider.Dispose(); /* Always a good idea */
            base.OnFormClosing(e);
        }

        /* Callbacks */
        private void OnKeyPress(object sender, KeyPressEventArgs e)
        {
            /* Works now, couldn't get this to work last night (for whatever reason). Do not code drunk, lol */
            if (e.KeyChar != 13)
            {
                return;
            }
            /* Because the DateTimePicker doesn't allow return to complete the text field entry; if this form's AcceptButton was set to the Ok button,
                 * the resultant output of MediaNewPosition could be not as expected. Example: if we were entering say 00:01:11, and the 01 entry field
                 * was highlighted, pressing return (AcceptButton fired) would result in MediaNewPosition = 11 seconds, ignoring the extra 60 seconds we're
                 * trying to add. Totally annoying operation of the control, so this is the work-around */
            e.Handled = true;
            _btnOk.Focus();
            ButtonOkClick(_btnOk, new EventArgs());
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Escape)
            {
                return;
            }
            /* Close this dialog with the Escape key */
            e.Handled = true;
            DialogResult = DialogResult.Cancel;
        }

        private void ButtonResetClick(object sender, EventArgs e)
        {
            var ts = TimeSpan.FromSeconds(0);
            _dtTimeCode.Value = Convert.ToDateTime(ts.ToString());
        }

        private void ButtonOkClick(object sender, EventArgs e)
        {
            /* We need to convert the DateTimePicker value to a TimeSpan to get the total in seconds */
            var dt = _dtTimeCode.Value; /* why use a variable? Looks neater */
            var ts = new TimeSpan(dt.Hour, dt.Minute, dt.Second);
            /* Then we need to convert it to a percentage of playback advance from the total length, if the float value is
             * greater than 1, then we've exceeded MediaLength, so DialogResult.Cancel and exit */
            var len = (int) MediaLength/1000; /* I could combine this variable with the next for the math, but it looks neater this way */
            var pos = (float) ts.TotalSeconds/len;
            if (pos > 1)
            {
                /* This is invalid, so we quit */
                DialogResult = DialogResult.Cancel;
                return;
            }
            MediaNewPosition = pos;
            DialogResult = DialogResult.OK;
        }
    }
}

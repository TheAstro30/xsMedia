/* xsMedia - xsSettings
 * (c)2013 - 2024
 * Jason James Newland
 * KangaSoft Software, All Rights Reserved
 * Licenced under the GNU public licence */
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using xsCore.Internal;

namespace xsCore.Settings.Controls.Base
{
    public partial class OptionBase : UserControl
    {
        public OptionBase()
        {
            InitializeComponent();
        }

        public OptionBase(string header)
        {
            InitializeComponent();
            lblHeader.Text = header;
        }

        public PlayerSettings Settings { get; set; }

        protected override void OnPaint(PaintEventArgs e)
        {
            /* Header box */
            using (var b = new SolidBrush(Color.LightGray))
            {
                using (var p = new Pen(b))
                {
                    e.Graphics.DrawRectangle(p, 0, 0, ClientRectangle.Width - 1, 30);
                }
            }
            /* Fill header area with gradient */
            var rect = new Rectangle(1, 1, ClientRectangle.Width - 2, 29);
            using (var b = new LinearGradientBrush(rect, Color.FromArgb(210, 210, 210), Color.FromArgb(240, 240, 240), LinearGradientMode.Horizontal))
            {
                e.Graphics.FillRectangle(b, rect);
            }
            /* Draw a ruled line at bottom */
            using (var b = new SolidBrush(Color.LightGray))
            {
                using (var p = new Pen(b))
                {
                    e.Graphics.DrawLine(p, 0, ClientRectangle.Height - 1, ClientRectangle.Width, ClientRectangle.Height - 1);
                }
            }
            /* Draw controls */
            base.OnPaint(e);
        }
    }
}

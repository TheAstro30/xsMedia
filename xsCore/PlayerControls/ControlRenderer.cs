/* xsMedia - sxCore
 * (c)2013 - 2020
 * Jason James Newland
 * KangaSoft Software, All Rights Reserved
 * Licenced under the GNU public licence */
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using xsCore.Skin;

namespace xsCore.PlayerControls
{
    /*  Player control container class
     *  By Jason James Newland & Ryan Alexander
     *  (C)Copyright 2011
     *  KangaSoft Software - All Rights Reserved
     */
    public class ControlRenderer : UserControl
    {
        private Bitmap _backBufferBitmap;
        private readonly ToolTip _tooltipProvider;

        private const int TooltipOffsetX = 10;
        private const int TooltipOffsetY = 10;

        public event Action<ControlRenderer> OnSkinStyleChanged;

        public ControlRenderer()
        {
            SetStyle(ControlStyles.DoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);           
            PlayerControls = new PlayerControlList(this);
            _tooltipProvider = new ToolTip();            
            _backBufferBitmap = new Bitmap(Width, Height);
        }

        public PlayerControlList PlayerControls { get; set; }
        public PlayerControl CaptureControl { get; set; }
        public PlayerControl FocusedControl { get; set; }
        public PlayerControl HoveredControl { get; set; }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (FocusedControl != null && FocusedControl.Visible && FocusedControl.Enabled) { FocusedControl.KeyEvent(new PlayerKeyEvent(KeyEventType.Down, e)); }
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            if (FocusedControl != null && FocusedControl.Visible && FocusedControl.Enabled) { FocusedControl.KeyEvent(new PlayerKeyEvent(KeyEventType.Up, e)); }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            CaptureControl = PlayerControls.SendMouseEvent(e.Location, MouseEventType.Down, e.Button, e.Delta);
            FocusedControl = CaptureControl;
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (CaptureControl == null) { return; }
            var childArea = TranslatePointToChildSpace(e.Location, CaptureControl);

            if (CaptureControl.Area.Contains(e.Location))
            {
                CaptureControl.MouseEvent(new PlayerMouseEvent(MouseEventType.Click, childArea, e.Button, e.Delta));
            }

            CaptureControl.MouseEvent(new PlayerMouseEvent(MouseEventType.Up, childArea, e.Button, e.Delta));
            CaptureControl = null;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (CaptureControl != null)
            {
                CaptureControl.MouseEvent(new PlayerMouseEvent(MouseEventType.Move, TranslatePointToChildSpace(e.Location, CaptureControl), e.Button, e.Delta));
                return;
            }

            var nowHovering = PlayerControls.SendMouseEvent(e.Location, MouseEventType.Move, e.Button, e.Delta);

            if (HoveredControl == nowHovering) { return; }
            if (HoveredControl != null)
            {
                _tooltipProvider.Hide(this);
                HoveredControl.MouseEvent(new PlayerMouseEvent(MouseEventType.Leave, TranslatePointToChildSpace(e.Location, HoveredControl), e.Button, e.Delta));
            }

            if (nowHovering != null)
            {
                if (!string.IsNullOrEmpty(nowHovering.Tag) && !string.IsNullOrEmpty(nowHovering.TooltipText))
                {
                    /* This stupid thing is so annoying - about as close as I'm going to get it */
                    _tooltipProvider.Hide(this);
                    var p = new Point(e.Location.X, nowHovering.Location.Y);
                    p.Offset(Cursor.Size.Width - TooltipOffsetX, Cursor.Size.Height - TooltipOffsetY);
                    _tooltipProvider.Show(nowHovering.TooltipText, this, p);
                }
                nowHovering.MouseEvent(new PlayerMouseEvent(MouseEventType.Enter, TranslatePointToChildSpace(e.Location, nowHovering), e.Button, e.Delta));
            }
            HoveredControl = nowHovering;
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            _tooltipProvider.Hide(this);
            Cursor = Cursors.Default;
            Refresh();
            /* Pass event back to base class - important */
            base.OnMouseLeave(e);
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            PlayerControls.SendMouseEvent(e.Location, MouseEventType.Wheel, e.Button, e.Delta);
        }

        protected override void OnDoubleClick(EventArgs e)
        {
            var mouseLoc = PointToClient(Cursor.Position);
            PlayerControls.SendMouseEvent(mouseLoc, MouseEventType.DblClick, MouseButtons.None, 0);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            using (var myBb = Graphics.FromImage(_backBufferBitmap))
            {
                myBb.Clip = e.Graphics.Clip;
                /* Calculate positions of images to be drawn in three parts */
                var img = SkinManager.GetResourceById("BAR_LEFT", null);
                if (img == null || img.Image == null)
                {
                    return;
                }
                var r = new Rectangle(0, 0, img.Image.Width, img.Image.Height);
                myBb.DrawImageUnscaled(img.Image, r);
                var x = img.Image.Width;
                img = SkinManager.GetResourceById("BAR_MID", null);
                var rgt = SkinManager.GetResourceById("BAR_RIGHT", null);
                var width = Width - rgt.Image.Width;
                r.Width = img.Image.Width;
                r.X = x;
                /* Loop and fill in the space */
                for (var i = 0; i <= width / img.Image.Width; i++)
                {
                    myBb.DrawImageUnscaled(img.Image, r);
                    r.X += r.Width;
                }
                r.X = width;
                myBb.DrawImageUnscaled(rgt.Image, r);

                /* Draw each control (p => p.Visible && ) */
                foreach (var p in PlayerControls.Where(p => e.ClipRectangle.IntersectsWith(new Rectangle(p.Location, p.Size))))
                {
                    myBb.ResetTransform();
                    myBb.TranslateTransform(p.Location.X, p.Location.Y);
                    p.Paint(myBb);
                }
                /* Draw out "compiled" graphic */
                e.Graphics.DrawImage(_backBufferBitmap, e.ClipRectangle, e.ClipRectangle, GraphicsUnit.Pixel);
            }
        }

        protected override void OnResize(EventArgs e)
        {     
            if (Width <= 0 || Height <= 0)
            {
                return;
            }
            _backBufferBitmap = new Bitmap(Width, Height);
            Refresh();
        }

        public virtual void SkinStyleChanged()
        {
            /* Reset control height */
            var r = SkinManager.GetResourceById("BAR_LEFT", null);
            Height = r.Image != null ? r.Image.Height : 40;
            if (OnSkinStyleChanged != null)
            {
                OnSkinStyleChanged(this);
            }
        }

        public static Point TranslatePointToChildSpace(Point p, PlayerControl c)
        {
            p.Offset(-c.Location.X, -c.Location.Y);
            return p;
        }

        public static Point TranslatePointToParentSpace(Point p, PlayerControl c)
        {
            p.Offset(c.Location.X, c.Location.Y);
            return p;
        }
    }
}


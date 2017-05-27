using System.Drawing;
using System.Windows.Forms;

namespace xsCore.PlayerControls
{
    public enum KeyEventType
    {
        Up = 0,
        Down = 1
    }

    public enum MouseEventType
    {
        Move = 0,
        Up = 1,
        Down = 2,
        Enter = 3,
        Leave = 4,
        Click = 5,
        DblClick = 6,
        Wheel = 7
    }

    public class PlayerKeyEvent
    {
        public KeyEventType EventType { get; set; }
        public KeyEventArgs EventArgs { get; set; }

        public PlayerKeyEvent(KeyEventType t, KeyEventArgs k)
        {
            EventType = t;
            EventArgs = k;
        }
    }

    public class PlayerMouseEvent
    {
        public MouseEventType EventType { get; set; }
        public Point Location { get; set; }
        public MouseButtons Button { get; set; }
        public int Delta { get; set; }

        public PlayerMouseEvent(MouseEventType t, Point p, MouseButtons b, int delta)
        {
            EventType = t;
            Location = p;
            Button = b;
            Delta = delta;
        }
    }

    public abstract class PlayerControl
    {
        private Point _location;
        private Size _size;
        private bool _visible = true;
        private bool _enabled = true;

        public Point Location
        {
            get { return _location; }
            set
            {
                _location = value;
                Refresh();
            }
        }

        public Size Size
        {
            get { return _size; }
            set
            {
                var old = _size;
                _size = value;
                Resized(old);
                Refresh();
            }
        }

        public Rectangle Area
        {
            get { return new Rectangle(Location, Size); }
            set
            {   
                /* Let them both be set before calling Refresh() (because setting Location + Size will invoke refresh twice...) */
                _location = value.Location;
                var old = _size;
                _size = value.Size;
                if (_size != old) { Resized(old); }
                Refresh();
            }
        }

        public string Tag { get; set; }
        public string TooltipText { get; set; }
        public ControlRenderer Parent { get; set; }
        public bool HasFocus { get; set; }
        
        public bool Visible
        {
            get { return _visible; }
            set
            {
                _visible = value;
                Refresh();
            }
        }
        
        public bool Enabled
        {
            get { return _enabled; }
            set { _enabled = value; Refresh(); }
        }

        public virtual void Paint(Graphics g) { }
        public abstract void KeyEvent(PlayerKeyEvent e);
        public abstract void MouseEvent(PlayerMouseEvent e);

        public void Refresh()
        {
            if (Parent != null) { Parent.Invalidate(Area); }
        }

        protected virtual void Resized(Size oldSize)
        {
            /* Empty by default */
        }

        public virtual void SkinStyleChanged()
        {
            /* Empty by default */
        }
    }
}

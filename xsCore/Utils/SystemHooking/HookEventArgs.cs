using System;
using System.Drawing;
using System.Windows.Forms;

namespace xsCore.Utils.SystemHooking
{
    /* Based on code by VBDT http://www.codeproject.com/Articles/19858/Global-Windows-Hooks */
    public class KeyboardHookEventArgs : KeyEventArgs
    {
        public KeyboardHookEventArgs(Keys keyData, Keys virtualKeyCode) : base(keyData)
        {
            VirtualKeyCode = virtualKeyCode;
        }
        
        public new bool SuppressKeyPress
        {
            get
            {
                return base.SuppressKeyPress;
            }
            set
            {
                base.SuppressKeyPress = value;
            }
        }

        public Keys VirtualKeyCode { get; private set; }
    }

    public class MouseHookEventArgs : MouseEventArgs
    {
        public MouseHookEventArgs(MouseButtons button, int clicks, Point pt, int delta) : base(button, clicks, pt.X, pt.Y, delta)
        {
            /* Empty constructor */
        }

        public bool Handled { get; set; }
    }

    public class StateChangedEventArgs : EventArgs
    {
        public StateChangedEventArgs(HookState hookState)
        {
            State = hookState;
        }

        public HookState State { get; private set; }
    }
}

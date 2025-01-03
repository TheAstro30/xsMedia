﻿/* xsMedia - sxCore
 * (c)2013 - 2025
 * Jason James Newland
 * KangaSoft Software, All Rights Reserved
 * Licenced under the GNU public licence */
using System;
using System.Windows.Forms;
using xsCore.Utils.SystemHooking;

namespace xsCore.Utils
{
    /* Wrapper class for SystemHooking classes */
    public class SystemHookManager
    {
        private readonly MouseHook _mouseHook;
        private readonly KeyboardHook _keyboardHook;

        public event Action<object, MouseEventArgs> OnMouseClick;
        public event Action<object, MouseEventArgs> OnMouseDoubleClick;
        public event Action<object, MouseEventArgs> OnMouseMove;
        public event Action<object, KeyEventArgs> OnKeyDown;

        public SystemHookManager()
        {
            _mouseHook = new MouseHook();
            _mouseHook.InstallHook();
            _mouseHook.MouseClick += MouseClick;
            _mouseHook.MouseDoubleClick += MouseDoubleClick;
            _mouseHook.MouseMove += MouseMove;

            _keyboardHook = new KeyboardHook();
            _keyboardHook.InstallHook();
            _keyboardHook.KeyDown += KeyDown;
        }

        ~SystemHookManager()
        {
            _mouseHook.MouseClick -= MouseClick;
            _mouseHook.MouseDoubleClick -= MouseDoubleClick;
            _mouseHook.MouseMove -= MouseMove;
            _mouseHook.RemoveHook();

            _keyboardHook.KeyDown -= KeyDown;
            _keyboardHook.RemoveHook();
        }

        private void MouseClick(object sender, MouseEventArgs e)
        {
            if (OnMouseClick != null)
            {
                OnMouseClick(sender, e);
            }
        }

        private void MouseDoubleClick(object sender, MouseEventArgs e)
        {            
            if (OnMouseDoubleClick != null)
            {
                OnMouseDoubleClick(sender, e);
            }
        }

        private void MouseMove(object sender, MouseEventArgs e)
        {
            if (OnMouseMove != null)
            {
                OnMouseMove(sender, e);
            }
        }

        private void KeyDown(object sender, KeyEventArgs e)
        {
            if (OnKeyDown != null)
            {
                OnKeyDown(sender, e);
            }
        }
    }
}

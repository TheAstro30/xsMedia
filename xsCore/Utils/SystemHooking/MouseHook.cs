/* xsMedia - sxCore
 * (c)2013 - 2020
 * Jason James Newland
 * KangaSoft Software, All Rights Reserved
 * Licenced under the GNU public licence */
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace xsCore.Utils.SystemHooking
{
    /* Based on code by VBDT http://www.codeproject.com/Articles/19858/Global-Windows-Hooks */
    public class MouseHook
    {
        private MouseButtons _button;
        private MouseButtons _buttonsDown;
        private IntPtr _cHandle;
        private int _clicks;
        private IntPtr _hMouseHook;
        private IntPtr _hwnd;        
        private MouseMessageEventHandler _mouseProc;        
        private Rectangle _rectangle;        
        private static uint _thisTime;
        
        public event MouseEventHandler MouseClick;
        public event MouseEventHandler MouseDoubleClick;
        public event EventHandler<MouseEventArgs> MouseDown;        
        public event EventHandler<MouseEventArgs> MouseMove;        
        public event EventHandler<MouseEventArgs> MouseUp;        
        public event EventHandler<MouseEventArgs> MouseWheel;        
        public event EventHandler<StateChangedEventArgs> StateChanged;

        private delegate IntPtr MouseMessageEventHandler(int nCode, IntPtr wParam, ref Win32.MouseInfo lParam);

        public MouseHook()
        {
            _rectangle = new Rectangle(0, 0, SystemInformation.DoubleClickSize.Width, SystemInformation.DoubleClickSize.Height);
        }

        ~MouseHook()
        {
            RemoveHook();
        }

        public HookState State
        {
            get
            {
                return _hMouseHook != IntPtr.Zero ? HookState.Installed : HookState.Uninstalled;
            }
        }     

        public void InstallHook()
        {
            if (_hMouseHook != IntPtr.Zero) { return; }
            _mouseProc = MouseProc;            
            _hMouseHook = Win32.SetWindowsHookEx(Win32.WindowsHookMouse, _mouseProc, IntPtr.Zero, 0);
            if (_hMouseHook == IntPtr.Zero)
            {
                _mouseProc = null;
                return;
            }
            OnStateChanged(new StateChangedEventArgs(State));
        }

        public void RemoveHook()
        {
            if (_hMouseHook == IntPtr.Zero) { return; }
            if (!Win32.UnhookWindowsHookEx(_hMouseHook))
            {
                _hMouseHook = IntPtr.Zero;
                return;
            }
            _mouseProc = null;
            _hMouseHook = IntPtr.Zero;
            _hwnd = IntPtr.Zero;
            _cHandle = IntPtr.Zero;
            _button = MouseButtons.None;
            _buttonsDown = MouseButtons.None;
            _clicks = 0;
            OnStateChanged(new StateChangedEventArgs(State));
        }

        private MouseHookEventArgs GetMouseDownArgs(Win32.MouseInfo lParam, MouseButtons btn)
        {
            MouseHookEventArgs args;
            if (_clicks == 1 && _button == btn && _hwnd == _cHandle && lParam.time - _thisTime <= SystemInformation.DoubleClickTime && _rectangle.Contains(lParam.pt.X, lParam.pt.Y))
            {
                _clicks = 2;
                args = new MouseHookEventArgs(btn, _clicks, lParam.pt, 0);
            }
            else
            {
                _clicks = 1;
                args = new MouseHookEventArgs(btn, _clicks, lParam.pt, 0);
            }
            _button = btn;
            _buttonsDown |= btn;
            _thisTime = lParam.time;
            _hwnd = _cHandle;
            _rectangle.Location = new Point(lParam.pt.X - (_rectangle.Width / 2), lParam.pt.Y - (_rectangle.Height / 2));
            return args;
        }

        private MouseHookEventArgs GetMouseUpArgs(Win32.MouseInfo lParam, MouseButtons btn)
        {
            _buttonsDown &= ~btn;
            _button = btn;
            return new MouseHookEventArgs(btn, 1, lParam.pt, 0);
        }

        private IntPtr MouseProc(int nCode, IntPtr wParam, ref Win32.MouseInfo lParam)
        {
            if (nCode >= 0)
            {
                _cHandle = Win32.WindowFromPoint(lParam.pt);
                var num = wParam.ToInt32();
                MouseHookEventArgs e = null;
                switch (num)
                {
                    case (int)Win32.MouseButton.MouseMove:
                        e = new MouseHookEventArgs(_buttonsDown, 0, lParam.pt, 0);
                        OnMouseMove(e);
                        break;
                    case (int)Win32.MouseButton.LButtonDown:
                        e = GetMouseDownArgs(lParam, MouseButtons.Left);
                        OnMouseDown(e);
                        break;
                    case (int)Win32.MouseButton.RButtonDown:
                        e = GetMouseDownArgs(lParam, MouseButtons.Right);
                        OnMouseDown(e);
                        break;
                    case (int)Win32.MouseButton.MButtonDown:
                        e = GetMouseDownArgs(lParam, MouseButtons.Middle);
                        OnMouseDown(e);
                        break;
                    default:
                        if ((num == (int)Win32.MouseXButton.XButtonDown) | (num == (int)Win32.MouseXButton.NcxButtonDown))
                        {
                            switch (((short)(lParam.mouseData >> (byte)Win32.MouseEventF.RightUp)))
                            {
                                case 1:
                                    e = GetMouseDownArgs(lParam, MouseButtons.XButton1);
                                    OnMouseDown(e);
                                    break;
                                case 2:
                                    e = GetMouseDownArgs(lParam, MouseButtons.XButton2);
                                    OnMouseDown(e);
                                    break;
                            }
                        }
                        else switch (num)
                        {
                            case (int)Win32.MouseButton.LButtonUp:
                                OnMouseClick(lParam, MouseButtons.Left);
                                e = GetMouseUpArgs(lParam, MouseButtons.Left);
                                OnMouseUp(e);
                                break;
                            case (int)Win32.MouseButton.RButtonUp:
                                OnMouseClick(lParam, MouseButtons.Right);
                                e = GetMouseUpArgs(lParam, MouseButtons.Right);
                                OnMouseUp(e);
                                break;
                            case (int)Win32.MouseButton.MButtonUp:
                                OnMouseClick(lParam, MouseButtons.Middle);
                                e = GetMouseUpArgs(lParam, MouseButtons.Middle);
                                OnMouseUp(e);
                                break;
                            default:
                                if ((num == (int)Win32.MouseXButton.XButtonUp) | (num == (int)Win32.MouseXButton.NcxButtonUp))
                                {
                                    switch (((short)(lParam.mouseData >> (byte)Win32.MouseEventF.RightUp)))
                                    {
                                        case 1:
                                            OnMouseClick(lParam, MouseButtons.XButton1);
                                            e = GetMouseUpArgs(lParam, MouseButtons.XButton1);
                                            OnMouseUp(e);
                                            break;
                                        case 2:
                                            OnMouseClick(lParam, MouseButtons.XButton2);
                                            e = GetMouseUpArgs(lParam, MouseButtons.XButton2);
                                            OnMouseUp(e);
                                            break;
                                    }
                                }
                                else if ((num == (int)Win32.MouseButton.MouseWheel) | (num == (int)Win32.MouseButton.MouseHWheel))
                                {
                                    int delta = (short)(lParam.mouseData >> (byte)Win32.MouseEventF.RightUp);
                                    e = new MouseHookEventArgs(MouseButtons.None, 0, lParam.pt, delta);
                                    OnMouseWheel(e);
                                }
                                break;
                        }
                        break;
                }
                if (e != null && e.Handled)
                {
                    return new IntPtr(1);
                }
            }
            return Win32.CallNextHookEx(_hMouseHook, nCode, wParam, ref lParam);
        }

        protected virtual void OnMouseClick(MouseEventArgs e)
        {
            if (MouseClick != null)
            {
                MouseClick(_cHandle, e);
            }
        }

        private void OnMouseClick(Win32.MouseInfo lParam, MouseButtons btn)
        {
            if (_button == btn && _hwnd == _cHandle && _clicks == 1)
            {
                OnMouseClick(new MouseEventArgs(btn, 1, lParam.pt.X, lParam.pt.Y, 0));
            }
            else if (_button == btn && _hwnd == _cHandle && _clicks == 2)
            {
                OnMouseDoubleClick(new MouseEventArgs(btn, 2, lParam.pt.X, lParam.pt.Y, 0));
                _clicks = 0;
            }
        }

        protected virtual void OnMouseDoubleClick(MouseEventArgs e)
        {
            if (MouseDoubleClick != null)
            {
                MouseDoubleClick(_cHandle, e);
            }
        }

        protected virtual void OnMouseDown(MouseEventArgs e)
        {
            if (MouseDown != null)
            {
                MouseDown(_cHandle, e);
            }
        }

        protected virtual void OnMouseMove(MouseEventArgs e)
        {
            if (MouseMove != null)
            {
                MouseMove(_cHandle, e);
            }
        }

        protected virtual void OnMouseUp(MouseEventArgs e)
        {
            if (MouseUp != null)
            {
                MouseUp(_cHandle, e);
            }
        }

        protected virtual void OnMouseWheel(MouseEventArgs e)
        {
            if (MouseWheel != null)
            {
                MouseWheel(IntPtr.Zero, e);
            }
        }

        protected virtual void OnStateChanged(StateChangedEventArgs e)
        {
            if (StateChanged != null)
            {
                StateChanged(this, e);
            }
        }

        public static void SynthesizeMouseDown(MouseButtons button)
        {
            SynthesizeMouseDown(button, IntPtr.Zero);
        }

        public static void SynthesizeMouseDown(MouseButtons button, IntPtr extraInfo)
        {
            var pInputs = new Win32.MsInput
                              {
                                  dwType = 0,
                                  xi = {dwExtraInfo = extraInfo}
                              };
            switch (button)
            {
                case MouseButtons.Left:
                    pInputs.xi.dwFlags = (int)Win32.MouseEventF.LeftDown;
                    break;
                case MouseButtons.Right:
                    pInputs.xi.dwFlags = (int)Win32.MouseEventF.RightDown;
                    break;
                case MouseButtons.Middle:
                    pInputs.xi.dwFlags = (int)Win32.MouseEventF.MiddleDown;
                    break;
                case MouseButtons.XButton1:
                    pInputs.xi.dwFlags = (int)Win32.MouseXButton.MouseEventfXDown;
                    pInputs.xi.mouseData = (int)Win32.MouseXButton.XButton1;
                    break;
                case MouseButtons.XButton2:
                    pInputs.xi.dwFlags = (int)Win32.MouseXButton.MouseEventfXDown;
                    pInputs.xi.mouseData = (int)Win32.MouseXButton.XButton2;
                    break;
            }
            Win32.SendInput(1, ref pInputs, Marshal.SizeOf(pInputs));            
        }

        public static void SynthesizeMouseMove(Point location, MapOn mapping)
        {
            SynthesizeMouseMove(location, mapping, IntPtr.Zero);
        }

        public static void SynthesizeMouseMove(Point location, MapOn mapping, IntPtr extraInfo)
        {
            var pInputs = new Win32.MsInput
                              {
                                  dwType = 0
                              };
            switch (mapping)
            {
                case MapOn.Relative:
                    pInputs.xi.pt = location;
                    break;
                case MapOn.PrimaryMonitor:
                    pInputs.xi.pt.X = ((int)Math.Ceiling(Math.Ceiling((double)(location.X * 0xffff)) / SystemInformation.PrimaryMonitorSize.Width)) + 1;
                    pInputs.xi.pt.Y = ((int)Math.Ceiling(Math.Ceiling((double)(location.Y * 0xffff)) / SystemInformation.PrimaryMonitorSize.Height)) + 1;
                    break;
                case MapOn.VirtualDesktop:
                    pInputs.xi.pt.X = ((int)Math.Ceiling(Math.Ceiling((double)(location.X * 0xffff)) / SystemInformation.VirtualScreen.Width)) + 1;
                    pInputs.xi.pt.Y = ((int)Math.Ceiling(Math.Ceiling((double)(location.Y * 0xffff)) / SystemInformation.VirtualScreen.Height)) + 1;
                    break;
            }
            pInputs.xi.dwExtraInfo = extraInfo;
            pInputs.xi.dwFlags = (uint)(((MapOn)1) | mapping);
            Win32.SendInput(1, ref pInputs, Marshal.SizeOf(pInputs));            
        }

        public static void SynthesizeMouseUp(MouseButtons button)
        {
            SynthesizeMouseUp(button, IntPtr.Zero);
        }

        public static void SynthesizeMouseUp(MouseButtons button, IntPtr extraInfo)
        {
            var pInputs = new Win32.MsInput
                              {
                                  dwType = 0,
                                  xi = {dwExtraInfo = extraInfo}
                              };
            switch (button)
            {
                case MouseButtons.Left:
                    pInputs.xi.dwFlags = (int)Win32.MouseEventF.LeftUp;
                    break;
                case MouseButtons.Right:
                    pInputs.xi.dwFlags = (int)Win32.MouseEventF.RightUp;
                    break;
                case MouseButtons.Middle:
                    pInputs.xi.dwFlags = (int)Win32.MouseEventF.MiddleUp;
                    break;
                case MouseButtons.XButton1:
                    pInputs.xi.dwFlags = (int)Win32.MouseXButton.MouseEventfXup;
                    pInputs.xi.mouseData = 1;
                    break;
                case MouseButtons.XButton2:
                    pInputs.xi.dwFlags = (int)Win32.MouseXButton.MouseEventfXup;
                    pInputs.xi.mouseData = 2;
                    break;
            }
            Win32.SendInput(1, ref pInputs, Marshal.SizeOf(pInputs));            
        }

        public static void SynthesizeMouseWheel(int wheelClicks)
        {
            SynthesizeMouseWheel(wheelClicks, IntPtr.Zero);
        }

        public static void SynthesizeMouseWheel(int wheelClicks, IntPtr extraInfo)
        {
            var pInputs = new Win32.MsInput
                              {
                                  dwType = 0,
                                  xi =
                                      {
                                          dwExtraInfo = extraInfo,
                                          dwFlags = (int)Win32.MouseEventF.Wheel,
                                          mouseData = (uint)((int)Win32.MouseButton.WheelDelta * wheelClicks)
                                      }
                              };
            Win32.SendInput(1, ref pInputs, Marshal.SizeOf(pInputs));            
        }   
    }
}

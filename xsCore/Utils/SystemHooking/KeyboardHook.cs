using System;
using System.Windows.Forms;

namespace xsCore.Utils.SystemHooking
{
    /* Based on code by VBDT http://www.codeproject.com/Articles/19858/Global-Windows-Hooks */
    public class KeyboardHook
    {
        private IntPtr _hKeyboardHook;        
        private KeyboardMessageEventHandler _keyboardProc;        
        private Keys _keyData;
        
        public event EventHandler<KeyboardHookEventArgs> KeyDown;
        public event EventHandler<KeyboardHookEventArgs> KeyUp;        
        public event EventHandler<StateChangedEventArgs> StateChanged;

        private delegate IntPtr KeyboardMessageEventHandler(int nCode, IntPtr wParam, ref Win32.KeyboardData lParam);

        ~KeyboardHook()
        {
            RemoveHook();            
        }

        public bool AltKeyDown
        {
            get
            {
                return ((_keyData & Keys.Alt) == Keys.Alt);
            }
        }

        public bool CtrlKeyDown
        {
            get
            {
                return ((_keyData & Keys.Control) == Keys.Control);
            }
        }

        public bool ShiftKeyDown
        {
            get
            {
                return ((_keyData & Keys.Shift) == Keys.Shift);
            }
        }

        public HookState State
        {
            get
            {
                return _hKeyboardHook != IntPtr.Zero ? HookState.Installed : HookState.Uninstalled;
            }
        }        

        public void InstallHook()
        {
            if (_hKeyboardHook != IntPtr.Zero) { return; }
            _keyboardProc = KeyboardProc;
            _hKeyboardHook = Win32.SetWindowsHookEx(Win32.WindowsHookKeyboard, _keyboardProc, IntPtr.Zero, 0);
            if (_hKeyboardHook == IntPtr.Zero)
            {
                _keyboardProc = null;
                return;
            }
            OnStateChanged(new StateChangedEventArgs(State));
        }

        public void RemoveHook()
        {
            if (_hKeyboardHook == IntPtr.Zero) { return; }
            if (!Win32.UnhookWindowsHookEx(_hKeyboardHook))
            {
                _hKeyboardHook = IntPtr.Zero;
                return;
            }
            _keyboardProc = null;
            _hKeyboardHook = IntPtr.Zero;
            _keyData = Keys.None;
            OnStateChanged(new StateChangedEventArgs(State));
        }

        private IntPtr KeyboardProc(int nCode, IntPtr wParam, ref Win32.KeyboardData lParam)
        {
            if (nCode >= 0)
            {
                KeyboardHookEventArgs args;
                var vkCode = (Keys)lParam.vkCode;
                if ((((int)wParam) == (int)Win32.KeyDown.KeyDown) | (((int)wParam) == (int)Win32.KeyDown.SysKeyDown))
                {
                    if (vkCode == Keys.LMenu | vkCode == Keys.RMenu)
                    {
                        _keyData |= Keys.Alt;
                        args = new KeyboardHookEventArgs(_keyData | Keys.Menu, vkCode);
                    }
                    else if (vkCode == Keys.LControlKey | vkCode == Keys.RControlKey)
                    {
                        _keyData |= Keys.Control;
                        args = new KeyboardHookEventArgs(_keyData | Keys.ControlKey, vkCode);
                    }
                    else if (vkCode == Keys.LShiftKey | vkCode == Keys.RShiftKey)
                    {
                        _keyData |= Keys.Shift;
                        args = new KeyboardHookEventArgs(_keyData | Keys.ShiftKey, vkCode);
                    }
                    else
                    {
                        args = new KeyboardHookEventArgs(_keyData | vkCode, vkCode);
                    }
                    OnKeyDown(args);
                    if (args.Handled)
                    {
                        return new IntPtr(1);
                    }
                }
                else if (((int)wParam) == (int)Win32.KeyDown.KeyUp | ((int)wParam) == (int)Win32.KeyDown.SysKeyUp)
                {
                    if (vkCode == Keys.LMenu | vkCode == Keys.RMenu)
                    {
                        _keyData &= ~Keys.Alt;
                        args = new KeyboardHookEventArgs(_keyData | Keys.Menu, vkCode);
                    }
                    else if (vkCode == Keys.LControlKey | vkCode == Keys.RControlKey)
                    {
                        _keyData &= ~Keys.Control;
                        args = new KeyboardHookEventArgs(_keyData | Keys.ControlKey, vkCode);
                    }
                    else if (vkCode == Keys.LShiftKey | vkCode == Keys.RShiftKey)
                    {
                        _keyData &= ~Keys.Shift;
                        args = new KeyboardHookEventArgs(_keyData | Keys.ShiftKey, vkCode);
                    }
                    else
                    {
                        args = new KeyboardHookEventArgs(_keyData | vkCode, vkCode);
                    }
                    OnKeyUp(args);
                    if (args.Handled)
                    {
                        return new IntPtr(1);
                    }
                }
            }
            return Win32.CallNextHookEx(_hKeyboardHook, nCode, wParam, ref lParam);
        }

        protected virtual void OnKeyDown(KeyboardHookEventArgs e)
        {
            if (KeyDown != null)
            {
                KeyDown(this, e);
            }
        }

        protected virtual void OnKeyUp(KeyboardHookEventArgs e)
        {
            if (KeyUp != null)
            {
                KeyUp(this, e);
            }
        }

        protected virtual void OnStateChanged(StateChangedEventArgs e)
        {
            if (StateChanged != null)
            {
                StateChanged(this, e);
            }
        } 
    }
}

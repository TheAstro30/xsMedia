using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Windows.Forms;

namespace xsCore.Utils.SystemUtils
{
    public static class AppMessenger
    {
        /* This class handles single instance and command line param message pumping to original instance if a new instance is
         * loaded */
        private const int MessageId = -1163005939; /* Unique random number for message */

        public static bool CheckPrevInstance(string message)
        {
            var hWnd = GetHWndOfPrevInstance(Process.GetCurrentProcess().ProcessName);
            if (hWnd != IntPtr.Zero)
            {
                SendCommandLine(hWnd, message);
                return true;
            }
            return false;
        }

        public static bool SendMessageToApp(string fileName, string message)
        {
            var hWnd = GetHWndOfPrevInstance(GetFileNameFromFullName(fileName));
            if (hWnd != IntPtr.Zero)
            {
                SendCommandLine(hWnd, message);
                return true;
            }
            /* could not find process so start it */
            var p = new Process
                        {
                            StartInfo =
                                {
                                    FileName = fileName
                                }
                        };
            p.Start();
            var t = Environment.TickCount + 30000; /* 30 second timeout */
            do
            {
                try
                {
                    hWnd = p.MainWindowHandle;
                }
                catch
                {
                    return false;
                }
            } 
            while (hWnd == IntPtr.Zero && t < Environment.TickCount);
            
            if (hWnd != IntPtr.Zero)
            {
                SendCommandLine(hWnd, message);
                return true;
            }
            return false;
        }

        public static string ProcessWmCopyData(Message m)
        {
            if (m.WParam.ToInt32() == MessageId)
            {
                var st = (Win32.CopyDataStruct)Marshal.PtrToStructure(m.LParam, typeof(Win32.CopyDataStruct));
                return st.lpData;
            }
            return null;
        }

        /* Private methods */
        private static string GetFileNameFromFullName(string fullName)
        {
            var pos = fullName.LastIndexOf("\\", StringComparison.Ordinal);
            return pos >= 0 ? fullName.Substring(pos + 1) : fullName;
        }

        private static IntPtr GetHWndOfPrevInstance(string processName)
        {
            var currentProcess = Process.GetCurrentProcess();
            var ps = Process.GetProcessesByName(processName);
            if (ps.Length > 1)
            {
                foreach (var p in ps.Where(p => p.Id != currentProcess.Id && p.ProcessName == processName))
                {
                    IntPtr hWnd;
                    try
                    {
                        hWnd = p.MainWindowHandle;
                    }
                    catch
                    {
                        return IntPtr.Zero;
                    }
                    if (hWnd.ToInt32() != 0)
                    {
                        return hWnd;
                    }
                }
            }
            return IntPtr.Zero;
        }

        private static void SendCommandLine(IntPtr hWnd, string commandLine)
        {
            Win32.SendMessage(hWnd, Win32.WmCopydata, MessageId, new Win32.CopyDataStruct(commandLine));
        }   
    }
}
/* xsMedia - sxCore
 * (c)2013 - 2024
 * Jason James Newland
 * KangaSoft Software, All Rights Reserved
 * Licenced under the GNU public licence */
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;

namespace xsCore.Utils
{
    [Flags]
    public enum MapOn
    {
        Relative = 0,
        PrimaryMonitor = 0x8000,
        VirtualDesktop = 0xc000
    }
    
    public enum HookState
    {
        Uninstalled = 0,
        Installed = 1
    }

    public static class Win32
    {
        public enum KeyDown
        {
            KeyDown = 0x100,
            KeyUp = 0x101,
            SysKeyDown = 260,
            SysKeyUp = 0x105
        }

        public enum MouseButton
        {
            WheelDelta = 120,
            LButtonDown = 0x201,
            LButtonUp = 0x202,
            MButtonDown = 0x207,
            MButtonUp = 520,
            MouseHWheel = 0x20e,
            MouseMove = 0x200,
            MouseWheel = 0x20a,
            RButtonDown = 0x204,
            RButtonUp = 0x205
        }

        public enum MouseEventF
        {
            LeftDown = 2,
            LeftUp = 4,
            MiddleDown = 0x20,
            MiddleUp = 0x40,
            RightDown = 8,
            RightUp = 0x10,
            Wheel = 0x800
        }

        public enum MouseXButton
        {
            XButton1 = 1,
            XButton2 = 2,
            XButtonDown = 0x20b,
            XButtonUp = 0x20c,
            NcxButtonDown = 0xab,
            NcxButtonUp = 0xac,
            MouseEventfXDown = 0x80,
            MouseEventfXup = 0x100
        }

        public enum ComputerNameFormat : uint
        {
            ComputerNameNetBios = 0,
            ComputerNameDnsHostname = 1,
            ComputerNameDnsDomain = 2,
            ComputerNameDnsFullyQualified = 3,
            ComputerNamePhysicalNetBios = 4,
            ComputerNamePhysicalDnsHostname = 5,
            ComputerNamePhysicalDnsDomain = 6,
            ComputerNamePhysicalDnsFullyQualified = 7,
            ComputerNameMax = 8
        }

        [StructLayout(LayoutKind.Sequential)]
        public class CdRomToc
        {
            public ushort Length;
            public byte FirstTrack = 0;
            public byte LastTrack = 0;

            public TrackDataList TrackDataList;

            public CdRomToc()
            {
                TrackDataList = new TrackDataList();
                Length = (ushort)Marshal.SizeOf(this);
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct TrackData
        {
            public byte Reserved;
            private byte BitMapped;

            public byte Control
            {
                get
                {
                    return (byte)(BitMapped & 0x0F);
                }
                set
                {
                    BitMapped = (byte)((BitMapped & 0xF0) | (value & 0x0F));
                }
            }

            public byte Adr
            {
                get
                {
                    return (byte)((BitMapped & 0xF0) >> 4);
                }
                set
                {
                    BitMapped = (byte)((BitMapped & 0x0F) | (value << 4));
                }
            }

            public byte TrackNumber;
            public byte Reserved1;

            public byte Address_0;
            public byte Address_1;
            public byte Address_2;
            public byte Address_3;
        }

        [StructLayout(LayoutKind.Sequential)]
        public class TrackDataList
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 100 * 8)]
            private readonly byte[] Data;

            public TrackData this[int index]
            {
                get
                {
                    if (index < 0 || index >= 100)
                    {
                        throw new IndexOutOfRangeException();
                    }
                    TrackData res;
                    var handle = GCHandle.Alloc(Data, GCHandleType.Pinned);
                    try
                    {
                        var buffer = handle.AddrOfPinnedObject();
                        buffer = (IntPtr)(buffer.ToInt32() + (index * Marshal.SizeOf(typeof(TrackData))));
                        res = (TrackData)Marshal.PtrToStructure(buffer, typeof(TrackData));
                    }
                    finally
                    {
                        handle.Free();
                    }
                    return res;
                }
            }

            public TrackDataList()
            {
                Data = new byte[100 * Marshal.SizeOf(typeof(TrackData))];
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public class CopyDataStruct
        {
            public int dwData = 0;
            public int cbData;
            public string lpData;

            public CopyDataStruct()
            {
                /* Empty constructor */
            }

            public CopyDataStruct(string data)
            {
                lpData = data + "\0";
                cbData = lpData.Length;
            }
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct KeyboardData
        {
            public uint vkCode;
            public uint scanCode;
            public uint flags;
            public uint time;
            public IntPtr dwExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct MouseInfo
        {
            public Point pt;
            public uint mouseData;
            public uint dwFlags;
            public uint time;
            public IntPtr dwExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct MsInput
        {
            public uint dwType;
            public MouseInfo xi;
        }

        public static readonly IntPtr HwndTopMost = new IntPtr(-1);
        public const UInt32 SwpNoSize = 0x0001;
        public const UInt32 SwpNoMove = 0x0002;
        public const UInt32 TopMostFlags = SwpNoMove | SwpNoSize;

        public const int WindowsHookKeyboard = 13;
        public const int WindowsHookMouse = 14;
        public const int WmCopydata = 0x4A;

        /* Interop */
        [DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        [DllImport("user32", EntryPoint = "SendMessageA")]
        public static extern int SendMessage(IntPtr hWnd, int wMsg, int wParam, CopyDataStruct lParam);

        [DllImport("user32.dll")]
        public static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, ref KeyboardData lParam);

        [DllImport("user32.dll")]
        public static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, ref MouseInfo lParam);
        [return: MarshalAs(UnmanagedType.Bool)]

        [DllImport("user32.dll", SetLastError = true)]
        public static extern uint SendInput(uint cInputs, ref MsInput pInputs, int cbSize);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr SetWindowsHookEx(int idHook, Delegate lpfn, IntPtr hMod, uint dwThreadId);
        [return: MarshalAs(UnmanagedType.Bool)]

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool UnhookWindowsHookEx(IntPtr hook);

        [DllImport("user32.dll")]
        public static extern IntPtr WindowFromPoint(Point pt);

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true, PreserveSig = true, CallingConvention = CallingConvention.Winapi)]
        public static extern bool GetComputerNameEx([MarshalAs(UnmanagedType.U4)] ComputerNameFormat nameType, [MarshalAs(UnmanagedType.LPWStr)] StringBuilder lpBuffer, ref uint dwSize);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern bool GetVolumeInformation(string volume, StringBuilder volumeName, uint volumeNameSize,
                                                        out uint serialNumber, out uint serialNumberLength,
                                                        out uint flags, StringBuilder fs, uint fsSize);

        [DllImport("Kernel32.dll", SetLastError = true)]
        public static extern int DeviceIoControl(IntPtr hDevice, uint ioControlCode, IntPtr inBuffer, uint inBufferSize,
                                                 [Out] CdRomToc outToc, uint outBufferSize, ref uint bytesReturned,
                                                 IntPtr overlapped);


        [DllImport("Kernel32.dll", SetLastError = true)]
        public static extern IntPtr CreateFile(string fileName, uint desiredAccess, uint shareMode,
                                               IntPtr lpSecurityAttributes, uint creationDisposition,
                                               uint dwFlagsAndAttributes, IntPtr hTemplateFile);

        [DllImport("kernel32.dll", CallingConvention = CallingConvention.Winapi, PreserveSig = true, CharSet = CharSet.Unicode)]
        public static extern int CloseHandle(IntPtr handle);

        [DllImport("kernel32.dll", CallingConvention = CallingConvention.Winapi, PreserveSig = true, CharSet = CharSet.Unicode)]
        public static extern uint GetLogicalDriveStringsW(uint dwBufferSize, IntPtr lpBuffer);

        [DllImport("kernel32.dll", CallingConvention = CallingConvention.Winapi, PreserveSig = true, CharSet = CharSet.Unicode)]
        public static extern uint GetDriveTypeW([MarshalAs(UnmanagedType.LPWStr)] string lpRootPathName);
    }
}

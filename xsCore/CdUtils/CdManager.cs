/* xsMedia - sxCore
 * (c)2013 - 2025
 * Jason James Newland
 * KangaSoft Software, All Rights Reserved
 * Licenced under the GNU public licence */
using System;
using System.IO;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using xsCore.Utils;

namespace xsCore.CdUtils
{
    /*  CD manager class (insertion and removal)
     *  By Jason James Newland (Parts by Rui Reis 2003)
     *  (C)Copyright 2003-2013
     *  KangaSoft Software - All Rights Reserved
     */
    public class CdManager : IDisposable
    {
        public class CdInfo
        {
            public string DriveLabel { get; set; }
            public string VolumeLabel { get; set; }
            public bool IsReady { get; set; }
            public DiscType Type { get; set; }
        }

        private CdMonitor _fInternal;
        private bool _fDisposed;

        public event Action OnCdLookupComplete;
        public event Action<int, string> OnCdMediaInserted;
        public event Action<int, string> OnCdMediaRemoved;
        public event Action<int, string> OnVcdMediaInserted;
        public event Action<int, string> OnVcdMediaRemoved;
        public event Action<int, string> OnDvdMediaInserted;
        public event Action<int, string> OnDvdMediaRemoved;

        public CdManager(IWin32Window window)
        {
            var fHandle = window.Handle;
            AvailableDrives = new Dictionary<int, CdInfo>();
            _fInternal = new CdMonitor(this);
            _fDisposed = false;
            CdTrackInfo = new CdDrive();
            if (_fInternal.Handle == IntPtr.Zero) { _fInternal.AssignHandle(fHandle); }
            /* Search for CD/DVD drives */
            var t = new Thread(BeginCdLookUp);
            t.Start();
        }

        ~CdManager()
        {
            Dispose(false);
        }

        public Dictionary<int, CdInfo> AvailableDrives { get; set; }
        public CdDrive CdTrackInfo { get; set; }

        public void Dispose()
        {
            CdTrackInfo.Close();
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool aDisposing)
        {
            if (!_fDisposed)
            {
                if (_fInternal.Handle != IntPtr.Zero)
                {
                    _fInternal.ReleaseHandle();
                    _fInternal = null;
                }
            }
            _fDisposed = true;
        }
        
        public int GetDeviceId(string volumeLetter)
        {
            if (!volumeLetter.EndsWith("\\"))
            {
                volumeLetter = string.Format("{0}\\", volumeLetter);
            }
            var i = 0;
            foreach (var cd in AvailableDrives)
            {
                if (cd.Value.DriveLabel.ToUpper() == volumeLetter.ToUpper())
                {
                    return i;
                }
                i++;
            }
            return -1;
        }

        private void BeginCdLookUp()
        {
            var drives = DriveInfo.GetDrives();
            var i = 0;
            foreach (var c in from drive in drives
                              where drive.DriveType == DriveType.CDRom
                              select new CdInfo
                              {                                 
                                  DriveLabel = drive.Name,
                                  VolumeLabel = drive.IsReady ? drive.VolumeLabel : "<No media>",
                                  IsReady = drive.IsReady,
                                  Type = GetDiscType(drive)
                              })
            {                
                AvailableDrives.Add(i, c);                
                i++;
            }
            if (OnCdLookupComplete != null)
            {
                OnCdLookupComplete();
            }
        }

        internal bool MediaIsDvd(string volumeLetter)
        {
            /* BluRay's gonna be a bitch to do this way ... there HAS to be a better way */
            return Directory.Exists(string.Format("{0}VIDEO_TS", volumeLetter)) && 
                File.Exists(string.Format("{0}VIDEO_TS\\VIDEO_TS.IFO", volumeLetter));
        }

        internal bool MediaIsVcd(string volumeLetter)
        {
            /* BluRay's gonna be a bitch to do this way ... there HAS to be a better way */
            return Directory.Exists(string.Format("{0}VCD", volumeLetter)) &&
                File.Exists(string.Format("{0}VCD\\INFO.VCD", volumeLetter));
        }

        internal DiscType GetDiscType(DriveInfo drive)
        {
            return !drive.IsReady ? DiscType.None : GetDiscType(drive.Name, drive.VolumeLabel, drive.IsReady);
        }

        internal DiscType GetDiscType(string name, string volumeLabel, bool isReady)
        {
            if (!isReady || string.IsNullOrEmpty(volumeLabel))
            {
                return DiscType.None;
            }
            if (volumeLabel.ToLower() == "audio cd")
            {
                return DiscType.Cdda;
            }
            if (MediaIsVcd(name))
            {
                return DiscType.Vcd;
            }
            return MediaIsDvd(name) ? DiscType.Dvd : DiscType.CdRom;
        }

        internal void TriggerEvents(bool inserted, string volumeLetter)
        {
            var deviceId = GetDeviceId(volumeLetter);
            var volumeName = new StringBuilder(256);
            DiscType type;
            string label;
            var ready = false;

            if (inserted)
            {
                uint serialNum;
                uint serialNumLength;
                uint flags;
                var fstype = new StringBuilder(256);

                Win32.GetVolumeInformation(volumeLetter, volumeName, (uint)volumeName.Capacity - 1, out serialNum,
                                     out serialNumLength, out flags, fstype, (uint)fstype.Capacity - 1);
                label = volumeName.ToString();
                ready = !string.IsNullOrEmpty(label);
                type = GetDiscType(volumeLetter, label, ready);
                switch (type)
                {
                    case DiscType.Vcd:
                        if (OnVcdMediaInserted != null)
                        {
                            OnVcdMediaInserted(deviceId, volumeLetter);
                        }
                        break;
                    case DiscType.Dvd:
                        if (OnDvdMediaInserted != null)
                        {
                            OnDvdMediaInserted(deviceId, volumeLetter);
                        }
                        break;
                    default:
                        if (OnCdMediaInserted != null)
                        {
                            OnCdMediaInserted(deviceId, volumeLetter);
                        }
                        break;
                }
            }
            else
            {
                type = DiscType.None;
                label = "<No Media>";
                if (AvailableDrives.ContainsKey(deviceId))
                {
                    type = AvailableDrives[deviceId].Type;
                    label = AvailableDrives[deviceId].VolumeLabel;
                }
                switch (type)
                {
                    case DiscType.Vcd:
                        if (OnVcdMediaRemoved != null)
                        {
                            OnVcdMediaRemoved(deviceId, volumeLetter);
                        }
                        break;
                    case DiscType.Dvd:
                        if (OnDvdMediaRemoved != null)
                        {
                            OnDvdMediaRemoved(deviceId, volumeLetter);
                        }
                        break;

                    default:
                        if (OnCdMediaRemoved != null)
                        {
                            OnCdMediaRemoved(deviceId, volumeLetter);
                        }
                        break;
                }
            }
            /* Reset volume label */
            if (!AvailableDrives.ContainsKey(deviceId)) { return; }
            var c = new CdInfo
                        {
                            DriveLabel = volumeLetter,
                            VolumeLabel = ready ? label : "<No Media>",
                            IsReady = ready,
                            Type = type
                        };
            AvailableDrives[deviceId] = c;
        }
    }

    internal class CdMonitor : NativeWindow
    {
        public enum DeviceEvent
        {
            Arrival = 0x8000,
            QueryRemove = 0x8001,
            QueryRemoveFailed = 0x8002,
            RemovePending = 0x8003,
            RemoveComplete = 0x8004,
            Specific = 0x8005,
            Custom = 0x8006
        }

        public enum DeviceType
        {
            Oem = 0x00000000,
            DeviceNode = 0x00000001,
            Volume = 0x00000002,
            Port = 0x00000003,
            Net = 0x00000004
        }

        public enum VolumeFlags
        {
            Media = 0x0001,
            Net = 0x0002
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct BroadcastHeader
        {
            public int Size;
            public DeviceType Type;
            private readonly int _reserved;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Volume
        {
            public int Size;
            public DeviceType Type;
            private readonly int _reserved;
            public int Mask;
            public int Flags;
        }

        private const int WmDevicechange = 0x0219;
        private readonly CdManager _device;

        public CdMonitor(CdManager device)
        {
            _device = device;
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            if (m.Msg != WmDevicechange) { return; }
            var lEvent = (DeviceEvent)m.WParam.ToInt32();
            if (lEvent != DeviceEvent.Arrival && lEvent != DeviceEvent.RemoveComplete) { return; }
            var lBroadcastHeader = (BroadcastHeader)Marshal.PtrToStructure(m.LParam, typeof(BroadcastHeader));
            if (lBroadcastHeader.Type != DeviceType.Volume) { return; }
            var lVolume = (Volume)Marshal.PtrToStructure(m.LParam, typeof(Volume));
            if ((lVolume.Flags & (int)VolumeFlags.Media) != 0)
            {
                _device.TriggerEvents(lEvent == DeviceEvent.Arrival, MaskToLogicalPath(lVolume.Mask));
            }
        }

        private static string MaskToLogicalPath(int mask)
        {
            switch (mask)
            {
                case 0:
                    return null;
                default:
                    int i;
                    for (i = 0; ((1 << i) & mask) == 0; i++)
                    {
                        /* Empty bitshift block */
                    }
                    return string.Format(@"{0}:\", (char)(65 + i));
            }
        }
    }
}

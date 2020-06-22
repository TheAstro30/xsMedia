/* xsMedia - sxCore
 * (c)2013 - 2020
 * Jason James Newland
 * KangaSoft Software, All Rights Reserved
 * Licenced under the GNU public licence */
using System;
using System.Globalization;
using System.Runtime.InteropServices;
using xsCore.Utils;

/* CDDB (Compact Disc Database) - Modified 2013
 * Written by: Idael Cardoso, Brian Weeres & Jason James Newland
 * Copyright (c) 2002 - 2004, 2013
 * 
 * Email: yetiicb@hotmail.com
 * Email: bweeres@protegra.com; bweeres@hotmail.com
 * Email: kangasoft@live.com.au
 * 
 * Permission to use, copy, modify, and distribute this software for any
 * purpose is hereby granted, provided that the above
 * copyright notice and this permission notice appear in all copies.
 *
 * If you modify it then please indicate so. 
 *
 * The software is provided "AS IS" and there are no warranties or implied warranties.
 * In no event shall Brian Weeres and/or Protegra Technology Group be liable for any special, 
 * direct, indirect, or consequential damages or any damages whatsoever resulting for any reason 
 * out of the use or performance of this software
 * 
 */

namespace xsCore.CdUtils
{
    public enum DiscType
    {
        None = 0,
        CdRom = 1,
        Cdda = 2,
        Vcd = 3,
        Dvd = 4,        
        BluRay = 5 /* Not implemented as of yet */
    }

    public class CdDrive : IDisposable
    {
        private IntPtr _cdHandle;
        private bool _tocValid;
        private Win32.CdRomToc _toc;

        private const uint GenericRead = 0x80000000;
        private const uint FileShareRead = 0x00000001;
        private const uint OpenExisting = 3;
        private const uint IoctlCdromReadToc = 0x00024000;

        ~CdDrive()
        {
            Dispose();
        }

        public void Dispose()
        {
            Close();
            GC.SuppressFinalize(this);
        }

        public bool Open(string drive)
        {
            Close();
            _toc = new Win32.CdRomToc();
            if (string.IsNullOrEmpty(drive)) { return false; }
            var d = drive.Substring(0, 2);
            _cdHandle = Win32.CreateFile(string.Format("\\\\.\\{0}", d), GenericRead, FileShareRead, IntPtr.Zero, OpenExisting, 0, IntPtr.Zero);
            if ((int)_cdHandle != -1 && (int)_cdHandle != 0)
            {
                /* Read TOC */
                uint bytesRead = 0;
                _tocValid = Win32.DeviceIoControl(_cdHandle, IoctlCdromReadToc, IntPtr.Zero,
                                                           0, _toc, (uint)Marshal.SizeOf(_toc), ref bytesRead, IntPtr.Zero) != 0;
            }
            return _tocValid;
        }

        public void Close()
        {
            if ((int)_cdHandle != -1 && (int)_cdHandle != 0)
            {
                Win32.CloseHandle(_cdHandle);
            }
            _cdHandle = IntPtr.Zero;
            _tocValid = false;
        }

        public int GetTrackCount()
        {
            return _tocValid ? _toc.LastTrack - _toc.FirstTrack + 1 : -1;
        }

        public int GetTrackTime(int track)
        {
            if (_tocValid && track >= _toc.FirstTrack && track <= _toc.LastTrack)
            {
                var start = (GetStartSector(track) + 150) / 75;
                var end = (GetEndSector(track) + 150) / 75;
                return end - start;
            }
            return -1;
        }

        public string GetDiscId()
        {
            var numTracks = GetTrackCount();
            if (numTracks == -1)
            {
                return string.Empty;
            }
            var postfix = numTracks.ToString(CultureInfo.InvariantCulture);
            var n = 0;
            /* For backward compatibility this algorithm must not change */
            var i = 0;
            while (i < numTracks)
            {
                double ofs = (((_toc.TrackDataList[i].Address_1 * 60) + _toc.TrackDataList[i].Address_2) * 75) + _toc.TrackDataList[i].Address_3;
                n = n + CddbSum((_toc.TrackDataList[i].Address_1 * 60) + _toc.TrackDataList[i].Address_2);
                postfix += "+" + string.Format("{0}", ofs);
                i++;
            }
            var numSecs = _toc.TrackDataList[i].Address_1 * 60 + _toc.TrackDataList[i].Address_2;
            postfix += string.Format("+{0}", numSecs);
            var last = _toc.TrackDataList[numTracks];
            var first = _toc.TrackDataList[0];
            var t = ((last.Address_1 * 60) + last.Address_2) - ((first.Address_1 * 60) + first.Address_2);
            ulong id = (((uint)n % 0xff) << 24 | (uint)t << 8 | (uint)numTracks);
            var discId = string.Format("{0:x8}", id);
            return string.Format("{0}+{1}", discId, postfix);
        }

        /* Private methods */
        private static int CddbSum(int n)
        {
            /* For backward compatibility this algorithm must not change */
            var ret = 0;
            while (n > 0)
            {
                ret = ret + (n % 10);
                n = n / 10;
            }
            return ret;
        }

        private int GetStartSector(int track)
        {
            if (_tocValid && (track >= _toc.FirstTrack) && (track <= _toc.LastTrack))
            {
                var td = _toc.TrackDataList[track - 1];
                return (td.Address_1 * 60 * 75 + td.Address_2 * 75 + td.Address_3) - 150;
            }
            return -1;
        }

        private int GetEndSector(int track)
        {
            if (_tocValid && (track >= _toc.FirstTrack) && (track <= _toc.LastTrack))
            {
                var td = _toc.TrackDataList[track];
                return (td.Address_1 * 60 * 75 + td.Address_2 * 75 + td.Address_3) - 151;
            }
            return -1;
        }
    }
}
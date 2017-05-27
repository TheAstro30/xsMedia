//    nVLC
//    
//    Author:  Roman Ginzburg
//
//    nVLC is free software: you can redistribute it and/or modify
//    it under the terms of the GNU General Public License as published by
//    the Free Software Foundation, either version 3 of the License, or
//    (at your option) any later version.
//
//    nVLC is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
//    GNU General Public License for more details.
//     
// ========================================================================

using System;
using System.Runtime.InteropServices;
using xsVlc.Common;
using xsVlc.Common.Media;
using xsVlc.Common.Structures;
using xsVlc.Interop;

namespace xsVlc.Core.Media
{
    internal class MediaFromFile : BasicMedia, IMediaFromFile
    {
        public MediaFromFile(IntPtr hMediaLib) : base(hMediaLib)
        {

        }

        public string GetMetaData(MetaDataType dataType)
        {
            var ip = Api.libvlc_media_get_meta(Pointer, (LibvlcMetaT)dataType);
            return Marshal.PtrToStringAnsi(ip);
        }

        public void SetMetaData(MetaDataType dataType, string argument)
        {
            Api.libvlc_media_set_meta(Pointer, (LibvlcMetaT)dataType, argument.ToUtf8());
        }

        public void SaveMetaData()
        {
            Api.libvlc_media_save_meta(Pointer);
        }

        public long Duration
        {
            get
            {
                return Api.libvlc_media_get_duration(Pointer);
            }
        }

        public MediaTrackInfo[] TracksInfo
        {
            get
            {
                IntPtr pTr;
                var num = Api.libvlc_media_get_tracks_info(Pointer, out pTr);
                if (num == 0 || pTr == IntPtr.Zero)
                {
                    return null;
                    //throw new LibVlcException();
                }
                var size = Marshal.SizeOf(typeof(LibvlcMediaTrackInfoT));
                var tracks = new LibvlcMediaTrackInfoT[num];
                for (var i = 0; i < num; i++)
                {
                    tracks[i] = (LibvlcMediaTrackInfoT)Marshal.PtrToStructure(pTr, typeof(LibvlcMediaTrackInfoT));
                    pTr = new IntPtr(pTr.ToInt64() + size);
                }
                var mtis = new MediaTrackInfo[num];
                for (var i = 0; i < num; i++)
                {
                    mtis[i] = tracks[i].ToMediaInfo();
                }
                return mtis;
            }
        }
    }
}

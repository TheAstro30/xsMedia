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
using xsVlc.Common.Filters;
using xsVlc.Interop;

namespace xsVlc.Core.Filters
{
    internal class MarqueeFilter : IMarqueeFilter
    {
        private readonly IntPtr _mediaPlayer;

        public MarqueeFilter(IntPtr hMediaPlayer)
        {
            _mediaPlayer = hMediaPlayer;
        }

        public bool Enabled
        {
            get
            {
                return GetMarquee(LibvlcVideoMarqueeOptionT.LibvlcMarqueeEnable) == 1;
            }
            set
            {
                SetMarquee(LibvlcVideoMarqueeOptionT.LibvlcMarqueeEnable, Convert.ToInt32(value));
            }
        }

        public string Text
        {
            get
            {
                return GetMarqueeString(LibvlcVideoMarqueeOptionT.LibvlcMarqueeText);
            }
            set
            {
                SetMarqueeString(LibvlcVideoMarqueeOptionT.LibvlcMarqueeText, value);
            }
        }

        public VlcColor Color
        {
            get
            {
                return (VlcColor)GetMarquee(LibvlcVideoMarqueeOptionT.LibvlcMarqueeColor);
            }
            set
            {
                SetMarquee(LibvlcVideoMarqueeOptionT.LibvlcMarqueeColor, (int)value);
            }
        }

        public Position Position
        {
            get
            {
                return (Position)GetMarquee(LibvlcVideoMarqueeOptionT.LibvlcMarqueePosition);
            }
            set
            {
                SetMarquee(LibvlcVideoMarqueeOptionT.LibvlcMarqueePosition, (int)value);
            }
        }

        public int Refresh
        {
            get
            {
                return GetMarquee(LibvlcVideoMarqueeOptionT.LibvlcMarqueeRefresh);
            }
            set
            {
                SetMarquee(LibvlcVideoMarqueeOptionT.LibvlcMarqueeRefresh, value);
            }
        }

        public int Size
        {
            get
            {
                return GetMarquee(LibvlcVideoMarqueeOptionT.LibvlcMarqueeSize);
            }
            set
            {
                SetMarquee(LibvlcVideoMarqueeOptionT.LibvlcMarqueeSize, value);
            }
        }

        public int Timeout
        {
            get
            {
                return GetMarquee(LibvlcVideoMarqueeOptionT.LibvlcMarqueeTimeout);
            }
            set
            {
                SetMarquee(LibvlcVideoMarqueeOptionT.LibvlcMarqueeTimeout, value);
            }
        }

        public int X
        {
            get
            {
                return GetMarquee(LibvlcVideoMarqueeOptionT.LibvlcMarqueeX);
            }
            set
            {
                SetMarquee(LibvlcVideoMarqueeOptionT.LibvlcMarqueeX, value);
            }
        }

        public int Y
        {
            get
            {
                return GetMarquee(LibvlcVideoMarqueeOptionT.LibvlcMarqueeY);
            }
            set
            {
                SetMarquee(LibvlcVideoMarqueeOptionT.LibvlcMarqueeY, value);
            }
        }

        public int Opacity
        {
            get
            {
                return GetMarquee(LibvlcVideoMarqueeOptionT.LibvlcMarqueeOpacity);
            }
            set
            {
                SetMarquee(LibvlcVideoMarqueeOptionT.LibvlcMarqueeOpacity, value);
            }
        }

        private int GetMarquee(LibvlcVideoMarqueeOptionT option)
        {
            return Api.libvlc_video_get_marquee_int(_mediaPlayer, option);
        }

        private void SetMarquee(LibvlcVideoMarqueeOptionT option, int argument)
        {
            Api.libvlc_video_set_marquee_int(_mediaPlayer, option, argument);
        }

        private string GetMarqueeString(LibvlcVideoMarqueeOptionT option)
        {
            var ip = Api.libvlc_video_get_marquee_string(_mediaPlayer, option);
            return Marshal.PtrToStringAnsi(ip);
        }

        private void SetMarqueeString(LibvlcVideoMarqueeOptionT option, string argument)
        {
            Api.libvlc_video_set_marquee_string(_mediaPlayer, option, argument.ToUtf8());
        }
    }
}

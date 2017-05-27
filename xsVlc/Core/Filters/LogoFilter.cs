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
using xsVlc.Common;
using xsVlc.Common.Filters;
using xsVlc.Interop;

namespace xsVlc.Core.Filters
{
    internal class LogoFilter : ILogoFilter
    {
        private readonly IntPtr _mediaPlayer;
        private string _file;

        public LogoFilter(IntPtr hMediaPlayer)
        {
            _mediaPlayer = hMediaPlayer;
        }

        public bool Enabled
        {
            get
            {
                return Api.libvlc_video_get_logo_int(_mediaPlayer, LibvlcVideoLogoOptionT.LibvlcLogoEnable) == 1;
            }
            set
            {
                Api.libvlc_video_set_logo_int(_mediaPlayer, LibvlcVideoLogoOptionT.LibvlcLogoEnable, Convert.ToInt32(value));
            }
        }

        public string File
        {
            get
            {
                return _file;
            }
            set
            {
                Api.libvlc_video_set_logo_string(_mediaPlayer, LibvlcVideoLogoOptionT.LibvlcLogoFile, value.ToUtf8());
                _file = value;
            }
        }

        public int X
        {
            get
            {
                return Api.libvlc_video_get_logo_int(_mediaPlayer, LibvlcVideoLogoOptionT.LibvlcLogoX);
            }
            set
            {
                Api.libvlc_video_set_logo_int(_mediaPlayer, LibvlcVideoLogoOptionT.LibvlcLogoX, value);
            }
        }

        public int Y
        {
            get
            {
                return Api.libvlc_video_get_logo_int(_mediaPlayer, LibvlcVideoLogoOptionT.LibvlcLogoY);
            }
            set
            {
                Api.libvlc_video_set_logo_int(_mediaPlayer, LibvlcVideoLogoOptionT.LibvlcLogoY, value);
            }
        }

        public int Delay
        {
            get
            {
                return Api.libvlc_video_get_logo_int(_mediaPlayer, LibvlcVideoLogoOptionT.LibvlcLogoDelay);
            }
            set
            {
                Api.libvlc_video_set_logo_int(_mediaPlayer, LibvlcVideoLogoOptionT.LibvlcLogoDelay, value);
            }
        }

        public int Repeat
        {
            get
            {
                return Api.libvlc_video_get_logo_int(_mediaPlayer, LibvlcVideoLogoOptionT.LibvlcLogoRepeat);
            }
            set
            {
                Api.libvlc_video_set_logo_int(_mediaPlayer, LibvlcVideoLogoOptionT.LibvlcLogoRepeat, value);
            }
        }

        public int Opacity
        {
            get
            {
                return Api.libvlc_video_get_logo_int(_mediaPlayer, LibvlcVideoLogoOptionT.LibvlcLogoOpacity);
            }
            set
            {
                Api.libvlc_video_set_logo_int(_mediaPlayer, LibvlcVideoLogoOptionT.LibvlcLogoOpacity, value);
            }
        }

        public Position Position
        {
            get
            {
                return (Position)Api.libvlc_video_get_logo_int(_mediaPlayer, LibvlcVideoLogoOptionT.LibvlcLogoPosition);
            }
            set
            {
                Api.libvlc_video_set_logo_int(_mediaPlayer, LibvlcVideoLogoOptionT.LibvlcLogoPosition, (int)value);
            }
        }
    }
}

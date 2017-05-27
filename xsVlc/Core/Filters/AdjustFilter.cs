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
using xsVlc.Common.Filters;
using xsVlc.Interop;

namespace xsVlc.Core.Filters
{
    internal class AdjustFilter : IAdjustFilter
    {
        private readonly IntPtr _mediaPlayer;

        public AdjustFilter(IntPtr hMediaPlayer)
        {
            _mediaPlayer = hMediaPlayer;
        }

        public bool Enabled
        {
            get
            {
                return GetVideoAdjustType<int>(LibvlcVideoAdjustOptionT.LibvlcAdjustEnable) == 1;
            }
            set
            {
                SetVideoAdjustType(LibvlcVideoAdjustOptionT.LibvlcAdjustEnable, Convert.ToInt32(value));
            }
        }

        public float Contrast
        {
            get
            {
                return GetVideoAdjustType<float>(LibvlcVideoAdjustOptionT.LibvlcAdjustContrast);
            }
            set
            {
                SetVideoAdjustType(LibvlcVideoAdjustOptionT.LibvlcAdjustContrast, value);
            }
        }

        public float Brightness
        {
            get
            {
                return GetVideoAdjustType<float>(LibvlcVideoAdjustOptionT.LibvlcAdjustBrightness);
            }
            set
            {
                SetVideoAdjustType(LibvlcVideoAdjustOptionT.LibvlcAdjustBrightness, value);
            }
        }

        public int Hue
        {
            get
            {
                return GetVideoAdjustType<int>(LibvlcVideoAdjustOptionT.LibvlcAdjustHue);
            }
            set
            {
                SetVideoAdjustType(LibvlcVideoAdjustOptionT.LibvlcAdjustHue, value);
            }
        }

        public float Saturation
        {
            get
            {
                return GetVideoAdjustType<float>(LibvlcVideoAdjustOptionT.LibvlcAdjustSaturation);
            }
            set
            {
                SetVideoAdjustType(LibvlcVideoAdjustOptionT.LibvlcAdjustSaturation, value);
            }
        }

        public float Gamma
        {
            get
            {
                return GetVideoAdjustType<float>(LibvlcVideoAdjustOptionT.LibvlcAdjustGamma);
            }
            set
            {
                SetVideoAdjustType(LibvlcVideoAdjustOptionT.LibvlcAdjustGamma, value);
            }
        }

        private void SetVideoAdjustType<T>(LibvlcVideoAdjustOptionT adjustType, T value)
        {
            if (typeof(int) == typeof(T))
            {
                Api.libvlc_video_set_adjust_int(_mediaPlayer, adjustType, (int)(object)value);
            }
            else
            {
                Api.libvlc_video_set_adjust_float(_mediaPlayer, adjustType, (float)(object)value);
            }
        }

        private T GetVideoAdjustType<T>(LibvlcVideoAdjustOptionT adjustType)
        {
            if (typeof(int) == typeof(T))
            {
                return (T)(object)Api.libvlc_video_get_adjust_int(_mediaPlayer, adjustType);
            }
            return (T)(object)Api.libvlc_video_get_adjust_float(_mediaPlayer, adjustType);
        }
    }

}

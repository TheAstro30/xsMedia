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
using System.Drawing;
using xsVlc.Common.Filters;

namespace xsVlc.Core.Filters
{
    internal class CropFilter : ICropFilter
    {
        private readonly IntPtr _mediaPlayer;
        private bool _enabled;

        public CropFilter(IntPtr hMediaPlayer)
        {
            _mediaPlayer = hMediaPlayer;
        }

        public bool Enabled
        {
            get
            {
                return _enabled;
            }
            set
            {
                _enabled = value;
                CropGeometry = _enabled ? CropArea.ToCropFilterString() : null;
            }
        }

        public Rectangle CropArea { get; set; }

        private string CropGeometry
        {
            set { Interop.Api.libvlc_video_set_crop_geometry(_mediaPlayer, value.ToUtf8()); }
        }
    }
}

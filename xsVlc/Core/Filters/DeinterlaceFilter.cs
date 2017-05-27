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

namespace xsVlc.Core.Filters
{
    internal class DeinterlaceFilter : IDeinterlaceFilter
    {
        private readonly IntPtr _mediaPlayer;
        private bool _enabled;
        private DeinterlaceMode _mode;

        public DeinterlaceFilter(IntPtr hMediaPlayer)
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
                Interop.Api.libvlc_video_set_deinterlace(_mediaPlayer, _enabled ? Mode.ToString().ToUtf8() : null);
            }
        }

        public DeinterlaceMode Mode
        {
            get
            {
                return _mode;
            }
            set
            {
                _mode = value;
                Interop.Api.libvlc_video_set_deinterlace(_mediaPlayer, _mode.ToString().ToUtf8());
            }
        }
    }
}

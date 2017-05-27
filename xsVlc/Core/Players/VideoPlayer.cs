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
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using xsVlc.Common;
using xsVlc.Common.Filters;
using xsVlc.Common.Players;
using xsVlc.Common.Rendering;
using xsVlc.Core.Filters;
using xsVlc.Core.Rendering;
using xsVlc.Core.Utils;

namespace xsVlc.Core.Players
{
    internal class VideoPlayer : AudioPlayer, IVideoPlayer
    {
        private MemoryRenderer _memRender;
        private MemoryRendererEx _memRenderEx;
        private IAdjustFilter _adjust;
        private ILogoFilter _logo;
        private IMarqueeFilter _marquee;
        private ICropFilter _crop;
        private IDeinterlaceFilter _deinterlace;

        private bool _keyInputEnabled = true;
        private bool _mouseInputEnabled = true;
        private readonly Dictionary<string, Enum> _aspectMapper;

        public VideoPlayer(IntPtr hMediaLib) : base(hMediaLib)
        {
            _aspectMapper = EnumUtils.GetEnumMapping(typeof(AspectRatioMode));
        }

        public override void Play()
        {
            base.Play();
            if (_memRender != null)
            {
                _memRender.StartTimer();
            }

            if (_memRenderEx != null)
            {
                _memRenderEx.StartTimer();
            }
        }

        public IntPtr WindowHandle
        {
            get
            {
                return Interop.Api.libvlc_media_player_get_hwnd(HMediaPlayer);
            }
            set
            {
                Interop.Api.libvlc_media_player_set_hwnd(HMediaPlayer, value);
            }
        }

        public void TakeSnapShot(uint stream, string path)
        {
            Interop.Api.libvlc_video_take_snapshot(HMediaPlayer, stream, path.ToUtf8(), 0, 0);
        }

        public float PlaybackRate
        {
            get
            {
                return Interop.Api.libvlc_media_player_get_rate(HMediaPlayer);
            }
            set
            {
                Interop.Api.libvlc_media_player_set_rate(HMediaPlayer, value);
            }
        }

        public float Fps
        {
            get
            {
                return Interop.Api.libvlc_media_player_get_fps(HMediaPlayer);
            }
        }

        public void NextFrame()
        {
            Interop.Api.libvlc_media_player_next_frame(HMediaPlayer);
        }

        public Size GetVideoSize(uint stream)
        {
            uint width;
            uint height;
            Interop.Api.libvlc_video_get_size(HMediaPlayer, stream, out width, out height);
            return new Size((int)width, (int)height);
        }

        public Point GetCursorPosition(uint stream)
        {
            int px;
            int py;
            Interop.Api.libvlc_video_get_cursor(HMediaPlayer, stream, out px, out py);
            return new Point(px, py);
        }

        public bool KeyInputEnabled
        {
            get
            {
                return _keyInputEnabled;
            }
            set
            {
                Interop.Api.libvlc_video_set_key_input(HMediaPlayer, value);
                _keyInputEnabled = value;
            }
        }

        public bool MouseInputEnabled
        {
            get
            {
                return _mouseInputEnabled;
            }
            set
            {
                Interop.Api.libvlc_video_set_mouse_input(HMediaPlayer, value);
                _mouseInputEnabled = value;
            }
        }

        public float VideoScale
        {
            get
            {
                return Interop.Api.libvlc_video_get_scale(HMediaPlayer);
            }
            set
            {
                Interop.Api.libvlc_video_set_scale(HMediaPlayer, value);
            }
        }

        public AspectRatioMode AspectRatio
        {
            get
            {
                var ip = Interop.Api.libvlc_video_get_aspect_ratio(HMediaPlayer);
                var str = Marshal.PtrToStringAnsi(ip);
                return str != null ? (AspectRatioMode)_aspectMapper[str] : AspectRatioMode.Default;
            }
            set
            {
                var val = EnumUtils.GetEnumDescription(value);
                Interop.Api.libvlc_video_set_aspect_ratio(HMediaPlayer, val.ToUtf8());
            }
        }

        public void SetSubtitleFile(string path)
        {
            Interop.Api.libvlc_video_set_subtitle_file(HMediaPlayer, path.ToUtf8());
        }

        public int Teletext
        {
            get
            {
                return Interop.Api.libvlc_video_get_teletext(HMediaPlayer);
            }
            set
            {
                Interop.Api.libvlc_video_set_teletext(HMediaPlayer, value);
            }
        }

        public void ToggleTeletext()
        {
            Interop.Api.libvlc_toggle_teletext(HMediaPlayer);
        }

        public bool PlayerWillPlay
        {
            get
            {
                return Interop.Api.libvlc_media_player_will_play(HMediaPlayer);
            }
        }

        public int VoutCount
        {
            get
            {
                return Interop.Api.libvlc_media_player_has_vout(HMediaPlayer);
            }
        }

        public bool IsSeekable
        {
            get
            {
                return Interop.Api.libvlc_media_player_is_seekable(HMediaPlayer);
            }
        }

        public bool CanPause
        {
            get
            {
                return Interop.Api.libvlc_media_player_can_pause(HMediaPlayer);
            }
        }

        public ICropFilter CropGeometry
        {
            get { return _crop ?? (_crop = new CropFilter(HMediaPlayer)); }
        }

        public IAdjustFilter Adjust
        {
            get { return _adjust ?? (_adjust = new AdjustFilter(HMediaPlayer)); }
        }

        public IMemoryRenderer CustomRenderer
        {
            get
            {
                if (_memRenderEx != null)
                {
                    throw new InvalidOperationException("CustomRenderer is mutually exclusive with CustomRendererEx");
                }

                return _memRender ?? (_memRender = new MemoryRenderer(HMediaPlayer));
            }
        }

        public IMemoryRendererEx CustomRendererEx
        {
            get
            {
                if (_memRender != null)
                {
                    throw new InvalidOperationException("CustomRendererEx is mutually exclusive with CustomRenderer");
                }

                return _memRenderEx ?? (_memRenderEx = new MemoryRendererEx(HMediaPlayer));
            }
        }

        public ILogoFilter Logo
        {
            get { return _logo ?? (_logo = new LogoFilter(HMediaPlayer)); }
        }

        public IMarqueeFilter Marquee
        {
            get { return _marquee ?? (_marquee = new MarqueeFilter(HMediaPlayer)); }
        }

        public IDeinterlaceFilter Deinterlace
        {
            get { return _deinterlace ?? (_deinterlace = new DeinterlaceFilter(HMediaPlayer)); }
        }

        protected override void Dispose(bool disposing)
        {
            if (_memRender != null)
            {
                _memRender.Dispose();
            }

            if (_memRenderEx != null)
            {
                _memRenderEx.Dispose();
            }

            base.Dispose(disposing);
        }        
    }
}

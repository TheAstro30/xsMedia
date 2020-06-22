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
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using xsVlc.Common;
using xsVlc.Common.Events;
using xsVlc.Common.Internal;
using xsVlc.Common.Media;
using xsVlc.Common.Structures;
using xsVlc.Core.Events;
using xsVlc.Interop;

namespace xsVlc.Core.Media
{
    internal class BasicMedia : DisposableBase, IMedia, INativePointer, IReferenceCount, IEventProvider
    {
        private IntPtr _media;
        private readonly IntPtr _mediaLib;        
        private string _path;
        private IntPtr _eventManager = IntPtr.Zero;
        private IMediaEvents _events;

        public BasicMedia(IntPtr hMediaLib)
        {
            _mediaLib = hMediaLib;
        }

        public BasicMedia(IntPtr hMedia, ReferenceCountAction refCountAction)
        {
            _media = hMedia;
            _path = Marshal.PtrToStringAnsi(Api.libvlc_media_get_mrl(hMedia));            
            switch (refCountAction)
            {
                case ReferenceCountAction.AddRef:
                    AddRef();
                    break;

                case ReferenceCountAction.Release:
                    Release();
                    break;
            }
        }

        protected override void Dispose(bool disposing)
        {
            Release();
        }

        public string Input
        {
            get
            {
                return _path;
            }
            set
            {
                _path = value;
                var bytes = Encoding.UTF8.GetBytes(_path);
                if (_path.StartsWith("dvd://") ||
                    _path.StartsWith("cdda://") ||
                    _path.StartsWith("vcd://") ||
                    _path.StartsWith("http://") ||
                    _path.StartsWith("https://") ||
                    _path.StartsWith("mms://") ||
                    _path.StartsWith("rtp://") ||
                    _path.StartsWith("rtsp://"))
                {
                    _media = Api.libvlc_media_new_location(_mediaLib, bytes); //changed from libvlc_media_new_path post 2.1.0
                }
                else
                {
                    _media = Api.libvlc_media_new_path(_mediaLib, bytes);
                }
            }
        }

        public MediaState State
        {
            get
            {
                return (MediaState)Api.libvlc_media_get_state(_media);
            }
        }

        public void AddOptions(IEnumerable<string> options)
        {
            if (options == null) { return; }
            foreach (var item in options.Where(item => !string.IsNullOrEmpty(item)))
            {
                Api.libvlc_media_add_option(_media, item.ToUtf8());
            }
        }

        public void AddOptionFlag(string option, int flag)
        {
            Api.libvlc_media_add_option_flag(_media, option.ToUtf8(), flag);
        }

        public IMedia Duplicate()
        {
            var clone = Api.libvlc_media_duplicate(_media);
            return new BasicMedia(clone, ReferenceCountAction.None);
        }

        public void Parse(bool aSync)
        {
            if (aSync)
            {
                Api.libvlc_media_parse_async(_media);
            }
            else
            {
                Api.libvlc_media_parse(_media);
            }
        }

        public bool IsParsed
        {
            get
            {
                return Api.libvlc_media_is_parsed(_media);
            }
        }

        public IntPtr Tag
        {
            get
            {
                return Api.libvlc_media_get_user_data(_media);
            }
            set
            {
                Api.libvlc_media_set_user_data(_media, value);
            }
        }

        public IMediaEvents Events
        {
            get { return _events ?? (_events = new MediaEventManager(this)); }
        }

        public MediaStatistics Statistics
        {
            get
            {
                LibvlcMediaStatsT t;
                Api.libvlc_media_get_stats(_media, out t);
                return t.ToMediaStatistics();
            }
        }

        public IMediaList SubItems
        {
            get
            {
                var hMediaList = Api.libvlc_media_subitems(_media);
                return hMediaList == IntPtr.Zero ? null : new MediaList(hMediaList, ReferenceCountAction.None);
            }
        }

        public IntPtr Pointer
        {
            get
            {
                return _media;
            }
        }

        public void AddRef()
        {
            Api.libvlc_media_retain(_media);
        }

        public void Release()
        {
            try
            {
                Api.libvlc_media_release(_media);
            }
            catch (AccessViolationException)
            {
                /* No real way to stop this */
            }
        }

        public IntPtr EventManagerHandle
        {
            get
            {
                if (_eventManager == IntPtr.Zero)
                {
                    _eventManager = Api.libvlc_media_event_manager(_media);
                }

                return _eventManager;
            }
        }

        public bool Equals(IMedia x, IMedia y)
        {
            var x1 = (INativePointer)x;
            var y1 = (INativePointer)y;
            return x1 != null && y1 != null && x1.Pointer == y1.Pointer;
        }

        public int GetHashCode(IMedia obj)
        {
            return ((INativePointer)obj).Pointer.GetHashCode();
        }
    }
}

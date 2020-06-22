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
using xsVlc.Common.Events;
using xsVlc.Common.Internal;
using xsVlc.Common.Media;
using xsVlc.Common.Players;
using xsVlc.Core.Events;
using xsVlc.Interop;

namespace xsVlc.Core.Players
{
    internal class MediaListPlayer : DisposableBase, IMediaListPlayer, IEventProvider
    {
        private readonly IntPtr _mediaListPlayer;
        private readonly IDiskPlayer _videoPlayer;
        private PlaybackMode _playbackMode = PlaybackMode.Default;
        private IntPtr _eventManager = IntPtr.Zero;
        private IMediaListPlayerEvents _mediaListEvents;

        public MediaListPlayer(IntPtr hMediaLib, IMediaList mediaList)
        {
            var mediaList1 = mediaList;
            _mediaListPlayer = Api.libvlc_media_list_player_new(hMediaLib);
            Api.libvlc_media_list_player_set_media_list(_mediaListPlayer, ((INativePointer)mediaList1).Pointer);
            mediaList1.Dispose();

            _videoPlayer = new DiskPlayer(hMediaLib);
            Api.libvlc_media_list_player_set_media_player(_mediaListPlayer, ((INativePointer)_videoPlayer).Pointer);
            _videoPlayer.Dispose();
        }

        protected override void Dispose(bool disposing)
        {
            _videoPlayer.Dispose();
            Api.libvlc_media_list_player_release(_mediaListPlayer);
        }

        public void PlayNext()
        {
            Api.libvlc_media_list_player_next(_mediaListPlayer);
        }

        public void PlayPrevious()
        {
            Api.libvlc_media_list_player_previous(_mediaListPlayer);
        }

        public PlaybackMode PlaybackMode
        {
            get
            {
                return _playbackMode;
            }
            set
            {
                Api.libvlc_media_list_player_set_playback_mode(_mediaListPlayer, (LibvlcPlaybackModeT)value);
                _playbackMode = value;
            }
        }

        public void PlayItemAt(int index)
        {
            Api.libvlc_media_list_player_play_item_at_index(_mediaListPlayer, index);
        }

        public MediaState PlayerState
        {
            get
            {
                return (MediaState)Api.libvlc_media_list_player_get_state(_mediaListPlayer);
            }
        }

        public IDiskPlayer InnerPlayer
        {
            get
            {
                return _videoPlayer;
            }
        }

        public IntPtr Pointer
        {
            get
            {
                return _mediaListPlayer;
            }
        }

        public void Play()
        {
            Api.libvlc_media_list_player_play(_mediaListPlayer);
        }

        public void Pause()
        {
            Api.libvlc_media_list_player_pause(_mediaListPlayer);
        }

        public void Stop()
        {
            Api.libvlc_media_list_player_stop(_mediaListPlayer);
        }

        public void Open(IMedia media)
        {
            _videoPlayer.Open(media);
        }

        public long Time
        {
            get
            {
                return _videoPlayer.Time;
            }
            set
            {
                _videoPlayer.Time = value;
            }
        }

        public float Position
        {
            get
            {
                return _videoPlayer.Position;
            }
            set
            {
                _videoPlayer.Position = value;
            }
        }

        public long Length
        {
            get
            {
                return _videoPlayer.Length;
            }
        }

        public IEventBroker Events
        {
            get
            {
                return _videoPlayer.Events;
            }
        }

        public bool IsPlaying
        {
            get
            {
                return _videoPlayer.IsPlaying;
            }
        }

        public IntPtr EventManagerHandle
        {
            get
            {
                if (_eventManager == IntPtr.Zero)
                {
                    _eventManager = Api.libvlc_media_list_player_event_manager(_mediaListPlayer);
                }
                return _eventManager;
            }
        }

        public IMediaListPlayerEvents MediaListPlayerEvents
        {
            get { return _mediaListEvents ?? (_mediaListEvents = new MediaListPlayerEventManager(this)); }
        }

        public bool Equals(IPlayer x, IPlayer y)
        {
            var x1 = (INativePointer)x;
            var y1 = (INativePointer)y;
            return x1 != null && y1 != null && x1.Pointer == y1.Pointer;
        }

        public int GetHashCode(IPlayer obj)
        {
            return ((INativePointer)obj).Pointer.GetHashCode();
        }
    }
}

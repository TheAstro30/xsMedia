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
using xsVlc.Common.Events;
using xsVlc.Common.Internal;
using xsVlc.Common.Media;
using xsVlc.Common.Players;
using xsVlc.Core.Events;

namespace xsVlc.Core.Players
{
    internal class BasicPlayer : DisposableBase, IPlayer, IEventProvider, IReferenceCount, INativePointer
    {
        protected readonly IntPtr HMediaPlayer;
        protected IntPtr HMediaLib;
        private readonly EventBroker _events;
        private IntPtr _eventManager = IntPtr.Zero;

        public BasicPlayer(IntPtr hMediaLib)
        {
            HMediaLib = hMediaLib;
            HMediaPlayer = Interop.Api.libvlc_media_player_new(HMediaLib);
            AddRef();
            _events = new EventBroker(this);
        }

        public virtual void Open(IMedia media)
        {
            Interop.Api.libvlc_media_player_set_media(HMediaPlayer, ((INativePointer)media).Pointer);
        }

        public virtual void Play()
        {
            Interop.Api.libvlc_media_player_play(HMediaPlayer);
        }

        public void Pause()
        {
            Interop.Api.libvlc_media_player_pause(HMediaPlayer);
        }

        public void Stop()
        {
            Interop.Api.libvlc_media_player_stop(HMediaPlayer);
        }

        public long Time
        {
            get
            {
                return Interop.Api.libvlc_media_player_get_time(HMediaPlayer);
            }
            set
            {
                Interop.Api.libvlc_media_player_set_time(HMediaPlayer, value);
            }
        }

        public float Position
        {
            get
            {
                return Interop.Api.libvlc_media_player_get_position(HMediaPlayer);
            }
            set
            {
                Interop.Api.libvlc_media_player_set_position(HMediaPlayer, value);
            }
        }

        public long Length
        {
            get
            {
                return Interop.Api.libvlc_media_player_get_length(HMediaPlayer);
            }
        }

        public IEventBroker Events
        {
            get
            {
                return _events;
            }
        }

        public bool IsPlaying
        {
            get
            {
                return Interop.Api.libvlc_media_player_is_playing(HMediaPlayer) == 1;
            }
        }

        protected override void Dispose(bool disposing)
        {
            Release();
        }

        public IntPtr EventManagerHandle
        {
            get
            {
                if (_eventManager == IntPtr.Zero)
                {
                    _eventManager = Interop.Api.libvlc_media_player_event_manager(HMediaPlayer);
                }

                return _eventManager;
            }
        }

        public void AddRef()
        {
            Interop.Api.libvlc_media_player_retain(HMediaPlayer);
        }

        public void Release()
        {
            try
            {
                Interop.Api.libvlc_media_player_release(HMediaPlayer);
            }
            catch (AccessViolationException)
            {
                /* No real way to stop this */
            }
        }

        public bool Equals(IPlayer x, IPlayer y)
        {
            var x1 = (INativePointer)x;
            var y1 = (INativePointer)y;
            return x1.Pointer == y1.Pointer;
        }

        public int GetHashCode(IPlayer obj)
        {
            return ((INativePointer)obj).Pointer.GetHashCode();
        }

        public IntPtr Pointer
        {
            get { return HMediaPlayer; }
        }

        public override bool Equals(object obj)
        {
            return Equals((IPlayer)obj, this);
        }

        public override int GetHashCode()
        {
            return HMediaPlayer.GetHashCode();
        }
    }
}

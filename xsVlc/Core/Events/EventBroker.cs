using System;
using System.Runtime.InteropServices;
using xsVlc.Common.Events;
using xsVlc.Core.Media;
using xsVlc.Interop;
using MediaPlayerLengthChanged = xsVlc.Common.Events.MediaPlayerLengthChanged;
using MediaPlayerMediaChanged = xsVlc.Common.Events.MediaPlayerMediaChanged;
using MediaPlayerPausableChanged = xsVlc.Common.Events.MediaPlayerPausableChanged;
using MediaPlayerPositionChanged = xsVlc.Common.Events.MediaPlayerPositionChanged;
using MediaPlayerSeekableChanged = xsVlc.Common.Events.MediaPlayerSeekableChanged;
using MediaPlayerSnapshotTaken = xsVlc.Common.Events.MediaPlayerSnapshotTaken;
using MediaPlayerTimeChanged = xsVlc.Common.Events.MediaPlayerTimeChanged;
using MediaPlayerTitleChanged = xsVlc.Common.Events.MediaPlayerTitleChanged;

namespace xsVlc.Core.Events
{
    internal class EventBroker : EventManager, IEventBroker
    {
        public EventBroker(IEventProvider eventProvider) : base(eventProvider)
        {

        }

        protected override void MediaPlayerEventOccured(ref LibvlcEventT libvlcEvent, IntPtr userData)
        {
            switch (libvlcEvent.type)
            {
                case LibvlcEventE.LibvlcMediaPlayerTimeChanged:
                    RaiseTimeChanged(libvlcEvent.MediaDescriptor.media_player_time_changed.new_time);
                    break;

                case LibvlcEventE.LibvlcMediaPlayerEndReached:
                    RaiseMediaEnded();
                    break;

                case LibvlcEventE.LibvlcMediaPlayerMediaChanged:
                    if (EventMediaChanged != null)
                    {
                        var media = new BasicMedia(libvlcEvent.MediaDescriptor.media_player_media_changed.new_media, ReferenceCountAction.AddRef);
                        EventMediaChanged(EventProvider, new MediaPlayerMediaChanged(media));
                        //media.Release();
                    }
                    break;

                case LibvlcEventE.LibvlcMediaPlayerNothingSpecial:
                    if (EventNothingSpecial != null)
                    {
                        EventNothingSpecial(EventProvider, EventArgs.Empty);
                    }
                    break;

                case LibvlcEventE.LibvlcMediaPlayerOpening:
                    if (EventPlayerOpening != null)
                    {
                        EventPlayerOpening(EventProvider, EventArgs.Empty);
                    }
                    break;

                case LibvlcEventE.LibvlcMediaPlayerBuffering:
                    if (EventPlayerBuffering != null)
                    {
                        EventPlayerBuffering(EventProvider, EventArgs.Empty);
                    }
                    break;

                case LibvlcEventE.LibvlcMediaPlayerPlaying:
                    if (EventPlayerPlaying != null)
                    {
                        EventPlayerPlaying(EventProvider, EventArgs.Empty);
                    }
                    break;

                case LibvlcEventE.LibvlcMediaPlayerPaused:
                    if (EventPlayerPaused != null)
                    {
                        EventPlayerPaused(EventProvider, EventArgs.Empty);
                    }
                    break;

                case LibvlcEventE.LibvlcMediaPlayerStopped:
                    if (EventPlayerStopped != null)
                    {
                        EventPlayerStopped(EventProvider, EventArgs.Empty);
                    }
                    break;

                case LibvlcEventE.LibvlcMediaPlayerForward:
                    if (EventPlayerForward != null)
                    {
                        EventPlayerForward(EventProvider, EventArgs.Empty);
                    }
                    break;

                case LibvlcEventE.LibvlcMediaPlayerBackward:
                    if (EventPlayerPaused != null)
                    {
                        EventPlayerPaused(EventProvider, EventArgs.Empty);
                    }
                    break;

                case LibvlcEventE.LibvlcMediaPlayerEncounteredError:
                    if (EventPlayerEncounteredError != null)
                    {
                        EventPlayerEncounteredError(EventProvider, EventArgs.Empty);
                    }
                    break;

                case LibvlcEventE.LibvlcMediaPlayerPositionChanged:
                    if (EventPlayerPositionChanged != null)
                    {
                        EventPlayerPositionChanged(EventProvider, new MediaPlayerPositionChanged(libvlcEvent.MediaDescriptor.media_player_position_changed.new_position));
                    }
                    break;

                case LibvlcEventE.LibvlcMediaPlayerSeekableChanged:
                    if (EventPlayerSeekableChanged != null)
                    {
                        EventPlayerSeekableChanged(EventProvider, new MediaPlayerSeekableChanged(libvlcEvent.MediaDescriptor.media_player_seekable_changed.new_seekable));
                    }
                    break;

                case LibvlcEventE.LibvlcMediaPlayerPausableChanged:
                    if (EventPlayerPausableChanged != null)
                    {
                        EventPlayerPausableChanged(EventProvider, new MediaPlayerPausableChanged(libvlcEvent.MediaDescriptor.media_player_pausable_changed.new_pausable));
                    }
                    break;

                case LibvlcEventE.LibvlcMediaPlayerTitleChanged:
                    if (EventPlayerTitleChanged != null)
                    {
                        EventPlayerTitleChanged(EventProvider, new MediaPlayerTitleChanged(libvlcEvent.MediaDescriptor.media_player_title_changed.new_title));
                    }
                    break;

                case LibvlcEventE.LibvlcMediaPlayerSnapshotTaken:
                    if (EventPlayerSnapshotTaken != null)
                    {
                        EventPlayerSnapshotTaken(EventProvider, new MediaPlayerSnapshotTaken(Marshal.PtrToStringAuto(libvlcEvent.MediaDescriptor.media_player_snapshot_taken.psz_filename)));
                    }
                    break;

                case LibvlcEventE.LibvlcMediaPlayerLengthChanged:
                    if (EventPlayerLengthChanged != null)
                    {
                        EventPlayerLengthChanged(EventProvider, new MediaPlayerLengthChanged(libvlcEvent.MediaDescriptor.media_player_length_changed.new_length));
                    }
                    break;
            }
        }

        private void RaiseTimeChanged(long p)
        {
            if (EventTimeChanged != null)
            {
                EventTimeChanged(EventProvider, new MediaPlayerTimeChanged(p));
            }
        }

        internal void RaiseMediaEnded()
        {
            if (EventMediaEnded != null)
            {
                EventMediaEnded.BeginInvoke(EventProvider, EventArgs.Empty, null, null);
            }
        }

        private event EventHandler<MediaPlayerTimeChanged> EventTimeChanged;
        public event EventHandler<MediaPlayerTimeChanged> TimeChanged
        {
            add
            {
                if (EventTimeChanged == null)
                {
                    Attach(LibvlcEventE.LibvlcMediaPlayerTimeChanged);
                }
                EventTimeChanged += value;
            }
            remove
            {
                if (EventTimeChanged != null)
                {
                    EventTimeChanged -= value;
                    if (EventTimeChanged == null)
                    {
                        Dettach(LibvlcEventE.LibvlcMediaPlayerTimeChanged);
                    }
                }
            }
        }

        private event EventHandler EventMediaEnded;
        public event EventHandler MediaEnded
        {
            add
            {
                if (EventMediaEnded == null)
                {
                    Attach(LibvlcEventE.LibvlcMediaPlayerEndReached);
                }
                EventMediaEnded += value;
            }
            remove
            {
                if (EventMediaEnded != null)
                {
                    EventMediaEnded -= value;
                    if (EventMediaEnded == null)
                    {
                        Dettach(LibvlcEventE.LibvlcMediaPlayerEndReached);
                    }
                }
            }
        }

        private event EventHandler<MediaPlayerMediaChanged> EventMediaChanged;
        public event EventHandler<MediaPlayerMediaChanged> MediaChanged
        {
            add
            {
                if (EventMediaChanged == null)
                {
                    Attach(LibvlcEventE.LibvlcMediaPlayerMediaChanged);
                }
                EventMediaChanged += value;
            }
            remove
            {
                if (EventMediaChanged != null)
                {
                    EventMediaChanged -= value;
                    if (EventMediaChanged == null)
                    {
                        Dettach(LibvlcEventE.LibvlcMediaPlayerMediaChanged);
                    }
                }
            }
        }

        private event EventHandler EventNothingSpecial;
        public event EventHandler NothingSpecial
        {
            add
            {
                if (EventNothingSpecial == null)
                {
                    Attach(LibvlcEventE.LibvlcMediaPlayerNothingSpecial);
                }
                EventNothingSpecial += value;
            }
            remove
            {
                if (EventNothingSpecial != null)
                {
                    EventNothingSpecial -= value;
                    if (EventNothingSpecial == null)
                    {
                        Dettach(LibvlcEventE.LibvlcMediaPlayerNothingSpecial);
                    }
                }
            }
        }

        private event EventHandler EventPlayerOpening;
        public event EventHandler PlayerOpening
        {
            add
            {
                if (EventPlayerOpening == null)
                {
                    Attach(LibvlcEventE.LibvlcMediaPlayerOpening);
                }
                EventPlayerOpening += value;
            }
            remove
            {
                if (EventPlayerOpening != null)
                {
                    EventPlayerOpening -= value;
                    if (EventPlayerOpening == null)
                    {
                        Dettach(LibvlcEventE.LibvlcMediaPlayerOpening);
                    }
                }
            }
        }

        private event EventHandler EventPlayerBuffering;
        public event EventHandler PlayerBuffering
        {
            add
            {
                if (EventPlayerBuffering == null)
                {
                    Attach(LibvlcEventE.LibvlcMediaPlayerBuffering);
                }
                EventPlayerBuffering += value;
            }
            remove
            {
                if (EventPlayerBuffering != null)
                {
                    EventPlayerBuffering -= value;
                    if (EventPlayerBuffering == null)
                    {
                        Dettach(LibvlcEventE.LibvlcMediaPlayerBuffering);
                    }
                }
            }
        }

        private event EventHandler EventPlayerPlaying;
        public event EventHandler PlayerPlaying
        {
            add
            {
                if (EventPlayerPlaying == null)
                {
                    Attach(LibvlcEventE.LibvlcMediaPlayerPlaying);
                }
                EventPlayerPlaying += value;
            }
            remove
            {
                if (EventPlayerPlaying != null)
                {
                    EventPlayerPlaying -= value;
                    if (EventPlayerPlaying == null)
                    {
                        Dettach(LibvlcEventE.LibvlcMediaPlayerPlaying);
                    }
                }
            }
        }

        private event EventHandler EventPlayerPaused;
        public event EventHandler PlayerPaused
        {
            add
            {
                if (EventPlayerPaused == null)
                {
                    Attach(LibvlcEventE.LibvlcMediaPlayerPaused);
                }
                EventPlayerPaused += value;
            }
            remove
            {
                if (EventPlayerPaused != null)
                {
                    EventPlayerPaused -= value;
                    if (EventPlayerPaused == null)
                    {
                        Dettach(LibvlcEventE.LibvlcMediaPlayerPaused);
                    }
                }
            }
        }

        private event EventHandler EventPlayerStopped;
        public event EventHandler PlayerStopped
        {
            add
            {
                if (EventPlayerStopped == null)
                {
                    Attach(LibvlcEventE.LibvlcMediaPlayerStopped);
                }
                EventPlayerStopped += value;
            }
            remove
            {
                if (EventPlayerStopped != null)
                {
                    EventPlayerStopped -= value;
                    if (EventPlayerStopped == null)
                    {
                        Dettach(LibvlcEventE.LibvlcMediaPlayerStopped);
                    }
                }
            }
        }

        private event EventHandler EventPlayerForward;
        public event EventHandler PlayerForward
        {
            add
            {
                if (EventPlayerForward == null)
                {
                    Attach(LibvlcEventE.LibvlcMediaPlayerForward);
                }
                EventPlayerForward += value;
            }
            remove
            {
                if (EventPlayerForward != null)
                {
                    EventPlayerForward -= value;
                    if (EventPlayerForward == null)
                    {
                        Dettach(LibvlcEventE.LibvlcMediaPlayerForward);
                    }
                }
            }
        }

        private event EventHandler EventPlayerBackward;
        public event EventHandler PlayerBackward
        {
            add
            {
                if (EventPlayerBackward == null)
                {
                    Attach(LibvlcEventE.LibvlcMediaPlayerBackward);
                }
                EventPlayerBackward += value;
            }
            remove
            {
                if (EventPlayerBackward != null)
                {
                    EventPlayerBackward -= value;
                    if (EventPlayerBackward == null)
                    {
                        Dettach(LibvlcEventE.LibvlcMediaPlayerBackward);
                    }
                }
            }
        }

        private event EventHandler EventPlayerEncounteredError;
        public event EventHandler PlayerEncounteredError
        {
            add
            {
                if (EventPlayerEncounteredError == null)
                {
                    Attach(LibvlcEventE.LibvlcMediaPlayerEncounteredError);
                }
                EventPlayerEncounteredError += value;
            }
            remove
            {
                if (EventPlayerEncounteredError != null)
                {
                    EventPlayerEncounteredError -= value;
                    if (EventPlayerEncounteredError == null)
                    {
                        Dettach(LibvlcEventE.LibvlcMediaPlayerEncounteredError);
                    }
                }
            }
        }

        private event EventHandler<MediaPlayerPositionChanged> EventPlayerPositionChanged;
        public event EventHandler<MediaPlayerPositionChanged> PlayerPositionChanged
        {
            add
            {
                if (EventPlayerPositionChanged == null)
                {
                    Attach(LibvlcEventE.LibvlcMediaPlayerPositionChanged);
                }
                EventPlayerPositionChanged += value;
            }
            remove
            {
                if (EventPlayerPositionChanged != null)
                {
                    EventPlayerPositionChanged -= value;
                    if (EventPlayerPositionChanged == null)
                    {
                        Dettach(LibvlcEventE.LibvlcMediaPlayerPositionChanged);
                    }
                }
            }
        }

        private event EventHandler<MediaPlayerSeekableChanged> EventPlayerSeekableChanged;
        public event EventHandler<MediaPlayerSeekableChanged> PlayerSeekableChanged
        {
            add
            {
                if (EventPlayerSeekableChanged == null)
                {
                    Attach(LibvlcEventE.LibvlcMediaPlayerSeekableChanged);
                }
                EventPlayerSeekableChanged += value;
            }
            remove
            {
                if (EventPlayerSeekableChanged != null)
                {
                    EventPlayerSeekableChanged -= value;
                    if (EventPlayerSeekableChanged == null)
                    {
                        Dettach(LibvlcEventE.LibvlcMediaPlayerSeekableChanged);
                    }
                }
            }
        }

        private event EventHandler<MediaPlayerPausableChanged> EventPlayerPausableChanged;
        public event EventHandler<MediaPlayerPausableChanged> PlayerPausableChanged
        {
            add
            {
                if (EventPlayerPausableChanged == null)
                {
                    Attach(LibvlcEventE.LibvlcMediaPlayerPausableChanged);
                }
                EventPlayerPausableChanged += value;
            }
            remove
            {
                if (EventPlayerPausableChanged != null)
                {
                    EventPlayerPausableChanged -= value;
                    if (EventPlayerPausableChanged == null)
                    {
                        Dettach(LibvlcEventE.LibvlcMediaPlayerPausableChanged);
                    }
                }
            }
        }

        private event EventHandler<MediaPlayerTitleChanged> EventPlayerTitleChanged;
        public event EventHandler<MediaPlayerTitleChanged> PlayerTitleChanged
        {
            add
            {
                if (EventPlayerTitleChanged == null)
                {
                    Attach(LibvlcEventE.LibvlcMediaPlayerTitleChanged);
                }
                EventPlayerTitleChanged += value;
            }
            remove
            {
                if (EventPlayerTitleChanged != null)
                {
                    EventPlayerTitleChanged -= value;
                    if (EventPlayerTitleChanged == null)
                    {
                        Dettach(LibvlcEventE.LibvlcMediaPlayerTitleChanged);
                    }
                }
            }
        }

        private event EventHandler<MediaPlayerSnapshotTaken> EventPlayerSnapshotTaken;
        public event EventHandler<MediaPlayerSnapshotTaken> PlayerSnapshotTaken
        {
            add
            {
                if (EventPlayerSnapshotTaken == null)
                {
                    Attach(LibvlcEventE.LibvlcMediaPlayerSnapshotTaken);
                }
                EventPlayerSnapshotTaken += value;
            }
            remove
            {
                if (EventPlayerSnapshotTaken != null)
                {
                    EventPlayerSnapshotTaken -= value;
                    if (EventPlayerSnapshotTaken == null)
                    {
                        Dettach(LibvlcEventE.LibvlcMediaPlayerSnapshotTaken);
                    }
                }
            }
        }

        private event EventHandler<MediaPlayerLengthChanged> EventPlayerLengthChanged;
        public event EventHandler<MediaPlayerLengthChanged> PlayerLengthChanged
        {
            add
            {
                if (EventPlayerLengthChanged == null)
                {
                    Attach(LibvlcEventE.LibvlcMediaPlayerLengthChanged);
                }
                EventPlayerLengthChanged += value;
            }
            remove
            {
                if (EventPlayerLengthChanged != null)
                {
                    EventPlayerLengthChanged -= value;
                    if (EventPlayerLengthChanged == null)
                    {
                        Dettach(LibvlcEventE.LibvlcMediaPlayerLengthChanged);
                    }
                }
            }
        }
    }
}

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
using xsVlc.Core.Media;
using xsVlc.Interop;

namespace xsVlc.Core.Events
{
    internal class MediaEventManager : EventManager, IMediaEvents
    {
        public MediaEventManager(IEventProvider eventProvider) : base(eventProvider)
        {
        }

        protected override void MediaPlayerEventOccured(ref LibvlcEventT libvlcEvent, IntPtr userData)
        {
            switch (libvlcEvent.type)
            {
                case LibvlcEventE.LibvlcMediaMetaChanged:
                    if (EventMetaDataChanged != null)
                    {
                        EventMetaDataChanged(EventProvider, new MediaMetaDataChange((MetaDataType)libvlcEvent.MediaDescriptor.media_meta_changed.meta_type));
                    }

                    break;
                case LibvlcEventE.LibvlcMediaSubItemAdded:
                    if (EventSubItemAdded != null)
                    {
                        var media = new BasicMedia(libvlcEvent.MediaDescriptor.media_subitem_added.new_child, ReferenceCountAction.AddRef);
                        EventSubItemAdded(EventProvider, new MediaNewSubItem(media));
                        media.Release();
                    }

                    break;
                case LibvlcEventE.LibvlcMediaDurationChanged:
                    if (EventDurationChanged != null)
                    {
                        EventDurationChanged(EventProvider, new MediaDurationChange(libvlcEvent.MediaDescriptor.media_duration_changed.new_duration));
                    }

                    break;
                case LibvlcEventE.LibvlcMediaParsedChanged:
                    if (EventParsedChanged != null)
                    {
                        EventParsedChanged(EventProvider, new MediaParseChange(Convert.ToBoolean(libvlcEvent.MediaDescriptor.media_parsed_changed.new_status)));
                    }

                    break;
                case LibvlcEventE.LibvlcMediaFreed:
                    if (EventMediaFreed != null)
                    {
                        EventMediaFreed(EventProvider, new MediaFree(libvlcEvent.MediaDescriptor.media_freed.md));
                    }

                    break;
                case LibvlcEventE.LibvlcMediaStateChanged:
                    if (EventStateChanged != null)
                    {
                        EventStateChanged(EventProvider, new MediaStateChange((MediaState)libvlcEvent.MediaDescriptor.media_state_changed.new_state));
                    }

                    break;
            }
        }

        #region IMediaEvents Members

        private event EventHandler<MediaMetaDataChange> EventMetaDataChanged;
        public event EventHandler<MediaMetaDataChange> MetaDataChanged
        {
            add
            {
                if (EventMetaDataChanged == null)
                {
                    Attach(LibvlcEventE.LibvlcMediaMetaChanged);
                }
                EventMetaDataChanged += value;
            }
            remove
            {
                if (EventMetaDataChanged != null)
                {
                    EventMetaDataChanged -= value;
                    if (EventMetaDataChanged == null)
                    {
                        Dettach(LibvlcEventE.LibvlcMediaMetaChanged);
                    }
                }
            }
        }

        private event EventHandler<MediaNewSubItem> EventSubItemAdded;
        public event EventHandler<MediaNewSubItem> SubItemAdded
        {
            add
            {
                if (EventSubItemAdded == null)
                {
                    Attach(LibvlcEventE.LibvlcMediaSubItemAdded);
                }
                EventSubItemAdded += value;
            }
            remove
            {
                if (EventSubItemAdded != null)
                {
                    EventSubItemAdded -= value;
                    if (EventSubItemAdded == null)
                    {
                        Dettach(LibvlcEventE.LibvlcMediaSubItemAdded);
                    }
                }
            }
        }

        private event EventHandler<MediaDurationChange> EventDurationChanged;
        public event EventHandler<MediaDurationChange> DurationChanged
        {
            add
            {
                if (EventDurationChanged == null)
                {
                    Attach(LibvlcEventE.LibvlcMediaDurationChanged);
                }
                EventDurationChanged += value;
            }
            remove
            {
                if (EventDurationChanged != null)
                {
                    EventDurationChanged -= value;
                    if (EventDurationChanged == null)
                    {
                        Dettach(LibvlcEventE.LibvlcMediaDurationChanged);
                    }
                }
            }
        }

        private event EventHandler<MediaParseChange> EventParsedChanged;
        public event EventHandler<MediaParseChange> ParsedChanged
        {
            add
            {
                if (EventParsedChanged == null)
                {
                    Attach(LibvlcEventE.LibvlcMediaParsedChanged);
                }
                EventParsedChanged += value;
            }
            remove
            {
                if (EventParsedChanged != null)
                {
                    EventParsedChanged -= value;
                    if (EventParsedChanged == null)
                    {
                        Dettach(LibvlcEventE.LibvlcMediaParsedChanged);
                    }
                }
            }
        }

        private event EventHandler<MediaFree> EventMediaFreed;
        public event EventHandler<MediaFree> MediaFreed
        {
            add
            {
                if (EventMediaFreed == null)
                {
                    Attach(LibvlcEventE.LibvlcMediaFreed);
                }
                EventMediaFreed += value;
            }
            remove
            {
                if (EventMediaFreed != null)
                {
                    EventMediaFreed -= value;
                    if (EventMediaFreed == null)
                    {
                        Dettach(LibvlcEventE.LibvlcMediaFreed);
                    }
                }
            }
        }

        private event EventHandler<MediaStateChange> EventStateChanged;
        public event EventHandler<MediaStateChange> StateChanged
        {
            add
            {
                if (EventStateChanged == null)
                {
                    Attach(LibvlcEventE.LibvlcMediaStateChanged);
                }
                EventStateChanged += value;
            }
            remove
            {
                if (EventStateChanged != null)
                {
                    EventStateChanged -= value;
                    if (EventStateChanged == null)
                    {
                        Dettach(LibvlcEventE.LibvlcMediaStateChanged);
                    }
                }
            }
        }

        #endregion
    }
}

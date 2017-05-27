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
using xsVlc.Core.Media;
using xsVlc.Interop;
using MediaListPlayerNextItemSet = xsVlc.Common.Events.MediaListPlayerNextItemSet;

namespace xsVlc.Core.Events
{
    internal class MediaListPlayerEventManager : EventManager, IMediaListPlayerEvents
    {
        public MediaListPlayerEventManager(IEventProvider eventProvider)
            : base(eventProvider)
        {

        }

        protected override void MediaPlayerEventOccured(ref LibvlcEventT libvlcEvent, IntPtr userData)
        {
            switch (libvlcEvent.type)
            {
                case LibvlcEventE.LibvlcMediaListPlayerPlayed:
                    if (EventMediaListPlayerPlayed != null)
                    {
                        EventMediaListPlayerPlayed(EventProvider, EventArgs.Empty);
                    }
                    break;
                case LibvlcEventE.LibvlcMediaListPlayerNextItemSet:
                    if (EventMediaListPlayerNextItemSet != null)
                    {
                        var media = new BasicMedia(libvlcEvent.MediaDescriptor.media_list_player_next_item_set.item, ReferenceCountAction.AddRef);
                        EventMediaListPlayerNextItemSet(EventProvider, new MediaListPlayerNextItemSet(media));
                        //media.Release();
                    }
                    break;
                case LibvlcEventE.LibvlcMediaListPlayerStopped:
                    if (EventMediaListPlayerStopped != null)
                    {
                        EventMediaListPlayerStopped(EventProvider, EventArgs.Empty);
                    }
                    break;
            }
        }

        #region IMediaListPlayerEvents Members

        private event EventHandler EventMediaListPlayerPlayed;
        public event EventHandler MediaListPlayerPlayed
        {
            add
            {
                if (EventMediaListPlayerPlayed == null)
                {
                    Attach(LibvlcEventE.LibvlcMediaListPlayerPlayed);
                }
                EventMediaListPlayerPlayed += value;
            }
            remove
            {
                if (EventMediaListPlayerPlayed != null)
                {
                    EventMediaListPlayerPlayed -= value;
                    if (EventMediaListPlayerPlayed == null)
                    {
                        Dettach(LibvlcEventE.LibvlcMediaListPlayerPlayed);
                    }
                }
            }
        }

        private event EventHandler<MediaListPlayerNextItemSet> EventMediaListPlayerNextItemSet;
        public event EventHandler<MediaListPlayerNextItemSet> MediaListPlayerNextItemSet
        {
            add
            {
                if (EventMediaListPlayerNextItemSet == null)
                {
                    Attach(LibvlcEventE.LibvlcMediaListPlayerNextItemSet);
                }
                EventMediaListPlayerNextItemSet += value;
            }
            remove
            {
                if (EventMediaListPlayerNextItemSet != null)
                {
                    EventMediaListPlayerNextItemSet -= value;
                    if (EventMediaListPlayerNextItemSet == null)
                    {
                        Dettach(LibvlcEventE.LibvlcMediaListPlayerNextItemSet);
                    }
                }
            }
        }

        private event EventHandler EventMediaListPlayerStopped;
        public event EventHandler MediaListPlayerStopped
        {
            add
            {
                if (EventMediaListPlayerStopped == null)
                {
                    Attach(LibvlcEventE.LibvlcMediaListPlayerStopped);
                }
                EventMediaListPlayerStopped += value;
            }
            remove
            {
                if (EventMediaListPlayerStopped != null)
                {
                    EventMediaListPlayerStopped -= value;
                    if (EventMediaListPlayerStopped == null)
                    {
                        Dettach(LibvlcEventE.LibvlcMediaListPlayerStopped);
                    }
                }
            }
        }

        #endregion
    }
}

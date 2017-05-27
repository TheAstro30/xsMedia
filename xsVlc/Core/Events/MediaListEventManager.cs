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
using MediaListItemAdded = xsVlc.Common.Events.MediaListItemAdded;
using MediaListItemDeleted = xsVlc.Common.Events.MediaListItemDeleted;
using MediaListWillAddItem = xsVlc.Common.Events.MediaListWillAddItem;
using MediaListWillDeleteItem = xsVlc.Common.Events.MediaListWillDeleteItem;

namespace xsVlc.Core.Events
{
    class MediaListEventManager : EventManager, IMediaListEvents
    {
        public MediaListEventManager(IEventProvider eventProvider)
            : base(eventProvider)
        {
        }

        protected override void MediaPlayerEventOccured(ref LibvlcEventT libvlcEvent, IntPtr userData)
        {
            switch (libvlcEvent.type)
            {
                case LibvlcEventE.LibvlcMediaListItemAdded:
                    if (EventItemAdded != null)
                    {
                        var media = new BasicMedia(libvlcEvent.MediaDescriptor.media_list_item_added.item, ReferenceCountAction.AddRef);
                        EventItemAdded(EventProvider, new MediaListItemAdded(media, libvlcEvent.MediaDescriptor.media_list_item_added.index));
                        media.Release();
                    }
                    break;

                case LibvlcEventE.LibvlcMediaListWillAddItem:
                    if (EventWillAddItem != null)
                    {
                        var media2 = new BasicMedia(libvlcEvent.MediaDescriptor.media_list_will_add_item.item, ReferenceCountAction.AddRef);
                        EventWillAddItem(EventProvider, new MediaListWillAddItem(media2, libvlcEvent.MediaDescriptor.media_list_will_add_item.index));
                        media2.Release();
                    }
                    break;

                case LibvlcEventE.LibvlcMediaListItemDeleted:
                    if (EventItemDeleted != null)
                    {
                        var media3 = new BasicMedia(libvlcEvent.MediaDescriptor.media_list_item_deleted.item, ReferenceCountAction.AddRef);
                        EventItemDeleted(EventProvider, new MediaListItemDeleted(media3, libvlcEvent.MediaDescriptor.media_list_item_deleted.index));
                        media3.Release();
                    }
                    break;

                case LibvlcEventE.LibvlcMediaListWillDeleteItem:
                    if (EventWillDeleteItem != null)
                    {
                        var media4 = new BasicMedia(libvlcEvent.MediaDescriptor.media_list_will_delete_item.item, ReferenceCountAction.AddRef);
                        EventWillDeleteItem(EventProvider, new MediaListWillDeleteItem(media4, libvlcEvent.MediaDescriptor.media_list_will_delete_item.index));
                        media4.Release();
                    }
                    break;
            }
        }

        #region IMediaListEvents Members

        private event EventHandler<MediaListItemAdded> EventItemAdded;
        public event EventHandler<MediaListItemAdded> ItemAdded
        {
            add
            {
                if (EventItemAdded == null)
                {
                    Attach(LibvlcEventE.LibvlcMediaListItemAdded);
                }
                EventItemAdded += value;
            }
            remove
            {
                if (EventItemAdded != null)
                {
                    EventItemAdded -= value;
                    if (EventItemAdded == null)
                    {
                        Dettach(LibvlcEventE.LibvlcMediaListItemAdded);
                    }
                }
            }
        }

        private event EventHandler<MediaListWillAddItem> EventWillAddItem;
        public event EventHandler<MediaListWillAddItem> WillAddItem
        {
            add
            {
                if (EventWillAddItem != null)
                {
                    Attach(LibvlcEventE.LibvlcMediaListWillAddItem);
                }
                EventWillAddItem += value;
            }
            remove
            {
                if (EventWillAddItem != null)
                {
                    EventWillAddItem -= value;
                    if (EventWillAddItem == null)
                    {
                        Dettach(LibvlcEventE.LibvlcMediaListWillAddItem);
                    }
                }
            }
        }

        private event EventHandler<MediaListItemDeleted> EventItemDeleted;
        public event EventHandler<MediaListItemDeleted> ItemDeleted
        {
            add
            {
                if (EventItemDeleted == null)
                {
                    Attach(LibvlcEventE.LibvlcMediaListItemDeleted);
                }
                EventItemDeleted += value;
            }
            remove
            {
                if (EventItemDeleted != null)
                {
                    EventItemDeleted -= value;
                    if (EventItemDeleted == null)
                    {
                        Dettach(LibvlcEventE.LibvlcMediaListItemDeleted);
                    }
                }
            }
        }

        private event EventHandler<MediaListWillDeleteItem> EventWillDeleteItem;
        public event EventHandler<MediaListWillDeleteItem> WillDeleteItem
        {
            add
            {
                if (EventWillDeleteItem == null)
                {
                    Attach(LibvlcEventE.LibvlcMediaListWillDeleteItem);
                }
                EventWillDeleteItem += value;
            }
            remove
            {
                if (EventWillDeleteItem != null)
                {
                    EventWillDeleteItem -= value;
                    if (EventWillDeleteItem == null)
                    {
                        Dettach(LibvlcEventE.LibvlcMediaListWillDeleteItem);
                    }
                }
            }
        }

        #endregion
    }
}

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
using System.Collections;
using System.Collections.Generic;
using xsVlc.Common.Events;
using xsVlc.Common.Internal;
using xsVlc.Common.Media;
using xsVlc.Core.Events;

namespace xsVlc.Core.Media
{
    internal class MediaList : DisposableBase, IMediaList, INativePointer, IEventProvider, IReferenceCount
    {
        protected IntPtr HMediaList;
        protected IntPtr HMediaLib;
        private IntPtr _eventManager = IntPtr.Zero;
        private IMediaListEvents _events;

        public MediaList(IntPtr hMediaLib)
        {
            HMediaLib = hMediaLib;
            HMediaList = Interop.Api.libvlc_media_list_new(hMediaLib);
        }

        public MediaList(IntPtr hMediaList, ReferenceCountAction action)
        {
            HMediaList = hMediaList;
            switch (action)
            {
                case ReferenceCountAction.AddRef:
                    AddRef();
                    break;
                case ReferenceCountAction.Release:
                    Release();
                    break;
            }
        }

        protected struct MediaListLock : IDisposable
        {
            private readonly IntPtr _mediaList;

            public MediaListLock(IntPtr hMediaList)
            {
                _mediaList = hMediaList;
                Interop.Api.libvlc_media_list_lock(_mediaList);
            }

            public void Dispose()
            {
                Interop.Api.libvlc_media_list_unlock(_mediaList);
            }
        }

        public int IndexOf(IMedia item)
        {
            using (new MediaListLock(HMediaList))
            {
                return Interop.Api.libvlc_media_list_index_of_item(HMediaList, ((INativePointer)item).Pointer);
            }
        }

        public void Insert(int index, IMedia item)
        {
            using (new MediaListLock(HMediaList))
            {
                Interop.Api.libvlc_media_list_insert_media(HMediaList, ((INativePointer)item).Pointer, index);
            }
        }

        public void RemoveAt(int index)
        {
            using (new MediaListLock(HMediaList))
            {
                Interop.Api.libvlc_media_list_remove_index(HMediaList, index);
            }
        }

        public IMedia this[int index]
        {
            get
            {
                using (new MediaListLock(HMediaList))
                {
                    var hMedia = Interop.Api.libvlc_media_list_item_at_index(HMediaList, index);
                    if (hMedia == IntPtr.Zero)
                    {
                        throw new Exception(string.Format("Media at index {0} not found", index));
                    }

                    return new BasicMedia(hMedia, ReferenceCountAction.AddRef);
                }
            }
            set
            {
                Insert(index, value);
            }
        }

        public void Add(IMedia item)
        {
            using (new MediaListLock(HMediaList))
            {
                Interop.Api.libvlc_media_list_add_media(HMediaList, ((INativePointer)item).Pointer);
            }
        }

        public void Clear()
        {
            using (new MediaListLock(HMediaList))
            {
                var count = Interop.Api.libvlc_media_list_count(HMediaList);
                for (var i = 0; i < count; i++)
                {
                    Interop.Api.libvlc_media_list_remove_index(HMediaList, 0);
                }
            }
        }

        public bool Contains(IMedia item)
        {
            return IndexOf(item) != -1;
        }

        public void CopyTo(IMedia[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public int Count
        {
            get
            {
                using (new MediaListLock(HMediaList))
                {
                    return Interop.Api.libvlc_media_list_count(HMediaList);
                }
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return Interop.Api.libvlc_media_list_is_readonly(HMediaList) == 0;
            }
        }

        public bool Remove(IMedia item)
        {
            var index = IndexOf(item);
            if (index < 0)
            {
                return false;
            }

            RemoveAt(index);
            return true;
        }

        public IEnumerator<IMedia> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        protected override void Dispose(bool disposing)
        {
            Release();
        }

        public IntPtr Pointer
        {
            get
            {
                return HMediaList;
            }
        }

        public IntPtr EventManagerHandle
        {
            get
            {
                if (_eventManager == IntPtr.Zero)
                {
                    _eventManager = Interop.Api.libvlc_media_list_event_manager(HMediaList);
                }

                return _eventManager;
            }
        }

        public IMediaListEvents Events
        {
            get { return _events ?? (_events = new MediaListEventManager(this)); }
        }

        public void AddRef()
        {
            Interop.Api.libvlc_media_list_retain(HMediaList);
        }

        public void Release()
        {
            try
            {
                Interop.Api.libvlc_media_list_release(HMediaList);
            }
            catch (AccessViolationException)
            {
                /* No real way to stop this */
            }
        }
    }
}

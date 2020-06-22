using System;
using System.Runtime.InteropServices;
using xsVlc.Common.Discovery;
using xsVlc.Common.Events;
using xsVlc.Common.Internal;
using xsVlc.Common.Media;
using xsVlc.Core.Events;
using xsVlc.Core.Media;

namespace xsVlc.Core.Discovery
{
    internal class MediaDiscoverer : DisposableBase, IMediaDiscoverer, INativePointer, IEventProvider
    {
        private readonly IntPtr _discovery;
        private IMediaDiscoveryEvents _events;

        public MediaDiscoverer(IntPtr hMediaLib, string name)
        {
            _discovery = Interop.Api.libvlc_media_discoverer_new_from_name(hMediaLib, name.ToUtf8());
        }

        protected override void Dispose(bool disposing)
        {
            Interop.Api.libvlc_media_discoverer_release(_discovery);
        }

        public bool IsRunning
        {
            get
            {
                return Interop.Api.libvlc_media_discoverer_is_running(_discovery) == 1;
            }
        }

        public string LocalizedName
        {
            get
            {
                var ip = Interop.Api.libvlc_media_discoverer_localized_name(_discovery);
                return Marshal.PtrToStringAnsi(ip);
            }
        }

        public IMediaList MediaList
        {
            get
            {
                return new MediaList(Interop.Api.libvlc_media_discoverer_media_list(_discovery), ReferenceCountAction.None);
            }
        }

        public IntPtr EventManagerHandle
        {
            get
            {
                return Interop.Api.libvlc_media_discoverer_event_manager(_discovery);
            }
        }

        public IntPtr Pointer
        {
            get
            {
                return _discovery;
            }
        }

        public IMediaDiscoveryEvents Events
        {
            get { return _events ?? (_events = new MediaDiscoveryEventManager(this)); }
        }
    }
}

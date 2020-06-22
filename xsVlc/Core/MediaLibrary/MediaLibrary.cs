using System;
using xsVlc.Common.Internal;
using xsVlc.Common.Media;
using xsVlc.Common.MediaLibrary;
using xsVlc.Core.Media;

namespace xsVlc.Core.MediaLibrary
{
    internal class MediaLibraryImpl : DisposableBase, IReferenceCount, INativePointer, IMediaLibrary
    {
        private readonly IntPtr _mediaLib;

        public MediaLibraryImpl(IntPtr mediaLib)
        {
            _mediaLib = Interop.Api.libvlc_media_library_new(mediaLib);
        }

        protected override void Dispose(bool disposing)
        {
            Release();
        }

        public void Load()
        {
            Interop.Api.libvlc_media_library_load(_mediaLib);
        }

        public IMediaList MediaList
        {
            get
            {
                return new MediaList(Interop.Api.libvlc_media_library_media_list(_mediaLib));
            }
        }

        public void AddRef()
        {
            Interop.Api.libvlc_media_library_retain(_mediaLib);
        }

        public void Release()
        {
            Interop.Api.libvlc_media_library_release(_mediaLib);
        }

        public IntPtr Pointer
        {
            get
            {
                return _mediaLib;
            }
        }
    }
}

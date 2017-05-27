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
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Threading;
using xsVlc.Common.Media;
using xsVlc.Common.Structures;
using xsVlc.Core.Structures;
using xsVlc.Core.Utils;
using xsVlc.Interop;

namespace xsVlc.Core.Media
{
    [MaxLibVlcVersion("1.1.x")]
    internal sealed unsafe class VideoInputMedia : BasicMedia, IVideoInputMedia
    {
        private BitmapFormat _format;
        private PixelData _data = default(PixelData);
        private readonly object _lock = new object();
        private IntPtr _pLock;
        private IntPtr _pUnlock;
        private GCHandle _pData;
        private List<Delegate> _callbacks = new List<Delegate>();

        public VideoInputMedia(IntPtr hMediaLib) : base(hMediaLib)
        {
            CallbackEventHandler pLock = LockCallback;
            CallbackEventHandler pUnlock = UnlockCallback;

            _pLock = Marshal.GetFunctionPointerForDelegate(pLock);
            _pUnlock = Marshal.GetFunctionPointerForDelegate(pUnlock);

            _callbacks.Add(pLock);
            _callbacks.Add(pUnlock);
        }

        #region IVideoInputMedia Members

        public void AddFrame(Bitmap frame)
        {
            Monitor.Enter(_lock);

            try
            {
                var rect = new Rectangle(0, 0, frame.Width, frame.Height);
                var bmpData = frame.LockBits(rect, ImageLockMode.ReadOnly, frame.PixelFormat);
                var pData = bmpData.Scan0.ToPointer();
                MemoryHeap.CopyMemory(_data.PPixelData, pData, _data.Size);

                frame.UnlockBits(bmpData);
            }
            finally
            {
                Monitor.Exit(_lock);
            }
        }

        public void SetFormat(BitmapFormat format)
        {
            if (_data == default(PixelData))
            {
                _format = format;
                _data = new PixelData(_format.ImageSize);
                _pData = GCHandle.Alloc(_data, GCHandleType.Pinned);
                InitMedia();
            }
            else
            {
                throw new InvalidOperationException("Bitmap format already set");
            }
        }

        #endregion

        private void InitMedia()
        {
            var options = new List<string>
                              {
                                  ":codec=invmem",
                                  string.Format(":invmem-width={0}", _format.Width),
                                  string.Format(":invmem-height={0}", _format.Height),
                                  string.Format(":invmem-lock={0}", _pLock.ToInt64()),
                                  string.Format(":invmem-unlock={0}", _pUnlock.ToInt64()),
                                  string.Format(":invmem-chroma={0}", _format.Chroma),
                                  string.Format(":invmem-data={0}", _pData.AddrOfPinnedObject().ToInt64())
                              };
            AddOptions(options);
        }

        void* LockCallback(void* data)
        {
            Monitor.Enter(_lock);
            var pd = (PixelData*)data;
            return pd->PPixelData;
        }

        void* UnlockCallback(void* data)
        {
            Monitor.Exit(_lock);
            var pd = (PixelData*)data;
            return pd->PPixelData;
        }

        protected override void Dispose(bool disposing)
        {
            _data.Dispose();
            _pData.Free();

            if (disposing)
            {
                _callbacks = null;
            }

            base.Dispose(disposing);
        }
    }
}

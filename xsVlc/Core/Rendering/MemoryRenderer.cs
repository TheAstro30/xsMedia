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
using System.Runtime.InteropServices;
using System.Timers;
using xsVlc.Common;
using xsVlc.Common.Rendering;
using xsVlc.Common.Structures;
using xsVlc.Core.Structures;
using xsVlc.Core.Utils;

namespace xsVlc.Core.Rendering
{
    internal sealed unsafe class MemoryRenderer : DisposableBase, IMemoryRenderer
    {
        private readonly IntPtr _mediaPlayer;
        private NewFrameEventHandler _callback;
        private BitmapFormat _format;
        private readonly Timer _timer = new Timer();
        private volatile int _frameRate;
        private int _latestFps;
        private readonly object _lock = new object();
        private readonly List<Delegate> _callbacks = new List<Delegate>();

        private readonly IntPtr _lockCallback;
        private readonly IntPtr _unlockCallback;
        private readonly IntPtr _displayCallback;
        private Action<Exception> _excHandler;
        private GCHandle _pixelDataPtr = default(GCHandle);
        private PixelData _pixelData;

        private void* _buffer = null;

        public MemoryRenderer(IntPtr hMediaPlayer)
        {
            _mediaPlayer = hMediaPlayer;

            LockEventHandler leh = OnpLock;
            UnlockEventHandler ueh = OnpUnlock;
            DisplayEventHandler deh = OnpDisplay;

            _lockCallback = Marshal.GetFunctionPointerForDelegate(leh);
            _unlockCallback = Marshal.GetFunctionPointerForDelegate(ueh);
            _displayCallback = Marshal.GetFunctionPointerForDelegate(deh);

            _callbacks.Add(leh);
            _callbacks.Add(deh);
            _callbacks.Add(ueh);

            _timer.Elapsed += TimerElapsed;
            _timer.Interval = 1000;
        }

        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            _latestFps = _frameRate;
            _frameRate = 0;
        }

        private static void* OnpLock(void* opaque, void** plane)
        {
            var px = (PixelData*)opaque;
            *plane = px->PPixelData;
            return null;
        }

        private static void OnpUnlock(void* opaque, void* picture, void** plane)
        {

        }

        private void OnpDisplay(void* opaque, void* picture)
        {
            lock (_lock)
            {
                try
                {
                    var px = (PixelData*)opaque;
                    MemoryHeap.CopyMemory(_buffer, px->PPixelData, px->Size);

                    _frameRate++;
                    if (_callback == null) { return; }
                    using (var frame = GetBitmap())
                    {
                        _callback(frame);
                    }
                }
                catch (Exception ex)
                {
                    if (_excHandler != null)
                    {
                        _excHandler(ex);
                    }
                    else
                    {
                        throw;
                    }
                }
            }
        }

        private Bitmap GetBitmap()
        {
            return new Bitmap(_format.Width, _format.Height, _format.Pitch, _format.PixelFormat, new IntPtr(_buffer));
        }

        public void SetCallback(NewFrameEventHandler callback)
        {
            _callback = callback;
        }

        public void SetFormat(BitmapFormat format)
        {
            _format = format;

            Interop.Api.libvlc_video_set_format(_mediaPlayer, _format.Chroma.ToUtf8(), _format.Width, _format.Height, _format.Pitch);
            _buffer = MemoryHeap.Alloc(_format.ImageSize);

            _pixelData = new PixelData(_format.ImageSize);
            _pixelDataPtr = GCHandle.Alloc(_pixelData, GCHandleType.Pinned);
            Interop.Api.libvlc_video_set_callbacks(_mediaPlayer, _lockCallback, _unlockCallback, _displayCallback, _pixelDataPtr.AddrOfPinnedObject());
        }

        internal void StartTimer()
        {
            _timer.Start();
        }

        public int ActualFrameRate
        {
            get
            {
                return _latestFps;
            }
        }

        public Bitmap CurrentFrame
        {
            get
            {
                lock (_lock)
                {
                    return GetBitmap();
                }
            }
        }

        public void SetExceptionHandler(Action<Exception> handler)
        {
            _excHandler = handler;
        }

        protected override void Dispose(bool disposing)
        {
            IntPtr zero = IntPtr.Zero;
            Interop.Api.libvlc_video_set_callbacks(_mediaPlayer, zero, zero, zero, zero);

            _pixelDataPtr.Free();
            _pixelData.Dispose();

            MemoryHeap.Free(_buffer);

            if (disposing)
            {
                _timer.Dispose();
                _callback = null;
                _callbacks.Clear();
            }
        }
    }
}

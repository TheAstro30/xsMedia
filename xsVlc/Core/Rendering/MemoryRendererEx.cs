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
using System.Runtime.InteropServices;
using System.Timers;
using xsVlc.Common;
using xsVlc.Common.Rendering;
using xsVlc.Common.Structures;
using xsVlc.Core.Structures;

namespace xsVlc.Core.Rendering
{
    internal sealed unsafe class MemoryRendererEx : DisposableBase, IMemoryRendererEx
    {
        private readonly IntPtr _mediaPlayer;
        private NewFrameDataEventHandler _callback;
        private readonly Timer _timer = new Timer();
        private volatile int _frameRate;
        private readonly object _lock = new object();
        private Func<BitmapFormat, BitmapFormat> _formatSetupCb;
        private readonly IntPtr[] _planes = new IntPtr[3];
        private BitmapFormat _format;
        private Action<Exception> _excHandler;

        private PlanarPixelData _pixelData = default(PlanarPixelData);

        public MemoryRendererEx(IntPtr hMediaPlayer)
        {
            _mediaPlayer = hMediaPlayer;

            LockEventHandler leh = OnpLock;
            DisplayEventHandler deh = OnpDisplay;
            VideoFormatCallback formatCallback = OnFormatCallback;

            var formatCallback1 = Marshal.GetFunctionPointerForDelegate(formatCallback);
            var lockCallback = Marshal.GetFunctionPointerForDelegate(leh);
            var displayCallback = Marshal.GetFunctionPointerForDelegate(deh);

            _timer.Elapsed += TimerElapsed;
            _timer.Interval = 1000;

            Interop.Api.libvlc_video_set_format_callbacks(_mediaPlayer, formatCallback1, IntPtr.Zero);
            Interop.Api.libvlc_video_set_callbacks(_mediaPlayer, lockCallback, IntPtr.Zero, displayCallback, IntPtr.Zero);
        }

        private int OnFormatCallback(void** opaque, char* chroma, int* width, int* height, int* pitches, int* lines)
        {
            var pChroma = new IntPtr(chroma);
            var chromaStr = Marshal.PtrToStringAnsi(pChroma);

            //ChromaType type;
            //if (!Enum.TryParse(chromaStr, out type))
            //{
            //    var exc = new ArgumentException("Unsupported chroma type " + chromaStr);
            //    if (_excHandler != null)
            //    {
            //        _excHandler(exc);
            //        return 0;
            //    }
            //    throw exc;
            //}
            if (chromaStr == null)
            {
                return 0;
            }
            ChromaType type;
            try
            {
                type = (ChromaType)Enum.Parse(typeof(ChromaType), chromaStr);
            }
            catch (Exception)
            {
                var exc = new ArgumentException("Unsupported chroma type " + chromaStr);
                if (_excHandler == null)
                {
                    throw exc;
                }
                _excHandler(exc);
                return 1;
            }

            _format = new BitmapFormat(*width, *height, type);
            if (_formatSetupCb != null)
            {
                _format = _formatSetupCb(_format);
            }

            Marshal.Copy(_format.Chroma.ToUtf8(), 0, pChroma, 4);
            *width = _format.Width;
            *height = _format.Height;

            for (var i = 0; i < _format.Planes; i++)
            {
                pitches[i] = _format.Pitches[i];
                lines[i] = _format.Lines[i];
            }
            _pixelData = new PlanarPixelData(_format.PlaneSizes);
            return _format.Planes;
        }

        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            ActualFrameRate = _frameRate;
            _frameRate = 0;
        }

        private void* OnpLock(void* opaque, void** plane)
        {
            for (var i = 0; i < _pixelData.Sizes.Length; i++)
            {
                plane[i] = _pixelData.Data[i];
            }

            return null;
        }

        private void OnpDisplay(void* opaque, void* picture)
        {
            lock (_lock)
            {
                try
                {
                    _frameRate++;
                    for (var i = 0; i < _pixelData.Sizes.Length; i++)
                    {
                        _planes[i] = new IntPtr(_pixelData.Data[i]);
                    }

                    if (_callback == null) { return; }
                    var pf = GetFrame();
                    _callback(pf);
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

        internal void StartTimer()
        {
            _timer.Start();
        }

        private PlanarFrame GetFrame()
        {
            return new PlanarFrame(_planes, _format.PlaneSizes);
        }

        public void SetCallback(NewFrameDataEventHandler callback)
        {
            _callback = callback;
        }

        public PlanarFrame CurrentFrame
        {
            get
            {
                lock (_lock)
                {
                    return GetFrame();
                }
            }
        }

        public void SetFormatSetupCallback(Func<BitmapFormat, BitmapFormat> setupCallback)
        {
            _formatSetupCb = setupCallback;
        }

        public int ActualFrameRate { get; private set; }

        public void SetExceptionHandler(Action<Exception> handler)
        {
            _excHandler = handler;
        }

        protected override void Dispose(bool disposing)
        {
            var zero = IntPtr.Zero;
            Interop.Api.libvlc_video_set_callbacks(_mediaPlayer, zero, zero, zero, zero);

            if (_pixelData != default(PlanarPixelData))
            {
                _pixelData.Dispose();
            }

            if (!disposing)
            {
                return;
            }
            _timer.Dispose();
            _formatSetupCb = null;
            _excHandler = null;
            _callback = null;
        }
    }
}

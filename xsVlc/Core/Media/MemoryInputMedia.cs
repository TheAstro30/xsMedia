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
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Drawing.Imaging;
using xsVlc.Common.Media;
using xsVlc.Common.Structures;
using xsVlc.Core.Utils;

namespace xsVlc.Core.Media
{
    internal sealed unsafe class MemoryInputMedia : BasicMedia, IMemoryInputMedia
    {
        private IntPtr _lock;
        private IntPtr _unlock;
        private List<Delegate> _callbacks = new List<Delegate>();
        private StreamInfo _streamInfo;
        private BlockingCollection<FrameData> _queue;
        private Action<Exception> _excHandler;
        private bool _initilaized;

        public MemoryInputMedia(IntPtr hMediaLib) : base(hMediaLib)
        {
            ImemGet pLock = OnImemGet;
            ImemRelease pUnlock = OnImemRelease;

            _lock = Marshal.GetFunctionPointerForDelegate(pLock);
            _unlock = Marshal.GetFunctionPointerForDelegate(pUnlock);

            _callbacks.Add(pLock);
            _callbacks.Add(pUnlock);
        }

        public void Initialize(StreamInfo streamInfo, int maxItemsInQueue)
        {
            if (streamInfo == null)
            {
                throw new ArgumentNullException("streamInfo");
            }

            _streamInfo = streamInfo;
            AddOptions(MediaOptions.ToList());
            _queue = new BlockingCollection<FrameData>(maxItemsInQueue);
            _initilaized = true;
        }

        public void AddFrame(FrameData frameData)
        {
            if (!_initilaized)
            {
                throw new InvalidOperationException("The instance must be initialized first. Call Initialize method before adding frames");
            }

            if (frameData.Data == IntPtr.Zero)
            {
                throw new ArgumentNullException("frameData");
            }

            if (frameData.DataSize == 0)
            {
                throw new ArgumentException("DataSize value must be greater than zero", "frameData");
            }

            if (frameData.Pts < 0)
            {
                throw new ArgumentException("Pts value must be greater than zero", "frameData");
            }

            _queue.Add(DeepClone(frameData));
        }

        public void AddFrame(byte[] data, long pts, long dts)
        {
            if (!_initilaized)
            {
                throw new InvalidOperationException("The instance must be initialized first. Call Initialize method before adding frames");
            }

            if (data == null || data.Length == 0)
            {
                throw new ArgumentException("data buffer size must be greater than zero", "data");
            }

            if (pts <= 0)
            {
                throw new ArgumentException("Pts value must be greater than zero", "pts");
            }

            FrameData frame = DeepClone(data);
            frame.Pts = pts;
            frame.Dts = dts;
            _queue.Add(frame);
        }

        public void AddFrame(Bitmap bitmap, long pts, long dts)
        {
            if (!_initilaized || bitmap == null || pts < 0 || (bitmap.PixelFormat != PixelFormat.Format24bppRgb && bitmap.PixelFormat != PixelFormat.Format32bppRgb))
            {
                return;
            }
            var rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
            var bmpData = bitmap.LockBits(rect, ImageLockMode.ReadOnly, bitmap.PixelFormat);
            var frame = DeepClone(bmpData.Scan0, bmpData.Stride * bmpData.Height);
            bitmap.UnlockBits(bmpData);
            frame.Pts = pts;
            frame.Dts = dts;
            _queue.Add(frame);
        }

        private FrameData DeepClone(byte[] buffer)
        {
            var clone = new FrameData
                            {
                                Data = new IntPtr(MemoryHeap.Alloc(buffer.Length))
                            };
            Marshal.Copy(buffer, 0, clone.Data, buffer.Length);
            clone.DataSize = buffer.Length;
            return clone;
        }

        private FrameData DeepClone(FrameData frameData)
        {
            FrameData clone = DeepClone(frameData.Data, frameData.DataSize);
            clone.Dts = frameData.Dts;
            clone.Pts = frameData.Pts;
            return clone;
        }

        private FrameData DeepClone(IntPtr data, int size)
        {
            var clone = new FrameData
                            {
                                Data = new IntPtr(MemoryHeap.Alloc(size))
                            };
            MemoryHeap.CopyMemory(clone.Data.ToPointer(), data.ToPointer(), size);
            clone.DataSize = size;
            return clone;
        }

        private int OnImemGet(void* data, char* cookie, long* dts, long* pts, int* flags, uint* dataSize, void** ppData)
        {
            try
            {
                FrameData fdata = _queue.Take();
                *ppData = fdata.Data.ToPointer();
                *dataSize = (uint)fdata.DataSize;
                *pts = fdata.Pts;
                *dts = fdata.Dts;
                *flags = 0;
                return 0;
            }
            catch (Exception ex)
            {
                if (_excHandler != null)
                {
                    _excHandler(ex);
                }
                else
                {
                    throw new Exception("imem-get callback failed", ex);
                }
                return 1;
            }
        }

        private void OnImemRelease(void* data, char* cookie, uint dataSize, void* pData)
        {
            try
            {
                MemoryHeap.Free(pData);
            }
            catch (Exception ex)
            {
                if (_excHandler != null)
                {
                    _excHandler(ex);
                }
                else
                {
                    throw new Exception("imem-release callback failed", ex);
                }
            }
        }

        private IEnumerable<string> MediaOptions
        {
            get
            {
                yield return string.Format(":imem-get={0}", _lock.ToInt64());
                yield return string.Format(":imem-release={0}", _unlock.ToInt64());
                yield return string.Format(":imem-codec={0}", EnumUtils.GetEnumDescription(_streamInfo.Codec));
                yield return string.Format(":imem-cat={0}", (int)_streamInfo.Category);
                yield return string.Format(":imem-id={0}", _streamInfo.Id);
                yield return string.Format(":imem-group={0}", _streamInfo.Group);
                yield return string.Format(":imem-fps={0}", _streamInfo.Fps);
                yield return string.Format(":imem-width={0}", _streamInfo.Width);
                yield return string.Format(":imem-height={0}", _streamInfo.Height);
                yield return string.Format(":imem-size={0}", _streamInfo.Size);
                yield return string.Format(":imem-channels={0}", _streamInfo.Channels);
                yield return string.Format(":imem-samplerate={0}", _streamInfo.Samplerate);
                yield return string.Format(":imem-dar={0}", EnumUtils.GetEnumDescription(_streamInfo.AspectRatio));
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
            {
                _callbacks = null;
                if (_queue.Count > 0)
                {
                    foreach (var item in _queue)
                    {
                        MemoryHeap.Free(item.Data.ToPointer());
                    }
                }
                _queue = null;
            }
        }

        public void SetExceptionHandler(Action<Exception> handler)
        {
            _excHandler = handler;
        }

        public int PendingFramesCount
        {
            get
            {
                return _queue.Count;
            }
        }
    }
}

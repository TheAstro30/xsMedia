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
using System.Runtime.InteropServices;
using System.Timers;
using xsVlc.Common;
using xsVlc.Common.Rendering;
using xsVlc.Common.Structures;

namespace xsVlc.Core.Rendering
{
    internal unsafe sealed class AudioRenderer : DisposableBase, IAudioRenderer
    {
        private readonly IntPtr _mediaPlayer;
        private AudioCallbacks _callbacks = new AudioCallbacks();
        private Func<SoundFormat, SoundFormat> _formatSetupCb;
        private SoundFormat _format;
        private readonly List<Delegate> _callbacksDelegates = new List<Delegate>();
        private Action<Exception> _excHandler;
        private readonly IntPtr _hSetup;
        private readonly IntPtr _hVolume;
        private readonly IntPtr _hSound;
        private readonly IntPtr _hPause;
        private readonly IntPtr _hResume;
        private readonly IntPtr _hFlush;
        private readonly IntPtr _hDrain;
        private readonly Timer _timer = new Timer();
        private volatile int _frameRate;
        private int _latestFps;

        public AudioRenderer(IntPtr hMediaPlayer)
        {
            _mediaPlayer = hMediaPlayer;

            PlayCallbackEventHandler pceh = PlayCallback;
            VolumeCallbackEventHandler vceh = VolumeCallback;
            SetupCallbackEventHandler sceh = SetupCallback;
            AudioCallbackEventHandler pause = PauseCallback;
            AudioCallbackEventHandler resume = ResumeCallback;
            AudioCallbackEventHandler flush = FlushCallback;
            AudioDrainCallbackEventHandler drain = DrainCallback;

            _hSound = Marshal.GetFunctionPointerForDelegate(pceh);
            _hVolume = Marshal.GetFunctionPointerForDelegate(vceh);
            _hSetup = Marshal.GetFunctionPointerForDelegate(sceh);
            _hPause = Marshal.GetFunctionPointerForDelegate(pause);
            _hResume = Marshal.GetFunctionPointerForDelegate(resume);
            _hFlush = Marshal.GetFunctionPointerForDelegate(flush);
            _hDrain = Marshal.GetFunctionPointerForDelegate(drain);

            _callbacksDelegates.Add(pceh);
            _callbacksDelegates.Add(vceh);
            _callbacksDelegates.Add(sceh);
            _callbacksDelegates.Add(pause);
            _callbacksDelegates.Add(resume);
            _callbacksDelegates.Add(flush);
            _callbacksDelegates.Add(drain);

            _timer.Elapsed += TimerElapsed;
            _timer.Interval = 1000;
        }

        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            _latestFps = _frameRate;
            _frameRate = 0;
        }

        public void SetCallbacks(VolumeChangedEventHandler volume, NewSoundEventHandler sound)
        {
            _callbacks.VolumeCallback = volume;
            _callbacks.SoundCallback = sound;
            Interop.Api.libvlc_audio_set_callbacks(_mediaPlayer, _hSound, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);
            Interop.Api.libvlc_audio_set_volume_callback(_mediaPlayer, _hVolume);
        }

        public void SetCallbacks(AudioCallbacks callbacks)
        {
            if (callbacks.SoundCallback == null)
            {
                return;
            }

            _callbacks = callbacks;
            Interop.Api.libvlc_audio_set_callbacks(_mediaPlayer, _hSound, _hPause, _hResume, _hFlush, _hDrain, IntPtr.Zero);
            Interop.Api.libvlc_audio_set_volume_callback(_mediaPlayer, _hVolume);
        }

        public void SetFormat(SoundFormat format)
        {
            _format = format;
            Interop.Api.libvlc_audio_set_format(_mediaPlayer, _format.Format.ToUtf8(), _format.Rate, _format.Channels);
        }

        public void SetFormatCallback(Func<SoundFormat, SoundFormat> formatSetup)
        {
            _formatSetupCb = formatSetup;
            Interop.Api.libvlc_audio_set_format_callbacks(_mediaPlayer, _hSetup, IntPtr.Zero);
        }

        internal void StartTimer()
        {
            _timer.Start();
        }

        private void PlayCallback(void* data, void* samples, uint count, long pts)
        {
            var s = new Sound
                        {
                            SamplesData = new IntPtr(samples), 
                            SamplesSize = (uint)(count * _format.BlockSize), 
                            Pts = pts
                        };

            if (_callbacks.SoundCallback != null)
            {
                _callbacks.SoundCallback(s);
            }
        }

        private void PauseCallback(void* data, long pts)
        {
            if (_callbacks.PauseCallback != null)
            {
                _callbacks.PauseCallback(pts);
            }
        }

        private void ResumeCallback(void* data, long pts)
        {
            if (_callbacks.ResumeCallback != null)
            {
                _callbacks.ResumeCallback(pts);
            }
        }

        private void FlushCallback(void* data, long pts)
        {
            if (_callbacks.FlushCallback != null)
            {
                _callbacks.FlushCallback(pts);
            }
        }

        private void DrainCallback(void* data)
        {
            if (_callbacks.DrainCallback != null)
            {
                _callbacks.DrainCallback();
            }
        }

        private void VolumeCallback(void* data, float volume, bool mute)
        {
            if (_callbacks.VolumeCallback != null)
            {
                _callbacks.VolumeCallback(volume, mute);
            }
        }

        private int SetupCallback(void** data, char* format, int* rate, int* channels)
        {
            var pFormat = new IntPtr(format);
            var formatStr = Marshal.PtrToStringAnsi(pFormat);
            if (formatStr == null)
            {
                return 1;
            }
            SoundType sType;
            try
            {
                sType = (SoundType)Enum.Parse(typeof(SoundType), formatStr);
            }
            catch (Exception)
            {
                var exc = new ArgumentException("Unsupported sound type " + formatStr);
                if (_excHandler != null)
                {
                    _excHandler(exc);
                    return 1;
                }
                throw exc;
            }
            _format = new SoundFormat(sType, *rate, *channels);
            if (_formatSetupCb != null)
            {
                _format = _formatSetupCb(_format);
            }
            Marshal.Copy(_format.Format.ToUtf8(), 0, pFormat, 4);
            *rate = _format.Rate;
            *channels = _format.Channels;
            return _format.UseCustomAudioRendering ? 0 : 1;
        }

        protected override void Dispose(bool disposing)
        {
            Interop.Api.libvlc_audio_set_format_callbacks(_mediaPlayer, IntPtr.Zero, IntPtr.Zero);
            Interop.Api.libvlc_audio_set_callbacks(_mediaPlayer, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);

            if (!disposing) { return; }
            _formatSetupCb = null;
            _excHandler = null;
            _callbacks = null;
            _callbacksDelegates.Clear();
        }

        public void SetExceptionHandler(Action<Exception> handler)
        {
            _excHandler = handler;
        }

        public int ActualFrameRate
        {
            get
            {
                return _latestFps;
            }
        }
    }
}

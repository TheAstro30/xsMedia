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
using xsVlc.Common;
using xsVlc.Common.Players;
using xsVlc.Common.Rendering;
using xsVlc.Common.Structures;
using xsVlc.Core.Exceptions;
using xsVlc.Core.Rendering;
using xsVlc.Interop;

namespace xsVlc.Core.Players
{
    internal class AudioPlayer : BasicPlayer, IAudioPlayer
    {
        private AudioRenderer _render;

        public AudioPlayer(IntPtr hMediaLib) : base(hMediaLib)
        {

        }

        public int Volume
        {
            get
            {
                return Api.libvlc_audio_get_volume(HMediaPlayer);
            }
            set
            {
                Api.libvlc_audio_set_volume(HMediaPlayer, value);
            }
        }

        public bool Mute
        {
            get
            {
                return Api.libvlc_audio_get_mute(HMediaPlayer);
            }
            set
            {
                Api.libvlc_audio_set_mute(HMediaPlayer, value);
            }
        }

        public long Delay
        {
            get
            {
                return Api.libvlc_audio_get_delay(HMediaPlayer);
            }
            set
            {
                Api.libvlc_audio_set_delay(HMediaPlayer, value);
            }
        }

        public AudioChannelType Channel
        {
            get
            {
                return (AudioChannelType)Api.libvlc_audio_get_channel(HMediaPlayer);
            }
            set
            {
                Api.libvlc_audio_set_channel(HMediaPlayer, (LibvlcAudioOutputChannelT)value);
            }
        }

        public void ToggleMute()
        {
            Api.libvlc_audio_toggle_mute(HMediaPlayer);
        }

        public IAudioRenderer CustomAudioRenderer
        {
            get { return _render ?? (_render = new AudioRenderer(HMediaPlayer)); }
        }

        public AudioOutputDeviceType DeviceType
        {
            get
            {
                return (AudioOutputDeviceType)Api.libvlc_audio_output_get_device_type(HMediaPlayer);
            }
            set
            {
                Api.libvlc_audio_output_set_device_type(HMediaPlayer, (LibvlcAudioOutputDeviceTypesT)value);
            }
        }

        public void SetAudioOutputModuleAndDevice(AudioOutputModuleInfo module, AudioOutputDeviceInfo device)
        {
            if (module == null)
            {
                throw new ArgumentNullException("module");
            }
            if (device != null)
            {
                Api.libvlc_audio_output_device_set(HMediaPlayer, module.Name.ToUtf8(), device.Id.ToUtf8());
            }
            var res = Api.libvlc_audio_output_set(HMediaPlayer, module.Name.ToUtf8());
            if (res < 0)
            {
                throw new LibVlcException();
            }
        }

        public override void Play()
        {
            base.Play();
            if (_render != null)
            {
                _render.StartTimer();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (_render != null)
            {
                _render.Dispose();
                _render = null;
            }

            base.Dispose(disposing);
        }
    }
}

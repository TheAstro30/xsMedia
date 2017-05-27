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
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using xsVlc.Common.Players;
using xsVlc.Common.Internal;
using xsVlc.Interop;

namespace xsVlc.Core.Equalizer
{
    public class Equalizer : DisposableBase
    {
        private readonly IntPtr _handle;
        private ReadOnlyCollection<Band> _bands;

        public static IEnumerable<Preset> Presets
        {
            get
            {
                var count = Api.libvlc_audio_equalizer_get_preset_count();
                for (var i = 0; i < count; i++)
                {
                    yield return new Preset(i, Marshal.PtrToStringAnsi(Api.libvlc_audio_equalizer_get_preset_name(i)));
                }
            }
        }

        public Equalizer()
        {
            _handle = Api.libvlc_audio_equalizer_new();
        }

        public Equalizer(Preset preset)
        {
            _handle = Api.libvlc_audio_equalizer_new_from_preset(preset.Index);
        }

        public float Preamp
        {
            get
            {
                return Api.libvlc_audio_equalizer_get_preamp(_handle);
            }
            set
            {
                Api.libvlc_audio_equalizer_set_preamp(_handle, value);
            }
        }

        protected override void Dispose(bool disposing)
        {
            Api.libvlc_audio_equalizer_release(_handle);
        }

        public ReadOnlyCollection<Band> Bands
        {
            get
            {
                if (_bands == null)
                {
                    var count = Api.libvlc_audio_equalizer_get_band_count();
                    var temp = new List<Band>(count);
                    for (var i = 0; i < count; i++)
                    {
                        temp.Add(new Band(i, Api.libvlc_audio_equalizer_get_band_frequency(i), _handle));
                    }
                    _bands = new ReadOnlyCollection<Band>(temp);
                }
                return _bands;
            }
        }

        public void SetEqualizer(IPlayer mPlayer, bool enable)
        {
            Api.libvlc_media_player_set_equalizer(((INativePointer)mPlayer).Pointer, enable ? _handle : IntPtr.Zero);
        }

        internal IntPtr Handle
        {
            get
            {
                return _handle;
            }
        }
    }
}

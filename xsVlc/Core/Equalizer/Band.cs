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
using xsVlc.Interop;

namespace xsVlc.Core.Equalizer
{
    public sealed class Band
    {
        private readonly IntPtr _handle;

        internal Band(int index, float frequency, IntPtr hEqualizer)
        {
            Index = index;
            Frequency = frequency;
            _handle = hEqualizer;
        }

        public int Index { get; private set; }

        public float Frequency { get; private set; }

        public float Amplitude
        {
            get
            {
                return Api.libvlc_audio_equalizer_get_amp_at_index(_handle, Index);
            }
            set
            {
                Api.libvlc_audio_equalizer_set_amp_at_index(_handle, value, Index);
            }
        }

        public override string ToString()
        {
            return string.Format("Frequency : {0}, Amplitude : {1}", Frequency, Amplitude);
        }
    }
}

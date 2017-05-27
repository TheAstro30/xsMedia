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

namespace xsVlc.Core.Equalizer
{
    public sealed class Preset
    {
        internal Preset(int index, string name)
        {
            Index = index;
            Name = name;
        }

        public int Index { get; private set; }

        public string Name { get; private set; }

        public override string ToString()
        {
            return Name;
        }
    }
}

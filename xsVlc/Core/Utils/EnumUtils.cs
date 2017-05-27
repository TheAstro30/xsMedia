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
using System.ComponentModel;
using System.Linq;

namespace xsVlc.Core.Utils
{
    class EnumUtils
    {
        public static string GetEnumDescription(Enum value)
        {
            var fi = value.GetType().GetField(value.ToString());
            var attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description : value.ToString();
        }

        public static Dictionary<string, Enum> GetEnumMapping(Type enumType)
        {
            if (!enumType.IsEnum)
            {
                throw new ArgumentException("Enum type expected");
            }
            var values = Enum.GetValues(enumType);
            return values.Cast<Enum>().ToDictionary(GetEnumDescription);
        }
    }
}

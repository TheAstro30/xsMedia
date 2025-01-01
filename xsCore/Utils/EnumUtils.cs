/* xsMedia - sxCore
 * (c)2013 - 2025
 * Jason James Newland
 * KangaSoft Software, All Rights Reserved
 * Licenced under the GNU public licence */
using System;
using System.ComponentModel;
using System.Linq;

namespace xsCore.Utils
{
    public sealed class EnumUtils
    {
        public static string GetDescriptionFromEnumValue(Enum value)
        {
            var attribute = value.GetType()
                                .GetField(value.ToString())
                                .GetCustomAttributes(typeof (DescriptionAttribute), false)
                                .SingleOrDefault() as DescriptionAttribute;
            return attribute == null ? value.ToString() : attribute.Description;
        }
    }

    public class EnumComboData
    {
        public string Text { get; set; }
        public uint Data { get; set; }

        public override string ToString()
        {
            return Text;
        }
    }
}

/* xsMedia - xsSettings
 * (c)2013 - 2024
 * Jason James Newland
 * KangaSoft Software, All Rights Reserved
 * Licenced under the GNU public licence */
using System;
using System.Xml.Serialization;

namespace xsCore.Settings.Data.Filter
{
    [Serializable]
    public class FilterCrop
    {
        public FilterCrop()
        {
            /* Default constructor */
            Enable = false;
        }

        public FilterCrop(FilterCrop crop)
        {
            /* Copy constructor */
            Enable = crop.Enable;
            Top = crop.Top;
            Left = crop.Left;
            Right = crop.Right;
            Bottom = crop.Bottom;
        }

        [XmlAttribute("enable")]
        public bool Enable { get; set; }

        [XmlAttribute("top")]
        public int Top { get; set; }

        [XmlAttribute("left")]
        public int Left { get; set; }

        [XmlAttribute("right")]
        public int Right { get; set; }

        [XmlAttribute("bottom")]
        public int Bottom { get; set; }
    }
}

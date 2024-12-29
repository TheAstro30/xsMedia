/* xsMedia - xsSettings
 * (c)2013 - 2024
 * Jason James Newland
 * KangaSoft Software, All Rights Reserved
 * Licenced under the GNU public licence */
using System;
using System.Xml.Serialization;

namespace xsSettings.Settings
{
    [Serializable]
    public class SettingsFilterCrop
    {
        public SettingsFilterCrop()
        {
            /* Default constructor */
            Enable = false;
        }

        public SettingsFilterCrop(SettingsFilterCrop crop)
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

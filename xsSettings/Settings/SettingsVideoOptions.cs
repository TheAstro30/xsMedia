/* xsMedia - xsSettings
 * (c)2013 - 2024
 * Jason James Newland
 * KangaSoft Software, All Rights Reserved
 * Licenced under the GNU public licence */
using System;
using System.Xml.Serialization;
using xsSettings.Settings.Enums;

namespace xsSettings.Settings
{
    [Serializable]
    public class SettingsVideoOptions
    {
        [XmlAttribute("resize")]
        public VideoWindowResizeOption Resize { get; set; }

        [XmlAttribute("enableTitle")]
        public bool EnableVideoTitle { get; set; }

        [XmlAttribute("titleTimeOut")]
        public int VideoTitleTimeOut { get; set; }
    }
}

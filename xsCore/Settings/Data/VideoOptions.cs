﻿/* xsMedia - xsSettings
 * (c)2013 - 2025
 * Jason James Newland
 * KangaSoft Software, All Rights Reserved
 * Licenced under the GNU public licence */
using System;
using System.Xml.Serialization;
using xsCore.Settings.Data.Enums;

namespace xsCore.Settings.Data
{
    [Serializable]
    public class VideoOptions
    {
        [XmlAttribute("resize")]
        public VideoWindowResizeOption Resize { get; set; }

        [XmlAttribute("enableTitle")]
        public bool EnableVideoTitle { get; set; }

        [XmlAttribute("titleTimeOut")]
        public int VideoTitleTimeOut { get; set; }
    }
}

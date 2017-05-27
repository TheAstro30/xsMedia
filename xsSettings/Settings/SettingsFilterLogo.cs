﻿using System;
using System.Xml.Serialization;

namespace xsSettings.Settings
{
    [Serializable]
    public class SettingsFilterLogo
    {
        public SettingsFilterLogo()
        {
            /* Default constructor */
            Enable = false;
            Opacity = 255;
        }

        public SettingsFilterLogo(SettingsFilterLogo logo)
        {
            /* Copy constructor */
            Enable = logo.Enable;
            File = logo.File;
            Opacity = logo.Opacity;
            LeftOffset = logo.LeftOffset;
            TopOffset = logo.TopOffset;
        }

        [XmlAttribute("enable")]
        public bool Enable { get; set; }

        [XmlAttribute("file")]
        public string File { get; set; }

        [XmlAttribute("opacity")]
        public int Opacity { get; set; }

        [XmlAttribute("leftOffset")]
        public int LeftOffset { get; set; }

        [XmlAttribute("topOffset")]
        public int TopOffset { get; set; }
    }
}

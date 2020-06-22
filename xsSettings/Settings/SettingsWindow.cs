/* xsMedia - xsSettings
 * (c)2013 - 2020
 * Jason James Newland
 * KangaSoft Software, All Rights Reserved
 * Licenced under the GNU public licence */
using System;
using System.Drawing;
using System.Xml.Serialization;
using xsCore.Utils;

namespace xsSettings.Settings
{
    [Serializable]
    public class SettingsWindow
    {
        public SettingsWindow()
        {
            /* Default constructor */
        }

        public SettingsWindow(SettingsWindow window)
        {
            Size = window.Size;
            Location = window.Location;
            CurrentSkin = window.CurrentSkin;
        }

        [XmlAttribute("location")]
        public string LocationString
        {
            get { return XmlFormatting.WritePointFormat(Location); }
            set { Location = XmlFormatting.ParsePointFormat(value); }
        }

        [XmlAttribute("size")]
        public string SizeString
        {
            get { return XmlFormatting.WriteSizeFormat(Size); }
            set { Size = XmlFormatting.ParseSizeFormat(value); }
        }

        [XmlElement("skin")]
        public string CurrentSkin { get; set; }

        [XmlIgnore]
        public Point Location { get; set; }

        [XmlIgnore]
        public Size Size { get; set; }
    }
}
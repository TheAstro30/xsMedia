/* xsMedia - xsSettings
 * (c)2013 - 2024
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
    public class SettingsWindowData
    {
        public SettingsWindowData()
        {
            /* Default constructor */
        }

        public SettingsWindowData(SettingsWindowData data)
        {
            Size = data.Size;
            Location = data.Location;
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

        [XmlIgnore]
        public Point Location { get; set; }

        [XmlIgnore]
        public Size Size { get; set; }
    }
}

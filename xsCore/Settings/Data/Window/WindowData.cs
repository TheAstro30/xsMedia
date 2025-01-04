/* xsMedia - xsSettings
 * (c)2013 - 2025
 * Jason James Newland
 * KangaSoft Software, All Rights Reserved
 * Licenced under the GNU public licence */
using System;
using System.Drawing;
using System.Xml.Serialization;
using xsCore.Utils;

namespace xsCore.Settings.Data.Window
{
    [Serializable]
    public class WindowData
    {
        public WindowData()
        {
            /* Default constructor */
        }

        public WindowData(WindowData data)
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

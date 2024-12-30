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
    public class FilterLogo
    {
        public FilterLogo()
        {
            /* Default constructor */
            Enable = false;
            Opacity = 255;
        }

        public FilterLogo(FilterLogo logo)
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

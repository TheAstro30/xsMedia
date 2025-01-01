/* xsMedia - xsSettings
 * (c)2013 - 2025
 * Jason James Newland
 * KangaSoft Software, All Rights Reserved
 * Licenced under the GNU public licence */
using System;
using System.Xml.Serialization;

namespace xsCore.Settings.Data.Filter
{
    [Serializable]
    public class FilterDeinterlace
    {
        public FilterDeinterlace()
        {
            /* Default constructor */
            Enable = false;
        }

        public FilterDeinterlace(FilterDeinterlace deint)
        {
            /* Copy constructor */
            Enable = deint.Enable;
            Mode = deint.Mode;
        }

        [XmlAttribute("enable")]
        public bool Enable { get; set; }

        [XmlAttribute("mode")]
        public int Mode { get; set; }
    }
}

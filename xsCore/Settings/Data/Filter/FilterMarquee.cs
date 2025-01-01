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
    public class FilterMarquee
    {
        public FilterMarquee()
        {
            /* Default constructor */
            Enable = false;
            Position = 4;
            Text = "xsMedia";
            Opacity = 255;
            Color = 0x00FFFFFF;
            TimeOut = 0;
        }

        public FilterMarquee(FilterMarquee marquee)
        {
            /* Copy constructor */
            Enable = marquee.Enable;
            Position = marquee.Position;
            Text = marquee.Text;
            Opacity = marquee.Opacity;
            Color = marquee.Color;
            TimeOut = marquee.TimeOut;
        }

        [XmlAttribute("enable")]
        public bool Enable { get; set; }

        [XmlAttribute("position")]
        public int Position { get; set; }

        [XmlAttribute("text")]
        public string Text { get; set; }

        [XmlAttribute("opacity")]
        public int Opacity { get; set; }

        [XmlAttribute("color")]
        public uint Color { get; set; }

        [XmlAttribute("timeOut")]
        public int TimeOut { get; set; }
    }
}

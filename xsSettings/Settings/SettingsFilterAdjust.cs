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
    public class SettingsFilterAdjust
    {
        public SettingsFilterAdjust()
        {
            /* Default constructor */
            Enable = false;
            Hue = 0;
            Brightness = (float)1.03;
            Contrast = (float)1.02;
            Saturation = 1;
            Gamma = (float)0.95;
        }

        public SettingsFilterAdjust(SettingsFilterAdjust adjust)
        {
            /* Copy constructor */
            Enable = adjust.Enable;
            Hue = adjust.Hue;
            Brightness = adjust.Brightness;
            Contrast = adjust.Contrast;
            Saturation = adjust.Saturation;
            Gamma = adjust.Gamma;
        }

        [XmlAttribute("enable")]
        public bool Enable { get; set; }

        [XmlAttribute("hue")]
        public int Hue { get; set; }

        [XmlAttribute("brightness")]
        public float Brightness { get; set; }

        [XmlAttribute("contrast")]
        public float Contrast { get; set; }

        [XmlAttribute("saturation")]
        public float Saturation { get; set; }

        [XmlAttribute("gamma")]
        public float Gamma { get; set; }
    }
}

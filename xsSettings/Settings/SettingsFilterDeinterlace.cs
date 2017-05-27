using System;
using System.Xml.Serialization;

namespace xsSettings.Settings
{
    [Serializable]
    public class SettingsFilterDeinterlace
    {
        public SettingsFilterDeinterlace()
        {
            /* Default constructor */
            Enable = false;
        }

        public SettingsFilterDeinterlace(SettingsFilterDeinterlace deint)
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

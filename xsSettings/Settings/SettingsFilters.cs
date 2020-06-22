/* xsMedia - xsSettings
 * (c)2013 - 2020
 * Jason James Newland
 * KangaSoft Software, All Rights Reserved
 * Licenced under the GNU public licence */
using System;
using System.Xml.Serialization;

namespace xsSettings.Settings
{
    [Serializable]
    public class SettingsFilters
    {
        public SettingsFilters()
        {
            /* Default empty constructor */           
        }

        public SettingsFilters(SettingsFilters filters)
        {
            /* Copy constructor */
            Eq = new SettingsFilterEq(filters.Eq);
            Adjust = new SettingsFilterAdjust(filters.Adjust);
            Marquee = new SettingsFilterMarquee(filters.Marquee);
            Logo = new SettingsFilterLogo(filters.Logo);
            Crop = new SettingsFilterCrop(filters.Crop);
        }

        [XmlElement("selectedRootTab")]
        public SettingsSelectedTab RootTab = new SettingsSelectedTab();

        [XmlElement("selectedAudioTab")]
        public SettingsSelectedTab AudioTab = new SettingsSelectedTab();

        [XmlElement("selectedVideoTab")]
        public SettingsSelectedTab VideoTab = new SettingsSelectedTab();

        [XmlElement("eq")]
        public SettingsFilterEq Eq = new SettingsFilterEq();

        [XmlElement("adjust")]
        public SettingsFilterAdjust Adjust = new SettingsFilterAdjust();

        [XmlElement("marquee")]
        public SettingsFilterMarquee Marquee = new SettingsFilterMarquee();

        [XmlElement("logo")]
        public SettingsFilterLogo Logo = new SettingsFilterLogo();

        [XmlElement("crop")]
        public SettingsFilterCrop Crop = new SettingsFilterCrop();

        [XmlElement("deinterlace")]
        public SettingsFilterDeinterlace Deinterlace = new SettingsFilterDeinterlace();
    }
}

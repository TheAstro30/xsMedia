/* xsMedia - xsSettings
 * (c)2013 - 2024
 * Jason James Newland
 * KangaSoft Software, All Rights Reserved
 * Licenced under the GNU public licence */
using System;
using System.Xml.Serialization;
using xsCore.Settings.Data.Filter;

namespace xsCore.Settings.Data
{
    [Serializable]
    public class Filters
    {
        public Filters()
        {
            /* Default empty constructor */           
        }

        public Filters(Filters filters)
        {
            /* Copy constructor */
            Eq = new FilterEq(filters.Eq);
            Adjust = new FilterAdjust(filters.Adjust);
            Marquee = new FilterMarquee(filters.Marquee);
            Logo = new FilterLogo(filters.Logo);
            Crop = new FilterCrop(filters.Crop);
        }

        [XmlElement("selectedRootTab")]
        public SelectedTab RootTab = new SelectedTab();

        [XmlElement("selectedAudioTab")]
        public SelectedTab AudioTab = new SelectedTab();

        [XmlElement("selectedVideoTab")]
        public SelectedTab VideoTab = new SelectedTab();

        [XmlElement("eq")]
        public FilterEq Eq = new FilterEq();

        [XmlElement("adjust")]
        public FilterAdjust Adjust = new FilterAdjust();

        [XmlElement("marquee")]
        public FilterMarquee Marquee = new FilterMarquee();

        [XmlElement("logo")]
        public FilterLogo Logo = new FilterLogo();

        [XmlElement("crop")]
        public FilterCrop Crop = new FilterCrop();

        [XmlElement("deinterlace")]
        public FilterDeinterlace Deinterlace = new FilterDeinterlace();
    }
}

﻿/* xsMedia - xsSettings
 * (c)2013 - 2025
 * Jason James Newland
 * KangaSoft Software, All Rights Reserved
 * Licenced under the GNU public licence */
using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using xsCore.Settings.Data.Media;

namespace xsCore.Settings.Data.Disc
{
    [Serializable]
    public class DiscData
    {
        public DiscData()
        {
            /* Default constructor */
        }

        public DiscData(DiscData disc)
        {
            Cddb = new CddbData(disc.Cddb);
            Options.Option = new List<MediaOptionData>(disc.Options.Option);
        }

        [XmlElement("cddb")]
        public CddbData Cddb = new CddbData();

        [XmlElement("options")]
        public MediaOptions Options = new MediaOptions();
    }
}
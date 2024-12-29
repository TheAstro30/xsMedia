/* xsMedia - xsSettings
 * (c)2013 - 2024
 * Jason James Newland
 * KangaSoft Software, All Rights Reserved
 * Licenced under the GNU public licence */
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace xsSettings.Settings
{
    [Serializable]
    public class SettingsDisc
    {
        public SettingsDisc()
        {
            /* Default constructor */
        }

        public SettingsDisc(SettingsDisc disc)
        {
            Cddb = new SettingsCddb(disc.Cddb);
            Options.Option = new List<SettingsMediaOptions.MediaOption>(disc.Options.Option);
        }

        [XmlElement("cddb")]
        public SettingsCddb Cddb = new SettingsCddb();

        [XmlElement("options")]
        public SettingsMediaOptions Options = new SettingsMediaOptions();
    }
}
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
    public class SettingsFavorites
    {
        public SettingsFavorites()
        {
            /* Default constructor */
        }

        public SettingsFavorites(SettingsFavorites favorites)
        {
            Favorite = new List<SettingsHistoryData>(favorites.Favorite);
        }

        [XmlElement("favorite")]
        public List<SettingsHistoryData> Favorite = new List<SettingsHistoryData>();
    }
}

/* xsMedia - xsSettings
 * (c)2013 - 2024
 * Jason James Newland
 * KangaSoft Software, All Rights Reserved
 * Licenced under the GNU public licence */
using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using xsCore.Structures;

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
            /* Copy constructor */
            Favorite = new List<HistoryData>(favorites.Favorite);
        }

        [XmlElement("favorite")]
        public List<HistoryData> Favorite = new List<HistoryData>();
    }
}

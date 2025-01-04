/* xsMedia - xsSettings
 * (c)2013 - 2025
 * Jason James Newland
 * KangaSoft Software, All Rights Reserved
 * Licenced under the GNU public licence */
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace xsCore.Settings.Data.History
{
    [Serializable]
    public class Favorites
    {
        public Favorites()
        {
            /* Default constructor */
        }

        public Favorites(Favorites favorites)
        {
            /* Copy constructor */
            Favorite = new List<HistoryData>(favorites.Favorite);
        }

        [XmlElement("file")]
        public List<HistoryData> Favorite = new List<HistoryData>();
    }
}

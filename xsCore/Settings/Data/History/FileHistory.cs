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
    public class FileHistory
    {
        public FileHistory()
        {
            /* Default constructor */
        }

        public FileHistory(FileHistory history)
        {
            /* Copy constructor */
            HistoryData = new List<HistoryData>(history.HistoryData);
        }

        [XmlElement("file")]
        public List<HistoryData> HistoryData = new List<HistoryData>();
    }
}
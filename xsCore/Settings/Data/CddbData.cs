/* xsMedia - xsSettings
 * (c)2013 - 2025
 * Jason James Newland
 * KangaSoft Software, All Rights Reserved
 * Licenced under the GNU public licence */
using System;
using System.Xml.Serialization;

namespace xsCore.Settings.Data
{
    [Serializable]
    public class CddbData
    {
        public CddbData()
        {
            /* Empty */
        }

        public CddbData(CddbData cddb)
        {
            Enabled = cddb.Enabled;
            Host = cddb.Host;
            Cache = cddb.Cache;
        }

        [XmlAttribute("enabled")]
        public bool Enabled { get; set; }

        [XmlAttribute("host")]
        public string Host { get; set; }

        [XmlAttribute("cache")]
        public bool Cache { get; set; }
    }
}
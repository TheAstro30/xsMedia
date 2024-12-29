/* xsMedia - xsSettings
 * (c)2013 - 2024
 * Jason James Newland
 * KangaSoft Software, All Rights Reserved
 * Licenced under the GNU public licence */
using System;
using System.Xml.Serialization;

namespace xsSettings.Settings
{
    [Serializable]
    public class SettingsCddb
    {
        public SettingsCddb()
        {
            /* Empty */
        }

        public SettingsCddb(SettingsCddb cddb)
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
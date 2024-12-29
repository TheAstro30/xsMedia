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
    public class SettingsHistoryData
    {
        [XmlAttribute("path")]
        public string Path { get; set; }

        [XmlAttribute("friendlyName")]
        public string FriendlyName { get; set; }

        public override string ToString()
        {
            return FriendlyName;
        }
    }
}

/* xsMedia - xsSettings
 * (c)2013 - 2024
 * Jason James Newland
 * KangaSoft Software, All Rights Reserved
 * Licenced under the GNU public licence */
using System;
using System.IO;
using System.Xml.Serialization;
using libolv.Implementation;

namespace xsSettings.Settings
{
    [Serializable]
    public class SettingsHistoryData
    {
        public SettingsHistoryData()
        {
            /* Default constructor */
        }

        public SettingsHistoryData(string fileName)
        {
            FilePath = fileName;
            FriendlyName = Path.GetFileNameWithoutExtension(fileName);
        }

        [OlvIgnore]
        [XmlAttribute("path")]
        public string FilePath { get; set; }

        [XmlAttribute("friendlyName")]
        public string FriendlyName { get; set; }

        public override string ToString()
        {
            return FriendlyName;
        }
    }
}

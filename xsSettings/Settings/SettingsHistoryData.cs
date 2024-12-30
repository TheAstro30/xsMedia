/* xsMedia - xsSettings
 * (c)2013 - 2024
 * Jason James Newland
 * KangaSoft Software, All Rights Reserved
 * Licenced under the GNU public licence */
using System;
using System.IO;
using System.Xml.Serialization;
using libolv.Implementation;
using xsCore.Utils;

namespace xsSettings.Settings
{
    [Serializable]
    public class SettingsHistoryData
    {
        private string _friendlyName;

        public SettingsHistoryData()
        {
            /* Default constructor */
        }

        public SettingsHistoryData(string fileName)
        {
            FilePath = fileName;
            FriendlyName = Path.GetFileNameWithoutExtension(fileName);
            /* Get media length */
            Length = GetLength();
        }

        public SettingsHistoryData(string fileName, int length)
        {
            FilePath = fileName;
            FriendlyName = Path.GetFileNameWithoutExtension(fileName);
            Length = length > 0 ? length : GetLength(); /* Shouldn't be 0 coming from the playlist... */
        }

        [OlvIgnore]
        [XmlAttribute("path")]
        public string FilePath { get; set; }

        [XmlAttribute("friendlyName")]
        public string FriendlyName
        {
            get
            {
                return !string.IsNullOrEmpty(_friendlyName) ? _friendlyName : FilePath;
            }
            set
            {
                _friendlyName = value;
            }
        }

        [OlvIgnore]
        [XmlAttribute("length")]
        public int Length { get; set; }

        public string LengthString
        {
            get
            {
                var length = Length;
                var ts = new TimeSpan(0, 0, 0, length);
                return (length >= 3600
                    ? string.Format("{0:00}:{1:00}:{2:00}", ts.Hours, ts.Minutes, ts.Seconds)
                    : string.Format("{0:00}:{1:00}", ts.Minutes, ts.Seconds));
            }
        }

        public override string ToString()
        {
            return FriendlyName;
        }

        internal int GetLength()
        {
            TimeSpan ts;
            if (MediaInfo.GetDuration(FilePath, out ts))
            {
                return (int)ts.TotalSeconds;
            }
            return 0; /* Totally failed */
        }
    }
}

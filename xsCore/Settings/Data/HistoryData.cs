/* xsMedia - xsSettings
 * (c)2013 - 2024
 * Jason James Newland
 * KangaSoft Software, All Rights Reserved
 * Licenced under the GNU public licence */
using System;
using System.IO;
using System.Xml.Serialization;
using libolv.Implementation;

namespace xsCore.Settings.Data
{
    [Serializable]
    public class HistoryData
    {
        private string _friendlyName;

        public HistoryData()
        {
            /* Default constructor */
        }

        public HistoryData(string fileName)
        {
            FilePath = fileName;
            FriendlyName = Path.GetFileNameWithoutExtension(fileName);
        }

        public HistoryData(string fileName, int length)
        {
            FilePath = fileName;
            FriendlyName = Path.GetFileNameWithoutExtension(fileName);
            Length = length;
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
    }
}

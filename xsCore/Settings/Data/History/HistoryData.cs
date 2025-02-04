﻿/* xsMedia - xsSettings
 * (c)2013 - 2025
 * Jason James Newland
 * KangaSoft Software, All Rights Reserved
 * Licenced under the GNU public licence */
using System;
using System.IO;
using System.Xml.Serialization;
using libolv.Implementation;
using xsCore.Utils.SystemUtils;

namespace xsCore.Settings.Data.History
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
                return MediaInfo.FormatDurationString(Length);
            }
        }

        public override string ToString()
        {
            return FriendlyName;
        }
    }
}

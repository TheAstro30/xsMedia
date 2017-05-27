using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace xsCore.CdUtils
{
    [Serializable, XmlRoot("cddbCache")]
    public class CddbCache
    {
        public CddbCache()
        {
            Entry = new List<CddbCacheData>();
        }

        [XmlElement("cddbEntry")]
        public List<CddbCacheData> Entry { get; set; }

        public List<CdTrackInfo> GetEntry(string discId)
        {
            /* Search for matching discId and return track list */
            return string.IsNullOrEmpty(discId)
                       ? null
                       : (from en in Entry where en.DiscId.ToLower() == discId.ToLower() select en.Tracks).
                             FirstOrDefault();
        }
    }

    [Serializable]
    public class CddbCacheData
    {
        public CddbCacheData()
        {
            Tracks = new List<CdTrackInfo>();
        }

        [XmlAttribute("id")]
        public string DiscId { get; set; }

        [XmlElement("track")]
        public List<CdTrackInfo> Tracks { get; set; }
    }

    [Serializable]
    public class CdTrackInfo
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("artist")]
        public string Artist { get; set; }

        [XmlAttribute("genre")]
        public string Genre { get; set; }

        [XmlAttribute("album")]
        public string Album { get; set; }

        [XmlAttribute("year")]
        public string Year { get; set; }

        public override string ToString()
        {
            return !string.IsNullOrEmpty(Artist) ? string.Format("{0} - {1}", Artist, Name) : Name;
        }
    }
}

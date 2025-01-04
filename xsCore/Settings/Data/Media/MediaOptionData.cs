/* xsMedia - xsSettings
 * (c)2013 - 2025
 * Jason James Newland
 * KangaSoft Software, All Rights Reserved
 * Licenced under the GNU public licence */
using System;
using System.Xml.Serialization;
using xsCore.Utils.SystemUtils;

namespace xsCore.Settings.Data.Media
{
    [Serializable]
    public class MediaOptionData
    {
        /* Constructors */
        public MediaOptionData()
        {
            /* Empty constructor */
        }

        public MediaOptionData(string id)
        {
            Id = id;
        }

        public MediaOptionData(string id, string data)
        {
            Id = id;
            Data = data;
        }

        [XmlAttribute("id")]
        public string Id { get; set; }

        [XmlAttribute("data")]
        public string Data { get; set; }

        /* Formatting method for libVlc --option=data */
        public override string ToString()
        {
            return string.IsNullOrEmpty(Data) ? Id : string.Format("{0}={1}", Id, AppPath.MainDir(Data));
        }
    }
}

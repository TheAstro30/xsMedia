/* xsMedia - xsSettings
 * (c)2013 - 2025
 * Jason James Newland
 * KangaSoft Software, All Rights Reserved
 * Licenced under the GNU public licence */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace xsCore.Settings.Data.Network
{
    [Serializable]
    public class NetworkPresetData
    {
        public class CompareById : IComparer<NetworkPresetData>
        {
            public int Compare(NetworkPresetData x, NetworkPresetData y)
            {
                return x == null || y == null ? 0 : new CaseInsensitiveComparer().Compare(x.Id, y.Id);
            }
        }

        [XmlAttribute("id")]
        public string Id { get; set; }

        [XmlAttribute("url")]
        public string Url { get; set; }

        public override string ToString()
        {
            return Url;
        }
    }
}

/* xsMedia - xsSettings
 * (c)2013 - 2025
 * Jason James Newland
 * KangaSoft Software, All Rights Reserved
 * Licenced under the GNU public licence */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using xsCore.Utils;

namespace xsCore.Settings.Data
{
    [Serializable]
    public class NetworkPresets
    {
        public class SettingsNetworkPresetData
        {
            public class CompareById : IComparer<SettingsNetworkPresetData>
            {
                public int Compare(SettingsNetworkPresetData x, SettingsNetworkPresetData y)
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

        public NetworkPresets()
        {
            /* Default constructor */
        }

        public NetworkPresets(NetworkPresets presets)
        {
            LastUrl = presets.LastUrl;
            ShowQuality = presets.ShowQuality;
            Proxy = new Proxy(presets.Proxy);
            Options.Option = new List<MediaOptions.MediaOption>(presets.Options.Option);
            Preset = new List<SettingsNetworkPresetData>(presets.Preset);
        }

        [XmlAttribute("last")]
        public string LastUrl { get; set; }

        [XmlAttribute("show-quality")]
        public bool ShowQuality { get; set; }

        [XmlElement("proxy")]
        public Proxy Proxy = new Proxy();

        [XmlElement("options")]
        public MediaOptions Options = new MediaOptions();

        [XmlElement("preset")]
        public List<SettingsNetworkPresetData> Preset = new List<SettingsNetworkPresetData>();

        public void Sort()
        {
            Preset.Sort(new SettingsNetworkPresetData.CompareById());
        }
    }
}
/* xsMedia - xsSettings
 * (c)2013 - 2024
 * Jason James Newland
 * KangaSoft Software, All Rights Reserved
 * Licenced under the GNU public licence */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using xsCore.Utils;

namespace xsSettings.Settings
{
    [Serializable]
    public class SettingsNetworkPresets
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

        public SettingsNetworkPresets()
        {
            /* Default constructor */
        }

        public SettingsNetworkPresets(SettingsNetworkPresets presets)
        {
            LastUrl = presets.LastUrl;
            ShowQuality = presets.ShowQuality;
            Proxy = new Proxy(presets.Proxy);
            Options.Option = new List<SettingsMediaOptions.MediaOption>(presets.Options.Option);
            Preset = new List<SettingsNetworkPresetData>(presets.Preset);
        }

        [XmlAttribute("last")]
        public string LastUrl { get; set; }

        [XmlAttribute("show-quality")]
        public bool ShowQuality { get; set; }

        [XmlElement("proxy")]
        public Proxy Proxy = new Proxy();

        [XmlElement("options")]
        public SettingsMediaOptions Options = new SettingsMediaOptions();

        [XmlElement("preset")]
        public List<SettingsNetworkPresetData> Preset = new List<SettingsNetworkPresetData>();

        public void Sort()
        {
            Preset.Sort(new SettingsNetworkPresetData.CompareById());
        }
    }
}
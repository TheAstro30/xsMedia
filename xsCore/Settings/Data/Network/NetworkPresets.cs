/* xsMedia - xsSettings
 * (c)2013 - 2025
 * Jason James Newland
 * KangaSoft Software, All Rights Reserved
 * Licenced under the GNU public licence */
using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using xsCore.Settings.Data.Media;
using xsCore.Utils;

namespace xsCore.Settings.Data.Network
{
    [Serializable]
    public class NetworkPresets
    {
        public NetworkPresets()
        {
            /* Default constructor */
        }

        public NetworkPresets(NetworkPresets presets)
        {
            LastUrl = presets.LastUrl;
            ShowQuality = presets.ShowQuality;
            Proxy = new Proxy(presets.Proxy);
            Options.Option = new List<MediaOptionData>(presets.Options.Option);
            Preset = new List<NetworkPresetData>(presets.Preset);
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
        public List<NetworkPresetData> Preset = new List<NetworkPresetData>();

        public void Sort()
        {
            Preset.Sort(new NetworkPresetData.CompareById());
        }
    }
}
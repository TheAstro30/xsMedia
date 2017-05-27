using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using xsCore.PlayerControls.Controls;

namespace xsSettings.Settings
{
    [Serializable]
    public class SettingsPlayer
    {
        public SettingsPlayer()
        {
            /* Default constructor */
        }

        public SettingsPlayer(SettingsPlayer player)
        {
            MediaVolume = player.MediaVolume;
            CounterType = player.CounterType;
            Options.Option = new List<SettingsMediaOptions.MediaOption>(player.Options.Option);
        }

        [XmlAttribute("volume")]
        public int MediaVolume { get; set; }

        [XmlAttribute("counter")]
        public MediaCounter.TimeDisplay CounterType { get; set; }

        [XmlElement("options")]
        public SettingsMediaOptions Options = new SettingsMediaOptions();
    }
}
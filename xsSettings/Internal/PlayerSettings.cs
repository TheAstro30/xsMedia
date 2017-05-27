﻿using System;
using System.Drawing;
using System.Xml.Serialization;
using xsCore.PlayerControls.Controls;
using xsSettings.Settings;

namespace xsSettings.Internal
{
    [Serializable, XmlRoot("settings")]
    public class PlayerSettings
    {
        public PlayerSettings()
        {
            Window.Location = new Point(100, 100);
            Window.Size = new Size(431, 235);
            Window.CurrentSkin = @"\skins\classic\classic.xml";
            /* Player */
            Player.MediaVolume = 50;
            Player.CounterType = MediaCounter.TimeDisplay.Elapsed;

            Player.Options.Option.AddRange(new[]
                                               {
                                                   new SettingsMediaOptions.MediaOption("--no-osd"),
                                                   new SettingsMediaOptions.MediaOption("--disable-screensaver"),
                                                   new SettingsMediaOptions.MediaOption("--ffmpeg-hw"),
                                                   new SettingsMediaOptions.MediaOption("--sub-filter=marq:logo"),
                                                   new SettingsMediaOptions.MediaOption("--plugin-path", @"\plugins")                                                   
                                               });
            /* Discs */
            Cdda.Options.Option.Add(new SettingsMediaOptions.MediaOption(":disc-caching", "300"));
            Cdda.Cddb.Enabled = true;
            Cdda.Cddb.Host = "http://freedb.freedb.org/~cddb/cddb.cgi";
            Cdda.Cddb.Cache = true;

            Vcd.Options.Option.Add(new SettingsMediaOptions.MediaOption(":disc-caching", "300"));

            Dvd.Options.Option.Add(new SettingsMediaOptions.MediaOption(":disc-caching", "300"));
            /* Basic network preset data */
            NetworkPresets.Options.Option.Add(new SettingsMediaOptions.MediaOption(":network-caching", "1000"));
            NetworkPresets.Preset.AddRange(new[]
                                               {
                                                   new SettingsNetworkPresets.SettingsNetworkPresetData
                                                       {
                                                           Id = "ABC Classic FM",
                                                           Url = "http://shoutmedia.abc.net.au:10422"
                                                       },
                                                   new SettingsNetworkPresets.SettingsNetworkPresetData
                                                       {
                                                           Id = "936 ABC Hobart",
                                                           Url = "http://shoutmedia.abc.net.au:10358"
                                                       },                                                   
                                                   new SettingsNetworkPresets.SettingsNetworkPresetData
                                                       {
                                                           Id = "ABC Country",
                                                           Url = "http://shoutmedia.abc.net.au:10432"
                                                       },
                                                   new SettingsNetworkPresets.SettingsNetworkPresetData
                                                       {
                                                           Id = "ABC Jazz",
                                                           Url = "http://shoutmedia.abc.net.au:10430"
                                                       },
                                                   new SettingsNetworkPresets.SettingsNetworkPresetData
                                                       {
                                                           Id = "ABC Newsradio",
                                                           Url = "http://shoutmedia.abc.net.au:10324"
                                                       },
                                                   new SettingsNetworkPresets.SettingsNetworkPresetData
                                                       {
                                                           Id = "ABC Radio National",
                                                           Url = "http://shoutmedia.abc.net.au:10420"
                                                       },
                                                   new SettingsNetworkPresets.SettingsNetworkPresetData
                                                       {
                                                           Id = "ABC Triple J",
                                                           Url = "http://shoutmedia.abc.net.au:10426"
                                                       },
                                                   new SettingsNetworkPresets.SettingsNetworkPresetData
                                                       {
                                                           Id = "Sky News",
                                                           Url = "mms://live1.wm.skynews.servecast.net/skynews_wmlz_live300k"
                                                       },
                                                   new SettingsNetworkPresets.SettingsNetworkPresetData
                                                       {
                                                           Id = "Slice Radio",
                                                           Url = "http://209.73.138.42:7963"
                                                       }
                                               });
            NetworkPresets.Sort();
            NetworkPresets.ShowQuality = true;
            NetworkPresets.Proxy.Port = 8080;
        }

        /* Copy constructor */
        public PlayerSettings(PlayerSettings settings)
        {
            Window = new SettingsWindow(settings.Window);
            Player = new SettingsPlayer(settings.Player);
            /* Disc */
            Cdda = new SettingsDisc(settings.Cdda);
            Vcd = new SettingsDisc(settings.Vcd);
            Dvd = new SettingsDisc(settings.Dvd);
            /* Network */
            NetworkPresets = new SettingsNetworkPresets(settings.NetworkPresets);
            /* Filters */
            Filters = new SettingsFilters(settings.Filters);
        }

        [XmlElement("window")]
        public SettingsWindow Window = new SettingsWindow();

        [XmlElement("player")]
        public SettingsPlayer Player = new SettingsPlayer();

        [XmlElement("cdda")]
        public SettingsDisc Cdda = new SettingsDisc();

        [XmlElement("vcd")]
        public SettingsDisc Vcd = new SettingsDisc();

        [XmlElement("dvd")]
        public SettingsDisc Dvd = new SettingsDisc();

        [XmlElement("network-presets")]
        public SettingsNetworkPresets NetworkPresets = new SettingsNetworkPresets();

        [XmlElement("paths")]
        public SettingsPaths Paths = new SettingsPaths();

        [XmlElement("filters")]
        public SettingsFilters Filters = new SettingsFilters();
    }
}
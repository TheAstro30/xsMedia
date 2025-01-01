/* xsMedia - xsSettings
 * (c)2013 - 2025
 * Jason James Newland
 * KangaSoft Software, All Rights Reserved
 * Licenced under the GNU public licence */
using System;
using System.Drawing;
using System.Xml.Serialization;
using xsCore.PlayerControls.Controls;
using xsCore.Settings.Data;
using xsCore.Settings.Data.Enums;

namespace xsCore.Internal
{
    [Serializable, XmlRoot("settings")]
    public class PlayerSettings
    {
        public PlayerSettings()
        {
            /* Penis window setup */
            Window.MainWindow.Location = new Point(100, 100);
            Window.MainWindow.Size = new Size(620, 420);
            Window.CurrentSkin = @"\skins\classic\classic.xml";
            /* Player */
            Player.MediaVolume = 50;
            Player.CounterType = MediaCounter.TimeDisplay.Elapsed;

            Player.Options.Option.AddRange(new[]
                {
                    new MediaOptions.MediaOption("--no-osd"),
                    new MediaOptions.MediaOption("--disable-screensaver"),
                    //new SettingsMediaOptions.MediaOption("--ffmpeg-hw"),
                    new MediaOptions.MediaOption("--sub-filter=marq:logo"),
                    //new SettingsMediaOptions.MediaOption("--plugin-path", @"\plugins"),
                    new MediaOptions.MediaOption("--soundfont", @"\soundfonts\Unison.sf2")
                });
            Option.Id = "Playback";
            /* Playback options */
            Player.Speed = 1;
            Player.Loop = PlaybackLoopMode.None;
            Player.JumpStep = 5;
            Player.VolumeStep = 5;

            Player.Video.Resize = VideoWindowResizeOption.WindowSize;
            Player.Video.EnableVideoTitle = true;
            Player.Video.VideoTitleTimeOut = 6;
            /* Discs */
            Cdda.Options.Option.Add(new MediaOptions.MediaOption(":disc-caching", "300"));
            Cdda.Cddb.Enabled = true;
            Cdda.Cddb.Host = "http://gnudb.gnudb.org/~cddb/cddb.cgi";
            Cdda.Cddb.Cache = true;

            Vcd.Options.Option.Add(new MediaOptions.MediaOption(":disc-caching", "300"));

            Dvd.Options.Option.Add(new MediaOptions.MediaOption(":disc-caching", "300"));
            /* Basic network preset data */
            NetworkPresets.Options.Option.Add(new MediaOptions.MediaOption(":network-caching", "1000"));
            NetworkPresets.Preset.AddRange(new[]
                {
                    new NetworkPresets.SettingsNetworkPresetData
                        {
                            Id = "Hobart FM",
                            Url = "http://203.45.159.151:88/broadwave.mp3?src=1&rate=1&ref="
                        },
                    new NetworkPresets.SettingsNetworkPresetData
                        {
                            Id = "ABC News Radio",
                            Url = "http://live-radio01.mediahubaustralia.com/PBW/mp3/"
                        },
                    new NetworkPresets.SettingsNetworkPresetData
                        {
                            Id = "Triple J (NSW)",
                            Url = "http://live-radio01.mediahubaustralia.com/2TJW/mp3/"
                        },
                    new NetworkPresets.SettingsNetworkPresetData
                        {
                            Id = "ABC Radio National (NSW)",
                            Url = "http://live-radio01.mediahubaustralia.com/2RNW/mp3/"
                        },
                    new NetworkPresets.SettingsNetworkPresetData
                        {
                            Id = "GrooveHitzRadio",
                            Url = "http://server10.reliastream.com:8003/stream"
                        }
                });
            NetworkPresets.Sort();
            NetworkPresets.ShowQuality = true;
            NetworkPresets.Proxy.Port = 8080;
        }

        /* Copy constructor */
        public PlayerSettings(PlayerSettings settings)
        {
            Window = new Window(settings.Window);
            Player = new PlayerData(settings.Player);
            Option = new SelectedTab(settings.Option);
            /* Disc */
            Cdda = new DiscData(settings.Cdda);
            Vcd = new DiscData(settings.Vcd);
            Dvd = new DiscData(settings.Dvd);
            /* Network */
            NetworkPresets = new NetworkPresets(settings.NetworkPresets);
            /* Paths */
            Paths = new Paths(settings.Paths);
            /* Filters */
            Filters = new Filters(settings.Filters);
            /* Favorites */
            Favorites = new Favorites(settings.Favorites);
        }

        [XmlElement("option")] 
        public SelectedTab Option = new SelectedTab();

        [XmlElement("window")]
        public Window Window = new Window();

        [XmlElement("player")]
        public PlayerData Player = new PlayerData();

        [XmlElement("cdda")]
        public DiscData Cdda = new DiscData();

        [XmlElement("vcd")]
        public DiscData Vcd = new DiscData();

        [XmlElement("dvd")]
        public DiscData Dvd = new DiscData();

        [XmlElement("network-presets")]
        public NetworkPresets NetworkPresets = new NetworkPresets();

        [XmlElement("paths")]
        public Paths Paths = new Paths();

        [XmlElement("filters")]
        public Filters Filters = new Filters();

        [XmlElement("favorites")]
        public Favorites Favorites = new Favorites();
    }
}

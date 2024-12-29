/* xsMedia - xsSettings
 * (c)2013 - 2024
 * Jason James Newland
 * KangaSoft Software, All Rights Reserved
 * Licenced under the GNU public licence */
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using xsCore.PlayerControls.Controls;
using xsSettings.Settings.Enums;

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
            Speed = player.Speed;
            Loop = player.Loop;
            JumpStep = player.JumpStep;
            VolumeStep = player.VolumeStep;

            Video.Resize = player.Video.Resize;
            Video.EnableVideoTitle = player.Video.EnableVideoTitle;
            Video.VideoTitleTimeOut = player.Video.VideoTitleTimeOut;

            Options.Option = new List<SettingsMediaOptions.MediaOption>(player.Options.Option);
            FileHistory = new List<SettingsHistoryData>(player.FileHistory);
        }

        [XmlAttribute("volume")]
        public int MediaVolume { get; set; }

        [XmlAttribute("counter")]
        public MediaCounter.TimeDisplay CounterType { get; set; }

        [XmlAttribute("speed")]
        public float Speed { get; set; }

        [XmlAttribute("loop")]
        public PlaybackLoopMode Loop { get; set; }

        [XmlAttribute("jumpStep")]
        public int JumpStep { get; set; }

        [XmlAttribute("volumeStep")]
        public int VolumeStep { get; set; }

        [XmlElement("video")]
        public SettingsVideoOptions Video = new SettingsVideoOptions();

        [XmlElement("options")]
        public SettingsMediaOptions Options = new SettingsMediaOptions();

        [XmlElement("fileHistory")]
        public List<SettingsHistoryData> FileHistory = new List<SettingsHistoryData>();

        public void AddHistory(string fileName)
        {
            /* First check it doesn't already exist in the list */
            if (FileHistory.Any(h => h.Path.Equals(fileName, StringComparison.InvariantCultureIgnoreCase)))
            {
                return;
            }
            /* Now add it */
            var data = new SettingsHistoryData
            {
                Path = fileName,
                FriendlyName = Path.GetFileNameWithoutExtension(fileName)
            };
            FileHistory.Insert(0, data);
            if (FileHistory.Count - 1 > 25)
            {
                /* Remove last entry */
                FileHistory.RemoveAt(FileHistory.Count -1);
            }
        }
    }
}
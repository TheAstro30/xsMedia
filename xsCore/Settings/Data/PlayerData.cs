/* xsMedia - xsSettings
 * (c)2013 - 2025
 * Jason James Newland
 * KangaSoft Software, All Rights Reserved
 * Licenced under the GNU public licence */
using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using xsCore.PlayerControls.Controls;
using xsCore.Settings.Data.Enums;

namespace xsCore.Settings.Data
{
    [Serializable]
    public class PlayerData
    {
        public PlayerData()
        {
            /* Default constructor */
        }

        public PlayerData(PlayerData player)
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

            Options.Option = new List<MediaOptions.MediaOption>(player.Options.Option);
            FileHistory = new List<HistoryData>(player.FileHistory);
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

        [XmlAttribute("onTop")]
        public bool AlwaysOnTop { get; set; }

        [XmlElement("video")]
        public VideoOptions Video = new VideoOptions();

        [XmlElement("options")]
        public MediaOptions Options = new MediaOptions();

        [XmlElement("fileHistory")]
        public List<HistoryData> FileHistory = new List<HistoryData>();
    }
}
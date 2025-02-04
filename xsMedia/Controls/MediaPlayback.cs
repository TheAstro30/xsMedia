﻿/* xsMedia - Media Player
 * (c)2013 - 2025
 * Jason James Newland
 * KangaSoft Software, All Rights Reserved
 * Licenced under the GNU public licence */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using xsCore;
using xsCore.CdUtils;
using xsCore.Controls.Forms;
using xsCore.Settings.Data.Enums;
using xsCore.Settings.Data.Filter;
using xsCore.Settings.Data.Media;
using xsCore.Utils.Asx;
using xsCore.Utils.IO;
using xsCore.Utils.SystemUtils;
using xsCore.Utils.UI;
using xsMedia.Logic;
using xsVlc.Common;
using xsVlc.Common.Events;
using xsVlc.Common.Filters;
using xsVlc.Common.Media;
using xsVlc.Common.Players;
using xsVlc.Core.Equalizer;
using xsVlc.Core;

namespace xsMedia.Controls
{
    public sealed class MediaPlayback : Spinner
    {
        private readonly UiSynchronize _sync;

        private readonly IMediaPlayerFactory _mediaFactory;
        private readonly IMediaListPlayer _listPlayer;
        private IMedia _originalMedia;        
       
        private bool _eqEnable;

        private readonly FolderSearch _folderSearch;

        private int _volume;
        private readonly Timer _tmrVolume;

        private bool _titleShown;
        private int _titleOpacity;
        private int _titleTimeOut;
        private readonly Timer _tmrTitleTimeOut;
        
        private List<CdTrackInfo> _cdTrackList;

        public event Action<object, MediaPlayerTimeChanged> TimeChanged;
        public event Action<object, MediaPlayerPositionChanged> PositionChanged;
        public event Action<object, EventArgs> MediaEnded;

        public event Action<object, MediaStateChange> MediaStateChanged;
        public event Action<object, MediaDurationChange> MediaDurationChanged;
        public event Action<object, MediaParseChange> MediaParseChanged;        

        public event Action<int, string> CdMediaInserted;
        public event Action<int, string> CdMediaRemoved;
        public event Action<int, string> DvdMediaInserted;
        public event Action<int, string> DvdMediaRemoved;

        #region MediaPlayback(IWin32Window parentWindow, string[] options)
        public MediaPlayback(IWin32Window parentWindow, string[] options)
        {
            BackColor = Color.Black;
            /* Spinner properties */
            OuterCircleRadius = 30;
            InnerCircleRadius = 20;
            NumberOfSpokes = 12;
            SpokeThickness = 6;            
            /* Cd control */
            CdControl = new CdManager(parentWindow);
            CdControl.OnCdMediaInserted += OnCdMediaInserted;
            CdControl.OnCdMediaRemoved += OnCdMediaRemoved;
            CdControl.OnDvdMediaInserted += OnDvdMediaInserted;
            CdControl.OnDvdMediaRemoved += OnDvdMediaRemoved;
            
            _mediaFactory = new MediaPlayerFactory(options);
            _listPlayer = _mediaFactory.CreateMediaListPlayer<IMediaListPlayer>(PlaylistManager.MediaList);

            _listPlayer.Events.PlayerPositionChanged += OnPlayerPositionChanged;
            _listPlayer.Events.TimeChanged += OnTimeChanged;
            _listPlayer.Events.MediaEnded += OnMediaEnded;

            _listPlayer.InnerPlayer.WindowHandle = Handle;
            _listPlayer.InnerPlayer.MouseInputEnabled = true;

            _folderSearch = new FolderSearch();
            _folderSearch.OnFileFound += OnFileSearchFileFound;
            _folderSearch.OnFileSearchCompleted += OnFileSearchCompleted;

            _sync = new UiSynchronize(this);

            _tmrVolume = new Timer
            {
                Interval = 100
            };
            _tmrVolume.Tick += TmrVolumeTick;

            _tmrTitleTimeOut = new Timer
            {
                Interval = 100
            };
            _tmrTitleTimeOut.Tick += TmrTitleTimeOutTick;

            ZoomRatio = ZoomRatioMode.Mode3;

            /* Audio device list */
            OutputDevices = new AudioOutputDevices(_mediaFactory);
            if (OutputDevices.Count > 0)
            {
                /* Which it should be... set the first output device as "default" */
                SetAudioOutputDevice(OutputDevices[0]);
            }

            /* Create a basic equalizer */
            EqInitPreset(SettingsManager.Settings.Filters.Eq.Preset);
            /* Read the settings and make sure eq is set to on or off */
            EqPreamp(SettingsManager.Settings.Filters.Eq.Preamp);
            var count = 0;
            var bands = new[] { 31.25, 62.5, 125, 250, 500, 1000, 2000, 4000, 8000, 16000 };
            foreach (var band in bands)
            {
                if (SettingsManager.Settings.Filters.Eq.Band.Any(b => b.Frequency.Equals(band)))
                {
                    EqAdjustBand(count, SettingsManager.Settings.Filters.Eq.Band[count].Amplitude);
                    count++;
                    continue;
                }
                SettingsManager.Settings.Filters.Eq.Band.Add(new BandData(band, 0));
                EqAdjustBand(count, 0);
                count++;
            }
            /* Make sure its enabled/disabled */
            EqEnable = SettingsManager.Settings.Filters.Eq.Enable;
        }
        #endregion

        #region Paint
        protected override void OnPaint(PaintEventArgs e)
        {
            if (LogoImage == null || (SpinnerActive && !LogoImageAlwaysOnTop) || (IsVideo && (_listPlayer.PlayerState == MediaState.Playing || _listPlayer.PlayerState == MediaState.Paused)))
            {
                base.OnPaint(e);
                return;
            }            
            var sz = Drawing.ResizeBitmapWithAspect(ClientRectangle.Size, LogoImage.Size, LogoImageMaximumSize);
            var x = (ClientRectangle.Width / 2) - (sz.Width / 2);
            var y = (ClientRectangle.Height / 2) - (sz.Height / 2);

            e.Graphics.InterpolationMode = InterpolationMode.High;
            e.Graphics.DrawImage(LogoImage, x, y, sz.Width, sz.Height);
            if (SpinnerActive && LogoImageAlwaysOnTop)
            {
                base.OnPaint(e);
            }
        }
        #endregion

        #region Control properties
        /* Control properties */
        public AudioOutputDevices OutputDevices { get; private set; }

        public string GetAudioOutputDevice
        {
            get { return _listPlayer == null ? string.Empty : _listPlayer.InnerPlayer.GetAudioOutputDevice; }
        }

        public Equalizer Eq { get; private set; }
        public IAdjustFilter AdjustFilter { get { return _listPlayer.InnerPlayer.Adjust; } }
        public IMarqueeFilter MarqueeFilter { get { return _listPlayer.InnerPlayer.Marquee; } }
        public ILogoFilter LogoFilter { get { return _listPlayer.InnerPlayer.Logo; } }
        public ICropFilter CropFilter { get { return _listPlayer.InnerPlayer.CropGeometry; } }
        public IDeinterlaceFilter DeinterlaceFilter { get { return _listPlayer.InnerPlayer.Deinterlace; } }

        public Bitmap LogoImage { get; set; }
        public Size LogoImageMaximumSize { get; set; }
        public bool LogoImageAlwaysOnTop { get; set; }

        public CdManager CdControl { get; set; }
        public DiscType OpenDiscType { get; set; }
        public int CurrentTrack { get; set; }
        public IMediaFromFile CurrentMedia { get; set; }
        public bool IsVideo { get; set; }

        /* Audio track control */
        public int AudioTrack
        {
            get { return _listPlayer.InnerPlayer.AudioTrack; }
            set { _listPlayer.InnerPlayer.AudioTrack = value; }
        }

        public int AudioTrackCount
        {
            get { return _listPlayer.InnerPlayer.AudioTrackCount; }
        }

        public List<xsVlc.Interop.LibvlcTrackDescriptionT> AudioTrackDescription
        {
            get { return _listPlayer.InnerPlayer.AudioTrackDescription; }
        }

        /* Subtitle control */
        public string SetSubtitleFile
        {
            set
            {
                _listPlayer.InnerPlayer.SetSubtitleFile(value);
            }
        }

        public int Subtitles
        {
            get { return _listPlayer.InnerPlayer.SubTitle; }
            set { _listPlayer.InnerPlayer.SubTitle = value; }
        }

        public int SubtitlesCount
        {
            get { return _listPlayer.InnerPlayer.SubTitleCount; }
        }

        public List<xsVlc.Interop.LibvlcTrackDescriptionT> SubtitleDescription
        {
            get { return _listPlayer.InnerPlayer.SubTitleDescription; }
        }
        
        /* Video track control */
        public int VideoTrack
        {
            get { return _listPlayer.InnerPlayer.VideoTrack; }
            set { _listPlayer.InnerPlayer.VideoTrack = value; }
        }

        public int VideoTrackCount
        {
            get { return _listPlayer.InnerPlayer.VideoTrackCount; }
        }

        public List<xsVlc.Interop.LibvlcTrackDescriptionT> VideoTrackDescription
        {
            get { return _listPlayer.InnerPlayer.VideoTrackDescription; }
        }

        /* Speed playback */
        public float PlaybackSpeed
        {
            get { return _listPlayer.InnerPlayer.PlaybackRate; }
            set { _listPlayer.InnerPlayer.PlaybackRate = value; }
        }

        /* Aspect ratio control */
        public AspectRatioMode AspectRatio
        {
            get { return _listPlayer.InnerPlayer.AspectRatio; }
            set { _listPlayer.InnerPlayer.AspectRatio = value; }
        }

        /* Zoom ratio */
        public ZoomRatioMode ZoomRatio { get; set; }

        /* Volume */
        public int Volume
        {
            get { return _volume; }
            set
            {
                _volume = value;
                if (_listPlayer == null)
                {
                    return;
                }
                _listPlayer.InnerPlayer.Volume = _volume;
            }
        }

        public void ToggleMute()
        {
            _listPlayer.InnerPlayer.ToggleMute();            
        }

        public bool IsMuted
        {
            get { return _listPlayer.InnerPlayer.Mute; }
        }

        public MediaState PlayerState
        {
            get
            {
                return _listPlayer == null ? MediaState.Stopped : _listPlayer.PlayerState;
            }
        }

        public float Position
        {
            get
            {
                return _listPlayer == null ? 0 : _listPlayer.InnerPlayer.Position;
            }
            set
            {
                if (_listPlayer == null)
                {
                    return;
                }
                _listPlayer.InnerPlayer.Position = value;
            }
        }

        public long Length
        {
            get
            {
                return _listPlayer == null ? 0 : _listPlayer.InnerPlayer.Length;
            }
        }

        public Size VideoSize
        {
            get
            {
                return _listPlayer == null ? Size.Empty : _listPlayer.InnerPlayer.GetVideoSize(0);
            }
        }

        /* Equalizer */
        public bool EqEnable
        {
            get { return _eqEnable; }
            set
            {
                _eqEnable = value;
                Eq.SetEqualizer(_listPlayer.InnerPlayer, _eqEnable);
            }
        }
        #endregion

        #region Public methods
        public void SetAudioOutputDevice(AudioOutputDevice device)
        {
            if (device == null || device.Device == null)
            {
                return;
            }
            /* Need to first disable audio track, or it won't switch */
            var audioTrack = _listPlayer.InnerPlayer.AudioTrack;
            _listPlayer.InnerPlayer.AudioTrack = -1;
            _listPlayer.InnerPlayer.SetAudioOutputModuleAndDevice(device.Module, device.Device);
            _listPlayer.InnerPlayer.AudioTrack = audioTrack;
        }

        public void TakeSnapShot()
        {
            if (_listPlayer == null || !IsVideo)
            {
                return;
            }
            /* Create path to My Pictures folder xsMedia Snapshots */
            var path = string.Format("{0}\\xsMedia Snapshots\\", Environment.GetFolderPath(Environment.SpecialFolder.MyPictures));
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            /* Create output filename from the current media's input path */
            var output = Path.GetFileNameWithoutExtension(Uri.UnescapeDataString(CurrentMedia.Input));
            /* Create a unique file name based on output file name - currently limited to 1024 instances */
            var fo = new FileInfo(string.Format("{0}\\{1}.png", path, output));
            var file = fo.MakeUnique();
            _listPlayer.InnerPlayer.TakeSnapShot(0, file.FullName);
        }

        public void ApplyFilters()
        {
            if (!IsVideo)
            {
                return;
            }
            var enable = SettingsManager.Settings.Filters.Adjust.Enable;
            /* Adjust */
            if (enable)
            {
                AdjustFilter.Hue = SettingsManager.Settings.Filters.Adjust.Hue;
                AdjustFilter.Brightness = SettingsManager.Settings.Filters.Adjust.Brightness;
                AdjustFilter.Contrast = SettingsManager.Settings.Filters.Adjust.Contrast;
                AdjustFilter.Saturation = SettingsManager.Settings.Filters.Adjust.Saturation;
                AdjustFilter.Gamma = SettingsManager.Settings.Filters.Adjust.Gamma;
            }
            AdjustFilter.Enabled = enable;
            /* Marquee */
            if (!_titleShown && SettingsManager.Settings.Player.Video.EnableVideoTitle)
            {
                _titleTimeOut = 0;
                _titleOpacity = 255;
                MarqueeFilter.Text = PlaylistManager.Playlist[CurrentTrack].Title;
                MarqueeFilter.Position = xsVlc.Common.Position.Top;
                MarqueeFilter.Color = VlcColor.White;
                MarqueeFilter.Opacity = _titleOpacity;
                
                _tmrTitleTimeOut.Enabled = true;
                _titleShown = true;

                MarqueeFilter.Enabled = true;
            }
            if (!_tmrTitleTimeOut.Enabled)
            {
                enable = SettingsManager.Settings.Filters.Marquee.Enable;
                if (enable)
                {
                    MarqueeFilter.Text = SettingsManager.Settings.Filters.Marquee.Text;
                    MarqueeFilter.Position = (Position) SettingsManager.Settings.Filters.Marquee.Position;
                    MarqueeFilter.Color = (VlcColor) SettingsManager.Settings.Filters.Marquee.Color;
                    MarqueeFilter.Timeout = SettingsManager.Settings.Filters.Marquee.TimeOut;
                    MarqueeFilter.Opacity = SettingsManager.Settings.Filters.Marquee.Opacity;
                }
                MarqueeFilter.Enabled = enable;
            }
            /* Logo */
            enable = SettingsManager.Settings.Filters.Logo.Enable;
            if (enable)
            {
                LogoFilter.File = SettingsManager.Settings.Filters.Logo.File;
                LogoFilter.Opacity = SettingsManager.Settings.Filters.Logo.Opacity;
                LogoFilter.X = SettingsManager.Settings.Filters.Logo.LeftOffset;
                LogoFilter.Y = SettingsManager.Settings.Filters.Logo.TopOffset;
            }
            else
            {
                /* If this isn't done, and during this instance of the program, it will always show the image if Marquee is showing */
                LogoFilter.File = null;
            }
            LogoFilter.Enabled = enable;
            /* Crop */
            enable = SettingsManager.Settings.Filters.Crop.Enable;
            if (enable)
            {
                var sz = VideoSize;
                CropFilter.CropArea = new Rectangle(SettingsManager.Settings.Filters.Crop.Left,
                    SettingsManager.Settings.Filters.Crop.Top,
                    sz.Width - SettingsManager.Settings.Filters.Crop.Right,
                    sz.Height - SettingsManager.Settings.Filters.Crop.Bottom);
            }
            CropFilter.Enabled = enable;
            /* Deinterlace */
            enable = SettingsManager.Settings.Filters.Deinterlace.Enable;
            if (enable)
            {
                DeinterlaceFilter.Mode = (DeinterlaceMode) SettingsManager.Settings.Filters.Deinterlace.Mode;
            }
            DeinterlaceFilter.Enabled = enable;
        }

        #region Media opening methods
        public void OpenFile(string fileName, bool play = true)
        {
            var media = _mediaFactory.CreateMedia<IMedia>(fileName);
            PlaylistManager.Add(media, (int)_listPlayer.Length / 1000);
            if (!play)
            {
                return;
            }
            CurrentTrack = PlaylistManager.MediaList.Count - 1;
            MediaEventHandlers(CurrentTrack);
            _listPlayer.PlayItemAt(CurrentTrack);
            SettingsManager.AddHistory(fileName);
        }

        public void OpenNetwork(string networkUrl)
        {
            OpenNetwork(networkUrl, null, 0);
        }

        public void OpenNetwork(string networkUrl, string title, int length)
        {
            if (networkUrl.ToLower().Contains(".asx"))
            {
                AsxParser.ParseCompleted += OnAsxParserCompleted;
                AsxParser.ParseFailed += OnAsxParserFailed;
                AsxParser.BeginParse(networkUrl, SettingsManager.Settings.NetworkPresets.Proxy);
                return;
            }
            var options = SettingsManager.Settings.NetworkPresets.Options.ToArray();
            var media = _mediaFactory.CreateMedia<IMedia>(networkUrl, options);
            PlaylistManager.Add(media, options, length * 1000, title);
            CurrentTrack = PlaylistManager.MediaList.Count - 1;
            MediaEventHandlers(CurrentTrack);
            _listPlayer.PlayItemAt(CurrentTrack);
            SettingsManager.AddHistory(networkUrl);
        }

        public void OpenDisc(string drive)
        {
            /* Set current track to the last known track on list (sequential) */
            bool increaseCount;
            CurrentTrack = PlaylistManager.MediaList.Count - 1;
            if (CurrentTrack < 0)
            {
                CurrentTrack = 0;
                increaseCount = false;
            }
            else
            {
                increaseCount = true;
            }
            IMedia media;
            MediaOptions options;
            string[] optionString;
            switch (OpenDiscType)
            {
                case DiscType.Cdda:
                    {
                        /* Get number of tracks and add them as separate media objects to the list */
                        if (!CdControl.CdTrackInfo.Open(drive)) { return; }
                        var total = CdControl.CdTrackInfo.GetTrackCount();
                        _cdTrackList = new List<CdTrackInfo>();
                        for (var i = 1; i <= total; i++)
                        {
                            options = new MediaOptions(SettingsManager.Settings.Cdda.Options.Option,
                                                               new[]
                                                                   {
                                                                       ":cdda-track=" + i
                                                                   });
                            optionString = options.ToArray();
                            media = _mediaFactory.CreateMedia<IMedia>(string.Format("cdda:///{0}/", drive), optionString);
                            /* Give basic title information for the CD */
                            PlaylistManager.Add(media, optionString, CdControl.CdTrackInfo.GetTrackTime(i) * 1000,
                                                "CD Track " + string.Format("{0:D2}", i));

                            var track = new CdTrackInfo
                                            {
                                                Name = "Track " + i,
                                                Album = "Unknown",
                                                Genre = "Unknown"
                                            };
                            _cdTrackList.Add(track);
                        }
                        /* Begin CDDB query lookup */
                        if (SettingsManager.Settings.Cdda.Cddb.Enabled)
                        {
                            var cddb = new Cddb(SettingsManager.Settings.NetworkPresets.Proxy)
                                           {
                                               CddbHost = SettingsManager.Settings.Cdda.Cddb.Host,
                                               CddbUserName = "xsMedia_User",
                                               CddbApplicationName = "xsMedia",
                                               CddbCacheFile =
                                                   SettingsManager.Settings.Cdda.Cddb.Cache
                                                       ? AppPath.MainDir(@"\KangaSoft\xsMedia\cddbCache.xml", true)
                                                       : string.Empty
                                           };
                            cddb.CddbQueryComplete += OnCddbQueryComplete;
                            cddb.CddbQueryLookup(CdControl.CdTrackInfo, _cdTrackList,
                                                 increaseCount ? CurrentTrack + 1 : CurrentTrack);
                        }
                    }
                    break;

                case DiscType.Vcd:
                    options = new MediaOptions(SettingsManager.Settings.Vcd.Options.Option);
                    optionString = options.ToArray();
                    media = _mediaFactory.CreateMedia<IMediaFromFile>(string.Format("vcd:///{0}/", drive), optionString);
                    PlaylistManager.Add(media, optionString);
                    break;

                default:
                    options = new MediaOptions(SettingsManager.Settings.Dvd.Options.Option);
                    optionString = options.ToArray();
                    media = _mediaFactory.CreateMedia<IMediaFromFile>(string.Format("dvd:///{0}/", drive), optionString);
                    PlaylistManager.Add(media, optionString);
                    break;
            }
            /* Increase current track pointer */
            if (increaseCount)
            {
                CurrentTrack++;
                if (CurrentTrack > PlaylistManager.MediaList.Count - 1)
                {
                    CurrentTrack = PlaylistManager.MediaList.Count - 1;
                }
            }
            MediaEventHandlers(CurrentTrack);
            _listPlayer.PlayItemAt(CurrentTrack);
        }

        public void OpenList(string fileName)
        {
            if (!File.Exists(fileName)) { return; }
            PlaylistManager.Load(fileName);
        }

        public void OpenFolder(string path)
        {
            if (string.IsNullOrEmpty(path)) { return; }
            CurrentTrack = PlaylistManager.MediaList.Count - 1;
            /* Increase current track pointer */
            CurrentTrack = CurrentTrack < 0 || CurrentTrack == 0 ? 0 : CurrentTrack++;
            /* Begin search */
            _folderSearch.BeginSearch(new DirectoryInfo(path), FileFilters.OpenFilters.SupportedMasks(), "*", true);                                   
        }
        #endregion

        #region Playback/transport control
        public bool Play(bool selected = false, int track = -1)
        {            
            if (_listPlayer.PlayerState == MediaState.Paused)
            {
                _listPlayer.Play();
                return true;
            }                       
            ZoomRatio = ZoomRatioMode.Mode3;
            var count = PlaylistManager.MediaList.Count;
            if (count <= 0)
            {
                return false;
            }            
            if (CurrentTrack > count - 1)
            {
                CurrentTrack = 0;
            }
            /* Need to check we haven't selected a different index in playlist; else play current track */
            CurrentTrack = !selected ? CurrentTrack : track;
            Play(CurrentTrack);
            return true;
        }

        public void Play(int position)
        {
            CurrentTrack = position;
            if (CurrentTrack > PlaylistManager.MediaList.Count - 1)
            {
                CurrentTrack = PlaylistManager.MediaList.Count - 1;
            }
            if (CurrentTrack < 0) { CurrentTrack = 0; }
            MediaEventHandlers(CurrentTrack);
            _listPlayer.PlayItemAt(CurrentTrack);
            /* Set disc type - this *should* always be done if an item in the playlist is selected (or auto-play next track) */
            var uri = new Uri(PlaylistManager.MediaList[CurrentTrack].Input).LocalPath;
            if (uri.Length < 2)
            {
                OpenDiscType = DiscType.None;
                return;
            }
            var driveLetter = uri.Substring(0, 2);
            var deviceId = CdControl.GetDeviceId(string.Format("{0}\\", driveLetter));
            if (deviceId != -1 && CdControl.AvailableDrives.ContainsKey(deviceId))
            {
                OpenDiscType = CdControl.AvailableDrives[deviceId].Type;                
            }
            else
            {
                OpenDiscType = DiscType.None;
            }
        }

        public void Pause()
        {
            _listPlayer.Pause();
        }

        public void Stop()
        {
            _listPlayer.Stop();            
        }

        public void Previous()
        {
            PreviousNextTrack(false);
        }

        public void Next()
        {
            PreviousNextTrack(true);
        }
        #endregion

        #region EQ
        public void EqInitPreset(int index)
        {
            if (Eq != null)
            {
                Eq.Dispose();
            }
            Eq = index > 0
                      ? new Equalizer(Equalizer.Presets.FirstOrDefault(i => i.Index == index - 1))
                      : new Equalizer();
        }

        public void EqPreamp(double value)
        {
            Eq.Preamp = (float)value;
        }

        public void EqAdjustBand(int band, double amplitude)
        {
            Eq.Bands[band].Amplitude = (float)amplitude;
        }
        #endregion
        #endregion

        #region Callbacks
        private void OnPlayerPositionChanged(object sender, MediaPlayerPositionChanged e)
        {
            if (PositionChanged != null)
            {
                PositionChanged(sender, e);
            }
        }

        private void OnTimeChanged(object sender, MediaPlayerTimeChanged e)
        {
            if (TimeChanged != null)
            {
                TimeChanged(sender, e);
            }
        }

        private void OnMediaDurationChanged(object sender, MediaDurationChange e)
        {
            if (MediaDurationChanged != null)
            {
                MediaDurationChanged(sender, e);
            }
        }

        private void OnMediaEnded(object sender, EventArgs e)
        {
            if (MediaEnded != null)
            {
                MediaEnded(sender, e);
            }
        }

        private void OnMediaParseChanged(object sender, MediaParseChange e)
        {
            if (MediaParseChanged != null)
            {
                MediaParseChanged(sender, e);
            }
        }

        private void OnMediaStateChanged(object sender, MediaStateChange e)
        {
            switch (e.NewState)
            {
                case MediaState.Opening:
                case MediaState.Buffering:
                    _titleShown = false;
                    SpinnerState(true);
                    BeginTimer();
                    break;

                case MediaState.Playing:
                    SpinnerState(false);
                    IsVideo = _listPlayer.InnerPlayer.VideoTrackCount > 0 || OpenDiscType == DiscType.Dvd || OpenDiscType == DiscType.Vcd;
                    LogoImageAlwaysOnTop = false;
                    break;

                case MediaState.Stopped:
                    _titleShown = false;
                    IsVideo = false;
                    OpenDiscType = DiscType.None;
                    break;

                case MediaState.Error:
                    _titleShown = false;
                    IsVideo = false;
                    OpenDiscType = DiscType.None;
                    SpinnerState(false);
                    LogoImageAlwaysOnTop = false;
                    break;
            }
            if (MediaStateChanged != null)
            {
                MediaStateChanged(sender, e);
            }
            InvalidateWindow();
        }

        private void OnCdMediaInserted(int deviceId, string volumeLetter)
        {
            if (CdMediaInserted != null)
            {
                CdMediaInserted(deviceId, volumeLetter);
            }
        }

        private void OnCdMediaRemoved(int deviceId, string volumeLetter)
        {
            if (CdMediaRemoved != null)
            {
                CdMediaRemoved(deviceId, volumeLetter);
            }
        }

        private void OnDvdMediaInserted(int deviceId, string volumeLetter)
        {
            if (DvdMediaInserted != null)
            {
                DvdMediaInserted(deviceId, volumeLetter);
            }
        }

        private void OnDvdMediaRemoved(int deviceId, string volumeLetter)
        {
            if (DvdMediaRemoved != null)
            {
                DvdMediaRemoved(deviceId, volumeLetter);
            }
        }

        /* CDDB */
        private void OnCddbQueryComplete(int playlistStartPosition)
        {
            var i = playlistStartPosition;
            foreach (var s in _cdTrackList)
            {
                var meta = PlaylistManager.GetMetaData(i);
                meta.Title = s.Name;
                meta.Artist = s.Artist;
                meta.Album = s.Album;
                PlaylistManager.SetMetaData(i, meta);
                i++;
            }
        }

        /* Folder search callbacks */
        private void OnFileSearchFileFound(string file)
        {
            var media = _mediaFactory.CreateMedia<IMedia>(file);
            PlaylistManager.Add(media);
        }

        private void OnFileSearchCompleted(FolderSearch sender)
        {
            if (PlaylistManager.MediaList.Count == 0)
            {
                return;
            }
            if (CurrentTrack > PlaylistManager.MediaList.Count - 1)
            {
                CurrentTrack = PlaylistManager.MediaList.Count - 1;
            }
            MediaEventHandlers(CurrentTrack);
            _listPlayer.PlayItemAt(CurrentTrack);
        }

        /* Network ASX parser callbacks */
        private void OnAsxParserCompleted(AsxData data)
        {
            AsxParser.ParseCompleted -= OnAsxParserCompleted;
            AsxParser.ParseFailed -= OnAsxParserFailed;
            var options = SettingsManager.Settings.NetworkPresets.Options.ToArray();
            CurrentTrack = CurrentTrack < 0 || CurrentTrack == 0 ? 0 : CurrentTrack++;
            foreach (var entry in data.Entries)
            {
                if (entry == null || string.IsNullOrEmpty(entry.Url))
                {
                    continue;
                }
                var media = _mediaFactory.CreateMedia<IMedia>(entry.Url, options);
                PlaylistManager.Add(media, options, 0, entry.Title);
            }
            if (PlaylistManager.Playlist.Count == 0)
            {
                /* If it fails, force an error */
                OnMediaStateChanged(this, new MediaStateChange(MediaState.Error));
                return;
            }
            MediaEventHandlers(CurrentTrack);
            _listPlayer.PlayItemAt(CurrentTrack);
        }

        private void OnAsxParserFailed()
        {
            AsxParser.ParseCompleted -= OnAsxParserCompleted;
            AsxParser.ParseFailed -= OnAsxParserFailed;
            /* If it fails, force an error */
            OnMediaStateChanged(this, new MediaStateChange(MediaState.Error));
        }

        private void TmrVolumeTick(object sender, EventArgs e)
        {
            /* Check video also */
            if (!IsVideo && _listPlayer.InnerPlayer.VideoTrackCount > 0)
            {
                IsVideo = true;
            }
            if (_listPlayer.InnerPlayer.Volume == -1)
            {
                /* Uninitialized - need to wait until it is initialized */
                return;
            }
            if (_listPlayer.InnerPlayer.Volume == _volume)
            {
                /* Volume is set correctly - disable timer */
                _tmrVolume.Enabled = false;
                return;
            }
            /* Volume is incorrectly set, reset it */
            _listPlayer.InnerPlayer.Volume = _volume;
        }

        private void TmrTitleTimeOutTick(object sender, EventArgs e)
        {
            /* This timer does 3 things:
             * 1) counts to roughly X seconds (based on settings)
             * 2) gradually fades out the title, and
             * 3) waits a couple of seconds before applying any currently set Marquee */
            var timeOut = SettingsManager.Settings.Player.Video.VideoTitleTimeOut * 1000;
            if (_titleTimeOut >= timeOut)
            {
                /* Begin opacity change */
                _titleOpacity -= 15;
                MarqueeFilter.Opacity = _titleOpacity;
                if (_titleOpacity > 0)
                {
                    return;
                }
                /* Wait a couple of seconds */
                _titleTimeOut = 0;
                _titleOpacity = 0;
                MarqueeFilter.Enabled = false;
            }
            else if (_titleOpacity == 0 && _titleTimeOut >= 2000)
            {
                _tmrTitleTimeOut.Enabled = false;
                ApplyFilters();
            }
            else
            {
                _titleTimeOut += 100;
            }
        }
        #endregion

        #region Private methods
        private void InvalidateWindow()
        {
            if (InvokeRequired)
            {
                _sync.Execute(InvalidateWindow);                
                return;
            }
            Invalidate();
        }

        private void SpinnerState(bool state)
        {
            if (InvokeRequired)
            {
                /* Animation timer will NOT work in cross-thread */
                _sync.Execute(() => SpinnerState(state));
                return;
            }
            SpinnerActive = state;
        }

        private void MediaEventHandlers(int index)
        {
            if (CurrentMedia != null && _originalMedia != null)
            {
                _originalMedia.Events.StateChanged -= OnMediaStateChanged;
                CurrentMedia.Events.DurationChanged -= OnMediaDurationChanged;
                CurrentMedia.Events.ParsedChanged -= OnMediaParseChanged;
            }
            /* Have to create a new object so we can get the meta data */
            _originalMedia = PlaylistManager.MediaList[index];
            CurrentMedia = _mediaFactory.CreateMedia<IMediaFromFile>(new Uri(_originalMedia.Input).LocalPath);
            CurrentMedia.Parse(true);
            /* Set events */
            _originalMedia.Events.StateChanged += OnMediaStateChanged;
            CurrentMedia.Events.DurationChanged += OnMediaDurationChanged;
            CurrentMedia.Events.ParsedChanged += OnMediaParseChanged;            
        }

        private void PreviousNextTrack(bool next)
        {
            if (PlaylistManager.MediaList.Count == 0)
            {
                return;
            }
            switch (OpenDiscType)
            {
                case DiscType.Vcd:
                case DiscType.Dvd:
                    /* TODO: verify this code below works with a DVD and if we need the "Play" part at the bottom */
                    if (next)
                    {
                        _listPlayer.InnerPlayer.NextChapter();
                    }
                    else
                    {
                        _listPlayer.InnerPlayer.PreviousChapter();
                    }
                    break;

                default:
                    /* Need to follow loop settings for shuffle, at least; ignore loop one */
                    switch (SettingsManager.Settings.Player.Loop)
                    {
                        case PlaybackLoopMode.Shuffle:
                            var rnd = new Random();
                            CurrentTrack = rnd.Next(0, PlaylistManager.MediaList.Count);
                            break;

                        case PlaybackLoopMode.LoopAll:
                        case PlaybackLoopMode.LoopOne:
                        case PlaybackLoopMode.None:
                            if (next)
                            {
                                CurrentTrack++;
                            }
                            else
                            {
                                CurrentTrack--;
                            }
                            break;
                    }
                    /* If we reach the end of the list, jump to the start; or go past the start, jump to the end */
                    if (CurrentTrack < 0)
                    {
                        CurrentTrack = PlaylistManager.MediaList.Count - 1;
                    }
                    if (CurrentTrack > PlaylistManager.MediaList.Count - 1)
                    {
                        CurrentTrack = 0;
                    }
                    /* Mainly reset this variable for video title */
                    Player.IsVideoWindowInit = false;
                    Play(CurrentTrack);
                    break;
            }
        }

        private void BeginTimer()
        {
            if (InvokeRequired)
            {
                _sync.Execute(BeginTimer);
                return;
            }
            _tmrVolume.Enabled = true;
        }
        #endregion       
    }
}

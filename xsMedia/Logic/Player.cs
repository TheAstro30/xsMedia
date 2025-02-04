﻿/* xsMedia - Media Player
 * (c)2013 - 2025
 * Jason James Newland
 * KangaSoft Software, All Rights Reserved
 * Licenced under the GNU public licence */
using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using xsCore;
using xsCore.CdUtils;
using xsCore.Settings.Data.Enums;
using xsCore.Utils.IO;
using xsCore.Utils.SystemUtils;
using xsCore.Utils.UI;
using xsMedia.Forms;
using xsMedia.Helpers;
using xsVlc.Common;

namespace xsMedia.Logic
{
    public sealed class Player
    {
        private static FrmPlayer _player;
        private static Timer _tmrStart;

        public static UiSynchronize Sync { get; set; }
        
        public static bool IsVideoWindowInit { get; set; }

        public static void Init(FrmPlayer player, string args)
        {
            _player = player;
            /* Sync object */
            Sync = new UiSynchronize(_player);
            /* Drag drop */
            _player.AllowDrop = true;
            _player.DragEnter += OnDragEnter;
            _player.DragDrop += OnDragDrop;
        }

        public static void BeginProcessCommandLine(string args)
        {
            /* Allows the form to show first */
            _tmrStart = new Timer
            {
                Interval = 100,
                Tag = args,
                Enabled = true
            };
            _tmrStart.Tick += TimerStartTick;
        }

        public static void ProcessCommandLine(string args)
        {
            if (string.IsNullOrEmpty(args)) { return; }   
            var sp = args.Split(':');
            //var path = new Uri(args).LocalPath; - this mucks with files that have # and & in them, so changed to next line v1.0.5
            var path = args.Replace(string.Format("{0}:", sp[0]), "");
            if (string.IsNullOrEmpty(path) || sp.Length == 0)
            {
                return;
            }
            if (path.Contains("/"))
            {
                path = path.Replace("/", "");
            }
            if (path.Contains(((char)34).ToString(CultureInfo.InvariantCulture)))
            {
                path = path.Replace(((char)34).ToString(CultureInfo.InvariantCulture), "");
            }

            Video.VideoControl.OpenDiscType = DiscType.None;
            IsVideoWindowInit = false;

            switch (sp[0].ToUpper())
            {
                case "FILE":
                    Video.VideoControl.OpenFile(path);
                    break;

                case "FOLDER":
                    Video.VideoControl.OpenFolder(path);
                    break;

                case "CDDA":
                    Video.VideoControl.OpenDiscType = DiscType.Cdda;
                    Open.DriveLetter = path.Replace(@"\", null);
                    Video.VideoControl.OpenDisc(Open.DriveLetter);
                    break;

                case "VCD":
                    Video.VideoControl.OpenDiscType = DiscType.Vcd;
                    Open.DriveLetter = path.Replace(@"\", null);
                    Video.VideoControl.OpenDisc(Open.DriveLetter);
                    break;

                case "DVD":
                    Video.VideoControl.OpenDiscType = DiscType.Dvd;
                    Open.DriveLetter = path.Replace(@"\", null);
                    Video.VideoControl.OpenDisc(Open.DriveLetter);
                    break;

                case "PLAYLIST":
                    Video.VideoControl.OpenList(path);
                    break;
            }
        }

        public static void StopClip(bool nextTrack, bool keepVideoSize = false)
        {
            /* I farted, and it was a ripper! */
            Video.VideoControl.Stop();
            //Video.VideoControl.LogoImage = Properties.Resources.videoWinIcon.ToBitmap();
            Video.VideoControl.LogoImage = MainIconUtil.VideoWindowIcon();
            Video.VideoControl.LogoImageMaximumSize = Video.VideoControl.LogoImage.Size;

            IsVideoWindowInit = false;

            var count = PlaylistManager.MediaList.Count;
            if (nextTrack && count > 1)
            {
                /* Process loop setting */
                switch (SettingsManager.Settings.Player.Loop)
                {
                    case PlaybackLoopMode.LoopOne:
                        /* Loop same track */
                        Video.VideoControl.Play(Video.VideoControl.CurrentTrack);
                        return;

                    case PlaybackLoopMode.LoopAll:
                        /* Check that current track is total list count */
                        if (Video.VideoControl.CurrentTrack == count - 1)
                        {
                            Video.VideoControl.CurrentTrack = 0;
                            Video.VideoControl.Play(0);
                            return;
                        }
                        break;

                    case PlaybackLoopMode.Shuffle:
                        /* Get a random playlist index */
                        var rnd = new Random();
                        var track = rnd.Next(0, count);
                        Video.VideoControl.CurrentTrack = track;
                        Video.VideoControl.Play(track);
                        return;
                }
                Video.VideoControl.CurrentTrack++;
                /* Check we haven't reached the end of the playlist */
                if (Video.VideoControl.CurrentTrack <= count - 1)
                {
                    Video.VideoControl.Play(Video.VideoControl.CurrentTrack);
                    return;
                }
            }
            _player.Text = @"xsMedia Player";
            /* Restore original window size */
            if (keepVideoSize || SettingsManager.Settings.Player.Video.Resize == VideoWindowResizeOption.WindowSize)
            {
                return;
            }
            _player.Size = SettingsManager.Settings.Window.MainWindow.Size;
            Video.VideoControl.SpinnerActive = false;
            Video.KeepVideoSize = false;
            Video.VideoControl.Refresh();
        }

        public static void InitPlayerWindow()
        {
            var path = Video.VideoControl.CurrentMedia.Input;
            if (!string.IsNullOrEmpty(path))
            {
                /* Remove the Uri/Mrl crap */
                Open.ClipFile = path.StartsWith("file:///") ? Path.GetFileName(new Uri(path).LocalPath) : path;
                switch (Video.VideoControl.OpenDiscType)
                {
                    case DiscType.Vcd:
                    case DiscType.Dvd:
                        _player.Text = string.Format("xsMedia Player - {0} {1}", Open.DriveLetter, Open.DriveLabel);
                        break;

                    default:
                        /* If "/" is in the path, most likely a network URL */
                        _player.Text = string.Format("xsMedia Player - {0}",
                            Open.ClipFile.Contains("/")
                                ? Playlist.PlaylistControl.GetItemAt(Video.VideoControl.CurrentTrack).Location
                                : AppPath.TruncatePath(Open.ClipFile, 30));
                        break;
                }
            }
            /* Bring control to front */
            Media.MediaBarControl.BringToFront();
            Media.MediaBarControl.PositionBarVisible = true;
        }

        public static void InitVideoWindow(ZoomRatioMode mode)
        {
            if (Video.KeepVideoSize && Video.VideoControl.IsVideo)
            {
                Video.VideoControl.ApplyFilters();
                return;
            }
            if (Video.VideoControl.IsVideo && SettingsManager.Settings.Player.Video.Resize == VideoWindowResizeOption.WindowSize)
            {
                IsVideoWindowInit = true;
                Video.VideoControl.ApplyFilters();
                return;
            }
            /* Get video size */
            var sz = Video.VideoControl.VideoSize;
            if (sz == Size.Empty) { return; }
            IsVideoWindowInit = true;
            /* Account for requests of normal, half, or double size */
            var multiplier = 1;
            var divider = 1;
            switch (mode)
            {
                case ZoomRatioMode.Mode1:
                    multiplier = 1;
                    divider = 4;
                    break;

                case ZoomRatioMode.Mode2:
                    multiplier = 1;
                    divider = 2;
                    break;

                case ZoomRatioMode.Mode4:
                    multiplier = 2;
                    divider = 1;
                    break;
            }
            var height = sz.Height * multiplier / divider;
            var width = sz.Width * multiplier / divider;
            /* Make sure width and height aren't below the form's minimum size */
            if (width < _player.MinimumSize.Width) { width = _player.MinimumSize.Width; }
            if (height < _player.MinimumSize.Height) { height = _player.MinimumSize.Height; }
            switch (_player.WindowState)
            {
                case FormWindowState.Normal:
                    _player.ClientSize = new Size(width, height + Menus.MainMenu.Height + Media.MediaBarControl.Height);
                    Video.KeepVideoSize = true;
                    break;

                case FormWindowState.Maximized:
                    Video.VideoClientSize = new Size(width, height + Menus.MainMenu.Height + Media.MediaBarControl.Height);
                    break;
            }            
            Video.VideoControl.ApplyFilters();
            ResizeVideoWindow();
        }

        public static void ResizeVideoWindow()
        {
            var y = Media.MediaBarControl.Visible ? Menus.MainMenu.Height : 0;
            var third = _player.ClientRectangle.Width/3;
            var width = _player.ClientRectangle.Width - third;
            var height = _player.ClientRectangle.Height - (Media.MediaBarControl.Visible ? y + Media.MediaBarControl.Height : 0);
            /* Adjust playlist size to start with */
            Playlist.PlaylistControl.SetBounds(third, y, width, height);

            if (Playlist.PlaylistControl.Visible)
            {
                /* Split the video and playlist window vertically */
                var cs = new Size(_player.ClientSize.Width - width,
                    _player.ClientSize.Height - (Media.MediaBarControl.Visible ? Media.MediaBarControl.Height : 0) - y);
                var startY = Video.VideoControl.Location.Y;
                Video.VideoControl.SetBounds(0, startY, third, cs.Height);
            }
            else 
            {        
                /* Normal video view */        
                Video.VideoControl.SetBounds(0, y, _player.ClientRectangle.Width, height);
            }
            Video.VideoControl.Refresh();
        }

        /* Callbacks/events */
        private static void OnDragEnter(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(DataFormats.FileDrop)) { return; }
            e.Effect = DragDropEffects.Copy;
        }

        private static void OnDragDrop(object sender, DragEventArgs e)
        {
            var files = (string[])e.Data.GetData(DataFormats.FileDrop);
            var added = false;
            var lastIndex = PlaylistManager.MediaList.Count;
            foreach (var file in files.Where(file => FileFilters.OpenFilters.IsSupported(file)))
            {                
                Video.VideoControl.OpenFile(file, false);
                added = true;
            }
            if (!added)
            {
                return;
            }
            /* Play the first added track */
            if (lastIndex > 0)
            {
                Video.VideoControl.CurrentTrack = lastIndex;
            }
            Video.VideoControl.Play(Video.VideoControl.CurrentTrack);
        }

        /* Start up timer */
        private static void TimerStartTick(object sender, EventArgs e)
        {            
            _tmrStart.Enabled = false;
            if (_tmrStart.Tag == null)
            {
                return;
            }
            ProcessCommandLine(_tmrStart.Tag.ToString());
        }
    }
}

/* xsMedia - Media Player
 * (c)2013 - 2020
 * Jason James Newland
 * KangaSoft Software, All Rights Reserved
 * Licenced under the GNU public licence */
using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using xsCore.CdUtils;
using xsCore.Utils.SystemUtils;
using xsCore.Utils.UI;
using xsMedia.Forms;
using xsMedia.Helpers;
using xsPlaylist;
using xsPlaylist.Utils;
using xsSettings;
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
                                Enabled = true,
                                Tag = args
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
            Video.VideoControl.Stop();
            //Video.VideoControl.LogoImage = Properties.Resources.videoWinIcon.ToBitmap();
            Video.VideoControl.LogoImage = MainIconUtil.VideoWindowIcon();
            Video.VideoControl.LogoImageMaximumSize = Video.VideoControl.LogoImage.Size;
            IsVideoWindowInit = false;
            Media.MediaBarControl.ElapsedTime = 0;
            Media.MediaBarControl.Position = 0;
            var count = PlaylistManager.MediaList.Count;
            if (nextTrack && count > 1)
            {
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
            if (keepVideoSize) { return; }
            _player.Size = SettingsManager.Settings.Window.Size;
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
                        _player.Text = string.Format("xsMedia Player - {0}", AppPath.TruncatePath(Open.ClipFile, 30));
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
            var y = !Video.IsFullScreen ? Menus.MainMenu.Height : 0;
            var height = _player.ClientRectangle.Height - (!Video.IsFullScreen ? Menus.MainMenu.Height + Media.MediaBarControl.Height : 0);
            Video.VideoControl.SetBounds(0, y, _player.ClientRectangle.Width, height);
            Video.VideoControl.Refresh();
            /* Also adjust playlist even if not in view */
            Playlist.PlaylistControl.SetBounds(0, y, _player.ClientRectangle.Width, height);            
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
            Video.VideoControl.CurrentTrack = Video.VideoControl.CurrentTrack < 0 || Video.VideoControl.CurrentTrack == 0 ? 0 : Video.VideoControl.CurrentTrack++;
            foreach (var file in files.Where(file => Filters.OpenFilters.IsSupported(file)))
            {                
                Video.VideoControl.OpenFile(file, false);
                added = true;
            }
            if (added)
            {
                Video.VideoControl.Play(Video.VideoControl.CurrentTrack);
            }
        }

        /* Start up timer */
        private static void TimerStartTick(object sender, EventArgs e)
        {
            _tmrStart.Enabled = false;
            if (_tmrStart.Tag == null) { return; }
            ProcessCommandLine(_tmrStart.Tag.ToString());            
        }
    }
}

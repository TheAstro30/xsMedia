using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using xsCore.CdUtils;
using xsCore.Utils;
using xsMedia.Controls;
using xsMedia.Forms;
using xsMedia.Helpers;
using xsSettings;
using xsSettings.Settings;
using xsVlc.Common;
using xsVlc.Common.Events;

namespace xsMedia.Logic
{
    public sealed class Video
    {
        private static FrmPlayer _player;
        private static SystemHookManager _mouseKeyboardHook;
        private static bool _positionChanged;
        private static Timer _tmrHide;

        public static MediaPlayback VideoControl { get; set; } 

        public static Size VideoClientSize { get; set; }
        public static bool KeepVideoSize { get; set; }
        public static bool IsFullScreen { get; set; }
        public static bool IsMouseOver { get; set; }

        public static void Init(FrmPlayer player)
        {
            _player = player;
            var options = new SettingsMediaOptions(SettingsManager.Settings.Player.Options.Option,
                                                   new[]
                                                       {
                                                           "-I",
                                                           "dummy",
                                                           "--ignore-config",
                                                           SettingsManager.Settings.NetworkPresets.Proxy.ToString()
                                                       });
            VideoControl = new MediaPlayback(_player, options.ToArray())
                               {
                                   //LogoImage = Properties.Resources.videoWinIcon.ToBitmap(),
                                   LogoImage = MainIconUtil.VideoWindowIcon(),
                                   LogoImageMaximumSize = Properties.Resources.videoWinIcon.Size,
                                   Volume = SettingsManager.Settings.Player.MediaVolume
                               };
            _player.Controls.Add(VideoControl);
            /* Events */
            VideoControl.PositionChanged += OnPlayerPositionChanged;
            VideoControl.TimeChanged += OnPlayerTimeChanged;
            VideoControl.MediaEnded += OnPlayerMediaEnded;

            VideoControl.MediaDurationChanged += OnMediaDurationChanged;
            VideoControl.MediaParseChanged += OnMediaParseChanged;
            VideoControl.MediaStateChanged += OnMediaStateChanged;

            VideoControl.DvdMediaRemoved += OnCdDvdMediaRemoved;
            VideoControl.CdMediaRemoved += OnCdDvdMediaRemoved;
            /* Mouse/key event hooking */
            _mouseKeyboardHook = new SystemHookManager();
            _mouseKeyboardHook.OnMouseDoubleClick += OnVideoDoubleClick;            
            _mouseKeyboardHook.OnMouseMove += OnVideoMouseMove;
            _mouseKeyboardHook.OnKeyDown += OnVideoKeyDown;
            /* Fullscreen timer */
            _tmrHide = new Timer
                           {
                               Interval = 5000
                           };
            _tmrHide.Tick += TimerHideTick;
        }        

        public static void OnVideoFullscreen(object sender, EventArgs e)
        {
            /* At least ALLOW yourself to get OUT of fullscreen mode */
            if (!VideoControl.IsVideo && !IsFullScreen) { return; }
            if (!IsFullScreen && Playlist.PlaylistControl.Visible)
            {
                /* Get rid of the playlist view before entering full screen */
                Menus.OnMenuListClicked(sender, EventArgs.Empty);
            }
            IsFullScreen = !IsFullScreen;
            if (IsFullScreen)
            {
                _player.FormBorderStyle = FormBorderStyle.None;
                _player.WindowState = FormWindowState.Maximized;
                _player.TopMost = true;
                _tmrHide.Enabled = true;
                Player.ResizeVideoWindow();
                Menus.MenuFull.Text = @"x";
                Menus.MenuFull.ToolTipText = @"Exit full screen mode (Esc)";
                return;
            }
            Menus.MenuFull.Text = @"{";
            Menus.MenuFull.ToolTipText = @"Enter full screen mode";
            _player.FormBorderStyle = FormBorderStyle.Sizable;
            _player.WindowState = FormWindowState.Normal;
            _player.TopMost = false;
            Menus.MainMenu.Visible = true;
            Media.MediaBarControl.Visible = true;
            _tmrHide.Enabled = false;
            /* Reset the client size */
            if (VideoClientSize != Size.Empty)
            {
                _player.ClientSize = VideoClientSize;
                VideoClientSize = Size.Empty;
            }
            Player.ResizeVideoWindow();
            _player.BringToFront();
        }

        /* Callbacks/events */
        private static void OnMediaDurationChanged(object sender, MediaDurationChange e)
        {
            Player.Sync.Execute(() => Media.MediaBarControl.Length = (int)(e.NewDuration / 1000));
            Player.Sync.Execute(() => Playlist.UpdatePlaylistMeta((int)e.NewDuration, false));
        }

        private static void OnMediaStateChanged(object sender, MediaStateChange e)
        {
            switch (e.NewState)
            {
                case MediaState.Opening:
                case MediaState.Buffering:
                    _positionChanged = false;
                    Player.Sync.Execute(() => _player.Text = string.Format(@"xsMedia Player - {0}", e.NewState));
                    break;
                case MediaState.Playing:
                    Player.Sync.Execute(Player.InitPlayerWindow);
                    break;
                case MediaState.Stopped:
                case MediaState.Ended:
                    _positionChanged = false;
                    Player.Sync.Execute(() => Media.MediaBarControl.PositionBarVisible = false);
                    break;
                case MediaState.Error:
                    _positionChanged = false;
                    Player.Sync.Execute(() => _player.Text =
                        string.Format(@"xsMedia Player - {0} {1}", e.NewState, Marshal.PtrToStringAnsi(xsVlc.Interop.Api.libvlc_errmsg())));
                    break;
            }
        }

        private static void OnMediaParseChanged(object sender, MediaParseChange e)
        {
            Player.Sync.Execute(Playlist.UpdatePlaylistMeta);
        }

        private static void OnPlayerMediaEnded(object sender, EventArgs e)
        {
            Player.Sync.Execute(() => Player.StopClip(true));
        }

        private static void OnPlayerPositionChanged(object sender, MediaPlayerPositionChanged e)
        {
            Player.Sync.Execute(() => Media.MediaBarControl.Position = e.NewPosition*100);
            Player.Sync.Execute(() => Media.MediaBarControl.PositionBarVisible = (_positionChanged && !Media.MediaBarControl.Position.Equals(0)));
            _positionChanged = true;
            if (!Player.IsVideoWindowInit)
            {
                Player.Sync.Execute(() => Player.InitVideoWindow(VideoControl.ZoomRatio));
            }
        }

        private static void OnPlayerTimeChanged(object sender, MediaPlayerTimeChanged e)
        {
            Player.Sync.Execute(() => Media.MediaBarControl.ElapsedTime = (int)(e.NewTime / 1000));
        }

        private static void OnCdDvdMediaRemoved(int deviceId, string volumeLetter)
        {
            if (VideoControl.OpenDiscType == DiscType.None)
            {
                return;
            }
            if (string.Format("{0}\\", Open.DriveLetter) == volumeLetter)
            {
                /* Disc was removed mid-stream ... stop playback */
                Player.StopClip(false);
            }
        }

        private static void OnVideoKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape && IsFullScreen)
            {
                OnVideoFullscreen(sender, EventArgs.Empty);
            }
        }

        private static void OnVideoDoubleClick(object sender, MouseEventArgs e)
        {
            if (Win32.GetForegroundWindow() != _player.Handle || Playlist.PlaylistControl.Visible || e.Button != MouseButtons.Left) { return; }
            var p = _player.PointToClient(e.Location);
            if (IsMouseOver && p.Y >= Media.MediaBarControl.Top) { return; }
            if (p.X >= 0 && p.X <= _player.ClientRectangle.Width && p.Y >= VideoControl.Top && p.Y <= VideoControl.Top + VideoControl.Height)
            {
                OnVideoFullscreen(sender, EventArgs.Empty);
            }
        }

        private static void OnVideoMouseMove(object sender, MouseEventArgs e)
        {
            if (!IsFullScreen || Win32.GetForegroundWindow() != _player.Handle || !VideoControl.IsVideo)
            {
                return;
            }
            if (!Media.MediaBarControl.Visible)
            {
                Menus.MainMenu.Visible = true;
                Media.MediaBarControl.Visible = true;
            }
            _tmrHide.Enabled = true;
        }

        private static void TimerHideTick(object sender, EventArgs e)
        {
            if (!IsMouseOver && Win32.GetForegroundWindow() == _player.Handle)
            {
                Menus.MainMenu.Visible = false;
                Media.MediaBarControl.Visible = false;
            }
            _tmrHide.Enabled = false;
        }
    }
}

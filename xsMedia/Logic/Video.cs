/* xsMedia - Media Player
 * (c)2013 - 2024
 * Jason James Newland
 * KangaSoft Software, All Rights Reserved
 * Licenced under the GNU public licence */
using System;
using System.Drawing;
using System.Linq;
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
        private static Timer _tmrVideoMenuHide;

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
                                   Volume = SettingsManager.Settings.Player.MediaVolume,
                                   PlaybackSpeed = SettingsManager.Settings.Player.Speed
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
            _mouseKeyboardHook.OnMouseClick += OnMouseClick;
            _mouseKeyboardHook.OnMouseDoubleClick += OnVideoDoubleClick;              
            _mouseKeyboardHook.OnMouseMove += OnVideoMouseMove;
            _mouseKeyboardHook.OnKeyDown += OnVideoKeyDown;
            /* Fullscreen timer */
            _tmrHide = new Timer {Interval = 5000};
            _tmrHide.Tick += TimerHideTick;
            /* Right-click video screen menu popup hide timer */
            _tmrVideoMenuHide = new Timer {Interval = 10};
            _tmrVideoMenuHide.Tick += TimerVideoMenuHideTick;
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
                Menus.MainMenu.BringToFront();
                Menus.MenuFull.Text = @"x";
                Menus.MenuFull.ToolTipText = @"Exit full screen mode (Esc)";
                Menus.MenuClose.Visible = true;
                return;
            }
            Menus.MenuFull.Text = @"{";
            Menus.MenuFull.ToolTipText = @"Enter full screen mode";
            _player.FormBorderStyle = FormBorderStyle.Sizable;
            _player.WindowState = FormWindowState.Normal;
            _player.TopMost = false;
            Menus.MenuClose.Visible = false;
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
            Player.Sync.Execute(() => Media.MediaBarControl.Position = e.NewPosition * 100);
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

        public static void OnVideoKeyDown(object sender, KeyEventArgs e)
        {
            if (Win32.GetForegroundWindow() != _player.Handle || Playlist.PlaylistControl.Visible)
            {
                return;
            }

            var vol = VideoControl.Volume;
            var jumpStep = SettingsManager.Settings.Player.JumpStep;
            var volStep = SettingsManager.Settings.Player.VolumeStep;

            switch (e.KeyCode)
            {                  
                case Keys.F:
                    OnVideoFullscreen(sender, EventArgs.Empty);
                    break;

                case Keys.Escape:
                    if (IsFullScreen)
                    {
                        OnVideoFullscreen(sender, EventArgs.Empty);
                    }
                    break;

                case Keys.Space:
                case Keys.K:
                    switch (VideoControl.PlayerState)
                    {
                        case MediaState.Playing:
                            VideoControl.Pause();
                            break;

                        case MediaState.Paused:
                            VideoControl.Play();
                            break;
                    }
                    break;

                /* Advance/retract playback position by X seconds -
                 * This took a bit for me to work out how to do it, as the "position" value is represented as a percentage of playback (0 - 1 as a float).
                 * I found taking the total length of the media / 1000 (as it's in milliseconds), then divide the desired advance/retraction amount by the total length
                 * gives us a percentage of playback position to add or retract from the current playback position */
                case Keys.Right:
                    if (VideoControl.PlayerState == MediaState.Playing)
                    {
                        /* Advance media position by X seconds */
                        var len = (int)VideoControl.Length / 1000;
                        VideoControl.Position += (float)jumpStep / len;
                    }
                    break;

                case Keys.Left:
                    if (VideoControl.PlayerState == MediaState.Playing)
                    {
                        /* Retract playback position by X seconds */
                        var len = (int)VideoControl.Length / 1000;
                        VideoControl.Position -= (float)jumpStep / len;
                    }
                    break;

                /* Volume up/down */
                case Keys.OemMinus:
                case Keys.Subtract:
                    /* Volume down */
                    if (VideoControl.IsMuted)
                    {
                        VideoControl.ToggleMute();
                    }
                    vol -= volStep;
                    if (vol < 0)
                    {
                        vol = 0;
                    }
                    break;

                case Keys.Oemplus:
                case Keys.Add:
                    /* Volume up */
                    if (VideoControl.IsMuted)
                    {
                        VideoControl.ToggleMute();
                    }
                    vol += volStep;
                    if (vol > 125)
                    {
                        vol = 125;
                    }
                    break;

                case Keys.M:
                    /* Mute */
                    VideoControl.ToggleMute();
                    break;
            }
            /* Keeps the volume up/down/mute code clean, even though it triggers on every other key press */
            Media.MediaBarControl.Volume = vol;
            VideoControl.Volume = vol;
            SettingsManager.Settings.Player.MediaVolume = vol;
        }

        private static void OnMouseClick(object sender, MouseEventArgs e)
        {
            if (Win32.GetForegroundWindow() != _player.Handle || Menus.IsControlHovering || Playlist.PlaylistControl.Visible)
            {
                return;
            }
            if (e.Button == MouseButtons.Left)
            {
                /* This timer is necessary because of the way video playback works on the hDC via VLC; left-click doesn't get detected on the player window
                 * allowing the popup to close normally as Windows reports focus changing. We can't just set visible to false, as the item click event will
                 * be lost. So we fudge it here... */
                _tmrVideoMenuHide.Enabled = true;
            }
            else if (e.Button == MouseButtons.Right &&  (VideoControl.PlayerState == MediaState.Playing || VideoControl.PlayerState == MediaState.Paused))
            {
                /* Only popup this menu if media is actually playing */
                Menus.ShowVideoMenu(e.Location);
            }
        }

        private static void OnVideoDoubleClick(object sender, MouseEventArgs e)
        {
            if (Win32.GetForegroundWindow() != _player.Handle || Playlist.PlaylistControl.Visible || e.Button != MouseButtons.Left)
            {
                return;
            }
            var p = _player.PointToClient(e.Location);
            if (IsMouseOver && p.Y >= Media.MediaBarControl.Top) { return; }
            if (p.X >= 0 && p.X <= _player.ClientRectangle.Width && p.Y >= VideoControl.Top && p.Y <= VideoControl.Top + VideoControl.Height)
            {
                OnVideoFullscreen(sender, EventArgs.Empty);
            }
        }

        private static void OnVideoMouseMove(object sender, MouseEventArgs e)
        {
            if (_player.IsClosing || !IsFullScreen || Win32.GetForegroundWindow() != _player.Handle || !VideoControl.IsVideo)
            {
                return;
            }
            if (!Media.MediaBarControl.Visible)
            {
                Menus.MainMenu.BringToFront();
                Menus.MainMenu.Visible = true;

                Media.MediaBarControl.BringToFront();
                Media.MediaBarControl.Visible = true;
            }
            _tmrHide.Enabled = true;
        }

        /* Timer callbacks */
        private static void TimerHideTick(object sender, EventArgs e)
        {
            if (!IsMouseOver && Win32.GetForegroundWindow() == _player.Handle && !Menus.IsControlHovering)
            {
                /* Only hide menubar if a drop down is not visible */
                if ((from ToolStripMenuItem m in Menus.MainMenu.Items where m.DropDown.Visible select m).Any())
                {
                    _tmrHide.Enabled = false;
                    return;
                }
                Menus.MainMenu.Visible = false;
                Media.MediaBarControl.Visible = false;
                Menus.MenuOpen.Visible = false;
            }
            _tmrHide.Enabled = false;
        }

        private static void TimerVideoMenuHideTick(object sender, EventArgs e)
        {
            if (!IsMouseOver && Win32.GetForegroundWindow() == _player.Handle && !Menus.IsControlHovering)
            {
                foreach (var m in from ToolStripMenuItem m in Menus.MainMenu.Items where m.DropDown.Visible select m)
                {
                    m.DropDown.Visible = false;
                    break;
                }
                Menus.MenuOpen.Visible = false;
                Menus.MenuVideoWindow.Visible = false;               
            }
            _tmrVideoMenuHide.Enabled = false;
        }
    }
}

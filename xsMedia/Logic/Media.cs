/* xsMedia - Media Player
 * (c)2013 - 2020
 * Jason James Newland
 * KangaSoft Software, All Rights Reserved
 * Licenced under the GNU public licence */
using System;
using System.Drawing;
using System.Windows.Forms;
using xsCore.CdUtils;
using xsCore.PlayerControls;
using xsCore.PlayerControls.Controls;
using xsCore.Skin;
using xsMedia.Controls;
using xsMedia.Forms;
using xsSettings;
using xsVlc.Common;

namespace xsMedia.Logic
{
    public sealed class Media
    {
        private static FrmPlayer _player;

        public static MediaBar MediaBarControl { get; set; }

        public static void Init(FrmPlayer player)
        {
            _player = player;
            MediaBarControl = new MediaBar
                                  {
                                      Dock = DockStyle.Bottom,
                                      Volume = SettingsManager.Settings.Player.MediaVolume,
                                  };
            _player.Controls.Add(MediaBarControl);
            /* Events */
            MediaBarControl.OnButtonPreviousClick += OnPrevious;
            MediaBarControl.OnButtonPlayClick += OnPlay;
            MediaBarControl.OnButtonPauseClick += OnPause;
            MediaBarControl.OnButtonStopClick += OnStop;
            MediaBarControl.OnButtonNextClick += OnNext;
            MediaBarControl.OnButtonOpenClick += OnOpen;
            MediaBarControl.OnPositionChanged += OnPositionChanged;
            MediaBarControl.OnVolumeChanged += OnVolumeChanged;
            MediaBarControl.OnSkinStyleChanged += OnMediaSkinStyleChanged;
            MediaBarControl.MouseEnter += OnMediaBarMouseEnter;
            MediaBarControl.MouseLeave += OnMediaBarMouseLeave;
        }

        /* Callbacks/events */
        private static void OnMediaBarMouseEnter(object sender, EventArgs e)
        {
            Video.IsMouseOver = true;
        }

        private static void OnMediaBarMouseLeave(object sender, EventArgs e)
        {
            Video.IsMouseOver = false;
        }

        private static void OnPrevious(MediaButton button)
        {
            if ((Video.VideoControl.PlayerState == MediaState.Playing || Video.VideoControl.PlayerState == MediaState.Paused) && (Video.VideoControl.OpenDiscType == DiscType.None))
            {
                Player.StopClip(false, true);
            }
            Video.VideoControl.Previous();
        }

        private static void OnPlay(MediaButton button)
        {
            if (Video.VideoControl.Play()) { return; }
            Open.OpenFile();
        }

        private static void OnPause(MediaButton button)
        {
            Video.VideoControl.Pause();
        }

        private static void OnStop(MediaButton button)
        {
            Player.StopClip(false);
        }

        private static void OnNext(MediaButton button)
        {
            if ((Video.VideoControl.PlayerState == MediaState.Playing || Video.VideoControl.PlayerState == MediaState.Paused) && (Video.VideoControl.OpenDiscType == DiscType.None))
            {
                Player.StopClip(false, true);
            }
            Video.VideoControl.Next();
        }

        private static void OnOpen(MediaButton button)
        {
            var p = new Point(button.Area.X + button.Area.Width, button.Area.Y + button.Area.Height);
            Menus.MenuOpen.Show(MediaBarControl, p);
        }

        private static void OnPositionChanged(PlayerControl control)
        {
            Video.VideoControl.Position = MediaBarControl.Position / 100;
        }

        private static void OnVolumeChanged(PlayerControl control)
        {
            SettingsManager.Settings.Player.MediaVolume = (int)MediaBarControl.Volume;
            Video.VideoControl.Volume = (int)MediaBarControl.Volume;
        }

        private static void OnMediaSkinStyleChanged(ControlRenderer cr)
        {
            Menus.MainMenu.Refresh();
            Playlist.PlaylistControl.BackColor = SkinManager.GetPlaylistColor("BACKCOLOR");            
            /* Resize video window control */
            Player.ResizeVideoWindow();
            /* Fix issue with control drawing ... */
            MediaBarControl.UpdateControls();
        }
    }
}

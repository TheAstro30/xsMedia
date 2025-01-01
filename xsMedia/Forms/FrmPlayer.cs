/* xsMedia - Media Player
 * (c)2013 - 2025
 * Jason James Newland
 * KangaSoft Software, All Rights Reserved
 * Licenced under the GNU public licence */
using System;
using System.Drawing;
using System.Windows.Forms;
using xsCore;
using xsCore.Settings.Data.Enums;
using xsCore.Skin;
using xsCore.Utils.SystemUtils;
using xsMedia.Logic;
using xsVlc.Common;

namespace xsMedia.Forms
{
    public sealed class FrmPlayer : Form
    {
        public bool IsClosing { get; private set; }

        public FrmPlayer(string args)
        {
            /* As there's no designer, we handle the setting of properties such as icon and text here */
            Name = "FrmPlayer";
            Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            Text = @"xsMedia Player";
            Font = new Font("Segoe UI", 9);
            BackColor = Color.Black;
            StartPosition = FormStartPosition.Manual;
            MinimumSize = new Size(620, 420);
            MaximizeBox = false;

            /* Setup settings file */
            SettingsManager.Load(AppPath.MainDir(@"\KangaSoft\xsMedia\xsMedia.xml", true));
            /* Main menu renderer */
            ToolStripManager.Renderer = new MenuRenderer();
            /* Init static classes for logic processing */            
            Player.Init(this, args);
            Video.Init(this);
            Media.Init(this);
            Open.Init(this);
            Playlist.Init(this);
            Menus.Init(this); /* Init last so controls are init'ed first */

            TopMost = SettingsManager.Settings.Player.AlwaysOnTop;

            /* Now process command line args */
            Player.BeginProcessCommandLine(args);
        }

        /* Form events */
        protected override void OnLoad(EventArgs e)
        {
            /* Set window position and size */
            var loc = SettingsManager.Settings.Window.MainWindow.Location;
            if (loc == Point.Empty)
            {
                /* Scale form to half the screen width/height */
                var screen = Monitor.GetCurrentMonitor(this);
                var x = screen.Bounds.Width / 2;
                var y = screen.Bounds.Height / 2;
                Size = new Size(x, y);
                /* Set location to center screen */
                Location = new Point(x - (Size.Width / 2), y - (Size.Height / 2));
            }
            else
            {
                Location = loc;
                Size = SettingsManager.Settings.Window.MainWindow.Size;
            }
            base.OnLoad(e);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            IsClosing = true;

            switch (Video.VideoControl.PlayerState)
            {
                case MediaState.Buffering:
                case MediaState.Paused:
                case MediaState.Opening:
                case MediaState.Playing:
                    Video.VideoControl.Stop();
                    break;
            }            
            /* Save settings - change path to user directory */
            if (WindowState == FormWindowState.Normal)
            {
                SettingsManager.Settings.Window.MainWindow.Location = Location;
            }
            SettingsManager.Save(AppPath.MainDir(@"\KangaSoft\xsMedia\xsMedia.xml", true));
            base.OnFormClosing(e);
        }

        protected override void OnResize(EventArgs e)
        {
            if (!Visible)
            {
                return;
            }
            if (WindowState == FormWindowState.Normal)
            {
                if (SettingsManager.Settings.Player.Video.Resize == VideoWindowResizeOption.VideoSize && Video.VideoControl.IsVideo)
                {
                    Player.ResizeVideoWindow();
                    base.OnResize(e);
                    return;
                }
                /* Only save the window size if not maximized & clip type isn't video */
                SettingsManager.Settings.Window.MainWindow.Location = Location;
                SettingsManager.Settings.Window.MainWindow.Size = Size;
            }
            Player.ResizeVideoWindow();
            base.OnResize(e);
        }
    }
}
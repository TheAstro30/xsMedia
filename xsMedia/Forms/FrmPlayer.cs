/* xsMedia - Media Player
 * (c)2013 - 2020
 * Jason James Newland
 * KangaSoft Software, All Rights Reserved
 * Licenced under the GNU public licence */
using System;
using System.Drawing;
using System.Windows.Forms;
using xsCore.Skin;
using xsCore.Utils;
using xsCore.Utils.SystemUtils;
using xsMedia.Logic;
using xsSettings;
using xsVlc.Common;

namespace xsMedia.Forms
{
    public sealed class FrmPlayer : Form
    {
        public FrmPlayer(string args)
        {
            /* As there's no designer, we handle the setting of properties such as icon and text here */
            Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            Text = @"xsMedia Player";
            Font = new Font("Segoe UI", 9);
            BackColor = Color.Black;
            StartPosition = FormStartPosition.Manual;
            MinimumSize = new Size(431, 235);
            MaximizeBox = false;            
            /* Setup settings file */
            SettingsManager.Load(AppPath.MainDir(@"\KangaSoft\xsMedia\xsMedia.xml", true));
            /* Main menu renderer */
            ToolStripManager.Renderer = new MenuRenderer();
            /* Set window position and size */
            Location = SettingsManager.Settings.Window.Location;
            Size = SettingsManager.Settings.Window.Size;
            /* Init static classes for logic processing */            
            Player.Init(this, args);
            Video.Init(this);
            Media.Init(this);
            Open.Init(this);
            Playlist.Init(this);
            Menus.Init(this); /* Init last so controls are init'ed first */
            /* Now process command line args */
            Player.BeginProcessCommandLine(args);
        }

        /* Form events */
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
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
            if (WindowState == FormWindowState.Normal && !Video.VideoControl.IsVideo)
            {
                SettingsManager.Settings.Window.Location = Location;
            }
            SettingsManager.Save(AppPath.MainDir(@"\KangaSoft\xsMedia\xsMedia.xml", true));
            base.OnFormClosing(e);
        }

        protected override void OnResize(EventArgs e)
        {
            if (!Visible) { return; }
            if (WindowState == FormWindowState.Normal && !Video.VideoControl.IsVideo)
            {
                /* Only save the window size if not maximized & clip type isn't video */
                SettingsManager.Settings.Window.Location = Location;
                SettingsManager.Settings.Window.Size = Size;
            }
            Player.ResizeVideoWindow();
            base.OnResize(e);
        }

        /* WndProc for single-instance command line processing */
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == Win32.WmCopydata)
            {
                var command = AppMessenger.ProcessWmCopyData(m);
                if (!string.IsNullOrEmpty(command))
                {
                    Player.ProcessCommandLine(command);
                    return;
                }
            }
            base.WndProc(ref m);
        }
    }
}
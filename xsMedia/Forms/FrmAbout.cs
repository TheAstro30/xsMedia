/* xsMedia - Media Player
 * (c)2013 - 2024
 * Jason James Newland
 * KangaSoft Software, All Rights Reserved
 * Licenced under the GNU public licence */
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using xsCore.Controls.Forms;
using xsCore.Utils.SystemUtils;
using xsMedia.Helpers;

namespace xsMedia.Forms
{
    public sealed class FrmAbout : FormEx
    {
        private readonly Panel _pnlIcon;

        public FrmAbout()
        {
            /* Scorched penises */
            ClientSize = new Size(461, 379);
            ControlBox = false;
            Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterScreen;
            Text = @"About xsMedia";

            /* Controls */            
            _pnlIcon = new Panel {BackColor = Color.Transparent, Location = new Point(12, 12), Size = new Size(64, 64)};

            var lblTitle = new Label
            {
                AutoSize = true,
                BackColor = Color.Transparent,
                Font = new Font("Segoe UI", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0),
                Location = new Point(82, 9),
                Size = new Size(161, 30),
                Text = @"xsMedia Player"
            };

            var lblCodeName = new Label
            {
                AutoSize = true,
                BackColor = Color.Transparent,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0),
                Location = new Point(84, 39),
                Size = new Size(138, 15),
                //Text = @"Code Name - ""Pegasus"""
                Text = @"Code Name - Perseus"
            };

            var lblAuthor = new Label
            {
                AutoSize = true,
                BackColor = Color.Transparent,
                Location = new Point(84, 61),
                Size = new Size(234, 15),
                Text = @"Written by: Jason James Newland && Ryan J."
            };

            /* Get version information */
            var ver = Assembly.GetExecutingAssembly().GetName().Version;
            var lblVersion = new Label
            {
                AutoSize = true,
                BackColor = Color.Transparent,
                Location = new Point(84, 89),
                Size = new Size(48, 15),
                Text = string.Format("Version: {0}.{1}.{2} (Build: {3})", ver.Major, ver.Minor, ver.Build, ver.MinorRevision)
            };

            var lblPlugins = new Label
            {
                AutoSize = true,
                BackColor = Color.Transparent,
                Location = new Point(84, 148),
                Size = new Size(107, 15),
                Text = @"Assembly versions:"
            };

            var txtPlugins = new TextBox
            {
                BackColor = Color.White,
                Location = new Point(87, 166),
                Multiline = true,
                ReadOnly = true,
                ScrollBars = ScrollBars.Vertical,
                Size = new Size(362, 162),
                Lines = new[]
                {
                    string.Format("libOlv: {0}", GetPluginVersion(AppPath.MainDir(@"\libolv.dll"))),
                    string.Format("xsCore: {0}", GetPluginVersion(AppPath.MainDir(@"\xsCore.dll"))),
                    string.Format("xsVlc: {0}", GetPluginVersion(AppPath.MainDir(@"\xsVlc.dll"))),
                    string.Format("xsSettings: {0}", GetPluginVersion(AppPath.MainDir(@"\xsSettings.dll"))),
                    string.Format("xsPlaylist: {0}", GetPluginVersion(AppPath.MainDir(@"\xsPlaylist.dll"))),
                    string.Format("libVlc: {0}", GetPluginVersion(AppPath.MainDir(@"\libvlc.dll"))),
                    string.Format("libVlcCore: {0}", GetPluginVersion(AppPath.MainDir(@"\libvlccore.dll")))
                }
            };

            var lblCopyright = new Label
            {
                BackColor = Color.Transparent,
                Location = new Point(84, 116),
                Size = new Size(365, 23),
                Text = @"Copyright ©2013 - 2024, KangaSoft Software. All Rights Reserved."
            };

            var linkVlc = new LinkLabel
            {
                AutoSize = true,
                Location = new Point(9, 351),
                Size = new Size(100, 15),
                Text = @"VideoLAN project"
            };
                     
            var btnOk = new Button
            {
                DialogResult = DialogResult.OK,
                Location = new Point(374, 347),
                Size = new Size(75, 23),
                TabIndex = 0,
                Text = @"Ok",
                UseVisualStyleBackColor = true
            };

            /* Add controls */
            Controls.AddRange(new Control[]
            {
                _pnlIcon, lblTitle, lblCodeName, lblCopyright, lblAuthor, lblVersion, lblPlugins, txtPlugins, linkVlc, btnOk
            });

            AcceptButton = btnOk;

            /* Event handlers */
            _pnlIcon.Paint += OnPanelIconPaint;
            linkVlc.LinkClicked += LinkVlcLinkClicked;  
        }

        private static void LinkVlcLinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ExecuteProcess.BeginProcess("http://www.videolan.org/vlc/libvlc.html");
        }

        private void OnPanelIconPaint(object sender, PaintEventArgs e)
        {
            /* Set icon */
            e.Graphics.DrawImage(MainIconUtil.VideoWindowIcon(), _pnlIcon.ClientRectangle);
        }

        private static string GetPluginVersion(string path)
        {
            if (!File.Exists(path))
            {
                return string.Empty;
            }
            var info = FileVersionInfo.GetVersionInfo(path);
            return string.Format("{0}.{1}.{2} - {3}", info.FileMajorPart, info.FileMinorPart, info.FileBuildPart != 0 ? info.FileBuildPart : info.FilePrivatePart, info.FileDescription);            
        }
    }
}

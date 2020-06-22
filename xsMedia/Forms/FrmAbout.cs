/* xsMedia - Media Player
 * (c)2013 - 2020
 * Jason James Newland
 * KangaSoft Software, All Rights Reserved
 * Licenced under the GNU public licence */
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using xsCore.Controls;
using xsCore.Utils.SystemUtils;
using xsMedia.Helpers;

namespace xsMedia.Forms
{
    public partial class FrmAbout : FormEx
    {
        public FrmAbout()
        {
            InitializeComponent();
            
            /* Get version information */
            var ver = Assembly.GetExecutingAssembly().GetName().Version;
            lblVersion.Text = string.Format("Version: {0}.{1}.{2}", ver.Major, ver.Minor, ver.MinorRevision);

            txtPlugins.Lines = new[]
                                   {
                                       string.Format("libOlv: {0}", GetPluginVersion(AppPath.MainDir(@"\libolv.dll"))),
                                       string.Format("xsCore: {0}", GetPluginVersion(AppPath.MainDir(@"\xsCore.dll"))),
                                       string.Format("xsVlc: {0}", GetPluginVersion(AppPath.MainDir(@"\xsVlc.dll"))),
                                       string.Format("xsSettings: {0}", GetPluginVersion(AppPath.MainDir(@"\xsSettings.dll"))),
                                       string.Format("xsPlaylist: {0}", GetPluginVersion(AppPath.MainDir(@"\xsPlaylist.dll"))),
                                       string.Format("xsTrackBar: {0}", GetPluginVersion(AppPath.MainDir(@"\xsTrackBar.dll"))),
                                       string.Format("libVlc: {0}", GetPluginVersion(AppPath.MainDir(@"\libvlc.dll"))),
                                       string.Format("libVlcCore: {0}", GetPluginVersion(AppPath.MainDir(@"\libvlccore.dll")))
                                   };

            pnlIcon.Paint += OnPanelIconPaint;
        }

        private void LinkVlcLinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ExecuteProcess.BeginProcess("http://www.videolan.org/vlc/libvlc.html");
        }

        private void OnPanelIconPaint(object sender, PaintEventArgs e)
        {
            /* Set icon */
            e.Graphics.DrawImage(MainIconUtil.VideoWindowIcon(), pnlIcon.ClientRectangle);
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

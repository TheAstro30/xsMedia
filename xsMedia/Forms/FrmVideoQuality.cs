using System.Collections.Generic;
using System.Windows.Forms;
using xsCore.Controls;
using xsCore.Utils.YouTube;
using xsSettings;

namespace xsMedia.Forms
{
    public partial class FrmVideoQuality : FormEx
    {
        public FrmVideoQuality(IEnumerable<YouTubeVideoQuality> urls)
        {
            InitializeComponent();
            /* Popuplate combo */
            foreach (var youTubeVideoQuality in urls)
            {
                cmbFormat.Items.Add(youTubeVideoQuality.ToString());
            }
            /* Select first item as default */
            cmbFormat.SelectedIndex = 0;
            chkShow.Checked = SettingsManager.Settings.NetworkPresets.ShowQuality;
        }

        public int SelectedFormat
        {
            get { return cmbFormat.SelectedIndex; }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            SettingsManager.Settings.NetworkPresets.ShowQuality = chkShow.Checked;
            base.OnFormClosing(e);
        }
    }
}

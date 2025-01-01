/* xsMedia - xsSettings
 * (c)2013 - 2025
 * Jason James Newland
 * KangaSoft Software, All Rights Reserved
 * Licenced under the GNU public licence */
using System;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using xsCore.Settings.Controls.Base;
using xsCore.Settings.Data;
using xsCore.Settings.Data.Enums;
using xsCore.Utils;

namespace xsCore.Settings.Controls
{
    public partial class OptionVideo : OptionBase
    {
        private readonly PlayerData _video;

        public OptionVideo(PlayerData video) : base("Video Playback Options")
        {
            InitializeComponent();
            _video = video;
            /* Init */
            foreach (var index in from v in (VideoWindowResizeOption[])Enum.GetValues(typeof(VideoWindowResizeOption)) let index = cmbResize.Items.Add(EnumUtils.GetDescriptionFromEnumValue(v)) where v == _video.Video.Resize select index)
            {
                cmbResize.SelectedIndex = index;
            }
            chkShowTitle.Checked = _video.Video.EnableVideoTitle;
            lblTimeOut.Enabled = _video.Video.EnableVideoTitle;
            txtTimeOut.Enabled = _video.Video.EnableVideoTitle;
            txtTimeOut.Text = _video.Video.VideoTitleTimeOut.ToString(CultureInfo.InvariantCulture);
            lblSeconds.Enabled = _video.Video.EnableVideoTitle;
            /* Handlers */
            cmbResize.SelectedIndexChanged += OnOptionChanged;
            chkShowTitle.CheckedChanged += OnOptionChanged;
            txtTimeOut.TextChanged += OnOptionChanged;
        }

        private void OnOptionChanged(object sender, EventArgs e)
        {
            var ctl = (Control)sender;
            switch (ctl.Tag.ToString())
            {
                case "RESIZE":
                    _video.Video.Resize = (VideoWindowResizeOption) cmbResize.SelectedIndex;
                    break;

                case "VIDEOTITLE":
                    lblTimeOut.Enabled = chkShowTitle.Checked;
                    txtTimeOut.Enabled = chkShowTitle.Checked;
                    lblSeconds.Enabled = chkShowTitle.Checked;
                    _video.Video.EnableVideoTitle = chkShowTitle.Checked;
                    break;

                case "TIMEOUT":
                    int t;
                    if (int.TryParse(txtTimeOut.Text, out t))
                    {
                        if (t < 5)
                        {
                            t = 5;
                        }
                        if (t > 30)
                        {
                            t = 30;
                        }
                    }
                    else
                    {
                        t = 5;
                    }
                    _video.Video.VideoTitleTimeOut = t;
                    break;
            }
        }
    }
}

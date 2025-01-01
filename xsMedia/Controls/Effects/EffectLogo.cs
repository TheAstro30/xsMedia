/* xsMedia - Media Player
 * (c)2013 - 2025
 * Jason James Newland
 * KangaSoft Software, All Rights Reserved
 * Licenced under the GNU public licence */
using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using xsCore;
using xsCore.Controls.TrackBar;
using xsCore.Settings.Data.Filter;
using xsCore.Utils.IO;
using xsMedia.Logic;

namespace xsMedia.Controls.Effects
{
    public partial class EffectLogo : UserControl
    {
        private readonly FilterLogo _logo;

        public EffectLogo(FilterLogo logo)
        {
            InitializeComponent();
            _logo = logo;
            /* Disable controls initially */
            EnableDisable(_logo.Enable);
            /* Set values */
            chkVLogoEnable.Checked = _logo.Enable;
            txtFile.Text = _logo.File;            
            trOpacity.Value = _logo.Opacity;
            /* Handlers */
            chkVLogoEnable.CheckedChanged += OnEffectEnable;
            btnLoad.Click += OnSelect;
            trOpacity.ValueChanged += OnValueChanged;
            noTop.ValueChanged += OnValueChanged;
            noLeft.ValueChanged += OnValueChanged;
            noTop.TextChanged += OnValueChanged;
            noLeft.TextChanged += OnValueChanged;
        }

        private void OnEffectEnable(object sender, EventArgs e)
        {
            var enable = chkVLogoEnable.Checked;
            EnableDisable(enable);         
            _logo.Enable = enable;
            Video.VideoControl.ApplyFilters();
        }

        private void OnSelect(object sender, EventArgs e)
        {
            var path = SettingsManager.Settings.Paths.GetPath("open-logo");
            using (var ofd = new OpenFileDialog
            {
                Title = @"Open Logo Image",
                InitialDirectory = path.Location,
                Multiselect = false,
                Filter = FileFilters.CoverArtFilters.ToString()
            })
            {
                if (ofd.ShowDialog(this) != DialogResult.OK)
                {
                    return;
                }
                path.Location = Path.GetDirectoryName(ofd.FileName);
                _logo.File = ofd.FileName;                
                txtFile.Text = _logo.File;
                Video.VideoControl.ApplyFilters();
            }
        }

        private void OnValueChanged(object sender, EventArgs e)
        {
            var ctl = (Control)sender;
            NumericUpDown no;
            switch (ctl.Tag.ToString())
            {
                case "OPACITY":
                    var tb = (TrackBarEx)ctl;
                    _logo.Opacity = tb.Value;
                    break;

                case "TOP":
                    no = (NumericUpDown)ctl;
                    _logo.TopOffset = (int)no.Value;
                    break;

                case "LEFT":
                    no = (NumericUpDown)ctl;
                    _logo.LeftOffset = (int)no.Value;
                    break;
            }
            Video.VideoControl.ApplyFilters();
        }

        private void EnableDisable(bool enable)
        {
            foreach (var ctl in Controls.Cast<Control>().Where(ctl => ctl != chkVLogoEnable))
            {
                ctl.Enabled = enable;
            }
        }
    }
}

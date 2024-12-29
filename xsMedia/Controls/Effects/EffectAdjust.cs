/* xsMedia - Media Player
 * (c)2013 - 2024
 * Jason James Newland
 * KangaSoft Software, All Rights Reserved
 * Licenced under the GNU public licence */
using System;
using System.Linq;
using System.Windows.Forms;
using xsCore.Controls.TrackBar;
using xsMedia.Logic;
using xsSettings.Settings;

namespace xsMedia.Controls.Effects
{
    public partial class EffectAdjust : UserControl
    {
        private readonly SettingsFilterAdjust _adjust;

        public EffectAdjust(SettingsFilterAdjust adjust)
        {
            InitializeComponent();
            _adjust = adjust;
            /* Disable controls initially */
            EnableDisable(_adjust.Enable);
            /* Set values */
            chkVAdjEnable.Checked = _adjust.Enable;
            trHue.Value = _adjust.Hue;
            trBright.Value = (int)(_adjust.Brightness*100);
            trContrast.Value = (int)(_adjust.Contrast*100);
            trSaturation.Value = (int)(_adjust.Saturation*100);
            trGamma.Value = (int)(_adjust.Gamma*100);
            /* Handlers */
            chkVAdjEnable.CheckedChanged += OnEffectEnable;
            trHue.ValueChanged += OnValueChanged;
            trBright.ValueChanged += OnValueChanged;
            trContrast.ValueChanged += OnValueChanged;
            trSaturation.ValueChanged += OnValueChanged;
            trGamma.ValueChanged += OnValueChanged;
        }

        private void OnEffectEnable(object sender, EventArgs e)
        {
            var enable = chkVAdjEnable.Checked;
            EnableDisable(enable);            
            _adjust.Enable = enable;
            Video.VideoControl.ApplyFilters();
        }

        private void OnValueChanged(object sender, EventArgs e)
        {
            var tb = (TrackBarEx)sender;
            var value = (float)tb.Value / 100;            
            switch (tb.Tag.ToString())
            {
                case "HUE":
                    _adjust.Hue = tb.Value;
                    break;

                case "BRIGHT":
                    _adjust.Brightness = value;
                    break;

                case "CONTRAST":
                    _adjust.Contrast = value;
                    break;

                case "SATURATION":
                    _adjust.Saturation = value;
                    break;

                case "GAMMA":
                    _adjust.Gamma = value;
                    break;
            }
            Video.VideoControl.ApplyFilters();
        }

        private void EnableDisable(bool enable)
        {
            foreach (var ctl in Controls.Cast<Control>().Where(ctl => ctl != chkVAdjEnable))
            {
                ctl.Enabled = enable;
            }
        }
    }
}

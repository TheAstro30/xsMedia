/* xsMedia - Media Player
 * (c)2013 - 2024
 * Jason James Newland
 * KangaSoft Software, All Rights Reserved
 * Licenced under the GNU public licence */
using System;
using System.Linq;
using System.Windows.Forms;
using xsCore.Settings.Data.Filter;
using xsCore.Utils;
using xsMedia.Logic;
using xsVlc.Common;

namespace xsMedia.Controls.Effects
{
    public partial class EffectDeinterlace : UserControl
    {
        private readonly FilterDeinterlace _deint;

        public EffectDeinterlace(FilterDeinterlace deint)
        {
            InitializeComponent();
            _deint = deint;
            /* Disable controls initially */
            EnableDisable(_deint.Enable);
            /* Set values */
            chkVDeintEnable.Checked = _deint.Enable;
            /* Combo */
            foreach (var deinterlace in (DeinterlaceMode[])Enum.GetValues(typeof(DeinterlaceMode)))
            {
                var data = new EnumComboData
                               {
                                   Text = EnumUtils.GetDescriptionFromEnumValue(deinterlace),
                                   Data = (uint)deinterlace
                               };
                cmbMode.Items.Add(data);
                if (deinterlace == (DeinterlaceMode)_deint.Mode)
                {
                    cmbMode.SelectedIndex = cmbMode.Items.Count - 1;
                }
            }
            /* Handlers */
            chkVDeintEnable.CheckedChanged += OnEffectEnable;
            cmbMode.SelectedIndexChanged += OnComboSelectedIndexChanged;
        }

        private void OnEffectEnable(object sender, EventArgs e)
        {
            var enable = chkVDeintEnable.Checked;
            EnableDisable(enable);            
            _deint.Enable = enable;
            Video.VideoControl.ApplyFilters();
        }

        private void OnComboSelectedIndexChanged(object sender, EventArgs e)
        {
            var cmb = (ComboBox)sender;
            var data = (EnumComboData)cmb.SelectedItem;
            switch (cmb.Tag.ToString())
            {
                case "MODE":
                    _deint.Mode = (int)data.Data;
                    break;
            }
            Video.VideoControl.ApplyFilters();
        }

        private void EnableDisable(bool enable)
        {
            foreach (var ctl in Controls.Cast<Control>().Where(ctl => ctl != chkVDeintEnable))
            {
                ctl.Enabled = enable;
            }
        }
    }
}

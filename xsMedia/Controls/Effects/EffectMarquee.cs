using System;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using xsCore.Utils;
using xsMedia.Logic;
using xsSettings.Settings;
using xsVlc.Common;
using xsTrackBar;

namespace xsMedia.Controls.Effects
{
    public partial class EffectMarquee : UserControl
    {
        private readonly SettingsFilterMarquee _marquee;

        public EffectMarquee(SettingsFilterMarquee marquee)
        {
            InitializeComponent();
            _marquee = marquee;
            /* Disable controls initially */
            EnableDisable(_marquee.Enable);
            /* Set values */
            chkVMarqEnable.Checked = _marquee.Enable;
            txtText.Text = _marquee.Text;
            txtTimeOut.Text = (_marquee.TimeOut/1000).ToString(CultureInfo.InvariantCulture);
            trOpacity.Value = _marquee.Opacity;
            /* Combos */
            foreach (var pos in (Position[])Enum.GetValues(typeof(Position)))
            {
                var data = new EnumComboData
                               {
                                   Text = EnumUtils.GetDescriptionFromEnumValue(pos),
                                   Data = (uint)pos
                               };
                cmbPosition.Items.Add(data);
                if (pos == (Position)_marquee.Position)
                {
                    cmbPosition.SelectedIndex = cmbPosition.Items.Count - 1;
                }
            }
            foreach (var col in (VlcColor[])Enum.GetValues(typeof(VlcColor)))
            {
                var data = new EnumComboData
                               {
                                   Text = EnumUtils.GetDescriptionFromEnumValue(col),
                                   Data = (uint)col
                               };
                cmbColor.Items.Add(data);
                if (col == (VlcColor)_marquee.Color)
                {
                    cmbColor.SelectedIndex = cmbColor.Items.Count - 1;
                }
            }
            /* Handlers */
            chkVMarqEnable.CheckedChanged += OnEffectEnable;
            txtText.TextChanged += OnTextChanged;
            txtTimeOut.TextChanged += OnTextChanged;
            cmbPosition.SelectedIndexChanged += OnComboSelectedIndexChanged;
            cmbColor.SelectedIndexChanged += OnComboSelectedIndexChanged;
            trOpacity.ValueChanged += OnValueChanged;
        }

        private void OnEffectEnable(object sender, EventArgs e)
        {
            var enable = chkVMarqEnable.Checked;
            EnableDisable(enable);
            _marquee.Enable = enable;
            Video.VideoControl.ApplyFilters();
        }

        private void OnValueChanged(object sender, EventArgs e)
        {
            var tb = (TrackBarEx)sender;
            switch (tb.Tag.ToString())
            {
                case "OPACITY":
                    _marquee.Opacity = tb.Value;
                    break;
            }
            Video.VideoControl.ApplyFilters();
        }

        private void OnTextChanged(object sender, EventArgs e)
        {
            var txt = (TextBox)sender;
            switch (txt.Tag.ToString())
            {
                case "TEXT":
                    _marquee.Text = txt.Text;
                    break;

                case "TIMEOUT":
                    int value;
                    if (int.TryParse(txt.Text, out value))
                    {
                        _marquee.TimeOut = value*1000;
                    }
                    break;
            }
            Video.VideoControl.ApplyFilters();
        }

        private void OnComboSelectedIndexChanged(object sender, EventArgs e)
        {
            var cmb = (ComboBox)sender;
            var data = (EnumComboData)cmb.SelectedItem;
            switch (cmb.Tag.ToString())
            {
                case "POSITION":
                    _marquee.Position = (int)data.Data;
                    break;

                case "COLOR":
                    _marquee.Color = data.Data;
                    break;
            }
            Video.VideoControl.ApplyFilters();
        }

        private void EnableDisable(bool enable)
        {
            foreach (var ctl in Controls.Cast<Control>().Where(ctl => ctl != chkVMarqEnable))
            {
                ctl.Enabled = enable;
            }
        }
    }
}

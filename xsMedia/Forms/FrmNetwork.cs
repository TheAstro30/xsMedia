using System;
using xsCore.Controls;
using xsSettings;
using xsSettings.Settings;

namespace xsMedia.Forms
{
    public partial class FrmNetwork : FormEx
    {
        public FrmNetwork()
        {
            InitializeComponent();
            /* Build the preset list */
            BuildCombo();
            /* Use last known network connection and attempt to find it in the list */
            txtUrl.Text = SettingsManager.Settings.NetworkPresets.LastUrl;
            var index = FindPresetIndex();
            if (index > -1)
            {
                cmbPreset.SelectedIndex = index;
            }
        }

        public string SelectedUrl
        {
            get { return txtUrl.Text; }
        }

        private void BtnAddClick(object sender, EventArgs e)
        {
            /* Add a new preset checking it doesn't already exist (url match) */
            if (string.IsNullOrEmpty(cmbPreset.Text) || string.IsNullOrEmpty(txtUrl.Text) || FindPresetIndex() != -1)
            {
                return;
            }
            var newData = new SettingsNetworkPresets.SettingsNetworkPresetData
                              {
                                  Id = cmbPreset.Text, 
                                  Url = txtUrl.Text
                              };
            SettingsManager.Settings.NetworkPresets.Preset.Add(newData);
            SettingsManager.Settings.NetworkPresets.Sort();            
            /* Update combo */
            BuildCombo();
        }

        private void BtnRemoveClick(object sender, EventArgs e)
        {
            /* Remove current selected index */
            if (string.IsNullOrEmpty(cmbPreset.Text) || string.IsNullOrEmpty(txtUrl.Text))
            {
                return;
            }
            var index = cmbPreset.SelectedIndex;
            if (index == - 1 || FindPresetIndex() != index)
            {
                return;
            }
            SettingsManager.Settings.NetworkPresets.Preset.RemoveAt(index);
            cmbPreset.Items.RemoveAt(index);
            cmbPreset.Text = null;
            txtUrl.Text = null;
        }

        private void CmbPresetSelectedIndexChanged(object sender, EventArgs e)
        {
            /* Update url field */
            if (string.IsNullOrEmpty(cmbPreset.Text))
            {
                return;
            }
            txtUrl.Text = SettingsManager.Settings.NetworkPresets.Preset[cmbPreset.SelectedIndex].ToString();
        }

        private void BtnOkClick(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtUrl.Text)) { return; }
            var sp = txtUrl.Text.Split(' ');
            SettingsManager.Settings.NetworkPresets.LastUrl = sp[0];
        }

        private void BuildCombo()
        {
            var insert = cmbPreset.Items.Count != 0;
            var count = SettingsManager.Settings.NetworkPresets.Preset.Count - 1;
            for (var i = 0; i <= count; ++i)
            {
                var id = SettingsManager.Settings.NetworkPresets.Preset[i];
                if (!insert)
                {
                    cmbPreset.Items.Add(id.Id);
                }
                else
                {
                    if (cmbPreset.Items.Count -1 < i || cmbPreset.Items[i].ToString() != id.Id)
                    {
                        cmbPreset.Items.Insert(i, id.Id);
                    }
                }
            }
        }

        private int FindPresetIndex()
        {
            var count = 0;
            foreach (var d in SettingsManager.Settings.NetworkPresets.Preset)
            {
                if (d.Url.ToLower() == txtUrl.Text.ToLower())
                {
                    return count;
                }
                ++count;
            }
            return -1;
        }
    }
}

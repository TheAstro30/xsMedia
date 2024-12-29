/* xsMedia - Media Player
 * (c)2013 - 2024
 * Jason James Newland
 * KangaSoft Software, All Rights Reserved
 * Licenced under the GNU public licence */
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using xsCore.Controls;
using xsMedia.Properties;
using xsSettings;
using xsSettings.Settings;

namespace xsMedia.Forms
{
    public sealed class FrmNetwork : FormEx
    {
        private readonly ComboBox _cmbPreset;
        private readonly TextBox _txtUrl;

        public FrmNetwork()
        {
            /* Used for tooltip provider */
            IContainer components = new Container();
            
            Name = "FrmNetwork";
            ClientSize = new Size(371, 150);
            Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.Manual;
            Text = @"Open Network Stream";

            /* Controls */
            var toolTipProvider = new ToolTip(components);

            var lblPreset = new Label
            {
                AutoSize = true,
                BackColor = Color.Transparent,
                Location = new Point(12, 9),
                Size = new Size(77, 15),
                Text = @"Preset Name:"
            };

            _cmbPreset = new ComboBox
            {
                FormattingEnabled = true,
                Location = new Point(15, 27),
                Size = new Size(285, 23),
                TabIndex = 2
            };

            var btnAdd = new Button
            {
                Location = new Point(306, 27),
                Size = new Size(23, 23),
                TabIndex = 3,
                Image = Resources.dlgAdd.ToBitmap(),
                UseVisualStyleBackColor = true
            };

            var btnRemove = new Button
            {
                Location = new Point(335, 27),
                Size = new Size(23, 23),
                TabIndex = 4,
                Image = Resources.dlgRemove.ToBitmap(),
                UseVisualStyleBackColor = true
            };
            
            var lblUrl = new Label
            {
                AutoSize = true,
                BackColor = Color.Transparent,
                Location = new Point(12, 62),
                Size = new Size(25, 15),
                Text = @"Url:"
            };

            _txtUrl = new TextBox
            {
                Location = new Point(15, 80),
                Size = new Size(343, 23),
                TabIndex = 5,
                Text = SettingsManager.Settings.NetworkPresets.LastUrl /* Use last known network connection */
            };

            var btnOk = new Button
            {
                DialogResult = DialogResult.OK,
                Location = new Point(203, 118),
                Size = new Size(75, 23),
                TabIndex = 0,
                Text = @"Open",
                UseVisualStyleBackColor = true
            };

            var btnCancel = new Button
            {
                DialogResult = DialogResult.Cancel,
                Location = new Point(284, 118),
                Size = new Size(75, 23),
                TabIndex = 1,
                Text = @"Cancel",
                UseVisualStyleBackColor = true
            };
                              
            /* Add controls */
            Controls.AddRange(new Control[]
            {
                lblPreset, _cmbPreset, btnAdd, btnRemove, lblUrl, _txtUrl, btnOk, btnCancel
            });

            /* Tooltips */
            toolTipProvider.SetToolTip(btnAdd, "Add to list");
            toolTipProvider.SetToolTip(btnRemove, "Remove from list");

            /* Handlers */
            btnOk.Click += BtnOkClick;
            btnAdd.Click += BtnAddClick;
            btnRemove.Click += BtnRemoveClick;
            _cmbPreset.SelectedIndexChanged += CmbPresetSelectedIndexChanged;

            AcceptButton = btnOk;

            /* Build the preset list */
            BuildCombo();
            /* Attempt to find last used connection in the list */            
            var index = FindPresetIndex();
            if (index > -1)
            {
                _cmbPreset.SelectedIndex = index;
            }
        }

        /* Overrides */
        protected override void OnLoad(EventArgs e)
        {
            var loc = SettingsManager.Settings.Window.NetworkWindow.Location;
            if (loc == Point.Empty)
            {
                if (Owner != null)
                {
                    /* We set this form to centered parent (which CenterToParent property doesn't work with the main form, for whatever reason) */
                    Location = new Point(Owner.Location.X + Owner.Width / 2 - Width / 2,
                        Owner.Location.Y + Owner.Height / 2 - Height / 2);
                }
            }
            else
            {
                Location = loc;
            }
            base.OnLoad(e);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            SettingsManager.Settings.Window.NetworkWindow.Location = Location;
        }

        public string SelectedUrl
        {
            get { return _txtUrl.Text; }
        }

        private void BtnAddClick(object sender, EventArgs e)
        {
            /* Add a new preset checking it doesn't already exist (url match) */
            if (string.IsNullOrEmpty(_cmbPreset.Text) || string.IsNullOrEmpty(_txtUrl.Text) || FindPresetIndex() != -1)
            {
                return;
            }
            var newData = new SettingsNetworkPresets.SettingsNetworkPresetData
                              {
                                  Id = _cmbPreset.Text, 
                                  Url = _txtUrl.Text
                              };
            SettingsManager.Settings.NetworkPresets.Preset.Add(newData);
            SettingsManager.Settings.NetworkPresets.Sort();            
            /* Update combo */
            BuildCombo();
        }

        private void BtnRemoveClick(object sender, EventArgs e)
        {
            /* Remove current selected index */
            if (string.IsNullOrEmpty(_cmbPreset.Text) || string.IsNullOrEmpty(_txtUrl.Text))
            {
                return;
            }
            var index = _cmbPreset.SelectedIndex;
            if (index == - 1 || FindPresetIndex() != index)
            {
                return;
            }
            SettingsManager.Settings.NetworkPresets.Preset.RemoveAt(index);
            _cmbPreset.Items.RemoveAt(index);
            _cmbPreset.Text = null;
            _txtUrl.Text = null;
        }

        private void CmbPresetSelectedIndexChanged(object sender, EventArgs e)
        {
            /* Update url field */
            if (string.IsNullOrEmpty(_cmbPreset.Text))
            {
                return;
            }
            _txtUrl.Text = SettingsManager.Settings.NetworkPresets.Preset[_cmbPreset.SelectedIndex].ToString();
        }

        private void BtnOkClick(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_txtUrl.Text)) { return; }
            var sp = _txtUrl.Text.Split(' ');
            SettingsManager.Settings.NetworkPresets.LastUrl = sp[0];
        }

        private void BuildCombo()
        {
            var insert = _cmbPreset.Items.Count != 0;
            var count = SettingsManager.Settings.NetworkPresets.Preset.Count - 1;
            for (var i = 0; i <= count; ++i)
            {
                var id = SettingsManager.Settings.NetworkPresets.Preset[i];
                if (!insert)
                {
                    _cmbPreset.Items.Add(id.Id);
                }
                else
                {
                    if (_cmbPreset.Items.Count -1 < i || _cmbPreset.Items[i].ToString() != id.Id)
                    {
                        _cmbPreset.Items.Insert(i, id.Id);
                    }
                }
            }
        }

        private int FindPresetIndex()
        {
            var count = 0;
            foreach (var d in SettingsManager.Settings.NetworkPresets.Preset)
            {
                if (String.Equals(d.Url, _txtUrl.Text, StringComparison.CurrentCultureIgnoreCase))
                {
                    return count;
                }
                ++count;
            }
            return -1;
        }
    }
}

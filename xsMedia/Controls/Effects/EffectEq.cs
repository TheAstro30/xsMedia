/* xsMedia - Media Player
 * (c)2013 - 2024
 * Jason James Newland
 * KangaSoft Software, All Rights Reserved
 * Licenced under the GNU public licence */
using System;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;
using xsCore.Controls.TrackBar;
using xsMedia.Logic;
using xsSettings;
using xsSettings.Settings;
using xsVlc.Core.Equalizer;

namespace xsMedia.Controls.Effects
{
    public partial class EffectEq : UserControl
    {
        private readonly SettingsFilterEq _eq;

        public EffectEq(SettingsFilterEq eq)
        {
            _eq = eq;
            InitializeComponent();            
            /* Initialize bands */
            InitBandControls();                     
            chkEnable.Checked = eq.Enable;
            /* Initialize presets */
            cmbPreset.Items.Add("<Custom>");
            foreach (var preset in Equalizer.Presets)
            {
                cmbPreset.Items.Add(preset.Name);
            }
            cmbPreset.SelectedIndex = SettingsManager.Settings.Filters.Eq.Preset;
            /* Disable controls initially */
            EnableDisable(_eq.Enable);
            /* Handlers */
            chkEnable.CheckedChanged += OnEffectEnable;
            cmbPreset.SelectedIndexChanged += OnPresetChanged;
        }

        private void InitBandControls()
        {
            var xPos = 25;
            for (var i = 0; i <= 10; i++)
            {
                var tb = new TrackBarEx
                             {
                                 BackColor = Color.Transparent,
                                 AutoSize = false,
                                 Orientation = Orientation.Vertical,
                                 Size = new Size(30, 130),
                                 Location = new Point(xPos, 43),
                                 Minimum = 0,
                                 Maximum = 400,
                                 TickStyle = TickStyle.None,
                                 LargeChange = 1,
                                 Tag = i
                             };
                var lb = new Label
                             {
                                 BackColor = Color.Transparent,
                                 AutoSize = false,
                                 Size = new Size(40, 12),
                                 Location = new Point(xPos - 9, 175),
                                 TextAlign = ContentAlignment.MiddleCenter,
                                 Font = new Font(Font.Name, 6.2F)
                             };

                if (i == 0)
                {
                    /* Preamp */
                    tb.Value = (int)(_eq.Preamp * 10) + 200;
                    Controls.Add(tb);
                    tb.ValueChanged += OnValueChanged;

                    lb.Text = @"Preamp";
                    Controls.Add(lb);

                    xPos += 50;
                    continue;
                }

                var band = _eq.Band[i - 1];
                tb.Value = (int)(band.Amplitude * 10) + 200;
                Controls.Add(tb);
                tb.ValueChanged += OnValueChanged;

                lb.Text = band.Frequency >= 1000
                              ? string.Format("{0}kHz", (band.Frequency/1000).ToString(CultureInfo.InvariantCulture))
                              : string.Format("{0}Hz", Math.Round(band.Frequency));
                Controls.Add(lb);

                xPos += 35;
            }
        }

        private void OnEffectEnable(object sender, EventArgs e)
        {
            var enable = chkEnable.Checked;
            EnableDisable(enable);
            Video.VideoControl.EqEnable = enable;
            _eq.Enable = enable;            
        }

        private void OnValueChanged(object sender, EventArgs e)
        {
            var tb = (TrackBarEx)sender;
            var band = (int)tb.Tag;
            var value = Math.Round((float) (tb.Value - 200)/10, 1);
            if (band == 0)
            {
                /* Preamp */
                _eq.Preamp = value;
                Video.VideoControl.EqPreamp(_eq.Preamp);
            }
            else
            {
                /* Floating value +/- 20 */
                _eq.Band[band - 1].Amplitude = value;
                Video.VideoControl.EqAdjustBand(band - 1, value);
            }            
            Video.VideoControl.EqEnable = chkEnable.Checked;
        }

        private void OnPresetChanged(object sender, EventArgs e)
        {
            var index = cmbPreset.SelectedIndex;
            SettingsManager.Settings.Filters.Eq.Preset = index;
            Video.VideoControl.EqInitPreset(index);
            /* Update trackbars */
            for (var i = 0; i <= 10; i++)
            {
                if (i == 0)
                {
                    GetTrackBar(i).Value = index > 0 ? (int) (Video.VideoControl.Eq.Preamp*10) + 200 : 320; /* Default value of 12 */
                    continue;
                }
                GetTrackBar(i).Value = (int) (Video.VideoControl.Eq.Bands[i - 1].Amplitude*10) + 200;
            }          
        }

        private void EnableDisable(bool enable)
        {
            foreach (var ctl in Controls.Cast<Control>().Where(ctl => ctl != chkEnable))
            {
                ctl.Enabled = enable;
            }
        }

        private TrackBarEx GetTrackBar(int index)
        {
            return Controls.OfType<TrackBarEx>().FirstOrDefault(ctl => Convert.ToInt32(ctl.Tag) == index);
        }
    }
}

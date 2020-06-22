/* xsMedia - Media Player
 * (c)2013 - 2020
 * Jason James Newland
 * KangaSoft Software, All Rights Reserved
 * Licenced under the GNU public licence */
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using xsMedia.Logic;
using xsSettings.Settings;

namespace xsMedia.Controls.Effects
{
    public partial class EffectCrop : UserControl
    {
        private readonly SettingsFilterCrop _crop;
        private Rectangle _cropArea;

        public EffectCrop(SettingsFilterCrop crop)
        {
            InitializeComponent();
            _crop = crop;
            /* Disable controls initially */
            EnableDisable(_crop.Enable);
            /* Set values */
            chkVCropEnable.Checked = _crop.Enable;
            var sz = Video.VideoControl.VideoSize;
            _cropArea = new Rectangle(_crop.Left, _crop.Top, sz.Width - _crop.Right, sz.Height - _crop.Bottom);
            noTop.Value = _crop.Top;
            noLeft.Value = _crop.Left;
            noBottom.Value = _crop.Bottom;
            noRight.Value = _crop.Right;
            /* Handlers */
            chkVCropEnable.CheckedChanged += OnEffectEnable;
            noTop.ValueChanged += OnValueChanged;
            noLeft.ValueChanged += OnValueChanged;
            noBottom.ValueChanged += OnValueChanged;
            noRight.ValueChanged += OnValueChanged;
            noTop.TextChanged += OnValueChanged;
            noLeft.TextChanged += OnValueChanged;
            noBottom.TextChanged += OnValueChanged;
            noRight.TextChanged += OnValueChanged;
        }

        private void OnEffectEnable(object sender, EventArgs e)
        {
            var enable = chkVCropEnable.Checked;
            EnableDisable(enable);            
            _crop.Enable = enable;
            Video.VideoControl.ApplyFilters();
        }

        private void OnValueChanged(object sender, EventArgs e)
        {
            var no = (NumericUpDown)sender;
            var sz = Video.VideoControl.VideoSize;
            switch (no.Tag.ToString())
            {
                case "TOP":
                    _cropArea.Y = (int)no.Value;
                    _crop.Top = _cropArea.Y;                    
                    break;

                case "LEFT":
                    _cropArea.X = (int)no.Value;
                    _crop.Left = _cropArea.Left;                    
                    break;

                case "BOTTOM":                    
                    _cropArea.Height = sz.Height - (int)no.Value;
                    _crop.Bottom = (int)no.Value;                    
                    break;

                case "RIGHT":                    
                    _cropArea.Width = sz.Width - (int)no.Value;
                    _crop.Right = (int)no.Value;                    
                    break;
            }
            Video.VideoControl.ApplyFilters();
        }

        private void EnableDisable(bool enable)
        {
            foreach (var ctl in Controls.Cast<Control>().Where(ctl => ctl != chkVCropEnable))
            {
                ctl.Enabled = enable;
            }
        }
    }
}

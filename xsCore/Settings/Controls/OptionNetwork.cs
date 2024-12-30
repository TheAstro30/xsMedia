/* xsMedia - xsSettings
 * (c)2013 - 2024
 * Jason James Newland
 * KangaSoft Software, All Rights Reserved
 * Licenced under the GNU public licence */
using System;
using System.Windows.Forms;
using xsCore.Internal;
using xsCore.Settings.Controls.Base;
using xsCore.Settings.Data;

namespace xsCore.Settings.Controls
{
    public partial class OptionNetwork : OptionBase
    {
        private readonly NetworkPresets _presets;

        public OptionNetwork(NetworkPresets presets) : base("Network Playback")
        {
            InitializeComponent();
            _presets = presets;
            /* Init */
            var index = OptionsList.GetOption(":network-caching", _presets.Options.Option);
            txtCaching.Text = index > -1 ? _presets.Options.Option[index].Data : string.Empty;
            /* Handlers */
            txtCaching.TextChanged += OnOptionChanged;
        }

        private void OnOptionChanged(object sender, EventArgs e)
        {
            var ctl = (Control)sender;
            switch (ctl.Tag.ToString())
            {
                case "CACHE":
                    var sp = txtCaching.Text.Split(' ');
                    var index = OptionsList.GetOption(":network-caching", _presets.Options.Option);
                    var option = new MediaOptions.MediaOption(":network-caching", sp[0]);
                    if (index == -1)
                    {
                        _presets.Options.Option.Add(option);
                    }
                    else
                    {
                        _presets.Options.Option[index] = option;
                    }
                    break;
            }
        }
    }
}

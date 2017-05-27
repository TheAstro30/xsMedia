using System;
using System.Windows.Forms;
using xsSettings.Internal;
using xsSettings.Settings;

namespace xsSettings.Controls
{
    public partial class OptionNetwork : OptionBase
    {
        private readonly SettingsNetworkPresets _presets;

        public OptionNetwork(SettingsNetworkPresets presets) : base("Network Options")
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
                    var option = new SettingsMediaOptions.MediaOption(":network-caching", sp[0]);
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

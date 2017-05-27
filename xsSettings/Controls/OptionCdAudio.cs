﻿using System;
using System.Windows.Forms;
using xsSettings.Internal;
using xsSettings.Settings;

namespace xsSettings.Controls
{
    public partial class OptionCdAudio : OptionBase
    {
        private readonly SettingsDisc _disc;

        public OptionCdAudio(SettingsDisc disc) : base("CD Audio Options")
        {
            InitializeComponent();
            _disc = disc;
            /* Init */
            var index = OptionsList.GetOption(":disc-caching", _disc.Options.Option);
            txtCaching.Text = index > -1 ? _disc.Options.Option[index].Data : string.Empty;
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
                    var index = OptionsList.GetOption(":disc-caching", _disc.Options.Option);
                    var option = new SettingsMediaOptions.MediaOption(":disc-caching", sp[0]);
                    if (index == -1)
                    {
                        _disc.Options.Option.Add(option);
                    }
                    else
                    {
                        _disc.Options.Option[index] = option;
                    }
                    break;
            }
        }
    }
}

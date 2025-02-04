﻿/* xsMedia - xsSettings
 * (c)2013 - 2025
 * Jason James Newland
 * KangaSoft Software, All Rights Reserved
 * Licenced under the GNU public licence */
using System;
using System.Windows.Forms;
using xsCore.Internal;
using xsCore.Settings.Controls.Base;
using xsCore.Settings.Data.Disc;
using xsCore.Settings.Data.Media;

namespace xsCore.Settings.Controls
{
    public partial class OptionDvd : OptionBase
    {
        private readonly DiscData _disc;

        public OptionDvd(DiscData disc) : base("DVD Playback")
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
                    var option = new MediaOptionData(":disc-caching", sp[0]);
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

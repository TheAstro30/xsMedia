﻿/* xsMedia - xsSettings
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
    public partial class OptionVcd : OptionBase
    {
        private readonly DiscData _disc;

        public OptionVcd(DiscData disc) : base("VCD/SVCD Playback")
        {
            InitializeComponent();
            _disc = disc;
            /* Init */
            var index = OptionsList.GetOption(":DiscData-caching", _disc.Options.Option);
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
                    var index = OptionsList.GetOption(":DiscData-caching", _disc.Options.Option);
                    var option = new MediaOptions.MediaOption(":DiscData-caching", sp[0]);
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
/* xsMedia - xsSettings
 * (c)2013 - 2020
 * Jason James Newland
 * KangaSoft Software, All Rights Reserved
 * Licenced under the GNU public licence */
using System;
using System.Windows.Forms;
using xsSettings.Settings;

namespace xsSettings.Controls
{
    public partial class OptionCddb : OptionBase
    {
        private readonly SettingsCddb _cddb;

        public OptionCddb(SettingsCddb cddb) : base("CDDB Lookup")
        {
            InitializeComponent();
            _cddb = cddb;
            /* Init */
            chkEnable.Checked = _cddb.Enabled;
            txtHost.Text = _cddb.Host;
            chkCache.Checked = cddb.Cache;
            /* Handlers */
            chkEnable.CheckedChanged += OnOptionChanged;
            txtHost.TextChanged += OnOptionChanged;
            chkCache.CheckedChanged += OnOptionChanged;
        }

        private void OnOptionChanged(object sender, EventArgs e)
        {
            var ctl = (Control)sender;
            switch (ctl.Tag.ToString())
            {
                case "ENABLE":
                    _cddb.Enabled = chkEnable.Checked;
                    break;

                case "HOST":
                    var sp = txtHost.Text.Split(' ');
                    _cddb.Host = sp[0];
                    break;

                case "CACHE":
                    _cddb.Cache = chkCache.Checked;
                    break;
            }
        }
    }
}

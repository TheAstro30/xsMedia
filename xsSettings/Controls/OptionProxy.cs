/* xsMedia - xsSettings
 * (c)2013 - 2024
 * Jason James Newland
 * KangaSoft Software, All Rights Reserved
 * Licenced under the GNU public licence */
using System;
using System.Globalization;
using System.Windows.Forms;
using xsCore.Utils;

namespace xsSettings.Controls
{
    public partial class OptionProxy : OptionBase
    {
        private readonly Proxy _proxy;        

        public OptionProxy(Proxy proxy) : base("HTTP Proxy")
        {
            InitializeComponent();
            _proxy = proxy;
            /* Data */
            txtHost.Text = _proxy.Host;
            txtPort.Text = _proxy.Port.ToString(CultureInfo.InvariantCulture);
            txtUserName.Text = _proxy.User;
            txtPassword.Text = _proxy.Password;
            /* Callbacks */
            txtHost.TextChanged += OnOptionChanged;
            txtPort.TextChanged += OnOptionChanged;
            txtUserName.TextChanged += OnOptionChanged;
            txtPassword.TextChanged += OnOptionChanged;
            OptionsChanged = false;
        }

        public bool OptionsChanged { get; private set; }        

        private void OnOptionChanged(object sender, EventArgs e)
        {            
            var text = (TextBox)sender;
            var sp = text.Text.Split(' ');
            switch (text.Tag.ToString())
            {
                case "HOST":
                    _proxy.Host = sp[0];
                    break;

                case "PORT":
                    int port;
                    int.TryParse(sp[0], out port);
                    _proxy.Port = port;
                    break;

                case "USER":
                    _proxy.User = sp[0];
                    break;

                case "PASS":
                    _proxy.Password = sp[0];
                    break;
            }
            OptionsChanged = true;
        }
    }
}

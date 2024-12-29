/* xsMedia - sxCore
 * (c)2013 - 2024
 * Jason James Newland
 * KangaSoft Software, All Rights Reserved
 * Licenced under the GNU public licence */
using System;
using System.Globalization;
using System.Xml.Serialization;

namespace xsCore.Utils
{
    [Serializable]
    public class Proxy
    {
        public Proxy()
        {
            /* Default constructor */
        }

        public Proxy(Proxy proxy)
        {
            Host = proxy.Host;
            Port = proxy.Port;
            User = proxy.User;
            Password = proxy.Password;
        }

        [XmlAttribute("host")]
        public string Host { get; set; }

        [XmlAttribute("port")]
        public string PortString { get; set; }

        [XmlAttribute("user")]
        public string User { get; set; }

        [XmlAttribute("password")]
        public string Password { get; set; }

        [XmlIgnore]
        public string Id
        {
            get { return "--http-proxy"; }
        }

        [XmlIgnore]
        public int Port
        {
            get
            {
                int port;
                Int32.TryParse(PortString, out port);
                return port;
            }
            set { PortString = value.ToString(CultureInfo.InvariantCulture); }
        }

        public override string ToString()
        {
            if (string.IsNullOrEmpty(Host) || Port == 0)
            {
                return string.Empty;
            }
            if (!string.IsNullOrEmpty(User))
            {
                return !string.IsNullOrEmpty(Password)
                           ? string.Format("{0}={1}:{2}@{3}:{4}", Id, User, Password, Host, Port)
                           : string.Format("{0}={1}@{2}:{3}", Id, User, Host, Port);
            }
            return string.Format("{0}={1}:{2}", Id, Host, Port);
        }
    }
}

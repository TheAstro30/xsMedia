/* xsMedia - sxCore
 * (c)2013 - 2024
 * Jason James Newland
 * KangaSoft Software, All Rights Reserved
 * Licenced under the GNU public licence */
using System.Net;
using xsCore.Utils;

namespace xsCore.Internal
{
    internal class ProxyProvider
    {
        private readonly Proxy _proxy;

        public ProxyProvider(Proxy proxy)
        {
            _proxy = proxy;
        }

        public HttpWebRequest BeginRequest(string url)
        {
            var webRequestObject = (HttpWebRequest)WebRequest.Create(url);
            if (_proxy != null && !string.IsNullOrEmpty(_proxy.Host) && _proxy.Port != 0)
            {
                var prx = new WebProxy(string.Format("{0}:{1}", _proxy.Host, _proxy.Port));
                if (!string.IsNullOrEmpty(_proxy.User) && !string.IsNullOrEmpty(_proxy.Password))
                {
                    prx.Credentials = new NetworkCredential(_proxy.User, _proxy.Password);
                }
                webRequestObject.PreAuthenticate = true;
                WebRequest.DefaultWebProxy = prx;
            }
            return webRequestObject;
        }
    }
}

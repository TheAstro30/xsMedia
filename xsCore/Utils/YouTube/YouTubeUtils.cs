using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using xsCore.Internal;

namespace xsCore.Utils.YouTube
{
    /* Based on code by akramKamal http://www.codeproject.com/Tips/323771/YouTube-Downloader-Using-Csharp-NET */
    public static class YouTubeUtils
    {
        public static string GetTextBetween(string input, string start, string end, int startIndex)
        {
            return GetTextBetween(input, start, end, startIndex, false);
        }

        private static string GetTextBetween(string input, string start, string end, int startIndex, bool useLastIndexOf)
        {
            if (string.IsNullOrEmpty(input)) { return null; }
            var index1 = useLastIndexOf ? input.LastIndexOf(start, startIndex, StringComparison.Ordinal) :
                                          input.IndexOf(start, startIndex, StringComparison.Ordinal);
            if (index1 == -1) return "";
            index1 += start.Length;
            var index2 = input.IndexOf(end, index1, StringComparison.Ordinal);
            return index2 == -1 ? input.Substring(index1) : input.Substring(index1, index2 - index1);
        }

        public static string DownloadWebPage(string url, Proxy proxy)
        {
            return DownloadWebPage(url, null, proxy);
        }

        public static string GetVideoId(string url)
        {
            return string.IsNullOrEmpty(url)
                       ? null
                       : Regex.Match(url,
                                     @"(?:youtube\.com\/(?:[^\/]+\/.+\/|(?:v|e(?:mbed)?)\/|.*[?&]v=)|youtu\.be\/)([^""&?\/ ]{11})")
                             .Groups[1].Value;
        }

        public static Bitmap GetVideoThumbNail(string url, Proxy proxy)
        {
            if (string.IsNullOrEmpty(url)) { return null; }
            HttpWebResponse response;
            try
            {
                /* Get a web response from the url */
                var proxyProvider = new ProxyProvider(proxy);
                var request = proxyProvider.BeginRequest(url);
                response = (HttpWebResponse)request.GetResponse();
            }
            catch
            {
                return null;
            }
            var length = response.ContentLength;
            if (length == 0) { return null; }
            Image img;
            using (Stream memoryStream = new MemoryStream())
            {
                /* Download the image to a memory stream */
                while (true)
                {
                    var bytes = new byte[4097];
                    var responseStream = response.GetResponseStream();
                    if (responseStream == null) { break; }
                    var bytesRead = responseStream.Read(bytes, 0, 4096);
                    if (bytesRead == 0) { break; }
                    /* Copy the byte data from http stream to memory stream */
                    memoryStream.Write(bytes, 0, bytesRead);
                }
                /* Create a new bitmap from the memory stream */
                img = Image.FromStream(memoryStream);
                memoryStream.Close();
            }
            return (Bitmap)img;
        }

        private static string DownloadWebPage(string url, string stopLine, Proxy proxy)
        {
            var proxyProvider = new ProxyProvider(proxy);
            var request = proxyProvider.BeginRequest(url);
            /* You can also specify additional header values like the user agent or the referer */
            request.UserAgent = ".NET Framework/2.0";
            /* Request response */
            string pageContent = null;
            try
            {
                using (var response = request.GetResponse())
                {
                    using (var webStream = response.GetResponseStream())
                    {
                        if (webStream != null)
                        {
                            using (var reader = new StreamReader(webStream))
                            {
                                pageContent = "";
                                if (stopLine == null)
                                {
                                    pageContent = reader.ReadToEnd();
                                }
                                else
                                {
                                    while (!reader.EndOfStream)
                                    {
                                        var line = reader.ReadLine();
                                        pageContent += line + Environment.NewLine;
                                        if (line != null && line.Contains(stopLine))
                                        {
                                            break;
                                        }
                                    }
                                }
                                /* Cleanup */
                                reader.Close();
                            }
                            webStream.Close();
                        }
                    }
                    response.Close();
                }
            }
            catch
            {
                return null;
            }
            return pageContent;
        }
    }
}

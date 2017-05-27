using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using xsCore.Internal;

namespace xsCore.Utils.Asx
{
    public sealed class AsxParser
    {
        public static event Action<AsxData> ParseCompleted;
        public static event Action ParseFailed;

        public static void BeginParse(string sourceUrl, Proxy proxy)
        {
            var t = new Thread(() => BackgroundParse(sourceUrl, proxy))
                        {
                            IsBackground = true
                        };
            t.Start();
        }

        private static void BackgroundParse(string sourceUrl, Proxy proxy)
        {
            /* We begin first by downloading the source file in the background */
            var tmpFile = Path.GetTempFileName();
            var proxyProvider = new ProxyProvider(proxy);
            var request = proxyProvider.BeginRequest(sourceUrl);
            /* You can also specify additional header values like the user agent or the referer */
            request.UserAgent = ".NET Framework/2.0";
            try
            {
                var response = request.GetResponse();
                var buffer = new byte[1024];

                using (var fileStream = File.OpenWrite(tmpFile))
                {
                    using (var input = response.GetResponseStream())
                    {
                        if (input == null)
                        {
                            if (ParseFailed != null)
                            {
                                ParseFailed();
                            }
                            return;
                        }
                        var size = input.Read(buffer, 0, buffer.Length);
                        while (size > 0)
                        {
                            fileStream.Write(buffer, 0, size);
                            size = input.Read(buffer, 0, buffer.Length);
                        }
                    }
                    fileStream.Flush();
                    fileStream.Close();
                }
                /* If we successfully get here, file was downloaded so now we can parse the XML */
                Parse(tmpFile);
                File.Delete(tmpFile);
            }
            catch
            {
                if (ParseFailed != null)
                {
                    ParseFailed();
                }
            }
        }

        private static void Parse(string fileName)
        {
            /* Open the downloaded file and read it ready for processing */
            try
            {
                using (var fs = new FileStream(fileName, FileMode.Open))
                {
                    using (var reader = new StreamReader(fs))
                    {
                        InternalParse(reader.ReadToEnd());
                        reader.Close();
                    }
                    fs.Close();
                }
            }
            catch
            {
                if (ParseFailed != null)
                {
                    ParseFailed();
                }
            }
        }

        private static void InternalParse(string html)
        {
            /* This may seem like a messy way to do this, but some ASX files are non-standard XML/XHTML formatted so presents and error
             * for the standard XMLReader ... */
            var data = new AsxData
                           {
                               HeaderTitle = GetTitle(html)
                           };
            /* Get the entry tags in a match collection */
            foreach (Match entry in GetHtmlBlock(html, "entry", false))
            {
                /* Get title */
                var t = GetTitle(entry.Groups[1].Value);
                /* Get the url data */
                foreach (Match url in GetHtmlBlock(entry.Groups[1].Value, @"ref\s", true))
                {                    
                    var entryData = new AsxEntryData
                                        {
                                            Title = t
                                        };
                    var block = url.Groups[1].Value.Split(new[] { '"' }, StringSplitOptions.RemoveEmptyEntries);
                    if (block.Length <= 1)
                    {
                        continue;
                    }
                    entryData.Url = block[1];
                    /* Add the entry data to the storage class */
                    data.Entries.Add(entryData);
                }
            }
            /* Raise events if successful or failed */
            if (data.Entries.Count > 0)
            {
                if (ParseCompleted != null)
                {
                    ParseCompleted(data);
                }
            }
            else
            {
                if (ParseFailed != null)
                {
                    ParseFailed();
                }
            }
        }

        /* Private helper methods */
        private static string GetTitle(string html)
        {
            var title = Regex.Match(html, HtmlTagPattern("title", false), RegexOptions.Singleline | RegexOptions.IgnoreCase | RegexOptions.Compiled);
            return title.Groups[1].Value.Replace(">", "");
        }

        private static MatchCollection GetHtmlBlock(string html, string tag, bool selfClose)
        {
            return Regex.Matches(html, HtmlTagPattern(tag, selfClose), RegexOptions.Singleline | RegexOptions.IgnoreCase | RegexOptions.Compiled);
        }

        private static string HtmlTagPattern(string tag, bool selfClose)
        {
            return selfClose ? string.Format(@"<{0}((?:(?!\/>)[\s\S])*)", tag) : string.Format(@"<{0}((?:(?!<\/{0}>)[\s\S])*)", tag);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;

namespace xsCore.Utils.YouTube
{
    /* Based on code by akramKamal http://www.codeproject.com/Tips/323771/YouTube-Downloader-Using-Csharp-NET */
    public sealed class YouTubeUrlParser
    {
        public static List<YouTubeVideoQuality> GetYouTubeVideoUrls(string videoUrl, Proxy proxy)
        {
            var urls = new List<YouTubeVideoQuality>();
            var html = YouTubeUtils.DownloadWebPage(videoUrl, proxy);
            var title = GetTitle(html);
            var url = videoUrl;
            foreach (var q in ExtractUrls(html).Select(videoLink => new YouTubeVideoQuality
                                                                        {
                                                                            VideoUrl = url,
                                                                            VideoTitle = title,
                                                                            DownloadUrl = string.Format("{0}&title={1}", videoLink, title)
                                                                        }))
            {
                var len = Regex.Match(html, "\"length_seconds\":(.+?),", RegexOptions.Singleline).Groups[1].ToString().Replace("\"", "");
                long t;
                if (long.TryParse(len, out t))
                {
                    q.Length = t;
                }
                var isWide = IsWideScreen(html);
                if (GetQuality(q, isWide))
                {
                    urls.Add(q);
                }
            }
            return urls;
        }

        private static string GetTitle(string html)
        {
            var title = YouTubeUtils.GetTextBetween(html, "'VIDEO_TITLE': '", "'", 0);
            if (string.IsNullOrEmpty(title))
            {
                title = YouTubeUtils.GetTextBetween(html, "\"title\" content=\"", "\"", 0);
            }
            if (string.IsNullOrEmpty(title))
            {
                title = YouTubeUtils.GetTextBetween(html, "&title=", "&", 0);
            }
            if (!string.IsNullOrEmpty(title))
            {
                title = title.Replace("&amp;", "&").Replace(@"\", "").Replace("\"", "_").Replace("<", "_").Replace(">", "_").
                    Replace("+", "_");
            }
            return title;
        }

        private static IEnumerable<string> ExtractUrls(string html)
        {            
            var urls = new List<string>();
            if (string.IsNullOrEmpty(html)) { return urls; }        
            const string dataBlockStart = "\"url_encoded_fmt_stream_map\":\"(.+?)&";
            /* Marks start of Javascript Data Block */
            html = Uri.UnescapeDataString(Regex.Match(html, dataBlockStart, RegexOptions.Singleline).Groups[1].ToString());
            var firstPatren = html.Substring(0, html.IndexOf('=') + 1);
            var matchs = Regex.Split(html, firstPatren);
            for (var i = 0; i < matchs.Length; i++)
            {
                matchs[i] = firstPatren + matchs[i];
            }
            foreach (var match in matchs)
            {
                if (!match.Contains("url=")) { continue; }
                var url = YouTubeUtils.GetTextBetween(match, "url=", "\\u0026", 0);
                if (url == "") { url = YouTubeUtils.GetTextBetween(match, "url=", ",url", 0); }
                if (url == "") { url = YouTubeUtils.GetTextBetween(match, "url=", "\",", 0); }
                var sig = YouTubeUtils.GetTextBetween(match, "sig=", "\\u0026", 0);
                if (sig == "") { sig = YouTubeUtils.GetTextBetween(match, "sig=", ",sig", 0); }
                if (sig == "") { sig = YouTubeUtils.GetTextBetween(match, "sig=", "\",", 0); }

                while (url.EndsWith(",") || url.EndsWith(".") || url.EndsWith("\""))
                {
                    url = url.Remove(url.Length - 1, 1);
                }
                while (sig.EndsWith(",") || sig.EndsWith(".") || sig.EndsWith("\""))
                {
                    sig = sig.Remove(sig.Length - 1, 1);
                }
                if (string.IsNullOrEmpty(url)) { continue; }
                if (!string.IsNullOrEmpty(sig))
                {
                    url += "&signature=" + sig;
                }
                urls.Add(url);
            }
            return urls;
        }

        private static bool GetQuality(YouTubeVideoQuality q, bool wide)
        {
            var tag = Regex.Match(q.DownloadUrl, @"itag=([1-9]?[0-9]?[0-9])", RegexOptions.Singleline).Groups[1].ToString();
            if (!string.IsNullOrEmpty(tag))
            {
                int tagValue;
                if (!Int32.TryParse(tag, out tagValue))
                {
                    tagValue = 0;
                }
                switch (tagValue)
                {
                    case 5:
                        q.SetQuality("flv", new Size(320, (wide ? 180 : 240)));
                        break;
                    case 6:
                        q.SetQuality("flv", new Size(480, (wide ? 270 : 360)));
                        break;
                    case 17:
                        q.SetQuality("3gp", new Size(176, (wide ? 99 : 144)));
                        break;
                    case 18:
                        q.SetQuality("mp4", new Size(640, (wide ? 360 : 480)));
                        break;
                    case 22:
                        q.SetQuality("mp4", new Size(1280, (wide ? 720 : 960)));
                        break;
                    case 34:
                        q.SetQuality("flv", new Size(640, (wide ? 360 : 480)));
                        break;
                    case 35:
                        q.SetQuality("flv", new Size(854, (wide ? 480 : 640)));
                        break;
                    case 36:
                        q.SetQuality("3gp", new Size(320, (wide ? 180 : 240)));
                        break;
                    case 37:
                        q.SetQuality("mp4", new Size(1920, (wide ? 1080 : 1440)));
                        break;
                    case 38:
                        q.SetQuality("mp4", new Size(2048, (wide ? 1152 : 1536)));
                        break;
                    case 43:
                        q.SetQuality("webm", new Size(640, (wide ? 360 : 480)));
                        break;
                    case 44:
                        q.SetQuality("webm", new Size(854, (wide ? 480 : 640)));
                        break;
                    case 45:
                        q.SetQuality("webm", new Size(1280, (wide ? 720 : 960)));
                        break;
                    case 46:
                        q.SetQuality("webm", new Size(1920, (wide ? 1080 : 1440)));
                        break;
                    case 82:
                        q.SetQuality("3D.mp4", new Size(480, (wide ? 270 : 360)));
                        break; /* 3D */
                    case 83:
                        q.SetQuality("3D.mp4", new Size(640, (wide ? 360 : 480)));
                        break; /* 3D */
                    case 84:
                        q.SetQuality("3D.mp4", new Size(1280, (wide ? 720 : 960)));
                        break; /* 3D */
                    case 85:
                        q.SetQuality("3D.mp4", new Size(1920, (wide ? 1080 : 1440)));
                        break; /* 3D */
                    case 100:
                        q.SetQuality("3D.webm", new Size(640, (wide ? 360 : 480)));
                        break; /* 3D */
                    case 101:
                        q.SetQuality("3D.webm", new Size(640, (wide ? 360 : 480)));
                        break; /* 3D */
                    case 102:
                        q.SetQuality("3D.webm", new Size(1280, (wide ? 720 : 960)));
                        break; /* 3D */
                    case 120:
                        q.SetQuality("live.flv", new Size(1280, (wide ? 720 : 960)));
                        break; /* Live-streaming - should be ignored? */
                    default:
                        q.SetQuality("itag-" + tag, new Size(0, 0));
                        break; /* Unknown or parse error */
                }
                return true;
            }
            return false;
        }

        public static Boolean IsWideScreen(string html)
        {
            var match =
                Regex.Match(html, @"'IS_WIDESCREEN':\s+(.+?)\s+", RegexOptions.Singleline).Groups[1].ToString().ToLower()
                    .Trim();
            return (match == "true" || match == "true,");
        }
    }
}
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;

namespace xsCore.Utils.YouTube
{
    /* Based on code by akramKamal http://www.codeproject.com/Tips/323771/YouTube-Downloader-Using-Csharp-NET */
    public class YouTubeVideoQuality
    {
        public string VideoTitle { get; set; }
        public string Extension { get; set; }
        public string DownloadUrl { get; set; }
        public string VideoUrl { get; set; }
        public Size Dimension { get; set; }
        public long Length { get; set; }

        public void SetQuality(string extention, Size dimension)
        {
            Extension = extention;
            Dimension = dimension;
        }

        public override string ToString()
        {
            return string.Format("{0} File {1}x{2}", Extension, Dimension.Width, Dimension.Height);
        }
    }

    /* Wrapper class for YouTube parsing */ 
    public class YouTubeUrl
    {
        private string _videoId;
        private readonly Proxy _proxy;

        public YouTubeUrl()
        {
            /* Empty */
        }

        public YouTubeUrl(Proxy proxy)
        {
            _proxy = proxy;
        }

        public event Action<Bitmap> YouTubeThumbNailParsed;
        public event Action<List<YouTubeVideoQuality>> YouTubeUrlParseCompleted;
        public event Action<string> YouTubeUrlParseFailed;

        public void BeginParse(string url)
        {
            _videoId = YouTubeUtils.GetVideoId(url);
            if (string.IsNullOrEmpty(_videoId))
            {
                /* If the url isn't a valid youtube url, pass back original url */
                var list = new List<YouTubeVideoQuality>
                               {
                                   new YouTubeVideoQuality
                                       {
                                           DownloadUrl = url
                                       }
                               };
                if (YouTubeUrlParseCompleted != null)
                {
                    YouTubeUrlParseCompleted(list);
                }
                return;
            }
            /* Begin parsing in a background thread */
            var t = new Thread(() => BeginParseThread(url))
                        {
                            IsBackground = true
                        };
            t.Start();
        }

        /* Parsing background thread */
        private void BeginParseThread(string url)
        {
            /* Attempt to get the thumbnail image */                      
            if (YouTubeThumbNailParsed != null)
            {
                /* Note: can be 0.jpg to get full-sized thumbnail image */
                YouTubeThumbNailParsed(YouTubeUtils.GetVideoThumbNail(string.Format("http://i3.ytimg.com/vi/{0}/default.jpg", _videoId), _proxy));                
            }
            /* Parse the available video formats */
            var urls = YouTubeUrlParser.GetYouTubeVideoUrls(url, _proxy);
            if (urls.Count == 0)
            {
                if (YouTubeUrlParseFailed != null)
                {
                    YouTubeUrlParseFailed(url);
                }
                return;
            }
            if (YouTubeUrlParseCompleted != null)
            {                
                YouTubeUrlParseCompleted(urls);
            }
        }
    }
}

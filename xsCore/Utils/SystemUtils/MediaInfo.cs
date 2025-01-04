/* xsMedia - Media Player
 * (c)2013 - 2025
 * Jason James Newland
 * KangaSoft Software, All Rights Reserved
 * Licenced under the GNU public licence */
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using xsVlc.Common;
using xsVlc.Common.Events;
using xsVlc.Common.Media;
using xsVlc.Core;

namespace xsCore.Utils.SystemUtils
{
    public static class MediaInfo
    {
        private static IMediaPlayerFactory _mediaFactory;
        private static IMedia _media;

        private static int _duration;

        public static int GetDuration(string fileName)
        {
            if (!File.Exists(fileName))
            {
                return 0;
            }
            _duration = 0; /* Make sure to reset this to 0 so we're not returning a value from a previous call */
            _mediaFactory = new MediaPlayerFactory();
            _media = _mediaFactory.CreateMedia<IMediaFromFile>(fileName);
            _media.Events.DurationChanged += MediaDurationChanged;
            _media.Parse(false); /* Needs to be non-async so we can return a value, it shouldn't be an issue for most media */
            /* Dispose and return */
            _media.Events.DurationChanged -= MediaDurationChanged;
            _mediaFactory.Dispose();
            _media.Dispose();
            return _duration;
        }

        /* Shell extension to get mostly mp3 album art */
        public static Bitmap GetAlbumArt(string fileName, string artist, string album)
        {
            /* Get album art - have to do it the long way */
            if (string.IsNullOrEmpty(artist) || string.IsNullOrEmpty(album))
            {
                return null;
            }
            var art = new[] { "art", "art.jpg", "art.png", "art.bmp" };
            foreach (var path in art.Select(a => string.Format("{0}\\vlc\\art\\artistalbum\\{1}\\{2}\\{3}",
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                CleanInvalidCharacters(artist), CleanInvalidCharacters(album), a)).Where(File.Exists))
            {
                return new Bitmap(Image.FromFile(path));
            }
            /* Attempt to get album art from file */
            var thumb = new ShellThumbnail
            {
                DesiredSize = new Size(128, 128)
            };
            var bmp = thumb.GetThumbnail(fileName);
            if (bmp != null)
            {
                return new Bitmap(bmp);
            }
            /* If folder.jpg exists in the media path, use it */
            var filePath = Path.GetDirectoryName(fileName);
            var file = string.Format("{0}\\Folder.jpg", filePath);
            return File.Exists(file) ? new Bitmap(Image.FromFile(file)) : null;
        }

        public static void ReplaceCachedArtFile(string fileName, string artist, string album, Bitmap newImage)
        {
            if (string.IsNullOrEmpty(artist) || string.IsNullOrEmpty(album))
            {
                return;
            }
            var art = new[] { "art", "art.jpg", "art.png", "art.bmp" };
            foreach (var path in art.Select(a => string.Format("{0}\\vlc\\art\\artistalbum\\{1}\\{2}\\{3}",
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                CleanInvalidCharacters(artist), CleanInvalidCharacters(album), a)).Where(File.Exists))
            {
                try
                {
                    File.Delete(path);
                    var newPath = string.Format("{0}\\art.png", Path.GetDirectoryName(path));
                    newImage.Save(newPath, ImageFormat.Png);                    
                }
                catch (Exception)
                {
                    /* Silently return */
                    return;
                }                
                return;
            }
        }

        public static string FormatDurationString(int duration)
        {
            /* Formats as either 00:00s or 00:00:00s */
            var ts = new TimeSpan(0, 0, 0, duration);
            return (duration >= 3600
                ? string.Format("{0:00}:{1:00}:{2:00}", ts.Hours, ts.Minutes, ts.Seconds)
                : string.Format("{0:00}:{1:00}", ts.Minutes, ts.Seconds));
        }

        private static string CleanInvalidCharacters(string s)
        {
            var invalid = string.Format("{0}{1}", new string(Path.GetInvalidFileNameChars()), new string(Path.GetInvalidPathChars()));// 
            var valid = invalid.Aggregate(s, (current, c) => current.Replace(c, '_'));
            if (valid.EndsWith(" "))
            {
                valid = string.Format("{0}_", valid.TrimEnd());
            }
            return valid;
        }

        /* Media parsing callback */
        private static void MediaDurationChanged(object sender, MediaDurationChange e)
        {
            _duration = (int) e.NewDuration/1000;            
        }
    }
}

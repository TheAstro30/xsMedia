/* xsMedia - Media Player
 * (c)2013 - 2024
 * Jason James Newland
 * KangaSoft Software, All Rights Reserved
 * Licenced under the GNU public licence */
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using Shell32;
using xsCore.Utils.SystemUtils;

namespace xsCore.Utils
{
    public static class MediaInfo
    {
        public static bool GetDuration(string filename, out TimeSpan timeSpan)
        {
            try
            {
                var shl = new Shell();
                var fldr = shl.NameSpace(Path.GetDirectoryName(filename));
                var itm = fldr.ParseName(Path.GetFileName(filename));
                /* Index 27 is the video duration [This may not always be the case] */
                var propValue = fldr.GetDetailsOf(itm, 27);
                return TimeSpan.TryParse(propValue, out timeSpan);
            }
            catch (Exception)
            {
                timeSpan = new TimeSpan();
                return false;
            }
        }

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
    }
}

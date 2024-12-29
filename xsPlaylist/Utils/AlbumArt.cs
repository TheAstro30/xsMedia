/* xsMedia - xsPlaylist
 * (c)2013 - 2024
 * Jason James Newland
 * KangaSoft Software, All Rights Reserved
 * Licenced under the GNU public licence */
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using xsCore.Utils.SystemUtils;

namespace xsPlaylist.Utils
{
    public static class AlbumArt
    {
        public static Bitmap GetAlbumArt(string fileName, string artist, string album)
        {
            /* Get album art - have to do it the long way */
            if (string.IsNullOrEmpty(artist) || string.IsNullOrEmpty(album))
            {
                return null;
            }
            var art = new[] {"art", "art.jpg", "art.png", "art.bmp"};
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

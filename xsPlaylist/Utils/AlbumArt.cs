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
            if (!string.IsNullOrEmpty(artist) && !string.IsNullOrEmpty(album))
            {
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
                if (File.Exists(file))
                {
                    return new Bitmap(Image.FromFile(file));
                }
            }
            return null;
        }

        private static string CleanInvalidCharacters(string s)
        {
            var invalid = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
            var valid = invalid.Aggregate(s, (current, c) => current.Replace(c, '_'));
            if (valid.EndsWith(" "))
            {
                valid = string.Format("{0}_", valid.Substring(0, valid.Length - 1));
            }
            return valid;
        }
    }
}

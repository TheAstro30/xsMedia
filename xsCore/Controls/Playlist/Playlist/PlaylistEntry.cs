﻿/* xsMedia - xsPlaylist
 * (c)2013 - 2025
 * Jason James Newland
 * KangaSoft Software, All Rights Reserved
 * Licenced under the GNU public licence */
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using libolv.Implementation;
using xsCore.Utils.SystemUtils;

namespace xsCore.Controls.Playlist.Playlist
{
    public enum PlaylistType
    {
        Default = 0,
        Xspf = 1,
        Wpl = 2,
        M3U = 3,
        M3U8 = 4,
        Pls = 5
    }

    public enum PlaylistSortType
    {
        Title = 0,
        Artist = 1,
        Path = 2,
        Random = 3
    }

    public class PlaylistEntry
    {
        /* Sort comparers */
        public class CompareByTitle : IComparer<PlaylistEntry>
        {
            public int Compare(PlaylistEntry x, PlaylistEntry y)
            {
                return x == null || y == null ? 0 : new CaseInsensitiveComparer().Compare(x.Title, y.Title);
            }
        }

        public class CompareByArtist : IComparer<PlaylistEntry>
        {
            public int Compare(PlaylistEntry x, PlaylistEntry y)
            {
                return x == null || y == null ? 0 : new CaseInsensitiveComparer().Compare(x.Artist, y.Artist);
            }
        }

        public class CompareByPath : IComparer<PlaylistEntry>
        {
            public int Compare(PlaylistEntry x, PlaylistEntry y)
            {
                return x == null || y == null ? 0 : new CaseInsensitiveComparer().Compare(x.Location.ToLower(), y.Location.ToLower());
            }
        }

        public class PlaylistDuplicateComparer : IEqualityComparer<PlaylistEntry>
        {
            public bool Equals(PlaylistEntry x, PlaylistEntry y)
            {
                return x != null && y != null && string.Equals(x.Location, y.Location, StringComparison.CurrentCultureIgnoreCase);
            }

            public int GetHashCode(PlaylistEntry obj)
            {
                return obj.Location.GetHashCode();
            }
        }

        /* Constructor */
        public PlaylistEntry()
        {
            Options = new List<string>();
        }

        [OlvIgnore]
        public string Title { get; set; }

        public string TitleString
        {
            get
            {
                if (!string.IsNullOrEmpty(Title))
                {
                    return !string.IsNullOrEmpty(Artist) ? string.Format("{0} - {1}", Title, Artist) : Title;
                }
                var location = Path.GetFileNameWithoutExtension(Location);
                return !string.IsNullOrEmpty(location) ? location : Location;
            }
        }

        public string Album { get; set; }

        public string LengthString
        {
            get
            {
                /* Format the "Length" field as 00:00 - lengths are in milliseconds */
                var length = Length > 1000 ? Length / 1000 : 0;
                return MediaInfo.FormatDurationString(length);
            }
        }

        [OlvIgnore]
        public int Length { get; set; }

        [OlvIgnore]
        public string Location { get; set; }
                       
        [OlvIgnore]
        public string Artist { get; set; }
        
        [OlvIgnore]
        public List<string> Options { get; set; }
    }
}
/* xsMedia - xsPlaylist
 * (c)2013 - 2025
 * Jason James Newland
 * KangaSoft Software, All Rights Reserved
 * Licenced under the GNU public licence */
using System.Collections.Generic;

namespace xsCore.Controls.Playlist.Playlist
{
    public interface IPlaylist : IList<PlaylistEntry>
    {
        PlaylistType Type { get; }

        string PlaylistTitle { get; set; }

        bool Read(string fileName);
        bool Write(string fileName);
        bool Write(string fileName, IList<PlaylistEntry> entries);

        void Sort(PlaylistSortType sortType);
        void RemoveDuplicates();
    }
}
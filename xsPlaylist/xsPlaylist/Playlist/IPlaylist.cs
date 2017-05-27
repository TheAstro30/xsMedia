using System.Collections.Generic;

namespace xsPlaylist.Playlist
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
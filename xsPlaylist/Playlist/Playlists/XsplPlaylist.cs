/* xsMedia - xsPlaylist
 * (c)2013 - 2020
 * Jason James Newland
 * KangaSoft Software, All Rights Reserved
 * Licenced under the GNU public licence */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using xsCore.Utils.Serialization;

namespace xsPlaylist.Playlist.Playlists
{
    [Serializable]
    public class XsplHeader
    {
        public XsplHeader()
        {
            Title = "Playlist";
        }

        [XmlElement("title")]
        public string Title { get; set; }
    }

    [Serializable]
    public class XsplMetaData
    {
        [XmlElement("mrl")]
        public string Mrl { get; set; }

        [XmlElement("length")]
        public int Length { get; set; }

        [XmlElement("options")]
        public List<string> Options = new List<string>();

        [XmlElement("tags")]
        public XsplTags Tags = new XsplTags();
    }

    [Serializable]
    public class XsplTags
    {
        [XmlElement("title")]
        public string Title { get; set; }

        [XmlElement("artist")]
        public string Artist { get; set; }

        [XmlElement("album")]
        public string Album { get; set; }
    }

    [Serializable, XmlRoot("playlist")]
    public class XsplRoot
    {
        [XmlElement("head")]
        public XsplHeader Header = new XsplHeader();

        [XmlElement("media")]
        public List<XsplMetaData> Media = new List<XsplMetaData>();
    }

    public class XsplPlaylist : IPlaylist
    {
        private List<PlaylistEntry> _list = new List<PlaylistEntry>();

        /* Constructors */
        public XsplPlaylist()
        {
            /* Default empty constructor */
        }

        public XsplPlaylist(IPlaylist playlist)
        {
            /* Copy playlist */
            if (playlist.Count == 0) { return; }
            PlaylistTitle = playlist.PlaylistTitle;
            foreach (var entry in playlist)
            {
                Add(entry);
            }
        }

        public PlaylistType Type
        {
            get { return PlaylistType.Default; }
        }

        public string PlaylistTitle { get; set; }

        public bool Read(string fileName)
        {
            var xspl = new XsplRoot();
            var success = XmlSerialize<XsplRoot>.Load(fileName, ref xspl);
            _list = new List<PlaylistEntry>();
            if (success)
            {
                PlaylistTitle = xspl.Header.Title;
                foreach (var meta in xspl.Media)
                {
                    var entry = new PlaylistEntry
                                    {
                                        Location = meta.Mrl,
                                        Length = meta.Length * 1000,
                                        Title = meta.Tags.Title,
                                        Artist = meta.Tags.Artist,
                                        Album = meta.Tags.Album,
                                        Options = meta.Options
                                    };
                    Add(entry);
                }
            }
            return success;
        }

        public bool Write(string fileName)
        {
            return Write(fileName, _list);
        }

        public bool Write(string fileName, IList<PlaylistEntry> entries)
        {
            if (entries.Count == 0) { return false; }
            var xspl = new XsplRoot
                           {
                               Header =
                                   {
                                       Title = PlaylistTitle
                                   }
                           };
            foreach (var entry in entries)
            {
                var meta = new XsplMetaData
                               {
                                   Mrl = entry.Location,
                                   Length = entry.Length / 1000,
                                   Tags =
                                       {
                                           Title = entry.Title,
                                           Artist = entry.Artist,
                                           Album = entry.Album
                                       },
                                   Options = entry.Options
                               };
                xspl.Media.Add(meta);
            }
            return XmlSerialize<XsplRoot>.Save(fileName, xspl);
        }

        public void Sort(PlaylistSortType sortType)
        {
            switch (sortType)
            {
                case PlaylistSortType.Title:
                    _list.Sort(new PlaylistEntry.CompareByTitle());
                    break;
                case PlaylistSortType.Artist:
                    _list.Sort(new PlaylistEntry.CompareByArtist());
                    break;
                case PlaylistSortType.Path:
                    _list.Sort(new PlaylistEntry.CompareByPath());
                    break;
            }
        }

        public void RemoveDuplicates()
        {
            _list = _list.Distinct(new PlaylistEntry.PlaylistDuplicateComparer()).ToList();
        }

        /* IList */
        public int IndexOf(PlaylistEntry item)
        {
            return _list.IndexOf(item);
        }

        public void Insert(int index, PlaylistEntry item)
        {
            if (index < 0 || index > _list.Count - 1)
            {
                return;
            }
            _list.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            if (index < 0 || index > _list.Count - 1)
            {
                return;
            }
            _list.RemoveAt(index);
        }

        public PlaylistEntry this[int index]
        {
            get { return index < 0 || index > _list.Count - 1 ? null : _list[index]; }
            set
            {
                if (index < 0 || index > _list.Count - 1)
                {
                    return;
                }
                _list[index] = value;
            }
        }

        public void Add(PlaylistEntry item)
        {
            _list.Add(item);
        }

        public void Clear()
        {
            _list.Clear();
        }

        public bool Contains(PlaylistEntry item)
        {
            return _list.Contains(item);
        }

        public void CopyTo(PlaylistEntry[] array, int arrayIndex)
        {
            _list.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return _list.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(PlaylistEntry item)
        {
            return _list.Remove(item);
        }

        /* IEnumerable */
        public IEnumerator<PlaylistEntry> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}

/* xsMedia - xsPlaylist
 * (c)2013 - 2024
 * Jason James Newland
 * KangaSoft Software, All Rights Reserved
 * Licenced under the GNU public licence */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Serialization;
using xsCore.Utils.Serialization;

namespace xsCore.Controls.Playlist.Playlist.Playlists
{
    /* Windows Media Playlist Format - relatively easy to serialize
     * (Note: The header of a WPL file is normally <?wpl version="1.0"?>, but WMP doesn't seem
     * to care if its the standard <?xml version="1.0"?> header) */
    [Serializable]
    public class WplHeader
    {
        public WplHeader() : this("Playlist", 0)
        {
            /* Empty default constructor */
        }

        public WplHeader(string title, int itemCount)
        {
            Meta.Add(new WplMetaData { Name = "Generator", Content = "Microsoft Windows Media Player -- 12.0.7600.16385" });
            Meta.Add(new WplMetaData { Name = "ItemCount", Content = itemCount.ToString(CultureInfo.InvariantCulture) });
            Title = title;
        }

        [XmlElement("meta")]
        public List<WplMetaData> Meta = new List<WplMetaData>();

        [XmlElement("title")]
        public string Title { get; set; }

        [XmlElement("author")]
        public string Author { get; set; }
    }

    [Serializable]
    public class WplMetaData
    {
        [XmlIgnore]
        public int Index { get; set; }

        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("content")]
        public string Content { get; set; }
    }

    [Serializable]
    public class WplBody
    {
        public class WplSequence
        {
            [XmlElement("media")]
            public List<WplMedia> Media = new List<WplMedia>();
        }

        [XmlElement("seq")]
        public WplSequence Sequence = new WplSequence();
    }

    [Serializable]
    public class WplMedia
    {
        [XmlAttribute("src")]
        public string Source { get; set; }

        [XmlAttribute("cid")]
        public string ContentId { get; set; }

        [XmlAttribute("tid")]
        public string TrackingId { get; set; }
    }

    [Serializable, XmlRoot(ElementName = "smil")]
    public class WplRoot
    {
        public WplRoot()
        {
            Header = new WplHeader();
        }

        public WplRoot(string title, int itemCount)
        {
            Header = new WplHeader(title, itemCount);
        }

        [XmlElement("head")]
        public WplHeader Header { get; set; }

        [XmlElement("body")]
        public WplBody Body = new WplBody();
    }


    public class WplPlaylist : IPlaylist
    {
        private List<PlaylistEntry> _list = new List<PlaylistEntry>();

        public WplPlaylist()
        {
            /* Empty constructor */
        }

        public WplPlaylist(IPlaylist playlist)
        {
            if (playlist.Count == 0) { return; }
            /* Copy playlist */
            PlaylistTitle = playlist.PlaylistTitle;
            foreach (var entry in playlist)
            {
                Add(entry);
            }
        }

        /* Interface methods */
        public PlaylistType Type
        {
            get { return PlaylistType.Wpl; }
        }

        public string PlaylistTitle { get; set; }

        public bool Read(string fileName)
        {
            var wpl = new WplRoot();
            var success = XmlSerialize<WplRoot>.Load(fileName, ref wpl);
            _list = new List<PlaylistEntry>();
            if (success)
            {
                foreach (var media in wpl.Body.Sequence.Media)
                {
                    var entry = new PlaylistEntry
                                    {
                                        Location = media.Source
                                    };
                    Add(entry);
                }
                PlaylistTitle = wpl.Header.Title;
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
            var wpl = new WplRoot(PlaylistTitle, entries.Count);
            foreach (var entry in entries)
            {
                var media = new WplMedia
                                {
                                    Source = entry.Location
                                };
                wpl.Body.Sequence.Media.Add(media);
            }
            return XmlSerialize<WplRoot>.Save(fileName, wpl,
                                              new XmlNamespace
                                                  {
                                                      Prefix = "",
                                                      Namespace = ""
                                                  });
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

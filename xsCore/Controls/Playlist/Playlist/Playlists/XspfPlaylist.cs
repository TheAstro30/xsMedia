/* xsMedia - xsPlaylist
 * (c)2013 - 2025
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
    /* XSPF ("spiff" - XML Sharable Playlist Format) is a little more difficult to serialize... */
    [Serializable]
    public class VlcTrackList
    {
        /* Internal <track /> class */
        public class Track
        {
            public Track()
            {
                Extension = new VlcExtension();
            }

            public Track(IEnumerable<string> options)
            {
                Extension = new VlcExtension();
                if (options != null)
                {
                    Extension.Options.AddRange(options.ToList());
                }
            }

            [XmlElement("location")]
            public string Location { get; set; }

            [XmlElement("title")]
            public string Title { get; set; }

            [XmlElement("creator")]
            public string Artist { get; set; }

            [XmlElement("album")]
            public string Album { get; set; }

            [XmlElement("image")]
            public string Image { get; set; }

            [XmlElement("annotation")]
            public string Annotation { get; set; }

            [XmlElement("trackNum")]
            public int TrackNum { get; set; }

            [XmlElement("duration")]
            public int Length { get; set; }

            [XmlElement("extension")]
            public VlcExtension Extension { get; set; }
        }

        [XmlElement("track")]
        public List<Track> Tracks = new List<Track>();
    }

    [Serializable]
    public class VlcTrackingId
    {
        [XmlAttribute("tid")]
        public string Id { get; set; }
    }

    [Serializable]
    public class VlcExtension
    {
        public VlcExtension()
        {
            Application = "http://www.videolan.org/vlc/playlist/0";
            Options = new List<string>();
        }

        [XmlAttribute("application")]
        public string Application { get; set; }

        [XmlElement(ElementName = "id", Namespace = "http://www.videolan.org/vlc/playlist/ns/0/")]
        public int Id { get; set; }

        [XmlElement(ElementName = "item", Namespace = "http://www.videolan.org/vlc/playlist/ns/0/")]
        public List<VlcTrackingId> TrackingIds = new List<VlcTrackingId>();

        [XmlElement("option", Namespace = "http://www.videolan.org/vlc/playlist/ns/0/")]
        public List<string> Options { get; set; }
    }

    [Serializable, XmlRoot(ElementName = "playlist", Namespace = "http://xspf.org/ns/0/")]
    public class XspfRoot
    {
        public XspfRoot()
            : this("Playlist")
        {
            /* Empty constructor */
        }

        public XspfRoot(string title)
        {
            Version = "1";
            Title = title;
            Extension = new VlcExtension();
        }

        [XmlAttribute("version")]
        public string Version { get; set; }

        [XmlElement("title")]
        public string Title { get; set; }

        [XmlElement("trackList")]
        public VlcTrackList TrackList = new VlcTrackList();

        [XmlElement("extension")]
        public VlcExtension Extension { get; set; }
    }

    public class XspfPlaylist : IPlaylist
    {
        private List<PlaylistEntry> _list = new List<PlaylistEntry>();

        public XspfPlaylist()
        {
            /* Empty default constructor */
        }

        public XspfPlaylist(IPlaylist playlist)
        {
            /* Copy playlist */
            if (playlist.Count == 0) { return; }
            PlaylistTitle = playlist.PlaylistTitle;
            foreach (var entry in playlist)
            {
                Add(entry);
            }
        }

        /* Interface methods */
        public PlaylistType Type
        {
            get { return PlaylistType.Xspf; }
        }

        public string PlaylistTitle { get; set; }

        public bool Read(string fileName)
        {
            var xspf = new XspfRoot();
            var success = XmlSerialize<XspfRoot>.Load(fileName, ref xspf);
            _list = new List<PlaylistEntry>();
            if (success)
            {
                foreach (var track in xspf.TrackList.Tracks)
                {
                    var entry = new PlaylistEntry
                                    {
                                        Location = new Uri(track.Location).LocalPath,
                                        Length = track.Length,
                                        Title = track.Title,
                                        Artist = track.Artist,
                                        Album = track.Album,
                                        Options = track.Extension.Options
                                    };
                    Add(entry);
                }
                PlaylistTitle = xspf.Title;
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
            var trackId = 0;
            var xspf = new XspfRoot
                           {
                               Title = PlaylistTitle
                           };

            foreach (var entry in entries)
            {
                var track = new VlcTrackList.Track
                                {
                                    Location = new Uri(entry.Location).AbsoluteUri,
                                    Length = entry.Length,
                                    Title = entry.Title,
                                    Artist = entry.Artist,
                                    Album = entry.Album,
                                    Extension =
                                        {
                                            Id = trackId,
                                            Options = entry.Options
                                        }
                                };
                xspf.TrackList.Tracks.Add(track);
                var id = new VlcTrackingId
                             {
                                 Id = trackId.ToString(CultureInfo.InvariantCulture)
                             };
                xspf.Extension.TrackingIds.Add(id);
                trackId++;
            }
            return XmlSerialize<XspfRoot>.Save(fileName, xspf,
                                               new XmlNamespace
                                                   {
                                                       Prefix = "vlc",
                                                       Namespace =
                                                           "http://www.videolan.org/vlc/playlist/ns/0/"
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
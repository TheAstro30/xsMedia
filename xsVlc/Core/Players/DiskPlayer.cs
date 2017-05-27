//    nVLC
//    
//    Author:  Roman Ginzburg
//
//    nVLC is free software: you can redistribute it and/or modify
//    it under the terms of the GNU General Public License as published by
//    the Free Software Foundation, either version 3 of the License, or
//    (at your option) any later version.
//
//    nVLC is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
//    GNU General Public License for more details.
//     
// ========================================================================

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using xsVlc.Common;
using xsVlc.Common.Players;
using xsVlc.Interop;

namespace xsVlc.Core.Players
{
    internal class DiskPlayer : VideoPlayer, IDiskPlayer
    {
        public DiskPlayer(IntPtr hMediaLib) : base(hMediaLib)
        {

        }

        public int AudioTrack
        {
            get
            {
                return Api.libvlc_audio_get_track(HMediaPlayer);
            }
            set
            {                
                Api.libvlc_audio_set_track(HMediaPlayer, value);
            }
        }

        public int AudioTrackCount
        {
            get
            {
                return Api.libvlc_audio_get_track_count(HMediaPlayer);
            }
        }

        public List<LibvlcTrackDescriptionT> AudioTrackDescription
        {
            get
            {
                var ip = Api.libvlc_audio_get_track_description(HMediaPlayer);
                return InternalTrackDescription(ip);
            }
        }

        public int SubTitle
        {
            get
            {
                return Api.libvlc_video_get_spu(HMediaPlayer);
            }
            set
            {
                Api.libvlc_video_set_spu(HMediaPlayer, value);                
            }
        }

        public int SubTitleCount
        {
            get
            {
                return Api.libvlc_video_get_spu_count(HMediaPlayer);
            }
        }

        public List<LibvlcTrackDescriptionT> SubTitleDescription
        {
            get
            {
                var ip = Api.libvlc_video_get_spu_description(HMediaPlayer);
                return InternalTrackDescription(ip);
            }
        }

        public void NextChapter()
        {
            Api.libvlc_media_player_next_chapter(HMediaPlayer);
        }

        public void PreviousChapter()
        {
            Api.libvlc_media_player_previous_chapter(HMediaPlayer);
        }

        public int Title
        {
            get
            {
                return Api.libvlc_media_player_get_title(HMediaPlayer);
            }
            set
            {
                Api.libvlc_media_player_set_title(HMediaPlayer, value);
            }
        }

        public int TitleCount
        {
            get
            {
                return Api.libvlc_media_player_get_title_count(HMediaPlayer);
            }
        }

        public int GetChapterCountForTitle(int title)
        {
            return Api.libvlc_media_player_get_chapter_count_for_title(HMediaPlayer, Title);
        }

        public int ChapterCount
        {
            get
            {
                return Api.libvlc_media_player_get_chapter_count(HMediaPlayer);
            }
        }

        public int Chapter
        {
            get
            {
                return Api.libvlc_media_player_get_chapter(HMediaPlayer);
            }
            set
            {
                Api.libvlc_media_player_set_chapter(HMediaPlayer, value);
            }
        }

        public int VideoTrack
        {
            get
            {
                return Api.libvlc_video_get_track(HMediaPlayer);
            }
            set
            {
                Api.libvlc_video_set_track(HMediaPlayer, value);                
            }
        }

        public int VideoTrackCount
        {
            get
            {
                return Api.libvlc_video_get_track_count(HMediaPlayer);
            }
        }

        public List<LibvlcTrackDescriptionT> VideoTrackDescription
        {
            get
            {
                var ip = Api.libvlc_video_get_track_description(HMediaPlayer);
                return InternalTrackDescription(ip);
            }
        }

        public void Navigate(NavigationMode mode)
        {
            Api.libvlc_media_player_navigate(HMediaPlayer, (LibvlcNavigateModeT)mode);
        }

        private static List<LibvlcTrackDescriptionT> InternalTrackDescription(IntPtr ip)
        {
            var tracks = new List<LibvlcTrackDescriptionT>();
            while (ip != IntPtr.Zero)
            {
                var trackDescription = (LibvlcTrackDescriptionT)Marshal.PtrToStructure(ip, typeof(LibvlcTrackDescriptionT));
                tracks.Add(trackDescription);
                ip = trackDescription.p_next;
            }
            return tracks;
        }
    }
}

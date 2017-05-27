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

using System.Drawing;
using System.Text;
using xsVlc.Common;
using xsVlc.Common.Structures;
using xsVlc.Interop;

namespace xsVlc.Core
{
    internal static class Extensions
    {
        public static byte[] ToUtf8(this string str)
        {
            return string.IsNullOrEmpty(str) ? null : Encoding.UTF8.GetBytes(str);
        }

        public static string ToCropFilterString(this Rectangle rect)
        {            
            return string.Format("{0}x{1}+{2}+{3}", rect.Width, rect.Height, rect.X, rect.Y);
        }

        public static Rectangle ToRectangle(this string str)
        {
            var items = str.Split(new[] { 'x', '+' }, 4);
            return new Rectangle(int.Parse(items[3]), int.Parse(items[2]), int.Parse(items[1]), int.Parse(items[0]));
        }

        public static MediaStatistics ToMediaStatistics(this LibvlcMediaStatsT stats)
        {
            var ms = new MediaStatistics
                         {
                             DecodedAudio = stats.i_decoded_audio,
                             DecodedVideo = stats.i_decoded_video,
                             DemuxBitrate = stats.f_demux_bitrate,
                             DemuxCorrupted = stats.i_demux_corrupted,
                             DemuxDiscontinuity = stats.i_demux_discontinuity,
                             DemuxReadBytes = stats.i_demux_read_bytes,
                             DisplayedPictures = stats.i_displayed_pictures,
                             InputBitrate = stats.f_input_bitrate,
                             LostAbuffers = stats.i_lost_abuffers,
                             LostPictures = stats.i_lost_pictures,
                             PlayedAbuffers = stats.i_played_abuffers,
                             ReadBytes = stats.i_read_bytes,
                             SendBitrate = stats.f_send_bitrate,
                             SentBytes = stats.i_sent_bytes,
                             SentPackets = stats.i_sent_packets
                         };
            return ms;
        }

        public static MediaTrackInfo ToMediaInfo(this LibvlcMediaTrackInfoT tInfo)
        {
            var mti = new MediaTrackInfo
                          {
                              Channels = tInfo.audio_video.audio.i_channels,
                              Codec = tInfo.i_codec,
                              Height = tInfo.audio_video.video.i_width,
                              Id = tInfo.i_id,
                              TrackType = (TrackType)(int)tInfo.i_type,
                              Level = tInfo.i_level,
                              Profile = tInfo.i_profile,
                              Rate = tInfo.audio_video.audio.i_rate,
                              Width = tInfo.audio_video.video.i_width
                          };
            return mti;
        }
    }
}

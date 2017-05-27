using System;

namespace xsVlc.Common.Structures
{
    [Serializable]
    public class MediaTrackInfo
    {
        public UInt32 Codec;
        public int Id;
        public TrackType TrackType;
        public int Profile;
        public int Level;
        public int Channels;
        public int Rate;
        public int Height;
        public int Width;
    }
}

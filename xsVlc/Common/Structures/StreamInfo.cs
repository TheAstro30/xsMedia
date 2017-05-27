using System;

namespace xsVlc.Common.Structures
{
    [Serializable]
    public class StreamInfo
    {
        public StreamInfo()
        {
            Id = 1;
            Group = 1;
        }

        public StreamCategory Category { get; set; }
        public int Id { get; set; }
        public int Group { get; set; }
        public int Size { get; set; }
        public int Fps { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public Enum Codec { get; set; }
        public AspectRatioMode AspectRatio { get; set; }
        public int Samplerate { get; set; }
        public int Channels { get; set; }
    }
}

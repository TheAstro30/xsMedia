using System;

namespace xsVlc.Common.Structures
{
    [Serializable]
    public struct Sound
    {
        public IntPtr SamplesData { get; set; }
        public uint SamplesSize { get; set; }
        public long Pts { get; set; }
    }
}

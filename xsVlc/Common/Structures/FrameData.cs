using System;

namespace xsVlc.Common.Structures
{
    public struct FrameData
    {
        public IntPtr Data { get; set; }
        public int DataSize { get; set; }
        public long Dts { get; set; }
        public long Pts { get; set; }
    }
}

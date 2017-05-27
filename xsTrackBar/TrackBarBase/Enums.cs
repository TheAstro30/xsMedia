using System;

namespace xsTrackBar.TrackBarBase
{
    public enum TrackBarItemState
    {
        Active = 3,
        Disabled = 5,
        Hot = 2,
        Normal = 1
    }

    [Flags]
    public enum TrackBarOwnerDrawParts
    {
        Channel = 4,
        None = 0,
        Thumb = 2,
        Ticks = 1
    }
}

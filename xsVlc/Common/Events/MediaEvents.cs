using System;
using xsVlc.Common.Media;

namespace xsVlc.Common.Events
{
    public class MediaMetaDataChange : EventArgs
    {
        public MediaMetaDataChange(MetaDataType type)
        {
            MetaType = type;
        }

        public MetaDataType MetaType { get; private set; }
    }

    public class MediaNewSubItem : EventArgs
    {
        public MediaNewSubItem(IMedia subItem)
        {
            SubItem = subItem;
        }

        public IMedia SubItem { get; private set; }
    }

    public class MediaDurationChange : EventArgs
    {
        public MediaDurationChange(long newDuration)
        {
            NewDuration = newDuration;
        }

        public long NewDuration { get; private set; }
    }

    public class MediaParseChange : EventArgs
    {
        public MediaParseChange(bool parsed)
        {
            Parsed = parsed;
        }

        public bool Parsed { get; private set; }
    }

    public class MediaFree : EventArgs
    {
        public MediaFree(IntPtr hMedia)
        {
            Media = hMedia;
        }

        public IntPtr Media { get; private set; }
    }

    public class MediaStateChange : EventArgs
    {
        public MediaStateChange(MediaState newState)
        {
            NewState = newState;
        }

        public MediaState NewState { get; private set; }
    }
}

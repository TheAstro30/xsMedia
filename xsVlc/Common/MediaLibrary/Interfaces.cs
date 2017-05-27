using xsVlc.Common.Media;

namespace xsVlc.Common.MediaLibrary
{
    public interface IMediaLibrary
    {
        void Load();
        IMediaList MediaList { get; }
    }
}

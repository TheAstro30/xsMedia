using System;
using System.Collections.Generic;
using System.Drawing;
using xsVlc.Common.Events;
using xsVlc.Common.Structures;

namespace xsVlc.Common.Media
{
    public interface IMedia : IDisposable, IEqualityComparer<IMedia>
    {
        string Input { get; set; }
        void AddOptions(IEnumerable<string> options);
        MediaState State { get; }
        void AddOptionFlag(string option, int flag);
        IMedia Duplicate();
        void Parse(bool aSync);
        bool IsParsed { get; }
        IntPtr Tag { get; set; }
        IMediaEvents Events { get; }
        MediaStatistics Statistics { get; }
        IMediaList SubItems { get; }
    }

    public interface IMediaFromFile : IMedia
    {
        string GetMetaData(MetaDataType dataType);
        void SetMetaData(MetaDataType dataType, string argument);
        void SaveMetaData();
        long Duration { get; }
        MediaTrackInfo[] TracksInfo { get; }
    }

    public interface IMediaList : IList<IMedia>, IDisposable
    {
        IMediaListEvents Events { get; }
    }

    public interface IScreenCaptureMedia : IMedia
    {
        Rectangle CaptureArea { get; set; }
        int Fps { get; set; }
        bool FollowMouse { get; set; }
        string CursorFile { get; set; }
    }

    public interface IVideoInputMedia : IMedia
    {
        void AddFrame(Bitmap frame);
        void SetFormat(BitmapFormat format);
    }
}

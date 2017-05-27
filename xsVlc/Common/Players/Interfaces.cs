using System;
using System.Collections.Generic;
using System.Drawing;
using xsVlc.Common.Events;
using xsVlc.Common.Filters;
using xsVlc.Common.Media;
using xsVlc.Common.Rendering;
using xsVlc.Common.Structures;
using xsVlc.Interop;

namespace xsVlc.Common.Players
{
    public interface IPlayer : IDisposable, IEqualityComparer<IPlayer>
    {
        void Open(IMedia media);
        void Play();
        void Pause();
        void Stop();
        long Time { get; set; }
        float Position { get; set; }
        long Length { get; }
        IEventBroker Events { get; }
        bool IsPlaying { get; }
    }

    public interface IAudioPlayer : IPlayer
    {
        int Volume { get; set; }
        bool Mute { get; set; }
        long Delay { get; set; }
        AudioChannelType Channel { get; set; }
        AudioOutputDeviceType DeviceType { get; set; }
        void ToggleMute();
        IAudioRenderer CustomAudioRenderer { get; }

        void SetAudioOutputModuleAndDevice(AudioOutputModuleInfo module, AudioOutputDeviceInfo device);
    }

    public interface IDiskPlayer : IVideoPlayer
    {
        int ChapterCount { get; }
        int Chapter { get; set; }
        int Title { get; set; }
        int TitleCount { get; }
        int GetChapterCountForTitle(int title);
        void NextChapter();
        void PreviousChapter();

        int AudioTrack { get; set; }
        int AudioTrackCount { get; }
        List<LibvlcTrackDescriptionT> AudioTrackDescription { get; }

        int SubTitle { get; set; }
        int SubTitleCount { get; }
        List<LibvlcTrackDescriptionT> SubTitleDescription { get; }
        
        int VideoTrack { get; set; }
        int VideoTrackCount { get; }
        List<LibvlcTrackDescriptionT> VideoTrackDescription { get; }

        void Navigate(NavigationMode mode);
    }

    public interface IMediaListPlayer : IPlayer
    {
        void PlayNext();
        void PlayPrevious();
        PlaybackMode PlaybackMode { get; set; }
        void PlayItemAt(int index);
        MediaState PlayerState { get; }
        IntPtr Pointer { get; }
        IDiskPlayer InnerPlayer { get; }
        IMediaListPlayerEvents MediaListPlayerEvents { get; }
    }

    public interface IVideoPlayer : IAudioPlayer
    {
        IntPtr WindowHandle { get; set; }
        void TakeSnapShot(uint stream, string path);
        float PlaybackRate { get; set; }
        float Fps { get; }
        void NextFrame();
        Size GetVideoSize(uint stream);
        Point GetCursorPosition(uint stream);
        bool KeyInputEnabled { get; set; }
        bool MouseInputEnabled { get; set; }
        float VideoScale { get; set; }
        AspectRatioMode AspectRatio { get; set; }
        void SetSubtitleFile(string path);
        int Teletext { get; set; }
        void ToggleTeletext();
        bool PlayerWillPlay { get; }
        int VoutCount { get; }
        bool IsSeekable { get; }
        bool CanPause { get; }
        IAdjustFilter Adjust { get; }
        ICropFilter CropGeometry { get; }
        IMemoryRenderer CustomRenderer { get; }
        IMemoryRendererEx CustomRendererEx { get; }
        ILogoFilter Logo { get; }
        IMarqueeFilter Marquee { get; }
        IDeinterlaceFilter Deinterlace { get; }
    }
}
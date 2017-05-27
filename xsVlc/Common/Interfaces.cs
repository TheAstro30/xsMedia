using System;
using System.Collections.Generic;
using xsVlc.Common.Discovery;
using xsVlc.Common.Media;
using xsVlc.Common.MediaLibrary;
using xsVlc.Common.Players;
using xsVlc.Common.Structures;
using xsVlc.Common.Vlm;

namespace xsVlc.Common
{
    public interface IMediaPlayerFactory : IDisposable
    {
        T CreatePlayer<T>() where T : IPlayer;
        T CreateMedia<T>(string input, params string[] options) where T : IMedia;
        T CreateMediaList<T>(IEnumerable<string> mediaItems, params string[] options) where T : IMediaList;
        T CreateMediaList<T>() where T : IMediaList;
        T CreateMediaListPlayer<T>(IMediaList mediaList) where T : IMediaListPlayer;

        IMediaDiscoverer CreateMediaDiscoverer(string name);
        IMediaLibrary CreateMediaLibrary();
        string Version { get; }
        IEnumerable<FilterInfo> AudioFilters { get; }
        IEnumerable<FilterInfo> VideoFilters { get; }
        IVideoLanManager VideoLanManager { get; }
        IEnumerable<AudioOutputModuleInfo> AudioOutputModules { get; }
        IEnumerable<AudioOutputDeviceInfo> GetAudioOutputDevices(AudioOutputModuleInfo audioOutputModule);
        long Clock { get; }
        long Delay(long pts);
    }
}
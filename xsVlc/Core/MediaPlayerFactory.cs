//    nVLC
//    
//    Author:  Roman Ginzburg
//
//    nVLC is free software: you can redistribute it and/or modify
//    it under the terms of the GNU General Public License as published by
//    the Free Software Foundation, either version 3 of the License, or
//    (at your option) any later version.
//
//    nVLC is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
//    GNU General Public License for more details.
//     
// ========================================================================
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using xsVlc.Common;
using xsVlc.Common.Discovery;
using xsVlc.Common.Internal;
using xsVlc.Common.Media;
using xsVlc.Common.MediaLibrary;
using xsVlc.Common.Players;
using xsVlc.Common.Structures;
using xsVlc.Common.Vlm;
using xsVlc.Core.Discovery;
using xsVlc.Core.Exceptions;
using xsVlc.Core.Media;
using xsVlc.Core.MediaLibrary;
using xsVlc.Core.Players;
using xsVlc.Core.Vlm;
using xsVlc.Interop;

namespace xsVlc.Core
{
    public class MediaPlayerFactory : DisposableBase, IMediaPlayerFactory, IReferenceCount, INativePointer
    {
        private IntPtr _mediaLib = IntPtr.Zero;
        private IVideoLanManager _vlm;

        public MediaPlayerFactory()
        {
            var args = new[]
                           {
                               "-I",
                               "dummy",
                               "--ignore-config",
                               "--no-osd",
                               "--disable-screensaver",
                               //"--ffmpeg-hw",
                               "--sub-filter=marq:logo"
                               //"--plugin-path=./plugins"
                           };
            Initialize(args);
        }

        public MediaPlayerFactory(string[] args)
        {
            Initialize(args);
        }

        private void Initialize(string[] args)
        {            
            try
            {
                _mediaLib = Api.libvlc_new(args.Length, args);
            }
            catch (DllNotFoundException ex)
            {
                throw new LibVlcNotFoundException(ex);
            }
            if (_mediaLib == IntPtr.Zero)
            {
                throw new LibVlcInitException();
            }
        }

        public T CreatePlayer<T>() where T : IPlayer
        {
            return ObjectFactory.Build<T>(_mediaLib);
        }

        public T CreateMediaListPlayer<T>(IMediaList mediaList) where T : IMediaListPlayer
        {
            return ObjectFactory.Build<T>(_mediaLib, mediaList);
        }

        public T CreateMedia<T>(string input, params string[] options) where T : IMedia
        {
            var media = ObjectFactory.Build<T>(_mediaLib);
            media.Input = input;
            media.AddOptions(options);
            return media;
        }

        public T CreateMediaList<T>(IEnumerable<string> mediaItems, params string[] options) where T : IMediaList
        {
            var mediaList = ObjectFactory.Build<T>(_mediaLib);
            foreach (var file in mediaItems)
            {
                mediaList.Add(CreateMedia<IMedia>(file, options));
            }

            return mediaList;
        }

        public T CreateMediaList<T>() where T : IMediaList
        {
            return ObjectFactory.Build<T>(_mediaLib);
        }

        public IMediaDiscoverer CreateMediaDiscoverer(string name)
        {
            return ObjectFactory.Build<IMediaDiscoverer>(_mediaLib, name);
        }

        public IMediaLibrary CreateMediaLibrary()
        {
            return ObjectFactory.Build<IMediaLibrary>(_mediaLib);
        }

        public string Version
        {
            get
            {
                var pStr = Api.libvlc_get_version();
                return Marshal.PtrToStringAnsi(pStr);
            }
        }

        protected override void Dispose(bool disposing)
        {
            Release();
        }

        private static class ObjectFactory
        {
            private static readonly Dictionary<Type, Type> ObjectMap = new Dictionary<Type, Type>();

            static ObjectFactory()
            {
                ObjectMap.Add(typeof(IMedia), typeof(BasicMedia));
                ObjectMap.Add(typeof(IMediaFromFile), typeof(MediaFromFile));
                ObjectMap.Add(typeof(IVideoInputMedia), typeof(VideoInputMedia));
                ObjectMap.Add(typeof(IScreenCaptureMedia), typeof(ScreenCaptureMedia));
                ObjectMap.Add(typeof(IPlayer), typeof(BasicPlayer));
                ObjectMap.Add(typeof(IAudioPlayer), typeof(AudioPlayer));
                ObjectMap.Add(typeof(IVideoPlayer), typeof(VideoPlayer));
                ObjectMap.Add(typeof(IDiskPlayer), typeof(DiskPlayer));
                ObjectMap.Add(typeof(IMediaList), typeof(MediaList));
                ObjectMap.Add(typeof(IMediaListPlayer), typeof(MediaListPlayer));
                ObjectMap.Add(typeof(IVideoLanManager), typeof(VideoLanManager));
                ObjectMap.Add(typeof(IMediaDiscoverer), typeof(MediaDiscoverer));
                ObjectMap.Add(typeof(IMediaLibrary), typeof(MediaLibraryImpl));
                //ObjectMap.Add(typeof(IMemoryInputMedia), typeof(MemoryInputMedia));
            }

            public static T Build<T>(params object[] args)
            {
                if (ObjectMap.ContainsKey(typeof(T)))
                {
                    return (T)Activator.CreateInstance(ObjectMap[typeof(T)], args);
                }

                throw new ArgumentException("Unregistered type", typeof(T).FullName);
            }
        }

        public void AddRef()
        {
            Api.libvlc_retain(_mediaLib);
        }

        public void Release()
        {
            try
            {
                Api.libvlc_release(_mediaLib);
            }
            catch (AccessViolationException)
            {
                /* Do nothing */
            }
        }

        public IntPtr Pointer
        {
            get
            {
                return _mediaLib;
            }
        }

        public long Clock
        {
            get
            {
                return Api.libvlc_clock();
            }
        }

        public long Delay(long pts)
        {
            return Api.libvlc_delay(pts);
        }

        public IEnumerable<FilterInfo> AudioFilters
        {
            get
            {
                var pList = Api.libvlc_audio_filter_list_get(_mediaLib);
                var item = (LibvlcModuleDescriptionT)Marshal.PtrToStructure(pList, typeof(LibvlcModuleDescriptionT));
                do
                {
                    yield return GetFilterInfo(item);
                    if (item.p_next != IntPtr.Zero)
                    {
                        item = (LibvlcModuleDescriptionT)Marshal.PtrToStructure(item.p_next, typeof(LibvlcModuleDescriptionT));
                    }
                    else
                    {
                        break;
                    }

                }
                while (true);

                Api.libvlc_module_description_list_release(pList);
            }
        }

        public IEnumerable<FilterInfo> VideoFilters
        {
            get
            {
                var pList = Api.libvlc_video_filter_list_get(_mediaLib);
                if (pList == IntPtr.Zero)
                {
                    yield break;
                }

                var item = (LibvlcModuleDescriptionT)Marshal.PtrToStructure(pList, typeof(LibvlcModuleDescriptionT));

                do
                {
                    yield return GetFilterInfo(item);
                    if (item.p_next != IntPtr.Zero)
                    {
                        item = (LibvlcModuleDescriptionT)Marshal.PtrToStructure(item.p_next, typeof(LibvlcModuleDescriptionT));
                    }
                    else
                    {
                        break;
                    }
                }
                while (true);

                Api.libvlc_module_description_list_release(pList);
            }
        }

        private static FilterInfo GetFilterInfo(LibvlcModuleDescriptionT item)
        {
            return new FilterInfo
                       {
                           Help = Marshal.PtrToStringAnsi(item.psz_help),
                           Longname = Marshal.PtrToStringAnsi(item.psz_longname),
                           Name = Marshal.PtrToStringAnsi(item.psz_name),
                           Shortname = Marshal.PtrToStringAnsi(item.psz_shortname)
                       };
        }

        public IVideoLanManager VideoLanManager
        {
            get { return _vlm ?? (_vlm = ObjectFactory.Build<IVideoLanManager>(_mediaLib)); }
        }

        public IEnumerable<AudioOutputModuleInfo> AudioOutputModules
        {
            get
            {
                var pList = Api.libvlc_audio_output_list_get(_mediaLib);
                var pDevice = (LibvlcAudioOutputT)Marshal.PtrToStructure(pList, typeof(LibvlcAudioOutputT));

                do
                {
                    var info = GetDeviceInfo(pDevice);

                    yield return info;
                    if (pDevice.p_next != IntPtr.Zero)
                    {
                        pDevice = (LibvlcAudioOutputT)Marshal.PtrToStructure(pDevice.p_next, typeof(LibvlcAudioOutputT));
                    }
                    else
                    {
                        break;
                    }
                }
                while (true);

                Api.libvlc_audio_output_list_release(pList);
            }
        }

        public IEnumerable<AudioOutputDeviceInfo> GetAudioOutputDevices(AudioOutputModuleInfo audioOutputModule)
        {            
            var pDevices = Api.libvlc_audio_output_device_list_get(_mediaLib, audioOutputModule.Name.ToUtf8());
            while (pDevices != IntPtr.Zero)
            {
                var d = new AudioOutputDeviceInfo();
                var device = (LibvlcAudioOutputDeviceT)Marshal.PtrToStructure(pDevices, typeof(LibvlcAudioOutputDeviceT));
                d.Longname = Marshal.PtrToStringAnsi(device.psz_description);
                d.Id = Marshal.PtrToStringAnsi(device.psz_device);
                pDevices = device.p_next;
                yield return d;
            }
        }

        private static AudioOutputModuleInfo GetDeviceInfo(LibvlcAudioOutputT pDevice)
        {
            return new AudioOutputModuleInfo
                       {
                           Name = Marshal.PtrToStringAnsi(pDevice.psz_name),
                           Description = Marshal.PtrToStringAnsi(pDevice.psz_description)
                       };
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using xsVlc.Common.Events;
using xsVlc.Common.Vlm;
using xsVlc.Core.Exceptions;

namespace xsVlc.Core.Vlm
{
    internal class VideoLanManager : DisposableBase, IEventProvider, IVideoLanManager
    {
        private readonly IntPtr _mediaLib;
        private readonly VlmEventManager _eventBroker;

        public IVlmEventManager Events
        {
            get
            {
                return _eventBroker;
            }
        }

        public VideoLanManager(IntPtr mediaLib)
        {
            _mediaLib = mediaLib;

            _eventBroker = new VlmEventManager(this);
        }

        public IntPtr EventManagerHandle
        {
            get
            {
                return Interop.Api.libvlc_vlm_get_event_manager(_mediaLib);
            }
        }

        protected override void Dispose(bool disposing)
        {
            try
            {
                Interop.Api.libvlc_vlm_release(_mediaLib);
            }
            catch (AccessViolationException)
            {
                /* No real way to stop this */
            }
        }

        public void AddBroadcast(string name, string input, string output, IEnumerable<string> options, bool bEnabled, bool bLoop)
        {
            var optionsNumber = 0;
            string[] optionsArray = null;

            if (options != null)
            {
                var enumerable = options as string[] ?? options.ToArray();
                optionsNumber = enumerable.Length;
                optionsArray = enumerable.ToArray();
            }

            if (Interop.Api.libvlc_vlm_add_broadcast(_mediaLib, name.ToUtf8(), input.ToUtf8(), output.ToUtf8(), optionsNumber, optionsArray, bEnabled ? 1 : 0, bLoop ? 1 : 0) != 0)
            {
                throw new LibVlcException();
            }
        }

        public void AddVod(string name, string input, IEnumerable<string> options, bool bEnabled, string mux)
        {
            int optionsNumber = 0;
            string[] optionsArray = null;

            if (options != null)
            {
                var enumerable = options as string[] ?? options.ToArray();
                optionsNumber = enumerable.Length;
                optionsArray = enumerable.ToArray();
            }
            if (Interop.Api.libvlc_vlm_add_vod(_mediaLib, name.ToUtf8(), input.ToUtf8(), optionsNumber, optionsArray, bEnabled ? 1 : 0, mux.ToUtf8()) != 0)
            {
                throw new LibVlcException();
            }
        }

        public void DeleteMedia(string name)
        {
            if (Interop.Api.libvlc_vlm_del_media(_mediaLib, name.ToUtf8()) != 0)
            {
                throw new LibVlcException();
            }
        }

        public void SetEnabled(string name, bool bEnabled)
        {
            if (Interop.Api.libvlc_vlm_set_enabled(_mediaLib, name.ToUtf8(), bEnabled ? 1 : 0) != 0)
            {
                throw new LibVlcException();
            }
        }

        public void SetInput(string name, string input)
        {
            if (Interop.Api.libvlc_vlm_set_input(_mediaLib, name.ToUtf8(), input.ToUtf8()) != 0)
            {
                throw new LibVlcException();
            }
        }

        public void SetOutput(string name, string output)
        {
            if (Interop.Api.libvlc_vlm_set_output(_mediaLib, name.ToUtf8(), output.ToUtf8()) != 0)
            {
                throw new LibVlcException();
            }
        }

        public void AddInput(string name, string input)
        {
            if (Interop.Api.libvlc_vlm_add_input(_mediaLib, name.ToUtf8(), input.ToUtf8()) != 0)
            {
                throw new LibVlcException();
            }
        }

        public void SetLoop(string name, bool bLoop)
        {
            if (Interop.Api.libvlc_vlm_set_loop(_mediaLib, name.ToUtf8(), bLoop ? 1 : 0) != 0)
            {
                throw new LibVlcException();
            }
        }

        public void SetMux(string name, string mux)
        {
            if (Interop.Api.libvlc_vlm_set_mux(_mediaLib, name.ToUtf8(), mux.ToUtf8()) != 0)
            {
                throw new LibVlcException();
            }
        }

        public void ChangeMedia(string name, string input, string output, IEnumerable<string> options, bool bEnabled, bool bLoop)
        {
            int optionsNumber = 0;
            string[] optionsArray = null;

            if (options != null)
            {
                var enumerable = options as string[] ?? options.ToArray();
                optionsNumber = enumerable.Length;
                optionsArray = enumerable.ToArray();
            }

            if (Interop.Api.libvlc_vlm_change_media(_mediaLib, name.ToUtf8(), input.ToUtf8(), output.ToUtf8(), optionsNumber, optionsArray, bEnabled ? 1 : 0, bLoop ? 1 : 0) != 0)
            {
                throw new LibVlcException();
            }
        }

        public void Play(string name)
        {
            Interop.Api.libvlc_vlm_play_media(_mediaLib, name.ToUtf8());
        }

        public void Stop(string name)
        {
            Interop.Api.libvlc_vlm_stop_media(_mediaLib, name.ToUtf8());
        }

        public void Pause(string name)
        {
            Interop.Api.libvlc_vlm_pause_media(_mediaLib, name.ToUtf8());
        }

        public void Seek(string name, float percentage)
        {
            Interop.Api.libvlc_vlm_seek_media(_mediaLib, name.ToUtf8(), percentage);
        }

        public float GetMediaPosition(string name)
        {
            return Interop.Api.libvlc_vlm_get_media_instance_position(_mediaLib, name.ToUtf8(), 0);
        }

        public int GetMediaTime(string name)
        {
            return Interop.Api.libvlc_vlm_get_media_instance_time(_mediaLib, name.ToUtf8(), 0);
        }

        public int GetMediaLength(string name)
        {
            return Interop.Api.libvlc_vlm_get_media_instance_length(_mediaLib, name.ToUtf8(), 0);
        }

        public int GetMediaRate(string name)
        {
            return Interop.Api.libvlc_vlm_get_media_instance_rate(_mediaLib, name.ToUtf8(), 0);
        }

        public int GetMediaTitle(string name)
        {
            return Interop.Api.libvlc_vlm_get_media_instance_title(_mediaLib, name.ToUtf8(), 0);
        }

        public int GetMediaChapter(string name)
        {
            return Interop.Api.libvlc_vlm_get_media_instance_chapter(_mediaLib, name.ToUtf8(), 0);
        }

        public bool IsMediaSeekable(string name)
        {
            return Interop.Api.libvlc_vlm_get_media_instance_seekable(_mediaLib, name.ToUtf8(), 0) == 1;
        }
    }
}

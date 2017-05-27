using System;
using System.Collections.Generic;

namespace xsVlc.Common.Vlm
{
    public interface IVideoLanManager : IDisposable
    {
        void AddBroadcast(string name, string input, string output, IEnumerable<string> options = null, bool bEnabled = true, bool bLoop = false);
        void AddInput(string name, string input);
        void AddVod(string name, string input, IEnumerable<string> options = null, bool bEnabled = true, string mux = null);
        void ChangeMedia(string name, string input, string output, IEnumerable<string> options, bool bEnabled, bool bLoop);
        void DeleteMedia(string name);
        IVlmEventManager Events { get; }
        int GetMediaLength(string name);
        float GetMediaPosition(string name);
        int GetMediaRate(string name);
        int GetMediaTime(string name);
        int GetMediaTitle(string name);
        int GetMediaChapter(string name);
        bool IsMediaSeekable(string name);
        void Pause(string name);
        void Play(string name);
        void Seek(string name, float percentage);
        void SetEnabled(string name, bool bEnabled);
        void SetInput(string name, string input);
        void SetLoop(string name, bool bLoop);
        void SetMux(string name, string mux);
        void SetOutput(string name, string output);
        void Stop(string name);
    }

    public interface IVlmEventManager
    {
        event EventHandler<VlmEvent> MediaAdded;
        event EventHandler<VlmEvent> MediaChanged;
        event EventHandler<VlmEvent> MediaInstanceEnd;
        event EventHandler<VlmEvent> MediaInstanceError;
        event EventHandler<VlmEvent> MediaInstanceInit;
        event EventHandler<VlmEvent> MediaInstanceOpening;
        event EventHandler<VlmEvent> MediaInstancePause;
        event EventHandler<VlmEvent> MediaInstancePlaying;
        event EventHandler<VlmEvent> MediaInstanceStarted;
        event EventHandler<VlmEvent> MediaInstanceStopped;
        event EventHandler<VlmEvent> MediaRemoved;
    }
}

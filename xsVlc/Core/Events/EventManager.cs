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
using xsVlc.Common.Events;
using xsVlc.Interop;

namespace xsVlc.Core.Events
{
    internal abstract class EventManager
    {
        public IEventProvider EventProvider;
        public readonly List<VlcEventHandlerDelegate> Callbacks = new List<VlcEventHandlerDelegate>();
        private readonly IntPtr _callback;

        protected EventManager(IEventProvider eventProvider)
        {
            EventProvider = eventProvider;

            VlcEventHandlerDelegate callback1 = MediaPlayerEventOccured;

            _callback = Marshal.GetFunctionPointerForDelegate(callback1);
            Callbacks.Add(callback1);

            GC.KeepAlive(callback1);
        }

        protected void Attach(LibvlcEventE eType)
        {
            if (Api.libvlc_event_attach(EventProvider.EventManagerHandle, eType, _callback, IntPtr.Zero) != 0)
            {
                throw new OutOfMemoryException("Failed to subscribe to event notification");
            }
        }

        protected void Dettach(LibvlcEventE eType)
        {
            Api.libvlc_event_detach(EventProvider.EventManagerHandle, eType, _callback, IntPtr.Zero);
        }

        protected abstract void MediaPlayerEventOccured(ref LibvlcEventT libvlcEvent, IntPtr userData);
    }
}

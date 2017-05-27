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
using xsVlc.Common.Events;
using xsVlc.Interop;

namespace xsVlc.Core.Events
{
    class MediaDiscoveryEventManager : EventManager, IMediaDiscoveryEvents
    {
        public MediaDiscoveryEventManager(IEventProvider eventProvider)
            : base(eventProvider)
        {

        }

        protected override void MediaPlayerEventOccured(ref LibvlcEventT libvlcEvent, IntPtr userData)
        {
            switch (libvlcEvent.type)
            {
                case LibvlcEventE.LibvlcMediaDiscovererStarted:

                    break;

                case LibvlcEventE.LibvlcMediaDiscovererEnded:

                    break;
            }
        }

        private event EventHandler EventMediaDiscoveryStarted;
        public event EventHandler MediaDiscoveryStarted
        {
            add
            {
                if (EventMediaDiscoveryStarted == null)
                {
                    Attach(LibvlcEventE.LibvlcMediaDiscovererStarted);
                }
                EventMediaDiscoveryStarted += value;
            }
            remove
            {
                if (EventMediaDiscoveryStarted != null)
                {
                    EventMediaDiscoveryStarted -= value;
                    if (EventMediaDiscoveryStarted == null)
                    {
                        Dettach(LibvlcEventE.LibvlcMediaDiscovererStarted);
                    }
                }
            }
        }

        private event EventHandler EventMediaDiscoveryEnded;
        public event EventHandler MediaDiscoveryEnded
        {
            add
            {
                if (EventMediaDiscoveryEnded == null)
                {
                    Attach(LibvlcEventE.LibvlcMediaDiscovererEnded);
                }
                EventMediaDiscoveryEnded += value;
            }
            remove
            {
                if (EventMediaDiscoveryEnded != null)
                {
                    EventMediaDiscoveryEnded -= value;
                    if (EventMediaDiscoveryEnded == null)
                    {
                        Dettach(LibvlcEventE.LibvlcMediaDiscovererEnded);
                    }
                }
            }
        }
    }
}

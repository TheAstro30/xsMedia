using System;
using System.Runtime.InteropServices;
using xsVlc.Common.Events;
using xsVlc.Common.Vlm;
using xsVlc.Core.Events;
using xsVlc.Interop;

namespace xsVlc.Core.Vlm
{
    internal class VlmEventManager : EventManager, IVlmEventManager
    {
        public VlmEventManager(IEventProvider eventProvider) : base(eventProvider)
        {

        }

        protected override void MediaPlayerEventOccured(ref LibvlcEventT libvlcEvent, IntPtr userData)
        {
            switch (libvlcEvent.type)
            {

                case LibvlcEventE.LibvlcVlmMediaAdded:
                    if (EventMediaAdded != null)
                    {
                        EventMediaAdded(EventProvider, new VlmEvent(Marshal.PtrToStringAuto(libvlcEvent.MediaDescriptor.vlm_media_event.psz_instance_name), Marshal.PtrToStringAuto(libvlcEvent.MediaDescriptor.vlm_media_event.psz_media_name)));
                    }
                    break;
                case LibvlcEventE.LibvlcVlmMediaRemoved:
                    if (EventMediaRemoved != null)
                    {
                        EventMediaRemoved(EventProvider, new VlmEvent(Marshal.PtrToStringAuto(libvlcEvent.MediaDescriptor.vlm_media_event.psz_instance_name), Marshal.PtrToStringAuto(libvlcEvent.MediaDescriptor.vlm_media_event.psz_media_name)));
                    }
                    break;
                case LibvlcEventE.LibvlcVlmMediaChanged:
                    if (EventMediaChanged != null)
                    {
                        EventMediaChanged(EventProvider, new VlmEvent(Marshal.PtrToStringAuto(libvlcEvent.MediaDescriptor.vlm_media_event.psz_instance_name), Marshal.PtrToStringAuto(libvlcEvent.MediaDescriptor.vlm_media_event.psz_media_name)));
                    }
                    break;
                case LibvlcEventE.LibvlcVlmMediaInstanceStarted:
                    if (EventMediaInstanceStarted != null)
                    {
                        EventMediaInstanceStarted(EventProvider, new VlmEvent(Marshal.PtrToStringAuto(libvlcEvent.MediaDescriptor.vlm_media_event.psz_instance_name), Marshal.PtrToStringAuto(libvlcEvent.MediaDescriptor.vlm_media_event.psz_media_name)));
                    }
                    break;
                case LibvlcEventE.LibvlcVlmMediaInstanceStopped:
                    if (EventMediaInstanceStopped != null)
                    {
                        EventMediaInstanceStopped(EventProvider, new VlmEvent(Marshal.PtrToStringAuto(libvlcEvent.MediaDescriptor.vlm_media_event.psz_instance_name), Marshal.PtrToStringAuto(libvlcEvent.MediaDescriptor.vlm_media_event.psz_media_name)));
                    }
                    break;
                case LibvlcEventE.LibvlcVlmMediaInstanceStatusInit:
                    if (EventMediaInstanceInit != null)
                    {
                        EventMediaInstanceInit(EventProvider, new VlmEvent(Marshal.PtrToStringAuto(libvlcEvent.MediaDescriptor.vlm_media_event.psz_instance_name), Marshal.PtrToStringAuto(libvlcEvent.MediaDescriptor.vlm_media_event.psz_media_name)));
                    }
                    break;
                case LibvlcEventE.LibvlcVlmMediaInstanceStatusOpening:
                    if (EventMediaInstanceOpening != null)
                    {
                        EventMediaInstanceOpening(EventProvider, new VlmEvent(Marshal.PtrToStringAuto(libvlcEvent.MediaDescriptor.vlm_media_event.psz_instance_name), Marshal.PtrToStringAuto(libvlcEvent.MediaDescriptor.vlm_media_event.psz_media_name)));
                    }
                    break;
                case LibvlcEventE.LibvlcVlmMediaInstanceStatusPlaying:
                    if (EventMediaInstancePlaying != null)
                    {
                        EventMediaInstancePlaying(EventProvider, new VlmEvent(Marshal.PtrToStringAuto(libvlcEvent.MediaDescriptor.vlm_media_event.psz_instance_name), Marshal.PtrToStringAuto(libvlcEvent.MediaDescriptor.vlm_media_event.psz_media_name)));
                    }
                    break;
                case LibvlcEventE.LibvlcVlmMediaInstanceStatusPause:
                    if (EventMediaInstancePause != null)
                    {
                        EventMediaInstancePause(EventProvider, new VlmEvent(Marshal.PtrToStringAuto(libvlcEvent.MediaDescriptor.vlm_media_event.psz_instance_name), Marshal.PtrToStringAuto(libvlcEvent.MediaDescriptor.vlm_media_event.psz_media_name)));
                    }
                    break;
                case LibvlcEventE.LibvlcVlmMediaInstanceStatusEnd:
                    if (EventMediaInstanceEnd != null)
                    {
                        EventMediaInstanceEnd(EventProvider, new VlmEvent(Marshal.PtrToStringAuto(libvlcEvent.MediaDescriptor.vlm_media_event.psz_instance_name), Marshal.PtrToStringAuto(libvlcEvent.MediaDescriptor.vlm_media_event.psz_media_name)));
                    }
                    break;
                case LibvlcEventE.LibvlcVlmMediaInstanceStatusError:
                    if (EventMediaInstanceError != null)
                    {
                        EventMediaInstanceError(EventProvider, new VlmEvent(Marshal.PtrToStringAuto(libvlcEvent.MediaDescriptor.vlm_media_event.psz_instance_name), Marshal.PtrToStringAuto(libvlcEvent.MediaDescriptor.vlm_media_event.psz_media_name)));
                    }
                    break;
            }
        }


        private event EventHandler<VlmEvent> EventMediaAdded;
        public event EventHandler<VlmEvent> MediaAdded
        {
            add
            {
                if (EventMediaAdded == null)
                {
                    Attach(LibvlcEventE.LibvlcVlmMediaAdded);
                }
                EventMediaAdded += value;
            }
            remove
            {
                if (EventMediaAdded != null)
                {
                    EventMediaAdded -= value;
                    if (EventMediaAdded == null)
                    {
                        Dettach(LibvlcEventE.LibvlcVlmMediaAdded);
                    }
                }
            }
        }


        private event EventHandler<VlmEvent> EventMediaRemoved;
        public event EventHandler<VlmEvent> MediaRemoved
        {
            add
            {
                if (EventMediaRemoved == null)
                {
                    Attach(LibvlcEventE.LibvlcVlmMediaRemoved);
                }
                EventMediaRemoved += value;
            }
            remove
            {
                if (EventMediaRemoved != null)
                {
                    EventMediaRemoved -= value;
                    if (EventMediaRemoved == null)
                    {
                        Dettach(LibvlcEventE.LibvlcVlmMediaRemoved);
                    }
                }
            }
        }

        private event EventHandler<VlmEvent> EventMediaChanged;
        public event EventHandler<VlmEvent> MediaChanged
        {
            add
            {
                if (EventMediaChanged == null)
                {
                    Attach(LibvlcEventE.LibvlcVlmMediaChanged);
                }
                EventMediaChanged += value;
            }
            remove
            {
                if (EventMediaChanged != null)
                {
                    EventMediaChanged -= value;
                    if (EventMediaChanged == null)
                    {
                        Dettach(LibvlcEventE.LibvlcVlmMediaChanged);
                    }
                }
            }
        }

        private event EventHandler<VlmEvent> EventMediaInstanceStarted;
        public event EventHandler<VlmEvent> MediaInstanceStarted
        {
            add
            {
                if (EventMediaInstanceStarted == null)
                {
                    Attach(LibvlcEventE.LibvlcVlmMediaInstanceStarted);
                }
                EventMediaInstanceStarted += value;
            }
            remove
            {
                if (EventMediaInstanceStarted != null)
                {
                    EventMediaInstanceStarted -= value;
                    if (EventMediaInstanceStarted == null)
                    {
                        Dettach(LibvlcEventE.LibvlcVlmMediaInstanceStarted);
                    }
                }
            }
        }


        private event EventHandler<VlmEvent> EventMediaInstanceStopped;
        public event EventHandler<VlmEvent> MediaInstanceStopped
        {
            add
            {
                if (EventMediaInstanceStopped == null)
                {
                    Attach(LibvlcEventE.LibvlcVlmMediaInstanceStopped);
                }
                EventMediaInstanceStopped += value;
            }
            remove
            {
                if (EventMediaInstanceStopped != null)
                {
                    EventMediaInstanceStopped -= value;
                    if (EventMediaInstanceStopped == null)
                    {
                        Dettach(LibvlcEventE.LibvlcVlmMediaInstanceStopped);
                    }
                }
            }
        }


        private event EventHandler<VlmEvent> EventMediaInstanceInit;
        public event EventHandler<VlmEvent> MediaInstanceInit
        {
            add
            {
                if (EventMediaInstanceInit == null)
                {
                    Attach(LibvlcEventE.LibvlcVlmMediaInstanceStatusInit);
                }
                EventMediaInstanceInit += value;
            }
            remove
            {
                if (EventMediaInstanceInit != null)
                {
                    EventMediaInstanceInit -= value;
                    if (EventMediaInstanceInit == null)
                    {
                        Dettach(LibvlcEventE.LibvlcVlmMediaInstanceStatusInit);
                    }
                }
            }
        }


        private event EventHandler<VlmEvent> EventMediaInstanceOpening;
        public event EventHandler<VlmEvent> MediaInstanceOpening
        {
            add
            {
                if (EventMediaInstanceOpening == null)
                {
                    Attach(LibvlcEventE.LibvlcVlmMediaInstanceStatusOpening);
                }
                EventMediaInstanceOpening += value;
            }
            remove
            {
                if (EventMediaInstanceOpening != null)
                {
                    EventMediaInstanceOpening -= value;
                    if (EventMediaInstanceOpening == null)
                    {
                        Dettach(LibvlcEventE.LibvlcVlmMediaInstanceStatusOpening);
                    }
                }
            }
        }


        private event EventHandler<VlmEvent> EventMediaInstancePlaying;
        public event EventHandler<VlmEvent> MediaInstancePlaying
        {
            add
            {
                if (EventMediaInstancePlaying == null)
                {
                    Attach(LibvlcEventE.LibvlcVlmMediaInstanceStatusPlaying);
                }
                EventMediaInstancePlaying += value;
            }
            remove
            {
                if (EventMediaInstancePlaying != null)
                {
                    EventMediaInstancePlaying -= value;
                    if (EventMediaInstancePlaying == null)
                    {
                        Dettach(LibvlcEventE.LibvlcVlmMediaInstanceStatusPlaying);
                    }
                }
            }
        }


        private event EventHandler<VlmEvent> EventMediaInstancePause;
        public event EventHandler<VlmEvent> MediaInstancePause
        {
            add
            {
                if (EventMediaInstancePause == null)
                {
                    Attach(LibvlcEventE.LibvlcVlmMediaInstanceStatusPause);
                }
                EventMediaInstancePause += value;
            }
            remove
            {
                if (EventMediaInstancePause != null)
                {
                    EventMediaInstancePause -= value;
                    if (EventMediaInstancePause == null)
                    {
                        Dettach(LibvlcEventE.LibvlcVlmMediaInstanceStatusPause);
                    }
                }
            }
        }

        private event EventHandler<VlmEvent> EventMediaInstanceEnd;
        public event EventHandler<VlmEvent> MediaInstanceEnd
        {
            add
            {
                if (EventMediaInstanceEnd == null)
                {
                    Attach(LibvlcEventE.LibvlcVlmMediaInstanceStatusEnd);
                }
                EventMediaInstanceEnd += value;
            }
            remove
            {
                if (EventMediaInstanceEnd != null)
                {
                    EventMediaInstanceEnd -= value;
                    if (EventMediaInstanceEnd == null)
                    {
                        Dettach(LibvlcEventE.LibvlcVlmMediaInstanceStatusEnd);
                    }
                }
            }
        }

        private event EventHandler<VlmEvent> EventMediaInstanceError;
        public event EventHandler<VlmEvent> MediaInstanceError
        {
            add
            {
                if (EventMediaInstanceError == null)
                {
                    Attach(LibvlcEventE.LibvlcVlmMediaInstanceStatusError);
                }
                EventMediaInstanceError += value;
            }
            remove
            {
                if (EventMediaInstanceError != null)
                {
                    EventMediaInstanceError -= value;
                    if (EventMediaInstanceError == null)
                    {
                        Dettach(LibvlcEventE.LibvlcVlmMediaInstanceStatusError);
                    }
                }
            }
        }
    }
}

/* xsMedia - Media Player
 * (c)2013 - 2025
 * Jason James Newland
 * KangaSoft Software, All Rights Reserved
 * Licenced under the GNU public licence */
using System;
using System.Collections.Generic;
using System.Linq;
using xsVlc.Common;
using xsVlc.Common.Structures;

namespace xsMedia.Helpers
{    
    public class AudioOutputDevice
    {
        /* Store mapping module and device info */
        public AudioOutputModuleInfo Module { get; private set; }
        public AudioOutputDeviceInfo Device { get; private set; }

        public AudioOutputDevice(AudioOutputModuleInfo module, AudioOutputDeviceInfo device)
        {
            Module = module;
            Device = device;
        }

        public override string ToString()
        {
            /* Only really used for displaying as text in a menu or combobox, etc. */
            return Device.Longname;
        }
    }

    public class AudioOutputDevices : List<AudioOutputDevice>
    {        
        public AudioOutputDevices(IMediaPlayerFactory factory)
        {
            /* Build a list of the output devices available to VLC -
             * VLC goes Default, then list of devices; which is why I'm seeing "Default" + a list as first entry -
             * We will need to build a list that contains the module and devices to use with _player.InnerPlayer.SetAudioOutputModuleAndDevice -
             * We also need to ignore the default Microsoft wavemapper device */            
            foreach (var module in factory.AudioOutputModules)
            {
                var deviceInfo = factory.GetAudioOutputDevices(module).ToList();
                if (deviceInfo.Count == 0)
                {
                    continue; /* May have empty entries */
                }
                /* Not interested in the Microsoft "wavemapper" device - it probably would work with VLC, 
                 * but there's little point as the devices are duplicated */
                foreach (var device in deviceInfo)
                {
                    if (device.Id.Equals("wavemapper", StringComparison.InvariantCultureIgnoreCase))
                    {
                        break;
                    }                 
                    var d = new AudioOutputDevice(module, device);
                    Add(d);
                }
            }            
        }
    }
}

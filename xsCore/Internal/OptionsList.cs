/* xsMedia - xsSettings
 * (c)2013 - 2025
 * Jason James Newland
 * KangaSoft Software, All Rights Reserved
 * Licenced under the GNU public licence */
using System.Collections.Generic;
using xsCore.Settings.Data;
using xsCore.Settings.Data.Media;

namespace xsCore.Internal
{
    internal sealed class OptionsList
    {
        public static int GetOption(string key, List<MediaOptionData> options)
        {
            var index = 0;
            foreach (var o in options)
            {
                if (o.Id.ToLower() == key.ToLower())
                {
                    return index;
                }
                index++;
            }
            return -1;
        }
    }
}

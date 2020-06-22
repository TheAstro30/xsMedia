/* xsMedia - sxCore
 * (c)2013 - 2020
 * Jason James Newland
 * KangaSoft Software, All Rights Reserved
 * Licenced under the GNU public licence */
using System.Collections.Generic;

namespace xsCore.Utils.Asx
{
    public class AsxEntryData
    {
        public string Title { get; set; }
        public string Url { get; set; }
    }

    public class AsxData
    {
        public string HeaderTitle { get; set; }

        public List<AsxEntryData> Entries = new List<AsxEntryData>();
    }
}

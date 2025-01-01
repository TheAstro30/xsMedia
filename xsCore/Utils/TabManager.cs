/* xsMedia - sxCore
 * (c)2013 - 2025
 * Jason James Newland
 * KangaSoft Software, All Rights Reserved
 * Licenced under the GNU public licence */
using System.Collections.Generic;
using System.Windows.Forms;

namespace xsCore.Utils
{
    public interface ITab
    {
        void Add(string id, TabPage page);
        TabPage GetTabPage(string id);
    }

    public class TabManager : ITab
    {
        private readonly Dictionary<string, TabPage> _collection = new Dictionary<string, TabPage>();

        public void Add(string id, TabPage page)
        {
            if (string.IsNullOrEmpty(id))
            {
                return;
            }
            if (_collection.ContainsKey(id.ToLower()))
            {
                _collection[id.ToLower()] = page;
            }
            else
            {
                _collection.Add(id.ToLower(), page);
            }
        }

        public TabPage GetTabPage(string id)
        {
            return !string.IsNullOrEmpty(id) && _collection.ContainsKey(id.ToLower()) ? _collection[id.ToLower()] : null;
        }
    }
}

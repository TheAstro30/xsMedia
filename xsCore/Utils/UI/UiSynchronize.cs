/* xsMedia - sxCore
 * (c)2013 - 2020
 * Jason James Newland
 * KangaSoft Software, All Rights Reserved
 * Licenced under the GNU public licence */
using System;
using System.ComponentModel;

namespace xsCore.Utils.UI
{
    public class UiSynchronize
    {
        /* This class eliminates the need for delegates when calling InvokeRequired/BeginInvoke invocation on UI objects */
        private readonly ISynchronizeInvoke _sync;

        public UiSynchronize(ISynchronizeInvoke sync)
        {
            _sync = sync;
        }

        public void Execute(Action action)
        {
            if (_sync == null)
            {
                /* It shouldn't be null, as the constructor forces us to use a synchronous object */
                return;
            }
            _sync.BeginInvoke(action, null);
        }
    }
}

/* xsMedia - sxCore
 * (c)2013 - 2020
 * Jason James Newland
 * KangaSoft Software, All Rights Reserved
 * Licenced under the GNU public licence */
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;

namespace xsCore.PlayerControls
{
    /*  Control renderer lists class
     *  By Ryan Alexander
     *  (C)Copyright 2011
     *  KangaSoft Software - All Rights Reserved
     */
    public class PlayerControlList : List<PlayerControl>
    {
        private readonly ControlRenderer _parent;

        public PlayerControlList(ControlRenderer cr)
        {
            _parent = cr;
        }

        public new void Add(PlayerControl item)
        {
            item.Parent = _parent;
            base.Add(item);
        }

        public T AddAttach<T>(T item) where T : PlayerControl
        {
            Add(item);
            return item;
        }

        public new void AddRange(IEnumerable<PlayerControl> collection)
        {
            var playerControls = collection as PlayerControl[] ?? collection.ToArray();
            foreach (var pc in playerControls) { pc.Parent = _parent; }
            base.AddRange(playerControls);
        }

        public PlayerControl FindControl(Point p)
        {
            return this.FirstOrDefault(pc => pc.Visible && pc.Enabled && pc.Area.Contains(p));
        }

        public PlayerControl SendMouseEvent(Point p, MouseEventType met, MouseButtons mb, int delta)
        {
            var pc = FindControl(p);
            if (pc != null) { pc.MouseEvent(new PlayerMouseEvent(met, ControlRenderer.TranslatePointToChildSpace(p, pc), mb, delta)); }
            return pc;
        }
    }
}

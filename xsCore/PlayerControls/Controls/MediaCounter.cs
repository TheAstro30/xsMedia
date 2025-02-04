﻿/* xsMedia - sxCore
 * (c)2013 - 2025
 * Jason James Newland
 * KangaSoft Software, All Rights Reserved
 * Licenced under the GNU public licence */
using System;
using System.ComponentModel;
using System.Drawing;
using xsCore.Skin;
using xsCore.Utils.SystemUtils;

namespace xsCore.PlayerControls.Controls
{
    public class MediaCounter : PlayerControl
    {
        public enum TimeDisplay
        {
            [Description("Elapsed time")]
            Elapsed = 0,

            [Description("Remaining time")]
            Remain = 1
        }

        private Bitmap _counter;
        private Size _counterCharacterSize;
        private int _elapsed;
        private TimeDisplay _counterType;
        private int _total;

        public event Action<MediaCounter> OnDoubleClick;

        public TimeDisplay CounterType
        {
            get { return _counterType; }
            set 
            {
                _counterType = value;
                Refresh();
            }
        }

        public int Elapsed
        {
            get { return _elapsed; }
            set 
            {                
                _elapsed = value;
                Refresh();                
            }
        }

        public int Total
        {
            get { return _total; }
            set
            {
                _total = value;
                if (_total > 0) { return; }
                _counterType = TimeDisplay.Elapsed;
                if (OnDoubleClick != null)
                {
                    OnDoubleClick(this);
                }
                Refresh();
            }
        }

        public override void KeyEvent(PlayerKeyEvent e)
        {
            /* Not implemented */
        }

        public override void MouseEvent(PlayerMouseEvent e)
        {
            switch (e.EventType)
            {
                case MouseEventType.DblClick:
                    if (_total == 0)
                    {
                        return;
                    }
                    CounterType = CounterType == TimeDisplay.Elapsed ? TimeDisplay.Remain : TimeDisplay.Elapsed;
                    if (OnDoubleClick != null)
                    {
                        OnDoubleClick(this);
                    }
                    Refresh();
                    break;
            }
        }

        public override void Paint(Graphics g)
        {
            /* Calculate which image section represents what number */
            var length = CounterType == TimeDisplay.Elapsed ? _elapsed : Total - _elapsed;
            var remain = CounterType == TimeDisplay.Remain ? "-" : string.Empty;
            var time = string.Format("{0}{1}", remain, MediaInfo.FormatDurationString(length));
            /* Center the counter */
            var width = (_counterCharacterSize.Width + 1) * time.Length;
            var left = (Area.Width / 2) - (width / 2);            
            var rect = new Rectangle(left, 0, _counterCharacterSize.Width, _counterCharacterSize.Height);
            /* Loop each character */
            foreach (var c in time)
            {
                int offset;
                if (c >= '0' && c <= '9')
                {
                    offset = c - '0';
                }
                else if (c == ':')
                {
                    offset = 10;
                }
                else
                {
                    offset = 11;
                }
                g.DrawImage(_counter, rect,
                            new Rectangle(_counterCharacterSize.Width * offset, 0, _counterCharacterSize.Width, _counterCharacterSize.Height),
                            GraphicsUnit.Pixel);
                /* Offset by one pixel */
                rect.X += _counterCharacterSize.Width + 1;
            }            
        }

        public override void SkinStyleChanged()
        {
            var s = SkinManager.GetResourceById(Tag, null);
            _counter = s.Image;
            _counterCharacterSize = s.ImageSize;
        }
    }
}

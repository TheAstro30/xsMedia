/* xsMedia - sxCore
 * (c)2013 - 2024
 * Jason James Newland
 * KangaSoft Software, All Rights Reserved
 * Licenced under the GNU public licence */
using System;
using System.Drawing;
using xsCore.Skin;

namespace xsCore.PlayerControls.Controls
{
    /*  Button control
     *  By Jason James Newland & Ryan Alexander
     *  (C)Copyright 2011
     *  KangaSoft Software - All Rights Reserved
     */
    public class MediaButton : PlayerControl
    {
        private Bitmap _normalImage;
        private Bitmap _downImage;
        private Bitmap _overImage; /* IMPLEMENTED! */
        private bool _hovering;

        public event Action<PlayerControl> OnClick;

        public MediaButton()
        {
            /* Empty by default */
        }

        public MediaButton(Rectangle area)
        {
            Area = area;
        }
       
        public bool IsPressed { get; set; }
        public bool IsLatchState { get; set; }

        public override void Paint(Graphics g)
        {            
            if (!IsPressed && _hovering)
            {
                /* Draw hovering image */
                if (_overImage != null) { g.DrawImage(_overImage, 0, 0, Size.Width, Size.Height); }
                return;
            }
            /* Draw state */
            if (IsPressed)
            {
                if (_downImage != null)
                {                    
                    g.DrawImage(_downImage, 0, 0, Size.Width, Size.Height);
                }
            }
            else
            {
                if (_normalImage != null) { g.DrawImage(_normalImage, 0, 0, Size.Width, Size.Height); }
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
                case MouseEventType.Down:
                    IsPressed = !IsLatchState || !IsPressed;
                    Refresh();
                    break;
                case MouseEventType.Up:
                    if (!IsLatchState) { IsPressed = false; }
                    Refresh();
                    break;
                case MouseEventType.Click:
                    if (OnClick != null) { OnClick(this); }
                    break;
                case MouseEventType.Enter:
                    _hovering = true;
                    Refresh();
                    break;
                case MouseEventType.Leave:
                    _hovering = false;
                    Refresh();
                    break;
            }
        }

        public override void SkinStyleChanged()
        {            
            _downImage = SkinManager.GetResourceById(Tag, "Down").Image;
            _normalImage = SkinManager.GetResourceById(Tag, "Normal").Image;
            _overImage = SkinManager.GetResourceById(Tag, "Over").Image;
        }

        public void Press(bool pressOn)
        {
            /* Change press state (doesn't raise OnClick) */
            if (!IsLatchState) { return; }
            IsPressed = pressOn;
            Refresh();
        }
    }
}

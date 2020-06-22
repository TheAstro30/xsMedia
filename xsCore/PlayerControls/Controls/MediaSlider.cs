/* xsMedia - sxCore
 * (c)2013 - 2020
 * Jason James Newland
 * KangaSoft Software, All Rights Reserved
 * Licenced under the GNU public licence */
using System;
using System.Drawing;
using System.Windows.Forms;
using xsCore.Skin;

namespace xsCore.PlayerControls.Controls
{
    /*  Slider control
     *  By Jason James Newland & Ryan Alexander
     *  (C)Copyright 2011
     *  KangaSoft Software - All Rights Reserved
     */
    public class MediaSlider : PlayerControl
    {
        public enum SliderStyle
        {
            Horizontal = 0,
            Vertical = 1
        }

        public enum SliderButtonPosition
        {
            Center = 0,
            Near = 1,
            Far = 2
        }

        private SliderStyle _orientation;
        private SliderButtonPosition _buttonPosition;
        private Bitmap _sliderButton;
        private bool _verticalFlip;
        private bool _constantValue;
        private int _minValue;
        private int _maxValue;
        private float _currentValue;
        private Size _sliderSize;
        private int _slideRailLength;

        private bool _isSliding;
        private int _slideOffset;
        private int _slidePadL;
        private int _slidePadR;
        private int _slideCurrent;

        public event Action<MediaSlider> OnValueChanged;
        public event Action<MediaSlider> OnDoubleClick;

        public MediaSlider()
        {
            _orientation = SliderStyle.Horizontal;
            _buttonPosition = SliderButtonPosition.Center;
            _minValue = 0;
            _maxValue = 10;
            _currentValue = 0;
            _constantValue = true;
        }

        public MediaSlider(Rectangle area, int sliderWidth, int sliderHeight)
        {
            Area = area;
            _sliderSize = new Size(sliderWidth, sliderHeight);
            _orientation = SliderStyle.Horizontal;
            _buttonPosition = SliderButtonPosition.Center;
            _minValue = 0;
            _maxValue = 10;
            _currentValue = 0;
            _constantValue = true;
        }

        public SliderStyle Orientation
        {
            get { return _orientation; }
            set
            {
                _orientation = value;
                Refresh();
            }
        }

        public SliderButtonPosition ButtonPosition
        {
            get { return _buttonPosition; }
            set
            {
                _buttonPosition = value;
                Refresh();
            }
        }

        public bool VerticalFlip
        {
            get { return _verticalFlip; }
            set
            {
                _verticalFlip = value;
                Refresh();
            }
        }

        public bool ConstantValueUpdate
        {
            /* By default this is always true, only used for a trackbar so the audio isn't updated
             * (seeked) until mouse up is fired */
            get { return _constantValue; }
            set
            {
                _constantValue = value;
                Refresh();
            }
        }

        public int MinimumValue
        {
            get { return _minValue; }
            set
            {
                _minValue = value;
                Refresh();
            }
        }

        public int MaximumValue
        {
            get { return _maxValue; }
            set
            {
                _maxValue = value;
                /* Reset value */
                if (_currentValue > _maxValue) { _currentValue = _maxValue; }
                Value = _currentValue;
                Refresh();
            }
        }

        public float Value
        {
            get { return _currentValue; }
            set
            {
                if (_isSliding) { return; }
                if (!_constantValue && _isSliding) { return; }
                _currentValue = value;
                if (_currentValue < _minValue) { _currentValue = _minValue; }
                if (_currentValue > _maxValue) { _currentValue = _maxValue; }
                var val = (int)(_slideRailLength * (_currentValue / (_maxValue - _minValue))) + _slidePadL;
                _slideCurrent = Orientation == SliderStyle.Horizontal
                                    ? val : (!_verticalFlip ? val : Area.Height - val);
                Refresh();
            }
        }

        protected RectangleF SliderArea
        {
            get
            {
                int fixedDistance;
                RectangleF area;

                switch (Orientation)
                {
                    case SliderStyle.Horizontal:
                        fixedDistance = GetFixedSliderDistance(Size.Height, _sliderSize.Height);
                        area = new RectangleF(new Point(_slideCurrent - (_sliderSize.Width / 2), fixedDistance), _sliderSize);
                        break;
                    default:
                        fixedDistance = GetFixedSliderDistance(Size.Width, _sliderSize.Width);
                        if (_slideCurrent < _slidePadL) { _slideCurrent = _slidePadL; }
                        area = new RectangleF(new Point(fixedDistance, _slideCurrent - (_sliderSize.Height / 2)), _sliderSize);
                        break;
                }

                return area;
            }
        }

        protected int GetFixedSliderDistance(int fixedsize, int fixedbuttonsize)
        {
            var sbp = _buttonPosition;

            if (_verticalFlip)
            {
                switch (sbp)
                {
                    case SliderButtonPosition.Far:
                        sbp = SliderButtonPosition.Near;
                        break;
                    case SliderButtonPosition.Near:
                        sbp = SliderButtonPosition.Far;
                        break;
                }
            }

            switch (sbp)
            {
                case SliderButtonPosition.Near:
                    return 0;
                case SliderButtonPosition.Far:
                    return fixedsize - fixedbuttonsize;
                default:
                    return (fixedsize / 2) - (fixedbuttonsize / 2);
            }
        }

        public override void Paint(Graphics g)
        {
            if (_sliderButton == null || _maxValue == 0 || !Visible) { return; }
            var s = SliderArea;
            if (_orientation == SliderStyle.Horizontal)
            {
                var r = SkinManager.GetResourceById(Tag, null);
                if (r.RailColor != Color.Empty)
                {                
                    var rect = new RectangleF(r.RailArea.X, r.RailArea.Y, s.X, r.RailArea.Height);
                    using (var b = new SolidBrush(r.RailColor))
                    {
                        g.FillRectangle(b, rect);
                    }
                }
            }
            g.DrawImage(_sliderButton, s);
        }

        protected override void Resized(Size oldSize)
        {
            int padding;
            switch (Orientation)
            {
                case SliderStyle.Horizontal:
                    padding = (_sliderSize.Width / 2) + 1;
                    _slidePadL = padding;
                    _slidePadR = Size.Width - padding;
                    _slideRailLength = _slidePadR - _slidePadL;
                    _slideCurrent = _slidePadL;
                    break;
                default:
                    padding = (_sliderSize.Height / 2) + 1;
                    _slidePadL = padding;
                    _slidePadR = Size.Height - padding;
                    _slideRailLength = _slidePadR - _slidePadL;
                    _slideCurrent = (_verticalFlip ? _slideRailLength + _slidePadL : _slidePadL);
                    break;
            }
            /* Update value */
            Value = _currentValue;
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
                    if (e.Button == MouseButtons.Left)
                    {                        
                        if (SliderArea.Contains(e.Location))
                        {
                            /* The slider button was clicked */
                            _isSliding = true;
                            _slideOffset = Orientation == SliderStyle.Horizontal
                                               ? e.Location.X - _slideCurrent
                                               : e.Location.Y - _slideCurrent;
                        }
                        else
                        {
                            /* It is assumed the rail has been clicked on */
                            _isSliding = true;
                            UpdateSliderValue(e.Location);
                        }
                    }
                    break;
                case MouseEventType.Up:
                    if (e.Button == MouseButtons.Left)
                    {
                        if (_isSliding)
                        {
                            _isSliding = false;
                            if (OnValueChanged != null) { OnValueChanged(this); }
                        }
                    }
                    break;
                case MouseEventType.Move:
                    if (_isSliding)
                    {
                        UpdateSliderValue(e.Location);
                        if (_constantValue)
                        {
                            if (OnValueChanged != null) { OnValueChanged(this); }
                        }
                    }
                    break;
                case MouseEventType.DblClick:
                    if (OnDoubleClick != null) { OnDoubleClick(this); }
                    break;
            }
        }

        public override void SkinStyleChanged()
        {
            /* Note - s.Exists says if the resource exists in the skin....it will not return null if it doesnt exist, its values will just be null
             * this is to prevent crashing from someone missing a resource, but this also means YOU will have to code, to check for null images/etc before painting
             * in reality, this needs to be done anyways, as there is no guarentee an image will be set before a skin is applied...... */
            var s = SkinManager.GetResourceById(Tag + "_SLIDER_BUTTON", null);
            _sliderButton = s.Image;
            _sliderSize = s.Area.Size;
        }

        private void UpdateSliderValue(Point mouseLocation)
        {
            switch (Orientation)
            {
                case SliderStyle.Horizontal:
                    _slideCurrent = mouseLocation.X - _slideOffset;
                    if (_slideCurrent < _slidePadL) { _slideCurrent = _slidePadL; }
                    if (_slideCurrent > _slidePadR) { _slideCurrent = _slidePadR; }
                    _currentValue = (((float)(_slideCurrent - _slidePadL) / _slideRailLength) * (_maxValue - _minValue));
                    break;
                default:
                    _slideCurrent = mouseLocation.Y - _slideOffset;
                    if (_slideCurrent < _slidePadL) { _slideCurrent = _slidePadL; }
                    if (_slideCurrent > _slidePadR) { _slideCurrent = _slidePadR; }
                    /* Needs to be tested to make sure we have the floating math correct */
                    var val = (((float)(_slideCurrent - _slidePadL) / _slideRailLength) * (_maxValue - _minValue));
                    _currentValue = (_verticalFlip ? ((float)_maxValue - _minValue) - val : val);
                    break;
            }
            Refresh();
        }
    }
}

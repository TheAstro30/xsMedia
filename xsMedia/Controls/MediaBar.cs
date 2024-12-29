/* xsMedia - Media Player
 * (c)2013 - 2024
 * Jason James Newland
 * KangaSoft Software, All Rights Reserved
 * Licenced under the GNU public licence */
using System;
using System.Drawing;
using System.IO;
using xsCore.PlayerControls;
using xsCore.Skin;
using xsCore.PlayerControls.Controls;
using xsCore.Utils.SystemUtils;
using xsSettings;

namespace xsMedia.Controls
{
    public class MediaBar : ControlRenderer
    {
        private readonly MediaSlider _position;
        private readonly MediaSlider _volume;

        public MediaCounter Counter { get; private set; }

        private float _currentPosition;
        private int _length;
        private float _volumeLevel;

        public event Action<MediaButton> OnButtonPlayClick;
        public event Action<MediaButton> OnButtonStopClick;
        public event Action<MediaButton> OnButtonPauseClick;
        public event Action<MediaButton> OnButtonNextClick;
        public event Action<MediaButton> OnButtonPreviousClick;
        public event Action<MediaButton> OnButtonOpenClick;

        public event Action<MediaSlider> OnPositionChanged;
        public event Action<MediaSlider> OnVolumeChanged;        

        public MediaBar()
        {
            /* Skin */
            LoadSkin(AppPath.MainDir(SettingsManager.Settings.Window.CurrentSkin));            
            /* Controls */
            PlayerControls.AddAttach(new MediaButton
                                         {
                                             Tag = "PLAYER_PREV",
                                             TooltipText = "Previous track"
                                         }).OnClick += OnButtonClick;

            PlayerControls.AddAttach(new MediaButton
                                         {
                                             Tag = "PLAYER_PLAY",
                                             TooltipText = "Play"
                                         }).OnClick += OnButtonClick;

            PlayerControls.AddAttach(new MediaButton
                                         {
                                             Tag = "PLAYER_PAUSE",
                                             TooltipText = "Pause"
                                         }).OnClick += OnButtonClick;

            PlayerControls.AddAttach(new MediaButton
                                         {
                                             Tag = "PLAYER_STOP",
                                             TooltipText = "Stop"
                                         }).OnClick += OnButtonClick;

            PlayerControls.AddAttach(new MediaButton
                                         {
                                             Tag = "PLAYER_NEXT",
                                             TooltipText = "Next track"
                                         }).OnClick += OnButtonClick;

            PlayerControls.AddAttach(new MediaButton
                                         {
                                             Tag = "PLAYER_OPEN",
                                             TooltipText = "Open media"
                                         }).OnClick += OnButtonClick;

            _position = PlayerControls.AddAttach(new MediaSlider
                                                     {
                                                         Tag = "PLAYER_POS",
                                                         TooltipText = "Playback position",
                                                         ConstantValueUpdate = false,
                                                         MaximumValue = 100,
                                                         Visible = false
                                                     });
            _position.OnValueChanged += OnSliderValueChanged;

            _volume = PlayerControls.AddAttach(new MediaSlider
                                                   {
                                                       Tag = "PLAYER_VOL",
                                                       TooltipText = "Volume",
                                                       MaximumValue = 125
                                                   });
            _volume.OnValueChanged += OnSliderValueChanged;

            Counter = PlayerControls.AddAttach(new MediaCounter
                                                    {
                                                        Tag = "PLAYER_TIME",
                                                        CounterType = SettingsManager.Settings.Player.CounterType
                                                    });
            Counter.OnDoubleClick += OnCounterDoubleClick;
        }

        public int ElapsedTime
        {
            set
            {
                Counter.Elapsed = value;
            }
        }

        public int Length
        {
            get
            {
                return _length;
            }
            set
            {
                _length = value;
                Counter.Total = value;
            }
        }

        public float Position
        {
            get
            {
                return _currentPosition;
            }
            set
            {
                _currentPosition = value;
                _position.Value = _currentPosition;
            }
        }

        public float Volume
        {
            get
            {
                return _volumeLevel;
            }
            set
            {
                _volumeLevel = value;
                _volume.Value = _volumeLevel;
                _volume.TooltipText = string.Format("Volume: {0}%", _volumeLevel);
            }
        }

        public bool PositionBarVisible
        {
            get { return _position.Visible; }
            set { _position.Visible = value; }
        }

        public void UpdateControls()
        {
            ResourceData r;
            if (_position != null)
            {
                r = SkinManager.GetResourceById("PLAYER_POS", null);
                _position.Size = new Size(Width - r.Area.Width, r.Area.Height);
            }
            if (_volume != null)
            {
                r = SkinManager.GetResourceById("PLAYER_VOL", null);
                _volume.Location = new Point(Width - r.AreaOffset.Width, r.Area.Y);
            }
        }

        /* Overrides */
        protected override void OnLoad(EventArgs e)
        {
            SkinManager.ApplySkinLayout(this);
            Refresh();
        }

        protected override void OnResize(EventArgs e)
        {
            UpdateControls();
            /* Pass back to base class otherwise skin will not draw correctly */
            base.OnResize(e);
        }

        private void OnButtonClick(PlayerControl control)
        {
            var button = (MediaButton)control;
            switch (button.Tag)
            {
                case "PLAYER_PREV":
                    if (OnButtonPreviousClick != null)
                    {
                        OnButtonPreviousClick(button);
                    }
                    break;
                case "PLAYER_PLAY":
                    if (OnButtonPlayClick != null)
                    {
                        OnButtonPlayClick(button);
                    }
                    break;
                case "PLAYER_PAUSE":
                    if (OnButtonPauseClick != null)
                    {
                        OnButtonPauseClick(button);
                    }
                    break;
                case "PLAYER_STOP":
                    if (OnButtonStopClick != null)
                    {
                        OnButtonStopClick(button);
                    }
                    break;
                case "PLAYER_NEXT":
                    if (OnButtonNextClick != null)
                    {
                        OnButtonNextClick(button);
                    }
                    break;
                case "PLAYER_OPEN":
                    if (OnButtonOpenClick != null)
                    {
                        OnButtonOpenClick(button);
                    }
                    break;
            }
        }

        private void OnCounterDoubleClick(PlayerControl control)
        {
            SettingsManager.Settings.Player.CounterType = Counter.CounterType;
        }

        private void OnSliderValueChanged(MediaSlider control)
        {
            switch (control.Tag)
            {
                case "PLAYER_POS":
                    _currentPosition = _position.Value;
                    if (OnPositionChanged != null)
                    {
                        OnPositionChanged(control);
                    }
                    break;
                case "PLAYER_VOL":
                    _volumeLevel = _volume.Value;
                    _volume.TooltipText = string.Format("Volume: {0}%", (int)_volumeLevel);
                    if (OnVolumeChanged != null)
                    {
                        OnVolumeChanged(control);
                    }
                    break;
            }
        }

        public void LoadSkin(string skinFile)
        {
            bool success;
            if (string.IsNullOrEmpty(skinFile) || !File.Exists(skinFile))
            {
                skinFile = AppPath.MainDir(@"\skins\classic\classic.xml");
                success = SkinManager.ReadSkin(skinFile);
            }
            else
            {
                if (!SkinManager.ReadSkin(skinFile))
                {
                    skinFile = AppPath.MainDir(@"\skins\classic\classic.xml");
                    success = SkinManager.ReadSkin(skinFile);
                }
                else
                {
                    success = true;
                }
            }
            if (!success)
            {
                /* Throw an error */
            }
            /* Call a redraw */
            SettingsManager.Settings.Window.CurrentSkin = AppPath.MainDir(skinFile);
            SkinManager.ApplySkinLayout(this);
            Refresh();
        }
    }
}

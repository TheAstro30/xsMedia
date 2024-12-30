/* xsMedia - Media Player
 * (c)2013 - 2024
 * Jason James Newland
 * KangaSoft Software, All Rights Reserved
 * Licenced under the GNU public licence */
using System;
using System.Drawing;
using System.Windows.Forms;
using xsCore;
using xsCore.Utils;
using xsMedia.Controls.Effects;
using xsMedia.Properties;

namespace xsMedia.Forms
{
    public sealed class FrmEffects : Form
    {
        private Button _btnOk;
        private TabControl _tbMain;
        private TabPage _tbAudio;
        private TabPage _tbVideo;
        private TabControl _tbVideoEffects;
        private TabPage _tbVAdj;
        private TabPage _tbVMarq;
        private TabPage _tbVLogo;
        private TabPage _tbVCrop;
        private TabPage _tbVDeint;
        private TabControl _tbAudioEffects;
        private TabPage _tbEqualizer;

        /* Tab managers */
        private readonly ITab _tabRoot = new TabManager();
        private readonly ITab _tabAudio = new TabManager();
        private readonly ITab _tabVideo = new TabManager();

        private EffectEq _eq;

        private EffectAdjust _adjust;
        private EffectMarquee _marquee;
        private EffectLogo _logo;
        private EffectCrop _crop;
        private EffectDeinterlace _deint;

        /* Constructors */
        public FrmEffects()
        {
            /* Default constructor */
            InitializeComponent();
            Init();
            SelectTabs();
        }

        public FrmEffects(string rootId, string audioId, string videoId)
        {
            InitializeComponent();
            Init();
            SelectTabs(rootId, audioId, videoId);
        }

        /* Override OnLoad for form positioning */
        protected override void OnLoad(EventArgs e)
        {
            var loc = SettingsManager.Settings.Window.EffectsWindow.Location;
            if (loc == Point.Empty)
            {
                if (Owner != null)
                {
                    /* We set this form to centered parent (which CenterToParent property doesn't work with the main form, for whatever reason) */
                    Location = new Point(Owner.Location.X + Owner.Width/2 - Width/2,
                        Owner.Location.Y + Owner.Height/2 - Height/2);
                }
            }
            else
            {
                Location = loc;
            }
            base.OnLoad(e);
        }

        /* Close */
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            SettingsManager.Settings.Window.EffectsWindow.Location = Location;

            SettingsManager.Settings.Filters.RootTab.Id = _tbMain.SelectedTab.Tag.ToString().ToLower();
            SettingsManager.Settings.Filters.AudioTab.Id = _tbAudioEffects.SelectedTab.Tag.ToString().ToLower();
            SettingsManager.Settings.Filters.VideoTab.Id = _tbVideoEffects.SelectedTab.Tag.ToString().ToLower();
        }

        public void SelectTabs(string rootId = null, string audioId = null, string videoId = null)
        {
            var tab = _tabRoot.GetTabPage(string.IsNullOrEmpty(rootId) ? SettingsManager.Settings.Filters.RootTab.Id : rootId);
            if (tab != null)
            {
                _tbMain.SelectTab(tab);
            }
            tab = _tabAudio.GetTabPage(string.IsNullOrEmpty(audioId) ? SettingsManager.Settings.Filters.AudioTab.Id : audioId);
            if (tab != null)
            {
                _tbAudioEffects.SelectTab(tab);
            }
            tab = _tabVideo.GetTabPage(string.IsNullOrEmpty(videoId) ? SettingsManager.Settings.Filters.VideoTab.Id : videoId);
            if (tab != null)
            {
                _tbVideoEffects.SelectTab(tab);
            }
        }

        private void InitializeComponent()
        {
            Name = "FrmEffects";
            ClientSize = new Size(472, 315);
            Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;            
            MaximizeBox = false;
            MinimizeBox = false;
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.Manual;
            Text = @"Effects";

            /* Controls */
            var images = new ImageList();
            images.Images.AddRange(new Image[]
            {
                Resources.menuAudioTrack.ToBitmap(),
                Resources.menuVideoTrack.ToBitmap(),
                Resources.menuEffects.ToBitmap(),
                Resources.fxAdjust.ToBitmap(),
                Resources.fxMarquee.ToBitmap(),
                Resources.fxLogo.ToBitmap(),
                Resources.fxCrop.ToBitmap(),
                Resources.fxDeinterlace.ToBitmap()
            });

            _tbMain = new TabControl
            {
                Location = new Point(7, 10),
                SelectedIndex = 0,
                Size = new Size(460, 267),
                TabIndex = 2,
                ImageList = images
            };

            _tbAudio = new TabPage
            {                
                Location = new Point(4, 24),
                Padding = new Padding(3),
                Size = new Size(452, 239),
                TabIndex = 0,
                Tag = "AUDIO",
                Text = @"Audio",
                ImageIndex = 0,
                UseVisualStyleBackColor = true
            };

            _tbAudioEffects = new TabControl
            {
                Dock = DockStyle.Fill,
                Location = new Point(3, 3),
                SelectedIndex = 0,
                Size = new Size(446, 233),
                TabIndex = 0,
                ImageList = images
            };

            _tbEqualizer = new TabPage
            {
                Location = new Point(4, 24),
                Padding = new Padding(3),
                Size = new Size(438, 205),
                TabIndex = 0,
                Tag = "EQ",
                Text = @"Equalizer",
                ImageIndex = 2,
                UseVisualStyleBackColor = true
            };

            _tbVideo = new TabPage
            {
                Location = new Point(4, 24),
                Padding = new Padding(3),
                Size = new Size(452, 239),
                TabIndex = 1,
                Tag = "VIDEO",
                Text = @"Video",
                ImageIndex = 1,
                UseVisualStyleBackColor = true
            };

            _tbVideoEffects = new TabControl
            {
                Location = new Point(6, 6),
                SelectedIndex = 0,
                Size = new Size(439, 227),
                TabIndex = 0,
                ImageList = images
            };

            _tbVAdj = new TabPage
            {
                Location = new Point(4, 24),
                Padding = new Padding(3),
                Size = new Size(431, 199),
                TabIndex = 0,
                Tag = "ADJUST",
                Text = @"Adjust",
                ImageIndex = 3,
                UseVisualStyleBackColor = true
            };

            _tbVMarq = new TabPage
            {
                Location = new Point(4, 24),
                Padding = new Padding(3),
                Size = new Size(431, 199),
                TabIndex = 1,
                Tag = "MARQUEE",
                Text = @"Marquee",
                ImageIndex = 4,
                UseVisualStyleBackColor = true
            };

            _tbVLogo = new TabPage
            {
                Location = new Point(4, 24),
                Size = new Size(431, 199),
                TabIndex = 2,
                Tag = "LOGO",
                Text = @"Logo",
                ImageIndex = 5,
                UseVisualStyleBackColor = true
            };

            _tbVCrop = new TabPage
            {
                Location = new Point(4, 24),
                Size = new Size(431, 199),
                TabIndex = 3,
                Tag = "CROP",
                Text = @"Crop",
                ImageIndex = 6,
                UseVisualStyleBackColor = true
            };

            _tbVDeint = new TabPage
            {
                Location = new Point(4, 24),
                Size = new Size(431, 199),
                TabIndex = 4,
                Tag = "DEINTERLACE",
                Text = @"Deinterlace",
                ImageIndex = 7,
                UseVisualStyleBackColor = true
            };

            _btnOk = new Button
            {
                Location = new Point(392, 283),
                Size = new Size(75, 23),
                TabIndex = 1,
                Text = @"&Done",
                UseVisualStyleBackColor = true
            };

            /* Add controls */
            _tbMain.Controls.AddRange(new Control[] {_tbAudio, _tbVideo});

            _tbAudio.Controls.Add(_tbAudioEffects);
            _tbVideo.Controls.Add(_tbVideoEffects);

            _tbAudioEffects.Controls.Add(_tbEqualizer);

            _tbVideoEffects.Controls.AddRange(new Control[]
            {
                _tbVAdj, _tbVMarq, _tbVLogo, _tbVCrop, _tbVDeint
            });

            Controls.AddRange(new Control[] {_tbMain, _btnOk});
        }

        /* Init method */
        private void Init()
        {                        
            _tabRoot.Add("audio", _tbAudio);
            _tabRoot.Add("video", _tbVideo);

            _eq = new EffectEq(SettingsManager.Settings.Filters.Eq);
            _tbEqualizer.Controls.Add(_eq);
            _tabAudio.Add("eq", _tbEqualizer);
            _adjust = new EffectAdjust(SettingsManager.Settings.Filters.Adjust);
            _tbVAdj.Controls.Add(_adjust);
            _tabVideo.Add("adjust", _tbVAdj);
            _marquee = new EffectMarquee(SettingsManager.Settings.Filters.Marquee);
            _tbVMarq.Controls.Add(_marquee);
            _tabVideo.Add("marquee", _tbVMarq);
            _logo = new EffectLogo(SettingsManager.Settings.Filters.Logo);
            _tbVLogo.Controls.Add(_logo);
            _tabVideo.Add("logo", _tbVLogo);
            _crop = new EffectCrop(SettingsManager.Settings.Filters.Crop);
            _tbVCrop.Controls.Add(_crop);
            _tabVideo.Add("crop", _tbVCrop);
            _deint = new EffectDeinterlace(SettingsManager.Settings.Filters.Deinterlace);
            _tbVDeint.Controls.Add(_deint);
            _tabVideo.Add("deinterlace", _tbVDeint);
            /* Handler */
            _btnOk.Click += BtnOkClick;
        }

        /* Button ok */
        private void BtnOkClick(object sender, EventArgs e)
        {
            Close();
        }
    }
}

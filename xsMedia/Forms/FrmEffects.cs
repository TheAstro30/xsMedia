/* xsMedia - Media Player
 * (c)2013 - 2020
 * Jason James Newland
 * KangaSoft Software, All Rights Reserved
 * Licenced under the GNU public licence */
using System;
using System.Windows.Forms;
using xsCore.Utils;
using xsMedia.Controls.Effects;
using xsSettings;

namespace xsMedia.Forms
{
    public partial class FrmEffects : Form
    {
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

        /* Close */
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            SettingsManager.Settings.Filters.RootTab.Id = tbMain.SelectedTab.Tag.ToString().ToLower();
            SettingsManager.Settings.Filters.AudioTab.Id = tbAudioEffects.SelectedTab.Tag.ToString().ToLower();
            SettingsManager.Settings.Filters.VideoTab.Id = tbVideoEffects.SelectedTab.Tag.ToString().ToLower();
        }

        public void SelectTabs(string rootId = null, string audioId = null, string videoId = null)
        {
            var tab = _tabRoot.GetTabPage(string.IsNullOrEmpty(rootId) ? SettingsManager.Settings.Filters.RootTab.Id : rootId);
            if (tab != null)
            {
                tbMain.SelectTab(tab);
            }
            tab = _tabAudio.GetTabPage(string.IsNullOrEmpty(audioId) ? SettingsManager.Settings.Filters.AudioTab.Id : audioId);
            if (tab != null)
            {
                tbAudioEffects.SelectTab(tab);
            }
            tab = _tabVideo.GetTabPage(string.IsNullOrEmpty(videoId) ? SettingsManager.Settings.Filters.VideoTab.Id : videoId);
            if (tab != null)
            {
                tbVideoEffects.SelectTab(tab);
            }
        }

        /* Init method */
        private void Init()
        {            
            _tabRoot.Add("audio", tbAudio);
            _tabRoot.Add("video", tbVideo);

            _eq = new EffectEq(SettingsManager.Settings.Filters.Eq);
            tbEqualizer.Controls.Add(_eq);
            _tabAudio.Add("eq", tbEqualizer);
            _adjust = new EffectAdjust(SettingsManager.Settings.Filters.Adjust);
            tbVAdj.Controls.Add(_adjust);
            _tabVideo.Add("adjust", tbVAdj);
            _marquee = new EffectMarquee(SettingsManager.Settings.Filters.Marquee);
            tbVMarq.Controls.Add(_marquee);
            _tabVideo.Add("marquee", tbVMarq);
            _logo = new EffectLogo(SettingsManager.Settings.Filters.Logo);
            tbVLogo.Controls.Add(_logo);
            _tabVideo.Add("logo", tbVLogo);
            _crop = new EffectCrop(SettingsManager.Settings.Filters.Crop);
            tbVCrop.Controls.Add(_crop);
            _tabVideo.Add("crop", tbVCrop);
            _deint = new EffectDeinterlace(SettingsManager.Settings.Filters.Deinterlace);
            tbVDeint.Controls.Add(_deint);
            _tabVideo.Add("deinterlace", tbVDeint);
            /* Handler */
            btnOk.Click += BtnOkClick;
        }

        /* Button ok */
        private void BtnOkClick(object sender, EventArgs e)
        {
            Close();
        }
    }
}

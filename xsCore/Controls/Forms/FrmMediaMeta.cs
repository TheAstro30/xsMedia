/* xsMedia - xsPlaylist
 * (c)2013 - 2024
 * Jason James Newland
 * KangaSoft Software, All Rights Reserved
 * Licenced under the GNU public licence */
using System;
using System.Drawing;
using System.Windows.Forms;
using xsCore.Utils;
using xsCore.Utils.IO;
using xsCore.Utils.UI;
using xsVlc.Common;
using xsVlc.Common.Events;
using xsVlc.Common.Media;
using xsVlc.Core;

namespace xsCore.Controls.Forms
{
    public sealed partial class FrmMediaMeta : FormEx
    {
        private bool _loading;
        private readonly string _fileName;
        private readonly UiSynchronize _sync;
        private readonly IMediaFromFile _mediaFile;
        private string _artworkUrl;

        private Bitmap _bmpArt;

        public FrmMediaMeta(string fileName)
        {
            _loading = true;
            InitializeComponent();

            _sync = new UiSynchronize(this);

            Text = string.Format("Media Info: {0}", fileName);

            pnlArt.Paint += OnPanelPaint;
            txtTitle.TextChanged += OnMetaTextChanged;
            txtArtist.TextChanged += OnMetaTextChanged;
            txtAlbum.TextChanged += OnMetaTextChanged;
            txtDate.TextChanged += OnMetaTextChanged;
            txtGenre.TextChanged += OnMetaTextChanged;
            txtTrack.TextChanged += OnMetaTextChanged;
            txtPublisher.TextChanged += OnMetaTextChanged;
            txtLanguage.TextChanged += OnMetaTextChanged;
            txtCopyright.TextChanged += OnMetaTextChanged;
            txtEncoded.TextChanged += OnMetaTextChanged;
            txtComments.TextChanged += OnMetaTextChanged;
            _fileName = fileName;
            IMediaPlayerFactory factory = new MediaPlayerFactory();
            _mediaFile = factory.CreateMedia<IMediaFromFile>(fileName);
            _mediaFile.Events.ParsedChanged += MediaParsed;
            _mediaFile.Parse(true);

            tmrMeta.Enabled = true;
        }

        private void MediaParsed(object sender, MediaParseChange e)
        {
            if (InvokeRequired)
            {
                _sync.Execute(() => MediaParsed(sender, e));
                return;
            }
            Init();
        }

        private void Init()
        {
            txtTitle.Text = _mediaFile.GetMetaData(MetaDataType.Title);
            txtArtist.Text = _mediaFile.GetMetaData(MetaDataType.Artist);
            txtAlbum.Text = _mediaFile.GetMetaData(MetaDataType.Album);
            txtDate.Text = _mediaFile.GetMetaData(MetaDataType.Date);
            txtGenre.Text = _mediaFile.GetMetaData(MetaDataType.Genre);
            txtTrack.Text = _mediaFile.GetMetaData(MetaDataType.TrackNumber);
            txtPublisher.Text = _mediaFile.GetMetaData(MetaDataType.Publisher);
            txtLanguage.Text = _mediaFile.GetMetaData(MetaDataType.Language);
            txtCopyright.Text = _mediaFile.GetMetaData(MetaDataType.Copyright);
            txtEncoded.Text = _mediaFile.GetMetaData(MetaDataType.EncodedBy);
            txtComments.Text = _mediaFile.GetMetaData(MetaDataType.Description);
            _bmpArt = MediaInfo.GetAlbumArt(_fileName, txtArtist.Text, txtAlbum.Text);
            if (_bmpArt != null)
            {
                pnlArt.Refresh();
            }
            _loading = false;
        }

        private void OnMetaTextChanged(object sender, EventArgs e)
        {
            if (_loading)
            {
                return;
            }
            cmdSave.Visible = true;
        }

        private void CmdNewClick(object sender, EventArgs e)
        {
            /* Select new image for media object */
            var ofd = new OpenFileDialog
                          {
                              Title = @"Select new cover art for media file",
                              Multiselect = false,
                              Filter = Filters.CoverArtFilters.ToString()
                          };
            if (ofd.ShowDialog(this) == DialogResult.Cancel)
            {
                return;
            }
            _artworkUrl = ofd.FileName;
            _bmpArt = new Bitmap(Image.FromFile(_artworkUrl));
            pnlArt.Refresh();
            cmdSave.Visible = true;
        }

        private void CmdSaveClick(object sender, EventArgs e)
        {
            /* Update meta for current media object */
            _mediaFile.SetMetaData(MetaDataType.Title, txtTitle.Text);
            _mediaFile.SetMetaData(MetaDataType.Artist, txtArtist.Text);
            _mediaFile.SetMetaData(MetaDataType.Album, txtAlbum.Text);
            _mediaFile.SetMetaData(MetaDataType.Date, txtDate.Text);
            _mediaFile.SetMetaData(MetaDataType.Genre, txtGenre.Text);
            _mediaFile.SetMetaData(MetaDataType.TrackNumber, txtTrack.Text);

            _mediaFile.SetMetaData(MetaDataType.Publisher, txtPublisher.Text);
            _mediaFile.SetMetaData(MetaDataType.Language, txtLanguage.Text);
            _mediaFile.SetMetaData(MetaDataType.Copyright, txtCopyright.Text);
            _mediaFile.SetMetaData(MetaDataType.EncodedBy, txtEncoded.Text);
            _mediaFile.SetMetaData(MetaDataType.Description, txtComments.Text);
            if (!string.IsNullOrEmpty(_artworkUrl))
            {
                _mediaFile.SetMetaData(MetaDataType.ArtworkUrl, new Uri(_artworkUrl).AbsoluteUri);
            }
            _mediaFile.SaveMetaData();
            cmdSave.Visible = false;
        }

        private void OnPanelPaint(object sender, PaintEventArgs e)
        {
            if (_bmpArt == null)
            {
                return;
            }
            /* Figure out the ratio */
            var ratioX = (double)pnlArt.Width / _bmpArt.Width;
            var ratioY = (double)pnlArt.Height / _bmpArt.Height;
            /* Use whichever multiplier is smaller */
            var ratio = ratioX < ratioY ? ratioX : ratioY;
            /* Now we can get the new height and width */
            var newHeight = Convert.ToInt32(_bmpArt.Height * ratio);
            var newWidth = Convert.ToInt32(_bmpArt.Width * ratio);
            /* Now calculate the X,Y position of the upper-left corner (one of these will always be zero) */
            var posX = Convert.ToInt32((pnlArt.Width - (_bmpArt.Width * ratio)) / 2);
            var posY = Convert.ToInt32((pnlArt.Height - (_bmpArt.Height * ratio)) / 2);
            e.Graphics.DrawImage(_bmpArt, posX, posY, newWidth, newHeight);
        }

        private void TmrMetaTick(object sender, EventArgs e)
        {
            /* Shouldn't take more than 5 seconds to read the data */
            tmrMeta.Enabled = false;
            _loading = false;
        }
    }
}

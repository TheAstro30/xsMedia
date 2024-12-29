/* xsMedia - Media Player
 * (c)2013 - 2024
 * Jason James Newland
 * KangaSoft Software, All Rights Reserved
 * Licenced under the GNU public licence */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using libolv;
using libolv.Rendering.Styles;
using xsCore.Controls;
using xsCore.Skin;
using xsMedia.Logic;
using xsMedia.Properties;
using xsPlaylist.Utils;
using xsSettings;
using xsSettings.Settings;

namespace xsMedia.Forms
{
    public partial class FrmFavorites : FormEx
    {
        private readonly TableLayoutPanel _dockPanel;
        private readonly ObjectListView _lv;
        private readonly HeaderFormatStyle _lVHeader;
        private readonly OlvColumn _lvFile;

        private IList _selectedObjects;

        public FrmFavorites()
        {
            InitializeComponent();

            _dockPanel = new TableLayoutPanel
            {
                BackColor = Color.Transparent,
                ColumnCount = 1,
                Location = new Point(0, 0),
                RowCount = 1,
                Size = new Size(235, 283)
            };

            _dockPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            _dockPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            _dockPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            _dockPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));

            var images = new ImageList();
            images.Images.Add(Resources.menuFavoriteItem);

            _lVHeader = new HeaderFormatStyle();
            _lv = new ObjectListView
            {
                View = View.Details,
                MultiSelect = true,
                HideSelection = false,
                FullRowSelect = true,
                HeaderStyle = ColumnHeaderStyle.Nonclickable,
                BorderStyle = BorderStyle.None,
                Dock = DockStyle.Fill,
                OwnerDraw = true,
                ShowItemToolTips = true,
                Visible = true,
                Enabled = true,
                HasCollapsibleGroups = false,
                ShowGroups = false,
                HeaderUsesThemes = false,
                UseExplorerTheme = false,
                HeaderFormatStyle = _lVHeader,
                SmallImageList = images
            };

            _lvFile = new OlvColumn(@"File:", "FriendlyName")
            {
                Groupable = false,
                Hideable = false,
                IsEditable = false,
                Searchable = false, 
                ImageIndex = 0,
                Width = 120
            };

            _lv.AllColumns.AddRange(new[] {_lvFile});
            _lv.Columns.AddRange(new ColumnHeader[] {_lvFile});

            _dockPanel.Controls.Add(_lv, 0, 0);

            btnAdd.Image = Resources.dlgAdd.ToBitmap();
            btnRemove.Image = Resources.dlgRemove.ToBitmap();
            btnClear.Image = Resources.dlgClear.ToBitmap();

            Controls.Add(_dockPanel);

            _lv.MouseDoubleClick += OnDoubleClick;
            _lv.SelectionChanged += OnSelectionChanged;

            btnAdd.Click += OnButtonClick;
            btnRemove.Click += OnButtonClick;
            btnClear.Click += OnButtonClick;
            btnOk.Click += OnButtonClick;

            var toolTip = new ToolTip();
            toolTip.SetToolTip(btnAdd, "Add file to favorites");
            toolTip.SetToolTip(btnRemove, "Remove selected file from favorites");
            toolTip.SetToolTip(btnClear, "Delete entire list");

            /* Apply skin format */
            SkinChanged();
        }

        /* Overrides */
        protected override void OnLoad(EventArgs e)
        {
            /* Set the forms saved position and size */
            var loc = SettingsManager.Settings.Window.FavoritesWindow.Location;
            if (loc == Point.Empty)
            {
                if (Owner != null)
                {
                    /* We set this form to centered parent (which CenterToParent property doesn't work with the main form, for whatever reason) */
                    Location = new Point(Owner.Location.X + Owner.Width / 2 - Width / 2,
                        Owner.Location.Y + Owner.Height / 2 - Height / 2);
                }
            }
            else
            {
                Location = loc;
            }
            Size = SettingsManager.Settings.Window.FavoritesWindow.Size;
            /* Populate list */
            _lv.AddObjects(SettingsManager.Settings.Favorites.Favorite);
            if (Size == MinimumSize)
            {
                OnResize(new EventArgs());
            }
            base.OnLoad(e);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            /* Save the forms position and size */
            if (WindowState == FormWindowState.Normal)
            {
                SettingsManager.Settings.Window.FavoritesWindow.Location = Location;
                SettingsManager.Settings.Window.FavoritesWindow.Size = Size;
            }
            base.OnFormClosing(e);
        }

        protected override void OnResize(EventArgs e)
        {
            if (!Visible)
            {
                return;
            }
            /* Move controls */
            _dockPanel.Size = new Size(ClientSize.Width, ClientSize.Height - 40);
            _lvFile.Width = _lv.Width;

            var buttonStartY = ClientSize.Height - 32;
            btnAdd.Location = new Point(btnAdd.Location.X, buttonStartY);
            btnRemove.Location = new Point(btnRemove.Location.X, buttonStartY);
            btnClear.Location = new Point(btnClear.Location.X, buttonStartY);

            btnOk.Location = new Point(ClientSize.Width - 86, buttonStartY);

            if (WindowState == FormWindowState.Normal)
            {
                SettingsManager.Settings.Window.FavoritesWindow.Location = Location;
                SettingsManager.Settings.Window.FavoritesWindow.Size = Size;
            }
            base.OnResize(e);
        }

        /* Public methods */
        public new void Focus()
        {
            /* Ensure listview has focus all the time */
            _lv.Focus();
        }

        /* Called externally if window is already open */
        public void AddFavorite(SettingsHistoryData data)
        {
            var favorites = SettingsManager.Settings.Favorites.Favorite;
            if (SettingsManager.GetHistoryItem(favorites, data.FilePath) != null)
            {
                /* Nothing was added */
            }
            if (favorites.Count == _lv.Items.Count)
            {
                /* Maxed out items, refresh */
                _lv.Items.RemoveAt(0);
            }
            _lv.AddObject(data);
        }

        /* Skin management - uses same color scheme as the playlist window, I mean - why not? */
        public void SkinChanged(bool refresh = false)
        {            
            /* Update listview's appearance */
            _lVHeader.Normal.BackColor = SkinManager.GetPlaylistColor("HEADER_BACKCOLOR");
            _lv.BackColor = SkinManager.GetPlaylistColor("BACKCOLOR");
            _lv.ForeColor = SkinManager.GetPlaylistColor("ITEM_FORECOLOR");
            _lv.HighlightBackgroundColor = SkinManager.GetPlaylistColor("SELECTED_BACKCOLOR");
            _lv.HighlightForegroundColor = SkinManager.GetPlaylistColor("SELECTED_FORECOLOR");
            if (!refresh)
            {
                return;
            }
            _lv.RefreshObjects(SettingsManager.Settings.Favorites.Favorite);
            _lv.Refresh();
        }

        /* Callbacks */
        private void OnSelectionChanged(object sender, EventArgs e)
        {
            var o = (ObjectListView) sender;
            _selectedObjects = o.SelectedObjects;
            var selected = _selectedObjects.Count > 0;
            btnRemove.Enabled = selected;
        }

        private void OnDoubleClick(object sender, MouseEventArgs e)
        {
            /* An item in listview is double clicked on */
            if (e.Button == MouseButtons.Left && _lv.SelectedItem != null)
            {
                Video.VideoControl.OpenFile(((SettingsHistoryData)_lv.SelectedObject).FilePath);
            }
        }

        private void OnButtonClick(object sender, EventArgs e)
        {
            var o = (Button) sender;
            var favorites = SettingsManager.Settings.Favorites.Favorite;
            switch (o.Tag.ToString())
            {
                case "ADD":
                    var path = SettingsManager.Settings.Paths.GetPath("open-file");
                    using (var ofd = new OpenFileDialog
                    {
                        Title = @"Add Media File...",
                        InitialDirectory = path.Location,
                        Multiselect = false,
                        Filter = Filters.OpenFilters.ToString()
                    })
                    {
                        if (ofd.ShowDialog(this) != DialogResult.OK)
                        {
                            return;
                        }
                        var f = new SettingsHistoryData(ofd.FileName);
                        var i = SettingsManager.AddFavorite(f);
                        if (i == -1)
                        {
                            /* Nothing was added, do nothing */
                            return;
                        }
                        if (i == _lv.Items.Count)
                        {
                            /* Maxed out items, refresh */
                            _lv.Items.RemoveAt(0);
                        }
                        /* Insert new item */
                        _lv.AddObject(f);
                    }
                    break;

                case "REMOVE":
                    if (_selectedObjects != null)
                    {
                        for (var i = _selectedObjects.Count - 1; i >= 0; i--)
                        {
                            var f = (SettingsHistoryData)_selectedObjects[i];
                            if (f != null && SettingsManager.RemoveFavorite(f))
                            {
                                _lv.RemoveObject(f);
                            }
                        }
                    }
                    btnRemove.Enabled = false;
                    break;

                case "CLEAR":
                    favorites.Clear();
                    _lv.Items.Clear();
                    break;

                case "OK":
                    Close();
                    break;
            }
        }
    }
}

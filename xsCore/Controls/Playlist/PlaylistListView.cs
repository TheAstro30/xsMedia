/* xsMedia - xsPlaylist
 * (c)2013 - 2025
 * Jason James Newland
 * KangaSoft Software, All Rights Reserved
 * Licenced under the GNU public licence */
using System;
using System.Drawing;
using System.Windows.Forms;
using libolv;
using libolv.Rendering.Styles;
using xsCore.Controls.Playlist.Playlist;
using xsCore.Skin;
using xsCore.Utils.UI;

namespace xsCore.Controls.Playlist
{
    /* Playlist listview - Threading aware for Add method
     * - I realise that this control could have been inherited from ObjectListView but it doesn't render correctly, so
     *   this is a bit of a work-around */
    public sealed class PlaylistListView : Control
    {
        private readonly UiSynchronize _sync;

        private Point _currentMousePos;
        private int _selectedIndex = -1;

        public event Action<Point> OnListRightClick;
        public event Action<int> OnItemSelected;
        public event Action<Point> OnItemRightClick;

        private readonly ObjectListView _lv;
        private readonly HeaderFormatStyle _lVHeader;

        private readonly OlvColumn _lvTrack;
        private readonly OlvColumn _lvAlbum;
        private readonly OlvColumn _lvLength;

        private Timer _tmrRightClick;
        
        /* Constructor - sets up double buffering and the rest of the magic ;) */
        public PlaylistListView()
        {
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
                          HeaderFormatStyle = _lVHeader
                      };
            _lv.SelectionChanged += OnSelectionChanged;
            _lv.MouseDoubleClick += OnDoubleClick;
            _lv.MouseDown += OnListMouseDown;
            _lv.KeyDown += OnListKeyDown;
            Controls.Add(_lv);
            _sync = new UiSynchronize(this);
            /* Create columns */
            _lvTrack = new OlvColumn(@"Track", "TitleString")
                           {
                               Groupable = false,
                               Hideable = false,
                               IsEditable = false,
                               Searchable = false,
                               Width = 120
                           };
            _lvAlbum = new OlvColumn(@"Album", "Album")
                           {
                               Groupable = false,
                               Hideable = false,
                               IsEditable = false,
                               Searchable = false,
                               Width = 140
                           };
            _lvLength = new OlvColumn(@"Length", "LengthString")
                            {
                                Groupable = false,
                                Hideable = false,
                                IsEditable = false,
                                Searchable = false, 
                                Width = 100                              
                            };
            _lv.AllColumns.AddRange(new[] {_lvTrack, _lvAlbum, _lvLength});
            _lv.Columns.AddRange(new ColumnHeader[] { _lvTrack, _lvAlbum, _lvLength });
            /* Apply skin format */
            SkinChanged();
        }

        /* Protected methods - overrides */
        protected override void OnResize(EventArgs e)
        {
            var width = ClientRectangle.Width - (_lvAlbum.Width + _lvLength.Width);
            _lvTrack.Width = width;
            base.OnResize(e);
        }

        /* Public methods */
        public new void Focus()
        {
            /* Ensure listview has focus all the time */
            _lv.Focus();
        }

        public int SelectedIndex
        {
            get { return _selectedIndex; }
            set
            {
                if (_selectedIndex > _lv.Items.Count - 1)
                {
                    return;
                }
                /* Clear current selected items */
                _lv.SelectedItems.Clear();
                _selectedIndex = value;
                var item = _lv.Items[_selectedIndex];
                item.Selected = true;
                _lv.FocusedItem = item;
                _lv.EnsureVisible(_selectedIndex);
            }
        }

        public ListView.SelectedIndexCollection SelectedIndices
        {
            get { return _lv.SelectedIndices; }
        }

        public void Add(PlaylistEntry entry)
        {
            if (InvokeRequired)
            {
                _sync.Execute(() => Add(entry));
                return;
            }
            _lv.AddObject(entry);
        }

        public void AddRange(PlaylistEntry[] entries)
        {
            if (InvokeRequired)
            {
                _sync.Execute(() => AddRange(entries));
                return;
            }
            _lv.AddObjects(entries);
        }

        public PlaylistEntry GetItemAt(int index)
        {
            if (index > _lv.Items.Count - 1)
            {
                return null;
            }
            return (PlaylistEntry) _lv.GetItem(index).RowObject;
        }

        public void RemoveAt(int index)
        {
            _lv.Items.RemoveAt(index);
        }

        public void Update(PlaylistEntry entry)
        {
            _lv.RefreshObject(entry);
        }        

        public void Clear()
        {
            if (InvokeRequired)
            {
                _sync.Execute(Clear);
                return;
            }
            _lv.Items.Clear();
        }

        public void SkinChanged()
        {
            if (InvokeRequired)
            {
                _sync.Execute(SkinChanged);
                return;
            }
            /* Update listview's appearance */            
            _lVHeader.Normal.BackColor = SkinManager.GetPlaylistColor("HEADER_BACKCOLOR");
            _lVHeader.Normal.ForeColor = SkinManager.GetPlaylistColor("HEADER_FORECOLOR");

            _lv.BackColor = SkinManager.GetPlaylistColor("BACKCOLOR");
            _lv.ForeColor = SkinManager.GetPlaylistColor("ITEM_FORECOLOR");
            _lv.HighlightBackgroundColor = SkinManager.GetPlaylistColor("SELECTED_BACKCOLOR");
            _lv.HighlightForegroundColor = SkinManager.GetPlaylistColor("SELECTED_FORECOLOR");
            /* We need to update the list view */
            if (_lv.Objects == null)
            {
                return;
            }
            /* Kind of annoying that the control doesn't do this by default; I didn't write it, so I ain't fixing it! */
            foreach (var o in _lv.Objects)
            {
                _lv.RefreshObject(o);
            }
            _lv.Refresh();
        }

        /* Private callbacks */
        private void OnSelectionChanged(object sender, EventArgs e)
        {
            _selectedIndex = _lv.SelectedIndex;
        }

        private void OnListKeyDown(object sender, KeyEventArgs e)
        {
            if (!Visible)
            {
                return;
            }
            switch (e.KeyCode)
            {
                case Keys.Return:
                    /* Most playlist editors allow you to press return to select an item for playing... */
                    if (OnItemSelected != null)
                    {
                        OnItemSelected(_lv.SelectedIndex);
                    }
                    e.SuppressKeyPress = true;
                    break;                    
            }
        }

        private void OnListMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right)
            {
                return;
            }
            /* Because of the order of events that are fired in ObjectListView, MouseDown comes BEFORE SelectionChange which is
             * kind of annoying */
            _currentMousePos = e.Location;
            if (_tmrRightClick != null)
            {
                _tmrRightClick.Tick -= TimerRightClickTick;
                _tmrRightClick.Enabled = false;
                _tmrRightClick = null;
            }
            _tmrRightClick = new Timer {Interval = 200, Enabled = true};
            _tmrRightClick.Tick += TimerRightClickTick;
        }

        private void OnDoubleClick(object sender, MouseEventArgs e)
        {
            /* An item in listview is double clicked on */           
            if (e.Button == MouseButtons.Left && _lv.SelectedItem != null && OnItemSelected != null)
            {
                OnItemSelected(_lv.SelectedIndex);
            }
        }

        private void TimerRightClickTick(object sender, EventArgs e)
        {
            _tmrRightClick.Enabled = false;
            _tmrRightClick.Tick -= TimerRightClickTick;
            _tmrRightClick = null;
            /* Check if we're clicking on an item or the list window */
            if (_lv.SelectedIndices.Count == 0)
            {
                /* List window */
                if (OnListRightClick != null)
                {
                    OnListRightClick(_currentMousePos);
                }
                return;
            }
            if (OnItemRightClick != null)
            {
                /* The first param isn't used externally, but is left here incase something gets changed in operation */
                OnItemRightClick(_currentMousePos);
            }
        }
    }
}

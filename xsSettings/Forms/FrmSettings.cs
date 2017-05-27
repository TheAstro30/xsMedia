using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using xsCore.Controls;
using xsCore.Utils.SystemUtils;
using xsSettings.Controls;
using xsSettings.Internal;

namespace xsSettings.Forms
{
    public partial class FrmSettings : FormEx
    {
        private readonly PlayerSettings _settings;

        private TreeNode _selectedNode;

        private readonly OptionCdAudio _optionCdAudio;
        private readonly OptionCddb _optionCddb;
        private readonly OptionVcd _optionVcd;
        private readonly OptionDvd _optionDvd;
        private readonly OptionNetwork _optionNetwork;
        private readonly OptionProxy _optionProxy;

        public FrmSettings()
        {
            InitializeComponent();
            /* Init settings */
            _settings = new PlayerSettings(SettingsManager.Settings);
            /* Init "panels" */
            _optionCdAudio = new OptionCdAudio(_settings.Cdda)
                                 {
                                     Size = new Size(348, 324),
                                     Location = new Point(135, 12),
                                     Visible = false
                                 };
            _optionCddb = new OptionCddb(_settings.Cdda.Cddb)
                              {
                                  Size = new Size(348, 324),
                                  Location = new Point(135, 12),
                                  Visible = false
                              };
            _optionVcd = new OptionVcd(_settings.Vcd)
                             {
                                 Size = new Size(348, 324),
                                 Location = new Point(135, 12),
                                 Visible = false
                             };
            _optionDvd = new OptionDvd(_settings.Dvd)
                             {
                                 Size = new Size(348, 324),
                                 Location = new Point(135, 12),
                                 Visible = false
                             };
            _optionNetwork = new OptionNetwork(_settings.NetworkPresets)
                                 {
                                     Size = new Size(348, 324),
                                     Location = new Point(135, 12),
                                     Visible = false
                                 };
            _optionProxy = new OptionProxy(_settings.NetworkPresets.Proxy)
                               {
                                   Size = new Size(348, 324),
                                   Location = new Point(135, 12),
                                   Visible = false
                               };
            /* Add controls */
            Controls.AddRange(new Control[]
                                  {
                                      _optionCdAudio,
                                      _optionCddb,
                                      _optionVcd,
                                      _optionDvd,
                                      _optionNetwork,
                                      _optionProxy
                                  });
            /* Treeview handler */
            tvMenu.AfterSelect += OnMenuClick;
            var node = tvMenu.Nodes[tvMenu.Nodes.IndexOfKey("nodeDisc")];
            if (node != null)
            {
                _selectedNode = node;
                tvMenu.SelectedNode = _selectedNode;
            }
            _optionCdAudio.Visible = true;
        }

        private void OnMenuClick(object sender, TreeViewEventArgs e)
        {
            /* We set all option panels to invisible */
            foreach (var s in from Control s in Controls where (s is UserControl) select s)
            {
                /* Check we aren't already viewing the selected "tab" */
                if (_selectedNode != null)
                {
                    if (_selectedNode.Name == tvMenu.SelectedNode.Name)
                    {
                        /* No need to continue */
                        return;
                    }
                }
                s.Visible = false;
            }
            _selectedNode = tvMenu.SelectedNode;
            ShowPanel(_selectedNode.Name);
        }

        private void ShowPanel(string tag)
        {
            switch (tag.ToUpper())
            {
                case "NODEDISC":
                case "NODEDISCCDDA":
                    _optionCdAudio.Visible = true;
                    break;

                case "NODEDISCCDDB":
                    _optionCddb.Visible = true;
                    break;

                case "NODEDISCVCD":
                    _optionVcd.Visible = true;
                    break;

                case "NODEDISCDVD":
                    _optionDvd.Visible = true;
                    break;

                case "NODENETWORK":
                    _optionNetwork.Visible = true;
                    break;

                case "NODENETWORKPROXY":
                    _optionProxy.Visible = true;
                    break;
            }
        }

        private void BtnOkClick(object sender, System.EventArgs e)
        {
            /* Update current settings */
            SettingsManager.Settings = new PlayerSettings(_settings);
            SettingsManager.Save(AppPath.MainDir(@"\KangaSoft\xsMedia\xsMedia.xml", true));
        }
    }
}

/* xsMedia - xsSettings
 * (c)2013 - 2024
 * Jason James Newland
 * KangaSoft Software, All Rights Reserved
 * Licenced under the GNU public licence */
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using xsCore.Controls;
using xsCore.Utils.SystemUtils;
using xsSettings.Controls;
using xsSettings.Internal;

namespace xsSettings.Forms
{
    public sealed class FrmSettings : FormEx
    {
        private readonly TreeView _tvMenu;
        private readonly PlayerSettings _settings;
        private TreeNode _selectedNode;

        private readonly OptionPlayback _optionPlayback;
        private readonly OptionVideo _optionVideo;
        private readonly OptionCdAudio _optionCdAudio;
        private readonly OptionCddb _optionCddb;
        private readonly OptionVcd _optionVcd;
        private readonly OptionDvd _optionDvd;
        private readonly OptionNetwork _optionNetwork;
        private readonly OptionProxy _optionProxy;

        public FrmSettings()
        {
            Text = @"xsMedia Playback";
            ClientSize = new Size(495, 389);
            Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            /* Controls */
            _tvMenu = new TreeView
            {
                Location = new Point(12, 12),
                Size = new Size(117, 324),
                TabIndex = 0
            };

            var btnOk = new Button
            {
                DialogResult = DialogResult.OK,
                Location = new Point(327, 357),
                Size = new Size(75, 23),
                TabIndex = 1,
                Text = @"OK",
                UseVisualStyleBackColor = true
            };

            var btnCancel = new Button
            {
                DialogResult = DialogResult.Cancel,
                Location = new Point(408, 357),
                Size = new Size(75, 23),
                TabIndex = 2,
                Text = @"Cancel",
                UseVisualStyleBackColor = true
            };

            /* Init settings */
            _settings = new PlayerSettings(SettingsManager.Settings);

            /* Init "panels" */
            _optionPlayback = new OptionPlayback(_settings.Player)
            {
                Size = new Size(348, 324),
                Location = new Point(135, 12),
                Visible = false
            };
            _optionVideo = new OptionVideo(_settings.Player)
            {
                Size = new Size(348, 324),
                Location = new Point(135, 12),
                Visible = false
            };
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
            /* Build treeview nodes */
            _tvMenu.Nodes.AddRange(new[]
            {
                new TreeNode("Playback", new[]
                {
                    new TreeNode("General"),
                    new TreeNode("Video")
                }),

                new TreeNode("Disc", new[]
                {
                    new TreeNode("CD Audio", new[]
                    {
                        new TreeNode("CDDB")
                    }),
                    new TreeNode("VCD/SVCD"),
                    new TreeNode("DVD")
                }),

                new TreeNode("Network", new[]
                {
                    new TreeNode("HTTP Proxy")
                })
            });

            /* Add controls */
            Controls.AddRange(new Control[]
            {
                _tvMenu,
                btnOk,
                btnCancel,
                _optionPlayback,
                _optionVideo,
                _optionCdAudio,
                _optionCddb,
                _optionVcd,
                _optionDvd,
                _optionNetwork,
                _optionProxy
            });

            AcceptButton = btnOk;
            _tvMenu.AfterSelect += OnMenuClick;
            btnOk.Click += BtnOkClick;
            /* Select a node */
            var n = GetNode(_settings.Option.Id);
            if (n == null)
            {
                return;
            }
            n.Expand();
            _tvMenu.SelectedNode = n;
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            /* Store current "tab" */
            SettingsManager.Settings.Option.Id = _tvMenu.SelectedNode.Text;
            base.OnFormClosing(e);
        }

        private void OnMenuClick(object sender, TreeViewEventArgs e)
        {            
            /* We set all option panels to invisible */
            foreach (var s in from Control s in Controls where s is UserControl select s)
            {
                /* Check we aren't already viewing the selected "tab" */
                if (_selectedNode != null)
                {
                    if (_selectedNode.Text.Equals(_tvMenu.SelectedNode.Text, StringComparison.InvariantCultureIgnoreCase))
                    {
                        /* No need to continue */
                        return;
                    }
                }
                s.Visible = false;
            }
            _selectedNode = _tvMenu.SelectedNode;
            ShowPanel(_selectedNode);
        }

        private void ShowPanel(TreeNode node)
        {
            switch (node.Text.ToUpper())
            {
                case "PLAYBACK":
                case "GENERAL":
                    _optionPlayback.Visible = true;
                    break;

                case "VIDEO":
                    _optionVideo.Visible = true;
                    break;

                case "DISC":
                case "CD AUDIO":
                    _optionCdAudio.Visible = true;
                    break;

                case "CDDB":
                    _optionCddb.Visible = true;
                    break;

                case "VCD/SVCD":
                    _optionVcd.Visible = true;
                    break;

                case "DVD":
                    _optionDvd.Visible = true;
                    break;

                case "NETWORK":
                    _optionNetwork.Visible = true;
                    break;

                case "HTTP PROXY":
                    _optionProxy.Visible = true;
                    break;
            }            
        }

        /* Find node by text */
        private TreeNode GetNode(string text)
        {
            TreeNode itemNode = null;
            foreach (TreeNode node in _tvMenu.Nodes)
            {
                if (node.Text.Equals(text))
                {
                    return node;
                }
                itemNode = GetNode(text, node);
                if (itemNode != null)
                {
                    break;
                }
            }
            return itemNode;
        }

        private static TreeNode GetNode(string text, TreeNode rootNode)
        {
            foreach (TreeNode node in rootNode.Nodes)
            {
                if (node.Text.Equals(text))
                {
                    return node;
                }
                /* Recursion */
                var next = GetNode(text, node);
                if (next != null)
                {
                    return next;
                }
            }
            return null;
        }

        private void BtnOkClick(object sender, EventArgs e)
        {
            /* Update current settings */
            SettingsManager.Settings = new PlayerSettings(_settings);
            SettingsManager.Save(AppPath.MainDir(@"\KangaSoft\xsMedia\xsMedia.xml", true));
        }
    }
}

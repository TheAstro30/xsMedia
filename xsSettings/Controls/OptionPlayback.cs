/* xsMedia - Media Player
 * (c)2013 - 2024
 * Jason James Newland
 * KangaSoft Software, All Rights Reserved
 * Licenced under the GNU public licence */
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using xsCore.PlayerControls.Controls;
using xsCore.Utils;
using xsCore.Utils.SystemUtils;
using xsSettings.Forms;
using xsSettings.Properties;
using xsSettings.Settings;
using xsSettings.Settings.Enums;

namespace xsSettings.Controls
{
    public partial class OptionPlayback : OptionBase
    {
        private readonly SettingsPlayer _player;

        public OptionPlayback(SettingsPlayer player) : base("General Playback Options")
        {
            InitializeComponent();

            _player = player;
            
            foreach (var index in from c in (MediaCounter.TimeDisplay[]) Enum.GetValues(typeof (MediaCounter.TimeDisplay)) let index = cmbCounter.Items.Add(EnumUtils.GetDescriptionFromEnumValue(c)) where c == _player.CounterType select index)
            {
                cmbCounter.SelectedIndex = index;
            }

            txtJump.Text = _player.JumpStep.ToString(CultureInfo.InvariantCulture);
            txtVol.Text = _player.VolumeStep.ToString(CultureInfo.InvariantCulture);

            cmbSpeed.Items.AddRange(new object[]
            {
                "0.25", "0.5", "0.75", "Normal", "1.25", "1.75", "2"
            });
            /* Match the set value and select it in the combo */
            var s = _player.Speed.ToString(CultureInfo.InvariantCulture);
            var m = cmbSpeed.FindStringExact(s == "1" ? "Normal" : s);
            if (m != -1)
            {
                cmbSpeed.SelectedIndex = m;
            }

            foreach (var index in from l in (PlaybackLoopMode[])Enum.GetValues(typeof(PlaybackLoopMode)) let index = cmbLoop.Items.Add(EnumUtils.GetDescriptionFromEnumValue(l)) where l == _player.Loop select index)
            {
                cmbLoop.SelectedIndex = index;
            }

            btnAdd.Image = Resources.dlgAdd.ToBitmap();
            btnRemove.Image = Resources.dlgRemove.ToBitmap();

            txtSoundFont.Text = GetSoundFont();

            /* Handlers */
            cmbCounter.SelectedIndexChanged += OnOptionChanged;
            txtJump.TextChanged += OnOptionChanged;
            txtVol.TextChanged += OnOptionChanged;
            cmbSpeed.SelectedIndexChanged += OnOptionChanged;
            cmbLoop.SelectedIndexChanged += OnOptionChanged;
            btnAdd.Click += OnOptionChanged;
            btnRemove.Click += OnOptionChanged;
        }        

        #region Option changed callback
        private void OnOptionChanged(object sender, EventArgs e)
        {
            int t;
            var ctl = (Control) sender;
            switch (ctl.Tag.ToString())
            {
                case "COUNTER":
                    _player.CounterType = (MediaCounter.TimeDisplay) cmbCounter.SelectedIndex;
                    break;

                case "SECONDS":
                    if (int.TryParse(txtJump.Text, out t))
                    {
                        if (t < 5)
                        {
                            t = 5;
                        }
                        if (t > 30)
                        {
                            t = 30;
                        }
                    }
                    else
                    {
                        t = 5;
                    }
                    _player.JumpStep = t;
                    break;

                case "STEPS":
                    if (int.TryParse(txtVol.Text, out t))
                    {
                        if (t < 2)
                        {
                            t = 2;
                        }
                        if (t > 10)
                        {
                            t = 10;
                        }
                    }
                    else
                    {
                        t = 5;
                    }
                    _player.VolumeStep = t;
                    break;

                case "SPEED":
                    /* Convert the string value to a float value */
                    float f;
                    var s = cmbSpeed.Items[cmbSpeed.SelectedIndex].ToString();
                    if (s.Equals("Normal"))
                    {
                        f = 1;
                    }
                    else
                    {
                        if (!float.TryParse(s, out f))
                        {
                            return;
                        }
                    }
                    _player.Speed = f;
                    break;

                case "LOOP":
                    _player.Loop = (PlaybackLoopMode) cmbLoop.SelectedIndex;
                    break;

                case "ADD":                    
                    var path = Settings.Paths.GetPath("sound-font");
                    var sfFile = GetSoundFont();
                    if (!string.IsNullOrEmpty(sfFile) && sfFile.StartsWith(@"\"))
                    {
                        /* Persist with app local path */
                        path.Location = Path.GetDirectoryName(AppPath.MainDir(sfFile));
                    }
                    using (var ofd = new OpenFileDialog
                    {
                        Title = @"Choose a sound font file",
                        Multiselect = false,
                        InitialDirectory = path.Location,
                        Filter = @"Sound font files (*.sf2)|*.sf2"
                    })
                    {
                        if (ofd.ShowDialog(this) == DialogResult.OK)
                        {
                            var file = ofd.FileName;
                            path.Location = Path.GetDirectoryName(file);
                            var sf = AppPath.MainDir(file);
                            AddSoundFont(sf);
                            txtSoundFont.Text = sf;                            
                        }
                    }
                    break;

                case "REMOVE":
                    RemoveSoundFont();
                    txtSoundFont.Text = string.Empty;
                    break;
            }
        }
        #endregion

        #region Private sound font methods (SF2)
        private string GetSoundFont()
        {
            foreach (var s in _player.Options.Option.Where(s => s.Id.ToLower().Equals("--soundfont")))
            {
                return s.Data;
            }
            return string.Empty;
        }

        private void AddSoundFont(string fileName)
        {
            foreach (var s in _player.Options.Option.Where(s => s.Id.ToLower().Equals("--soundfont")))
            {
                s.Data = fileName;
                break;
            }
            /* Still here? */
            var m = new SettingsMediaOptions.MediaOption("--soundfont", fileName);
            _player.Options.Option.Add(m);
        }

        private void RemoveSoundFont()
        {
            foreach (var s in _player.Options.Option.Where(s => s.Id.ToLower().Equals("--soundfont")))
            {
                _player.Options.Option.Remove(s);
                break;
            }
        }
        #endregion
    }
}

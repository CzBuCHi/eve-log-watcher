using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using eve_log_watcher.Properties;

namespace eve_log_watcher
{
    public partial class FormConfig : Form
    {
        private bool _Changed;

        public FormConfig() {
            InitializeComponent();

            foreach (PictureBox pictureBox in Controls.OfType<PictureBox>()) {
                string colorName = (string)pictureBox.Tag;
                pictureBox.BackColor = (Color) Settings.Default[colorName];
            }

            kosHotkey.Hotkey = Settings.Default.kosCheckKey & ~cModifiers;
            kosHotkey.HotkeyModifiers = Settings.Default.kosCheckKey & cModifiers;
        }

        private void OnChange(PictureBox pictureBox) {
            string colorName = (string) pictureBox.Tag;
            if ((Color)Settings.Default[colorName] != pictureBox.BackColor && !_Changed) {
                _Changed = true;
                Text = @"Configuration (unsaved)";
            }
        }

        private void pictureBoxColor_Click(object sender, EventArgs e) {
            PictureBox pictureBox = (PictureBox) sender;
            colorDialog.Color = pictureBox.BackColor;
            if (colorDialog.ShowDialog() == DialogResult.OK) {
                pictureBox.BackColor = colorDialog.Color;
                OnChange(pictureBox);
            }
        }

        private void buttonClose_Click(object sender, EventArgs e) {
            if (_Changed) {
                if (MessageBox.Show(@"There are some unsaved changes ... are u sure wanna close without saving?", @"Confirm close", MessageBoxButtons.YesNo) == DialogResult.Yes) {
                    DialogResult = DialogResult.Cancel;
                    return;
                }
            }
            DialogResult = DialogResult.OK;
        }

        private const Keys cModifiers = Keys.Control | Keys.Shift | Keys.Alt;

        private void buttonSave_Click(object sender, EventArgs e) {
            foreach (PictureBox pictureBox in Controls.OfType<PictureBox>()) {
                string colorName = (string) pictureBox.Tag;
                Settings.Default[colorName] = pictureBox.BackColor;
            }
            
            Settings.Default.kosCheckKey = (kosHotkey.Hotkey & ~cModifiers) | (kosHotkey.HotkeyModifiers & cModifiers);            
            Settings.Default.Save();

            _Changed = false;
            Text = @"Configuration";
        }
    }
}

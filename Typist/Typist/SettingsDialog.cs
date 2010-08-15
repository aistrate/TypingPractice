using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Typist
{
    public partial class SettingsDialog : Form
    {
        public SettingsDialog()
        {
            InitializeComponent();
        }

        private void SettingsDialog_Load(object sender, EventArgs e)
        {
        }

        private void SettingsDialog_VisibleChanged(object sender, EventArgs e)
        {
            btnOk.Focus();
        }

        public UserSettings UserSettings
        {
            get { return userSettings; }
            set
            {
                userSettings = value;

                chkBeepOnError.Checked = userSettings.BeepOnError;
                chkAllowBackspace.Checked = userSettings.AllowBackspace;
                chkVisibleNewlines.Checked = userSettings.VisibleNewlines;
                chkCountWhitespaceAsWordChars.Checked = userSettings.CountWhitespaceAsWordChars;
                chkCountErrorsAsWordChars.Checked = userSettings.CountErrorsAsWordChars;
                chkAskBeforeCloseDuringPractice.Checked = userSettings.AskBeforeCloseDuringPractice;
                chkShowCursorWhenPaused.Checked = userSettings.ShowCursorWhenPaused;

                chkPauseAfterElapsed.Checked = userSettings.PauseAfterElapsed > 0;
                displayPauseAfterElapsed();
            }
        }
        private UserSettings userSettings = new UserSettings();

        private void btnOk_Click(object sender, EventArgs e)
        {
            userSettings = new UserSettings()
            {
                BeepOnError = chkBeepOnError.Checked,
                AllowBackspace = chkAllowBackspace.Checked,
                VisibleNewlines = chkVisibleNewlines.Checked,
                CountWhitespaceAsWordChars = chkCountWhitespaceAsWordChars.Checked,
                CountErrorsAsWordChars = chkCountErrorsAsWordChars.Checked,
                AskBeforeCloseDuringPractice = chkAskBeforeCloseDuringPractice.Checked,
                ShowCursorWhenPaused = chkShowCursorWhenPaused.Checked,

                PauseAfterElapsed = chkPauseAfterElapsed.Checked ? readInt(txtPauseAfterElapsed) : 0,
            };
        }

        private void chkPauseAfterElapsed_CheckedChanged(object sender, EventArgs e)
        {
            displayPauseAfterElapsed();
        }

        private void displayPauseAfterElapsed()
        {
            txtPauseAfterElapsed.Enabled = chkPauseAfterElapsed.Checked;

            if (chkPauseAfterElapsed.Checked && userSettings.PauseAfterElapsed > 0)
                txtPauseAfterElapsed.Text = userSettings.PauseAfterElapsed.ToString();
            else
                txtPauseAfterElapsed.Text = "";
        }

        private int readInt(TextBox textBox)
        {
            int number;
            int.TryParse(textBox.Text, out number);
            number = Math.Max(0, number);

            textBox.Text = number > 0 ? number.ToString() : "";
            return number;
        }

        private void txtPauseAfterElapsed_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !(char.IsControl(e.KeyChar) || char.IsDigit(e.KeyChar));
        }

        private void txtPauseAfterElapsed_Leave(object sender, EventArgs e)
        {
            readInt(txtPauseAfterElapsed);
        }
    }
}

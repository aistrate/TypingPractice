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
        private const bool showReleaseDebugButtons = true;

        public SettingsDialog()
        {
            InitializeComponent();

            btnPredefRelease.Visible = showReleaseDebugButtons;
            btnPredefDebug.Visible = showReleaseDebugButtons;
        }

        private void SettingsDialog_Load(object sender, EventArgs e)
        {
        }

        private void SettingsDialog_VisibleChanged(object sender, EventArgs e)
        {
            resetCheckBoxesBackColor();
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
                chkWordWrap.Checked = userSettings.WordWrap;
                chkVisibleNewlines.Checked = userSettings.VisibleNewlines;
                chkRemoveEndOfLineSpaces.Checked = userSettings.RemoveEndOfLineSpaces;
                chkRemoveMultipleWhitespace.Checked = userSettings.RemoveMultipleWhitespace;
                chkRememberLastImportedFile.Checked = userSettings.RememberLastImportedFile;
                chkHideStatisticsWhileTyping.Checked = userSettings.HideStatisticsWhileTyping;
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
                WordWrap = chkWordWrap.Checked,
                VisibleNewlines = chkVisibleNewlines.Checked,
                RemoveEndOfLineSpaces = chkRemoveEndOfLineSpaces.Checked,
                RemoveMultipleWhitespace = chkRemoveMultipleWhitespace.Checked,
                RememberLastImportedFile = chkRememberLastImportedFile.Checked,
                HideStatisticsWhileTyping = chkHideStatisticsWhileTyping.Checked,
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

        private void resetCheckBoxesBackColor()
        {
            Color color = Color.FromKnownColor(KnownColor.Control);

            foreach (CheckBox checkBox in this.Controls.OfType<CheckBox>())
                checkBox.BackColor = color;

            lblSeconds.BackColor = color;
        }

        private void setCheckBox(CheckBox checkBox, bool isChecked)
        {
            checkBox.Checked = isChecked;
            checkBox.BackColor = Color.Silver;

            if (checkBox == chkPauseAfterElapsed)
                lblSeconds.BackColor = Color.Silver;
        }

        private void btnPredefChars_Click(object sender, EventArgs e)
        {
            resetCheckBoxesBackColor();

            setCheckBox(chkWordWrap, false);
            setCheckBox(chkRemoveEndOfLineSpaces, true);
            setCheckBox(chkRemoveMultipleWhitespace, true);
        }

        private void btnPredefArticle_Click(object sender, EventArgs e)
        {
            resetCheckBoxesBackColor();

            setCheckBox(chkWordWrap, true);
            setCheckBox(chkRemoveEndOfLineSpaces, true);
            setCheckBox(chkRemoveMultipleWhitespace, true);
        }

        private void btnPredefCode_Click(object sender, EventArgs e)
        {
            resetCheckBoxesBackColor();

            setCheckBox(chkWordWrap, false);
            setCheckBox(chkRemoveEndOfLineSpaces, true);
            setCheckBox(chkRemoveMultipleWhitespace, false);
        }

        private void btnPredefRelease_Click(object sender, EventArgs e)
        {
            resetCheckBoxesBackColor();

            setCheckBox(chkCountWhitespaceAsWordChars, true);
            setCheckBox(chkCountErrorsAsWordChars, false);
            setCheckBox(chkShowCursorWhenPaused, false);

            setCheckBox(chkAskBeforeCloseDuringPractice, true);
            setCheckBox(chkPauseAfterElapsed, true);
            txtPauseAfterElapsed.Text = "10";
        }

        private void btnPredefDebug_Click(object sender, EventArgs e)
        {
            resetCheckBoxesBackColor();

            setCheckBox(chkAskBeforeCloseDuringPractice, false);
            setCheckBox(chkPauseAfterElapsed, false);
        }
    }
}

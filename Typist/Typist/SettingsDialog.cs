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
            };
        }
    }
}

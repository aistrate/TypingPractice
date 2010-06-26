using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace WindowsFormsApplication1
{
    public partial class CharTrainingCreator : Form
    {
        public CharTrainingCreator()
        {
            InitializeComponent();
        }

        private void CharTrainingCreator_Load(object sender, EventArgs e)
        {
            txtOutputFolder.Select(txtOutputFolder.Text.Length, 0);
            cbSaveToFolder_CheckedChanged(cbSaveToFolder, EventArgs.Empty);

            txtLettersLower.Focus();
        }

        private void btnClearText_Click(object sender, EventArgs e)
        {
            txtResult.Text = "";
        }

        private void btnClearFrequencies_Click(object sender, EventArgs e)
        {
            TextBox[] frequencyTextBoxes = new TextBox[] {
                txtLettersLower, txtLettersUpper, txtDigits, txtDigitSymbols,
                txtEasySymbols, txtEasySymbolsShift, txtHardSymbols, txtHardSymbolsShift };

            if (frequencyTextBoxes.Where(tb => !string.IsNullOrEmpty(tb.Text) && readInt(tb) != 0).Count() > 0)
            {

                DialogResult dialogResult = MessageBox.Show("Are you sure you want to clear frequency TextBoxes?", "Question",
                                                            MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);

                if (dialogResult == DialogResult.Yes)
                    foreach (TextBox textBox in frequencyTextBoxes)
                        textBox.Text = "";
            }
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            char[] characters = generateCycle(txtLettersLower, lblLettersLower)
                        .Concat(generateCycle(txtLettersUpper, lblLettersUpper))
                        .Concat(generateCycle(txtDigits, lblDigits))
                        .Concat(generateCycle(txtDigitSymbols, lblDigitSymbols))
                        .Concat(generateCycle(txtEasySymbols, lblEasySymbols))
                        .Concat(generateCycle(txtEasySymbolsShift, lblEasySymbolsShift))
                        .Concat(generateCycle(txtHardSymbols, lblHardSymbols))
                        .Concat(generateCycle(txtHardSymbolsShift, lblHardSymbolsShift))
                        .ToArray();

            int paragraphCount = readInt(txtParagraphs),
                lineCount = readInt(txtLinesPerParagraph),
                groupCount = readInt(txtGroupsPerLine),
                charCount = readInt(txtCharsPerGroup);

            Random random = new Random();

            if (!cbSaveToFolder.Checked)
            {
                if (!cbAppend.Checked)
                    txtResult.Text = "";

                txtResult.Text += generateRandomText(random, characters, paragraphCount, lineCount, groupCount, charCount) + "\r\n";
            }
            else
            {
                int from = readInt(txtFileNumbersFrom), to = readInt(txtFileNumbersTo),
                    filesCreated = 0;

                if (characters.Length > 0 && from * to != 0 && from <= to)
                {
                    try
                    {
                        string outputFolder = txtOutputFolder.Text.Trim();
                        if (outputFolder == "")
                        {
                            MessageBox.Show("Output folder name is empty.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            txtOutputFolder.Focus();
                            return;
                        }

                        for (int n = from; n <= to; n++)
                        {
                            using (StreamWriter sw = new StreamWriter(string.Format("{0}\\Ex{1:000}.txt", outputFolder, n)))
                                sw.Write(generateRandomText(random, characters, paragraphCount, lineCount, groupCount, charCount));

                            filesCreated++;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        if (filesCreated == 0)
                            return;
                    }
                }

                MessageBox.Show(string.Format("{0} files were created.", filesCreated),
                                "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private string generateRandomText(Random random, char[] characters, int paragraphCount, int lineCount, int groupCount, int charCount)
        {
            StringBuilder result = new StringBuilder();
            int maxCharIndex = characters.Length;

            if (maxCharIndex > 0)
            {
                for (int paragraph = 0; paragraph < paragraphCount; paragraph++)
                {
                    for (int line = 0; line < lineCount; line++)
                    {
                        for (int group = 0; group < groupCount; group++)
                        {
                            for (int c = 0; c < charCount; c++)
                                result.Append(characters[random.Next(maxCharIndex)]);

                            result.Append(group == groupCount - 1 ? "" :
                                          (group + 1) % 3 == 0 ? "  " :
                                                                    " ");
                        }

                        result.Append("\r\n");
                    }

                    if (paragraph < paragraphCount - 1)
                        result.Append("\r\n");
                }
            }

            return result.ToString();
        }

        private char[] generateCycle(TextBox textBox, Label label)
        {
            return Enumerable.Repeat(label.Text.Replace("&&", "&"), readInt(textBox))
                             .Aggregate(string.Empty, (s1, s2) => s1 + s2)
                             .ToCharArray();
        }

        private int readInt(TextBox textBox)
        {
            int number;
            int.TryParse(textBox.Text, out number);
            number = Math.Abs(number);
            
            textBox.Text = number.ToString();
            return number;
        }

        private void cbSaveToFolder_CheckedChanged(object sender, EventArgs e)
        {
            txtOutputFolder.Enabled = cbSaveToFolder.Checked;
            btnBrowse.Enabled = cbSaveToFolder.Checked;
            txtFileNumbersFrom.Enabled = cbSaveToFolder.Checked;
            txtFileNumbersTo.Enabled = cbSaveToFolder.Checked;

            txtResult.Enabled = !cbSaveToFolder.Checked;
        }

        FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog() { ShowNewFolderButton = true };

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            if (txtOutputFolder.Text.Trim() != "")
                folderBrowserDialog.SelectedPath = txtOutputFolder.Text.Trim();

            DialogResult dialogResult = folderBrowserDialog.ShowDialog();
            if (dialogResult == DialogResult.OK)
                txtOutputFolder.Text = folderBrowserDialog.SelectedPath;

            txtOutputFolder.Select(txtOutputFolder.Text.Length, 0);
        }
    }
}

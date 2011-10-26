using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using HtmlAgilityPack;

namespace TypingTextCreator
{
    public partial class TypingTextCreator : Form
    {
        public TypingTextCreator()
        {
            InitializeComponent();
        }

        private void TypingTextCreator_Load(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.OutputFolder != "")
                txtOutputFolder.Text = Properties.Settings.Default.OutputFolder;
            else
                txtOutputFolder.Text = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            txtOutputFolder.Select(txtOutputFolder.Text.Length, 0);

            txtUrl.Focus();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            Properties.Settings.Default.OutputFolder = txtOutputFolder.Text.Trim();
            Properties.Settings.Default.Save();

            base.OnClosing(e);
        }

        FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog() { ShowNewFolderButton = true };

        private void btnReadUrl_Click(object sender, EventArgs e)
        {
            string remoteUri = txtUrl.Text.Trim();

            if (remoteUri == "")
            {
                MessageBox.Show("URL is empty.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                WebClient webClient = new WebClient();
                byte[] dataBuffer = webClient.DownloadData(remoteUri);
                MemoryStream memoryStream = new MemoryStream(dataBuffer);
                
                HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                doc.Load(memoryStream, true);
                
                string text = "";
                string title = "";

                HtmlNodeCollection headings = doc.DocumentNode.SelectNodes("//h1[@class='firstHeading']");

                if (headings != null)
                {
                    title = headings[0].InnerText;
                    text = title + "\r\n\r\n";

                    txtFileName.Text = string.Format(@"{0}.txt", replaceNonAscii(title));
                }

                HtmlNodeCollection divs = doc.DocumentNode.SelectNodes("//div[@class='mw-content-ltr']") ??
                                          doc.DocumentNode.SelectNodes("//div[@id='bodyContent']");

                if (divs != null)
                {
                    int paragraphCount = 0;

                    foreach (HtmlNode node in divs[0].ChildNodes)
                    {
                        if (isTocOrH2(node) || node.ChildNodes.Any(c => isTocOrH2(c)))
                            break;

                        if (node.InnerText.Trim() != "")
                        {
                            if (node.Name == "p")
                            {
                                text += extractText(node) + "\r\n\r\n";
                                paragraphCount++;
                            }
                            else if (paragraphCount > 0 && node.Name == "ul" || node.Name == "ol")
                            {
                                foreach (HtmlNode listItem in node.SelectNodes("li"))
                                    text += "- " + extractText(listItem) + "\r\n";
                                text += "\r\n";
                                paragraphCount++;
                            }
                        }
                    }
                }

                txtText.Text = text;

                MessageBox.Show("URL was read successfully.", "OK", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool hasClass(HtmlNode node, string className)
        {
            return node.Attributes["class"] != null &&
                   node.Attributes["class"].Value == className;
        }

        private bool isTocOrH2(HtmlNode node)
        {
            return (node.Name == "table" && hasClass(node, "toc")) ||
                    node.Name == "h2";
        }

        private string extractText(HtmlNode node)
        {
            var sups = node.SelectNodes("sup");

            if (sups != null)
                foreach (HtmlNode sup in sups)
                    if (sup.Attributes.Count == 0)
                        sup.InnerHtml = "^" + sup.InnerText;
            
            return HtmlEntity.DeEntitize(node.InnerText.Trim().Replace("&#160;", " "));
        }

        private string repeatString(string s, int times)
        {
            return string.Join("", Enumerable.Repeat(s, times).ToArray());
        }

        private void btnConvert_Click(object sender, EventArgs e)
        {
            txtText.Text = replaceNonAscii(txtText.Text);

            string[] problemStrings = new string[] { "\\*", "\\[", @"\bsee\b" };
            Match firstMatch = null;
            
            List<string> problems = new List<string>();
            foreach (string problemString in problemStrings)
            {
                MatchCollection matches = Regex.Matches(txtText.Text, problemString, RegexOptions.IgnoreCase);
                if (matches.Count > 0)
                {
                    problems.Add(string.Format(@"""{0}""   ({1})", problemString, matches.Count));

                    if (firstMatch == null || matches[0].Index < firstMatch.Index)
                        firstMatch = matches[0];
                }
            }

            if (problems.Count == 0)
                MessageBox.Show("Text is clean.", "OK", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
            {
                problems.Insert(0, "");
                MessageBox.Show("Problems:\n" + string.Join("\n   ", problems.ToArray()), "Warning",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);

                txtText.Select(firstMatch.Index, firstMatch.Length);
                txtText.Focus();
            }
        }

        private string replaceNonAscii(string text)
        {
            text = Regex.Replace(text, @"\[\d+\]", "");
            text = Regex.Replace(text, @"\[[a-z]\]", "");
            text = Regex.Replace(text, @"\s*\[citation needed\]", "");
            text = Regex.Replace(text, @"\s*\[necesită citare\]", "");
            text = Regex.Replace(text, @"\[update\]", "");
            text = Regex.Replace(text, @"\[nb \d+\]", "");
            text = Regex.Replace(text, @"\[[Nn]otes? \d+\]", "");
            text = Regex.Replace(text, @"simplified Chinese: \w+;\s*", "");
            text = Regex.Replace(text, @"traditional Chinese: \w+;\s*", "");
            text = Regex.Replace(text, @"\s*listen\s+\(help·info\)", "");
            text = Regex.Replace(text, @"\s*\(\s*listen\)", "");
            text = Regex.Replace(text, @"English pronunciation: /(\w|\s)+/;\s*", "");
            text = Regex.Replace(text, @"\(English pronunciation: /(\w|\s)+/\)", "");
            text = Regex.Replace(text, @"\s*\((pronounced|i) /(\w|\s)+/(,? or /(\w|\s)+/)*\)", "");
            text = Regex.Replace(text, @"(pronounced|i) /(\w|\s)+/(,? or /(\w|\s)+/)*;\s*", "");
            text = Regex.Replace(text, @"\s*IPA: \[(\w| )+\]", "");

            text = text.Replace('ă', 'a').Replace('Ă', 'A')
                       .Replace('â', 'a').Replace('Â', 'A')
                       .Replace('î', 'i').Replace('Î', 'I')
                       .Replace('ş', 's').Replace('Ş', 'S')
                       .Replace('ţ', 't').Replace('Ţ', 'T')

                       .Replace('á', 'a').Replace('Á', 'A')
                       .Replace('à', 'a').Replace('À', 'A')
                       .Replace('å', 'a').Replace('Å', 'A')
                       .Replace('ā', 'a').Replace('Ā', 'A')
                       .Replace('ã', 'a').Replace('Ã', 'A')
                       .Replace('ǎ', 'a').Replace('Ǎ', 'A')
                       .Replace("ä", "ae").Replace("Ä", "AE")
                       .Replace("æ", "ae").Replace("Æ", "AE")
                       .Replace('é', 'e').Replace('É', 'E')
                       .Replace('è', 'e').Replace('È', 'E')
                       .Replace('ê', 'e').Replace('Ê', 'E')
                       .Replace('ë', 'e').Replace('Ë', 'E')
                       .Replace('ē', 'e').Replace('Ē', 'E')
                       .Replace('í', 'i').Replace('Í', 'I')
                       .Replace('ì', 'i').Replace('Ì', 'I')
                       .Replace('ï', 'i').Replace('Ï', 'I')
                       .Replace('ī', 'i').Replace('Ī', 'I')
                       .Replace('ǐ', 'i').Replace('Ǐ', 'I')
                       .Replace('ĭ', 'i').Replace('Ĭ', 'I')
                       .Replace('ı', 'i')
                       .Replace('İ', 'I')
                       .Replace('ý', 'y').Replace('Ý', 'Y')
                       .Replace('ó', 'o').Replace('Ó', 'O')
                       .Replace('ò', 'o').Replace('Ò', 'O')
                       .Replace('ô', 'o').Replace('Ô', 'O')
                       .Replace('ø', 'o').Replace('Ø', 'O')
                       .Replace('ō', 'o').Replace('Ō', 'O')
                       .Replace("ö", "oe").Replace("Ö", "OE")
                       .Replace("œ", "oe").Replace("Œ", "OE")
                       .Replace('ú', 'u').Replace('Ú', 'U')
                       .Replace('ù', 'u').Replace('Ù', 'U')
                       .Replace('ū', 'u').Replace('Ū', 'U')
                       .Replace('ǔ', 'u').Replace('Ǔ', 'U')
                       .Replace("ü", "ue").Replace("Ü", "UE")

                       .Replace('ç', 'c').Replace('Ç', 'C')
                       .Replace('ğ', 'g').Replace('Ğ', 'G')
                       .Replace("ñ", "ny").Replace("Ñ", "NY")
                       .Replace('ń', 'n').Replace('Ń', 'N')
                       .Replace('ś', 's').Replace('Ś', 'S')
                       .Replace("ß", "ss")

                       .Replace("α", "Alpha")
                       .Replace("β", "Beta")
                       .Replace(" °C", " C").Replace("°C", " C")
                       .Replace(" °F", " F").Replace("°F", " F")
                       .Replace("®", "(R)")
                       .Replace('†', '+')
                       .Replace('¡', '!')
                       .Replace('¿', '?')
                       .Replace("€", "EUR")
                       .Replace("́", "")

                       .Replace('„', '"').Replace('”', '"').Replace('“', '"')
                       .Replace('’', '\'').Replace('‘', '\'')
                       .Replace('ʻ', '\'')
                       .Replace("« ", "\"").Replace(" »", "\"")
                       .Replace('«', '"').Replace('»', '"')
                       .Replace("—", "--")
                       .Replace('–', '-').Replace('−', '-')
                       .Replace('²', '2')
                       .Replace('³', '3')
                       .Replace("¼", " 1/4")
                       .Replace("½", " 1/2")
                       .Replace("¾", " 3/4")
                       .Replace('×', 'x')
                       .Replace('ª', 'a')
                       .Replace("…", "...")
                       .Replace(" .", ".")
                       .Replace(" ,", ",");

            return Regex.Replace(text, @"[^A-Za-z0-9\s`~!@#$%\^&*();',./:""<>?\-=\[\]\\_+{}\|]", "*");
        }

        private void btnBrowseOutputFolder_Click(object sender, EventArgs e)
        {
            if (txtOutputFolder.Text.Trim() != "")
                folderBrowserDialog.SelectedPath = txtOutputFolder.Text.Trim();

            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                txtOutputFolder.Text = folderBrowserDialog.SelectedPath;
        }

        private void btnSaveToFile_Click(object sender, EventArgs e)
        {
            txtText.Text = txtText.Text.Trim();
            txtOutputFolder.Text = txtOutputFolder.Text.Trim();
            txtFileName.Text = txtFileName.Text.Trim();

            if (txtText.Text == "")
                MessageBox.Show("Text box is empty.", "Warning",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else if (txtFileName.Text == "")
                MessageBox.Show("File name is empty.", "Warning",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
            {
                string fullFilePath = string.Format("{0}\\{1}", txtOutputFolder.Text, txtFileName.Text);

                try
                {
                    using (StreamWriter sw = new StreamWriter(fullFilePath))
                        sw.WriteLine(txtText.Text);

                    MessageBox.Show("Text was written to the file successfully.", "OK",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void txtUrl_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
                btnReadUrl_Click(btnReadUrl, EventArgs.Empty);
        }
    }
}

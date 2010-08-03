using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Typist
{
    public partial class Typist : Form
    {
        public Typist()
        {
            InitializeComponent();
            TypedText = new StringBuilder();
        }

        private void Typist_Load(object sender, EventArgs e)
        {
        }


        protected string ImportedText { get; private set; }

        protected StringBuilder TypedText { get; private set; }

        protected bool PracticeMode
        {
            get { return practiceMode; }
            private set
            {
                practiceMode = value;
                btnStart.Text = practiceMode ? "Pause" : "Start";

                txtImportedText.Focus();
            }
        }
        private bool practiceMode = false;

        private bool afterImport = false;


        private void btnImport_Click(object sender, EventArgs e)
        {
            if (ofdImport.ShowDialog() == DialogResult.OK)
            {
                using (StreamReader sr = new StreamReader(ofdImport.FileName))
                    ImportedText = sr.ReadToEnd();

                TypedText = new StringBuilder();
                pbTyping.Refresh();

                txtImportedText.Text = ImportedText;
                txtImportedText.Select(0, 0);

                afterImport = true;
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            PracticeMode = !PracticeMode;
        }

        private void pbTyping_Paint(object sender, PaintEventArgs e)
        {
            drawText(ImportedText, e.Graphics, e.ClipRectangle, Brushes.Black);
            drawText(TypedText.ToString(), e.Graphics, e.ClipRectangle, Brushes.Red);
        }

        private void drawText(string text, Graphics graphics, Rectangle rectangle, Brush brush)
        {
            StringFormat stringFormat = new StringFormat(StringFormatFlags.LineLimit)
            {
                Trimming = StringTrimming.Word,
            };

            graphics.DrawString(text,
                                new Font("Courier New", 9),
                                brush,
                                new Rectangle()
                                {
                                    X = 1,
                                    Y = 2,
                                    Width = rectangle.Width - 24,
                                    Height = rectangle.Height - 4
                                },
                                stringFormat);
        }

        private void pbTyping_Resize(object sender, EventArgs e)
        {
            pbTyping.Refresh();
        }

        private void pbTyping_Click(object sender, EventArgs e)
        {
            pbTyping.Refresh();
        }

        private void Typist_Move(object sender, EventArgs e)
        {
            pbTyping.Refresh();
        }

        private void Typist_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (afterImport)
            {
                PracticeMode = true;
                afterImport = false;
            }

            e.Handled = true;

            if (PracticeMode)
            {
                if (e.KeyChar == '\b')
                {
                    if (TypedText.Length > 0)
                        TypedText.Remove(TypedText.Length - 1, 1);
                }
                else
                {
                    string str;
                    switch (e.KeyChar)
                    {
                        case '\r':
                            str = "\n";
                            break;
                        default:
                            str = e.KeyChar.ToString();
                            break;
                    }

                    TypedText.Append(str);
                }

                pbTyping.Refresh();
            }
        }
    }
}

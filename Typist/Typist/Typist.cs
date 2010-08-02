using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

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

        protected bool PracticeMode { get; private set; }


        private void btnImport_Click(object sender, EventArgs e)
        {
            if (ofdImport.ShowDialog() == DialogResult.OK)
            {
                using (StreamReader sr = new StreamReader(ofdImport.FileName))
                    ImportedText = sr.ReadToEnd();

                TypedText = new StringBuilder();
                pbTyping.Refresh();

                rtbImportedText.Text = ImportedText;

                PracticeMode = false;
                btnStart_Click(btnStart, EventArgs.Empty);
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            PracticeMode = !PracticeMode;
            btnStart.Text = PracticeMode ? "Pause" : "Start";
            rtbImportedText.Focus();
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
                Trimming = StringTrimming.None,
                HotkeyPrefix = System.Drawing.Text.HotkeyPrefix.Show
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

            //stringFormat.SetMeasurableCharacterRanges(new[]
            //    {
            //        new CharacterRange(0, 100),
            //        new CharacterRange(100, 200),
            //        new CharacterRange(200, 300),
            //        new CharacterRange(300, 400),
            //    });

            //Region[] regions = graphics.MeasureCharacterRanges(text,
            //             new Font("Courier New", 9),
            //             new Rectangle()
            //             {
            //                 X = 1,
            //                 Y = 2,
            //                 Width = rectangle.Width - 24,
            //                 Height = rectangle.Height - 4
            //             },
            //             stringFormat);

            //foreach (var region in regions)
            //{
            //    RectangleF rectF = region.GetBounds(g);
            //    graphics.DrawRectangle(Pens.Black, new Rectangle((int)rectF.X, (int)rectF.Y, (int)rectF.Width, (int)rectF.Height));
            //}
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

            e.Handled = true;
        }
    }
}

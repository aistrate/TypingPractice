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
using System.Diagnostics;

namespace Typist
{
    public partial class TypistForm : Form
    {
        public TypistForm()
        {
            InitializeComponent();
        }

        private void TypistForm_Load(object sender, EventArgs e)
        {
            CurrentFont = new Font("Courier New", 10);

            ImportedText = "";
            TypedText = new StringBuilder();

            PracticeMode = false;
        }


        protected string ImportedText { get; private set; }

        protected StringBuilder TypedText { get; private set; }

        protected bool PracticeMode
        {
            get { return practiceMode; }
            private set
            {
                if (string.IsNullOrEmpty(ImportedText))
                    value = false;

                practiceMode = value;

                btnImport.Enabled = !value;

                btnStart.Enabled = !string.IsNullOrEmpty(ImportedText);
                btnStart.Text = practiceMode ? "Pause" : "Start";

                if (practiceMode)
                    afterImport = false;

                pbTyping.Refresh();

                IsTimerRunning = practiceMode;

                if (practiceMode)
                    pbTyping.Focus();
            }
        }
        private bool practiceMode = false;

        private bool afterImport = false;

        protected bool IsTimerRunning
        {
            get { return stopwatch.IsRunning; }
            private set
            {
                displayTime();
                displayWPM();

                if (value)
                    stopwatch.Start();
                else
                    stopwatch.Stop();

                tmrTimer.Enabled = value;
            }
        }

        private Stopwatch stopwatch = new Stopwatch();

        protected Font CurrentFont { get; private set; }


        private void btnImport_Click(object sender, EventArgs e)
        {
            PracticeMode = false;

            if (ofdImport.ShowDialog() == DialogResult.OK)
            {
                using (StreamReader sr = new StreamReader(ofdImport.FileName))
                    ImportedText = sr.ReadToEnd().Replace("\r\n", "\n");

                TypedText = new StringBuilder();

                stopwatch.Reset();

                PracticeMode = false;
                afterImport = true;
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            PracticeMode = !PracticeMode;
        }

        private void pbTyping_Paint(object sender, PaintEventArgs e)
        {
            drawText(showNewLineChars(ImportedText),
                     e.Graphics, e.ClipRectangle, Brushes.Black);

            drawText(showNewLineChars(TypedText.ToString() + (PracticeMode ? "_" : "")),
                     e.Graphics, e.ClipRectangle, Brushes.CornflowerBlue);
        }

        private void drawText(string text, Graphics graphics, Rectangle rectangle, Brush brush)
        {
            graphics.DrawString(text,
                                CurrentFont,
                                brush,
                                new Rectangle()
                                {
                                    X = 1,
                                    Y = 2,
                                    Width = rectangle.Width - 2,
                                    Height = rectangle.Height - 4
                                },
                                new StringFormat(StringFormatFlags.LineLimit)
                                {
                                    Trimming = StringTrimming.Word,
                                });
        }

        private string showNewLineChars(string text)
        {
            //return text.Replace("\n", "\xB6\n");
            return text;
        }

        private void pbTyping_Resize(object sender, EventArgs e)
        {
            pbTyping.Invalidate();
        }

        private void TypistForm_Activated(object sender, EventArgs e)
        {
            pbTyping.Invalidate();
        }

        private bool controlKeyPressed = false;

        private Keys[] suppressKeys = new Keys[] { Keys.Tab, Keys.Escape, Keys.Left, Keys.Right, Keys.Up, Keys.Down };

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (PracticeMode)
            {
                Keys keyMinusShift = keyData & ~Keys.Shift;

                if (suppressKeys.Contains(keyMinusShift))
                    return true;

                if (keyMinusShift == Keys.Return || keyMinusShift == Keys.Space)
                {
                    controlKeyPressed = false;
                    typeKey((char)keyMinusShift);
                    return true;
                }
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void TypistForm_KeyDown(object sender, KeyEventArgs e)
        {
            controlKeyPressed = e.Control;

            if (controlKeyPressed)
                switch (e.KeyCode)
                {
                    case Keys.P:
                        PracticeMode = !PracticeMode;
                        break;
                    case Keys.M:
                        PracticeMode = false;
                        TypistForm.ActiveForm.WindowState = FormWindowState.Minimized;
                        break;
                    case Keys.I:
                        if (btnImport.Enabled)
                            btnImport_Click(btnImport, EventArgs.Empty);
                        break;
                }
        }

        private void TypistForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            typeKey(e.KeyChar);
        }

        private void typeKey(char keyChar)
        {
            if (controlKeyPressed)
                return;

            if (afterImport && keyChar != ' ')
                PracticeMode = true;

            if (PracticeMode)
            {
                if (keyChar == '\b')
                    removeLast(TypedText);
                else
                {
                    string s;
                    switch (keyChar)
                    {
                        case '\r':
                            s = "\n";
                            break;
                        default:
                            s = keyChar.ToString();
                            break;
                    }

                    append(TypedText, s);
                }

                pbTyping.Refresh();
            }
        }

        private void removeLast(StringBuilder sb)
        {
            if (sb.Length > 0)
                TypedText.Remove(sb.Length - 1, 1);
        }

        private void append(StringBuilder sb, string s)
        {
            sb.Append(s);
        }

        private string whitespaceChars = " \n\r\t";

        private int countWords(StringBuilder sb)
        {
            return sb.ToString().Where(c => whitespaceChars.IndexOf(c) < 0).Count() / 5;
        }

        private void tmrTimer_Tick(object sender, EventArgs e)
        {
            displayTime();
            displayWPM();
        }

        private void displayTime()
        {
            lblTime.Text = string.Format("{0:00}:{1:00}",
                                         stopwatch.Elapsed.Minutes,
                                         stopwatch.Elapsed.Seconds);
        }

        private DateTime lastWPMCalcTime = DateTime.MinValue;

        private void displayWPM()
        {
            if (DateTime.Now - lastWPMCalcTime > new TimeSpan(0, 0, 2))
            {
                lastWPMCalcTime = DateTime.Now;

                int wordCount = countWords(TypedText);
                double elapsedMinutes = (double)stopwatch.ElapsedMilliseconds / 60000.0;

                lblWPM.Text = string.Format("{0:#0} wpm", elapsedMinutes != 0.0 ? (double)wordCount / elapsedMinutes : 0.0);
            }
        }
    }
}

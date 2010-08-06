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
            CurrentFont = new Font("Courier New", 10, FontStyle.Regular);

            ImportedText = new TextBuffer("");

            PracticeMode = false;

            this.Top = 0;
        }

        private const bool beepOnError = true;
        private const bool visibleNewlines = false;
        private const bool countWhitespaceAsWordChars = true;
        private const int pauseAfterInterval = 10;


        protected TextBuffer ImportedText
        {
            get { return importedText; }
            private set
            {
                importedText = value;

                TypedText = new TextBuffer(importedText, countWhitespaceAsWordChars);

                if (beepOnError)
                    TypedText.Error += new EventHandler(TypedText_Error);
            }
        }
        private TextBuffer importedText;

        protected TextBuffer TypedText { get; private set; }


        protected bool PracticeMode
        {
            get { return practiceMode; }
            private set
            {
                if (!IsImported)
                    value = false;

                practiceMode = value;

                btnImport.Enabled = !value;

                btnStart.Enabled = IsImported;
                btnStart.Text =
                    practiceMode ? "&Pause" :
                    IsStarted ? "&Resume" :
                    "&Start";

                if (practiceMode)
                    rightAfterImport = false;

                picTyping.Refresh();

                timeOfLastCharTyped = DateTime.Now;

                IsTimerRunning = practiceMode;
                displayStats();
                displayErrorCount();

                if (practiceMode)
                    picTyping.Focus();
            }
        }
        private bool practiceMode = false;

        private bool rightAfterImport = false;

        protected bool IsTimerRunning
        {
            get { return stopwatch.IsRunning; }
            private set
            {
                if (value)
                {
                    stopwatch.Start();
                    tmrTimer.Start();
                }
                else
                {
                    stopwatch.Stop();
                    tmrTimer.Stop();
                }
            }
        }

        private Stopwatch stopwatch = new Stopwatch();

        protected Font CurrentFont { get; private set; }

        protected bool IsImported { get { return ImportedText.Length > 0; } }

        protected bool IsStarted { get { return IsImported && !rightAfterImport; } }


        private void btnImport_Click(object sender, EventArgs e)
        {
            PracticeMode = false;

            if (dlgImport.ShowDialog() == DialogResult.OK)
            {
                this.Text = dlgImport.SafeFileName + " - Typist";

                using (StreamReader sr = new StreamReader(dlgImport.FileName))
                    ImportedText = new TextBuffer(sr.ReadToEnd()
                                                    .Replace("\r\n", "\n")
                                                    .Replace("\t", "    "));

                stopwatch.Reset();

                rightAfterImport = true;
                PracticeMode = false;
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            PracticeMode = !PracticeMode;
        }

        private void pbTyping_Paint(object sender, PaintEventArgs e)
        {
            RectangleF innerRect = new RectangleF()
            {
                X = 1,
                Y = 2,
                Width = e.Graphics.ClipBounds.Width - 2,
                Height = e.Graphics.ClipBounds.Height - 4,
            };


            drawText(ImportedText.Text, e.Graphics, innerRect, Brushes.Black);


            string typedText = ImportedText.Substring(0, TypedText.Length);

            int missingAtEol = countMissingAtEol(ImportedText.Text, TypedText.LastIndex, e.Graphics, innerRect);
            if (missingAtEol > 0)
                typedText = typedText.Insert(typedText.LastIndexOf(' ') + 1,
                                             new string(' ', missingAtEol));

            drawText(typedText, e.Graphics, innerRect, Brushes.CornflowerBlue);


            if (PracticeMode && TypedText.Length < ImportedText.Length)
            {
                RectangleF cursorRect = getRectangle(ImportedText.Text, TypedText.LastIndex + 1, e.Graphics, innerRect);

                e.Graphics.DrawString("_", CurrentFont, Brushes.Red, cursorRect, SingleCharStringFormat);
            }


            RectangleF[] errorRects = getRectangles(ImportedText.Text, TypedText.ErrorsUncorrected.ToArray(), e.Graphics, innerRect);

            for (int i = 0; i < TypedText.ErrorsUncorrected.Count; i++)
            {
                e.Graphics.FillRectangle(Brushes.LightGray, errorRects[i]);

                e.Graphics.DrawString(TypedText[TypedText.ErrorsUncorrected[i]].ToString(), CurrentFont,
                                      Brushes.Purple, errorRects[i], SingleCharStringFormat);
            }
        }

        private int countMissingAtEol(string text, int lastIndex, Graphics graphics, RectangleF innerRect)
        {
            if (lastIndex < 0)
                return 0;

            int missing = 0;
            RectangleF originalPos, typedPos;
            do
            {
                originalPos = getRectangle(text, lastIndex + missing, graphics, innerRect);
                typedPos = getRectangle(text.Substring(0, lastIndex + missing + 1), lastIndex + missing, graphics, innerRect);

                if (typedPos.X - originalPos.X > 5)
                    missing++;
                else
                    break;
            }
            while (lastIndex + missing < text.Length);

            return missing;
        }

        private void drawText(string text, Graphics graphics, RectangleF innerRect, Brush brush)
        {
            if (visibleNewlines)
                text = text.Replace("\n", "\xB6\n");

            graphics.DrawString(text, CurrentFont, brush, innerRect, TextStringFormat);
        }

        private RectangleF getRectangle(string text, int index, Graphics graphics, RectangleF innerRect)
        {
            return getRectangles(text, new[] { index }, graphics, innerRect).First();
        }

        private RectangleF[] getRectangles(string text, int[] indexes, Graphics graphics, RectangleF innerRect)
        {
            indexes = indexes.Where(i => i < text.Length)
                             .ToArray();

            RectangleF[] rectangles = indexes.Split(32)
                                             .SelectMany(grp => get32Rectangles(text, grp.ToArray(), graphics, innerRect))
                                             .ToArray();

            for (int i = 0; i < indexes.Length; i++)
                if (rectangles[i].IsEmpty)
                    rectangles[i] = get32Rectangles(text.Insert(indexes[i], "-"), new[] { indexes[i] }, graphics, innerRect)[0];

            return rectangles;
        }

        private RectangleF[] get32Rectangles(string text, int[] indexes, Graphics graphics, RectangleF innerRect)
        {
            CharacterRange[] ranges = indexes.Select(i => new CharacterRange(i, 1)).ToArray();

            StringFormat stringFormat = new StringFormat(TextStringFormat);
            stringFormat.SetMeasurableCharacterRanges(ranges);

            Region[] regions = graphics.MeasureCharacterRanges(text, CurrentFont, innerRect, stringFormat);

            return regions.Select(r => r.GetBounds(graphics))
                          .ToArray();
        }

        protected readonly static StringFormat TextStringFormat =
            new StringFormat(StringFormatFlags.LineLimit)
            {
                Trimming = StringTrimming.Word,
                Alignment = StringAlignment.Near,
                LineAlignment = StringAlignment.Near,
            };

        protected readonly static StringFormat SingleCharStringFormat =
            new StringFormat(StringFormatFlags.NoWrap)
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Far,
            };

        private void pbTyping_Resize(object sender, EventArgs e)
        {
            picTyping.Invalidate();
        }

        private void TypistForm_Activated(object sender, EventArgs e)
        {
            picTyping.Invalidate();
        }

        private bool controlKeyPressed = false;

        private Keys[] suppressKeys = new Keys[] { Keys.Escape, Keys.Left, Keys.Right, Keys.Up, Keys.Down };

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (PracticeMode)
            {
                Keys keyMinusShift = keyData & ~Keys.Shift;

                if (suppressKeys.Contains(keyMinusShift))
                    return true;

                if (keyMinusShift == Keys.Return || keyMinusShift == Keys.Space || keyMinusShift == Keys.Tab)
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

            if (rightAfterImport && keyChar != ' ')
                PracticeMode = true;

            if (PracticeMode)
            {
                TypedText.ProcessKey(keyChar);

                timeOfLastCharTyped = DateTime.Now;
                displayErrorCount();

                if (TypedText.Length >= ImportedText.Length)
                    PracticeMode = false;

                picTyping.Refresh();
            }
        }

        private void tmrTimer_Tick(object sender, EventArgs e)
        {
            pauseIfNotTyping();
            displayStats();
        }

        private void pauseIfNotTyping()
        {
            if (pauseAfterInterval > 0 && DateTime.Now - timeOfLastCharTyped > new TimeSpan(0, 0, pauseAfterInterval))
                PracticeMode = false;
        }

        private void displayStats()
        {
            displayTime();
            displayWPM();
        }

        private void displayTime()
        {
            if (IsImported)
                lblTime.Text = string.Format("{0:00}:{1:00}",
                                             stopwatch.Elapsed.Minutes,
                                             stopwatch.Elapsed.Seconds);
            else
                lblTime.Text = "";
        }

        private void displayWPM()
        {
            if (IsImported)
            {
                if (DateTime.Now - timeOfLastWPMCalc > new TimeSpan(0, 0, 1))
                {
                    timeOfLastWPMCalc = DateTime.Now;

                    double elapsedMinutes = (double)stopwatch.ElapsedMilliseconds / 60000.0;

                    lblWPM.Text = string.Format("{0:#0} wpm",
                                                elapsedMinutes != 0.0 ?
                                                    (double)TypedText.WordCount / elapsedMinutes :
                                                    0.0);
                }
            }
            else
                lblWPM.Text = "";
        }

        private void displayErrorCount()
        {
            if (IsImported)
            {
                lblErrorCount.Text = string.Format("{0:#0} errs", TypedText.ErrorsCommitted);

                lblAccuracy.Text = string.Format("({0:p0})", TypedText.Accuracy);
            }
            else
            {
                lblErrorCount.Text = "";
                lblAccuracy.Text = "";
            }
        }

        private DateTime timeOfLastWPMCalc = DateTime.MinValue;
        private DateTime timeOfLastCharTyped = DateTime.MaxValue;

        protected void TypedText_Error(object sender, EventArgs e)
        {
            System.Media.SystemSounds.Beep.Play();
        }

        private void doOnce(Action action)
        {
            if (!done)
                action();
            done = true;
        }
        private bool done = false;
    }
}

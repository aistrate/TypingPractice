using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Windows.Forms;

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
            this.Top = 0;

            ImportedText = new TextBuffer("");
            PracticeMode = false;
        }

        private const bool beepOnError = true;
        private const bool allowBackspace = true;
        private const bool visibleNewlines = false;
        private const bool countWhitespaceAsWordChars = true;
        private const int pauseAfterElapsed = 10;

        private static readonly Font typingFont = new Font("Courier New", 10, FontStyle.Regular);

        private static readonly Brush importedTextColor = Brushes.Black;
        private static readonly Brush typedTextColor = Brushes.CornflowerBlue;
        private static readonly Brush cursorColor = Brushes.Red;
        private static readonly Brush errorBackColor = Brushes.LightGray;
        private static readonly Brush errorForeColor = Brushes.Purple;


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

                IsStopwatchRunning = practiceMode;

                displayTimeElapsed();
                displayWPM();
                displayErrorCount();

                if (practiceMode)
                    picTyping.Focus();
                else
                    btnStart.Focus();
            }
        }
        private bool practiceMode = false;

        private bool rightAfterImport = false;

        protected bool IsStopwatchRunning
        {
            get { return stopwatch.IsRunning; }
            private set
            {
                if (value)
                    stopwatch.Start();
                else
                    stopwatch.Stop();
            }
        }

        private Stopwatch stopwatch = new Stopwatch();

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

        private bool repaintNeeded = false;

        private void TypistForm_Paint(object sender, PaintEventArgs e)
        {
            repaintNeeded = true;
        }

        private void picTyping_Paint(object sender, PaintEventArgs e)
        {
            RectangleF typingArea = new RectangleF()
            {
                X = 1,
                Y = 2,
                Width = e.Graphics.ClipBounds.Width - 2,
                Height = e.Graphics.ClipBounds.Height - 4,
            };

            drawImportedText(e.Graphics, typingArea);

            drawTypedText(e.Graphics, typingArea);

            drawCursor(e.Graphics, typingArea);

            drawErrorChars(e.Graphics, typingArea);
        }

        private void drawImportedText(Graphics g, RectangleF typingArea)
        {
            drawText(ImportedText.Text, g, importedTextColor, typingArea);
        }

        private void drawTypedText(Graphics g, RectangleF typingArea)
        {
            string typedText = ImportedText.Substring(0, TypedText.Length);

            int missing = countMissingAtEol(ImportedText.Text, TypedText.LastIndex, g, typingArea);
            if (missing > 0)
                typedText = typedText.Insert(typedText.LastIndexOf(' ') + 1,
                                             new string(' ', missing));

            drawText(typedText, g, typedTextColor, typingArea);
        }

        private void drawCursor(Graphics g, RectangleF typingArea)
        {
            if (PracticeMode && TypedText.Length < ImportedText.Length)
            {
                RectangleF cursorArea = getCharArea(ImportedText.Text,
                                                    TypedText.LastIndex + 1,
                                                    g, typingArea);

                drawChar('_', g, cursorColor, cursorArea);
            }
        }

        private void drawErrorChars(Graphics g, RectangleF typingArea)
        {
            RectangleF[] errorCharAreas = getCharAreas(ImportedText.Text,
                                                       TypedText.ErrorsUncorrected.ToArray(),
                                                       g, typingArea);

            for (int i = 0; i < TypedText.ErrorsUncorrected.Count; i++)
            {
                g.FillRectangle(errorBackColor, errorCharAreas[i]);

                drawChar(TypedText[TypedText.ErrorsUncorrected[i]],
                         g, errorForeColor,
                         errorCharAreas[i]);
            }
        }

        private int countMissingAtEol(string text, int lastIndex, Graphics g, RectangleF typingArea)
        {
            if (lastIndex < 0)
                return 0;

            RectangleF importedCharArea, typedCharArea;
            int missing = 0;

            do
            {
                importedCharArea = getCharArea(text,
                                               lastIndex + missing,
                                               g, typingArea);

                typedCharArea = getCharArea(text.Substring(0, lastIndex + missing + 1),
                                            lastIndex + missing,
                                            g, typingArea);

                if (typedCharArea.X - importedCharArea.X > 5)
                    missing++;
                else
                    break;
            }
            while (lastIndex + missing < text.Length);

            return missing;
        }

        private void drawChar(char ch, Graphics g, Brush brush, RectangleF charArea)
        {
            g.DrawString(ch.ToString(), typingFont, brush, charArea, SingleCharStringFormat);
        }

        private void drawText(string text, Graphics g, Brush brush, RectangleF typingArea)
        {
            if (visibleNewlines)
                text = text.Replace("\n", "\xB6\n");

            g.DrawString(text, typingFont, brush, typingArea, TextStringFormat);
        }

        private RectangleF getCharArea(string text, int charIndex, Graphics g, RectangleF typingArea)
        {
            return getCharAreas(text, new[] { charIndex }, g, typingArea).First();
        }

        private RectangleF[] getCharAreas(string text, int[] charIndexes, Graphics g, RectangleF typingArea)
        {
            charIndexes = charIndexes.Where(i => i < text.Length)
                                     .ToArray();

            RectangleF[] charAreas = charIndexes.Split(32)
                                                .SelectMany(grp => getCharAreas32(text, grp.ToArray(),
                                                                                  g, typingArea))
                                                .ToArray();

            for (int i = 0; i < charIndexes.Length; i++)
                if (charAreas[i].IsEmpty)
                    charAreas[i] = getCharAreas32(text.Insert(charIndexes[i], "-"),
                                                  new[] { charIndexes[i] },
                                                  g, typingArea)[0];

            return charAreas;
        }

        private RectangleF[] getCharAreas32(string text, int[] charIndexes, Graphics g, RectangleF typingArea)
        {
            CharacterRange[] ranges = charIndexes.Select(i => new CharacterRange(i, 1)).ToArray();

            StringFormat stringFormat = new StringFormat(TextStringFormat);
            stringFormat.SetMeasurableCharacterRanges(ranges);

            Region[] regions = g.MeasureCharacterRanges(text, typingFont, typingArea, stringFormat);

            return regions.Select(r => r.GetBounds(g))
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


        private void picTyping_Resize(object sender, EventArgs e)
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

        protected override bool ProcessMnemonic(char charCode)
        {
            return false;
            //return base.ProcessMnemonic(charCode);
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

        private DateTime timeOfLastCharTyped;

        private void tmrTimer_Tick(object sender, EventArgs e)
        {
            if (repaintNeeded)
            {
                picTyping.Invalidate();
                repaintNeeded = false;
            }

            if (PracticeMode)
            {
                displayTimeElapsed();

                if (timeChanged)
                    displayWPM();

                if (pauseAfterElapsed > 0 &&
                    DateTime.Now - timeOfLastCharTyped >= new TimeSpan(0, 0, pauseAfterElapsed))
                    PracticeMode = false;
            }
        }

        private void displayTimeElapsed()
        {
            if (IsImported)
            {
                string oldTime = lblTime.Text;

                lblTime.Text = string.Format("{0:00}:{1:00}",
                                             stopwatch.Elapsed.Minutes,
                                             stopwatch.Elapsed.Seconds);

                timeChanged = (oldTime != lblTime.Text);

            }
            else
                lblTime.Text = "";
        }

        private bool timeChanged = false;

        private void displayWPM()
        {
            if (IsImported)
            {
                double elapsedMinutes = (double)stopwatch.ElapsedMilliseconds / 60000.0;

                lblWPM.Text = string.Format("{0:#0} wpm",
                                            elapsedMinutes != 0.0 ?
                                                (double)TypedText.WordCount / elapsedMinutes :
                                                0.0);
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

        protected void TypedText_Error(object sender, EventArgs e)
        {
            SystemSounds.Beep.Play();
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

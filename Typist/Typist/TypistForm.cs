using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Media;
using System.Windows.Forms;
using Typist.Appearance;

namespace Typist
{
    public partial class TypistForm : Form
    {
        #region Flags and Settings

        private const bool allowBackspace = true;
        private const bool visibleNewlines = false;
        private const bool countWhitespaceAsWordChars = true;
        private const bool countErrorsAsWordChars = true;

        private const bool beepOnError = false;
        private const bool askBeforeCloseDuringPractice = false;

        private const int pauseAfterElapsed = 10;
        private const bool pauseOnMinimize = true;
        private const bool pauseOnDeactivate = false;

        private const bool cursorAsVerticalBar = true;
        private const char charCursorChar = '_';
        private const bool showCursorWhenPaused = false;

        private const float barCursorVOffset = -0.1f;
        private const float charCursorVOffset = 0;
        private const float errorBackgroundVOffset = -0.1f;

        private const int marginLeft = 1;
        private const int marginRight = 1;
        private const int marginTop = 2;
        private const int marginBottom = 2;

        private static readonly Theme theme = new Theme(Theme.Discreet, FontNames.FixedWidth.CourierNew);

        #endregion


        #region General Behavior

        public TypistForm()
        {
            InitializeComponent();

            this.Width = theme.WindowWidth;
            this.Height = theme.WindowHeight;
        }

        private void TypistForm_Load(object sender, EventArgs e)
        {
            if (theme.PositionWindowAtTopScreen)
                this.Top = 0;

            ImportedText = new TextBuffer("", countWhitespaceAsWordChars);
            PracticeMode = false;
        }

        protected TextBuffer ImportedText
        {
            get { return importedText; }
            private set
            {
                importedText = value;

                TypedText = new TextBuffer(importedText, countWhitespaceAsWordChars, countErrorsAsWordChars);

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
                    practiceMode ? "Pause" :
                    IsStarted ? "Resume" :
                    "Start";

                this.Text = string.Format("{0}Typist{1}",
                                          IsImported ? dlgImport.SafeFileName + " - " : "",
                                          IsPaused ? (IsFinished ? " (Finished)" : " (Paused)") : "");

                if (practiceMode)
                    rightAfterImport = false;

                picTyping.Refresh();

                timeOfLastCharTyped = DateTime.Now;

                IsStopwatchRunning = practiceMode;

                displayTimeElapsed();
                displayWPM();
                displayErrorCount();

                if (practiceMode || IsFinished)
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

        protected bool IsPaused { get { return IsStarted && !PracticeMode; } }

        protected bool IsFinished { get { return TypedText.Length >= ImportedText.Length; } }


        private void btnImport_Click(object sender, EventArgs e)
        {
            PracticeMode = false;

            if (dlgImport.ShowDialog() == DialogResult.OK)
            {
                importedFileName = dlgImport.SafeFileName;

                using (StreamReader sr = new StreamReader(dlgImport.FileName))
                    ImportedText = new TextBuffer(sr.ReadToEnd(), countWhitespaceAsWordChars);

                stopwatch.Reset();

                rightAfterImport = true;
                PracticeMode = false;
            }
        }

        private string importedFileName = "";

        private void btnStart_Click(object sender, EventArgs e)
        {
            PracticeMode = !PracticeMode;
        }

        private void TypistForm_Deactivate(object sender, EventArgs e)
        {
            if (pauseOnDeactivate && PracticeMode)
                PracticeMode = false;
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            if (askBeforeCloseDuringPractice && IsStarted)
            {
                PracticeMode = false;

                if (MessageBox.Show("Practice session is in progress. Are you sure you want to quit?", "Typist",
                                    MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1)
                                    == DialogResult.No)
                    e.Cancel = true;
            }

            base.OnClosing(e);
        }

        protected void TypedText_Error(object sender, EventArgs e)
        {
            playBeep();
        }

        private void playBeep()
        {
            if (beepOnError)
                SystemSounds.Beep.Play();
        }

        #endregion


        #region Paint Event Handler

        private void TypistForm_Paint(object sender, PaintEventArgs e)
        {
            repaintNeeded = true;
        }

        private bool repaintNeeded = false;

        private void picTyping_Paint(object sender, PaintEventArgs e)
        {
            RectangleF typingArea = calcTypingArea(e.Graphics);

            drawImportedText(e.Graphics, typingArea);
            drawShadowText(e.Graphics, typingArea);
            drawErrorChars(e.Graphics, typingArea);
            drawCursor(e.Graphics, typingArea);
        }

        private RectangleF calcTypingArea(Graphics g)
        {
            RectangleF sampleCharArea = getCharArea("_", 0, g, g.ClipBounds);

            return new RectangleF()
            {
                X = marginLeft,
                Y = marginTop,
                Width = g.ClipBounds.Width - marginLeft - marginRight - sampleCharArea.Width,
                Height = g.ClipBounds.Height - marginTop - marginBottom,
            };
        }

        private void drawImportedText(Graphics g, RectangleF typingArea)
        {
            drawText(ImportedText.ToString(), g, theme.ImportedTextColor, typingArea);
        }

        private void drawShadowText(Graphics g, RectangleF typingArea)
        {
            string shadowText = ImportedText.Substring(0, TypedText.Length);

            int lineJumpIndex = findLineJump(ImportedText.ToString(), TypedText.LastIndex,
                                             g, typingArea);
            if (lineJumpIndex < TypedText.LastIndex)
                shadowText = insertSpacesAfterJump(ImportedText.ToString(), shadowText,
                                                   lineJumpIndex, TypedText.LastIndex,
                                                   g, typingArea);

            drawText(shadowText, g, theme.TypedTextColor, typingArea);
        }

        private int findLineJump(string text, int lastIndex, Graphics g, RectangleF typingArea)
        {
            RectangleF importedCharArea, shadowCharArea;
            int lineJumpIndex = lastIndex;

            while (lineJumpIndex >= 0)
            {
                importedCharArea = getCharArea(text,
                                               lineJumpIndex,
                                               g, typingArea);

                shadowCharArea = getCharArea(text.Substring(0, lineJumpIndex + 1),
                                             lineJumpIndex,
                                             g, typingArea);

                if (shadowCharArea.Y == importedCharArea.Y)
                    return lineJumpIndex;

                lineJumpIndex--;
            }

            return lastIndex;
        }

        private string insertSpacesAfterJump(string text, string shadowText, int lineJumpIndex, int lastIndex,
                                             Graphics g, RectangleF typingArea)
        {
            RectangleF importedCharArea = getCharArea(text,
                                                      lastIndex,
                                                      g, typingArea);

            RectangleF shadowCharArea;
            int spaces = 0;

            do
            {
                shadowText = shadowText.Insert(lineJumpIndex + 1, " ");
                spaces++;

                shadowCharArea = getCharArea(shadowText,
                                             lastIndex + spaces,
                                             g, typingArea);
            }
            while (shadowCharArea.Y < importedCharArea.Y);

            return shadowText;
        }

        private void drawErrorChars(Graphics g, RectangleF typingArea)
        {
            RectangleF[] errorCharAreas = getCharAreas(ImportedText.ToString(),
                                                       TypedText.ErrorsUncorrected.ToArray(),
                                                       g, typingArea);

            for (int i = 0; i < TypedText.ErrorsUncorrected.Count; i++)
            {
                g.FillRectangle(theme.ErrorBackColor, fracOffsetCharArea(errorCharAreas[i], 0, errorBackgroundVOffset));

                drawChar(TypedText[TypedText.ErrorsUncorrected[i]],
                         g, theme.ErrorForeColor,
                         errorCharAreas[i]);
            }
        }

        private void drawCursor(Graphics g, RectangleF typingArea)
        {
            if ((PracticeMode || showCursorWhenPaused && IsPaused) &&
                !IsFinished)
            {
                RectangleF cursorArea = getCharArea(ImportedText.ToString(),
                                                    TypedText.LastIndex + 1,
                                                    g, typingArea);

                cursorArea = fracOffsetCharArea(cursorArea, 0,
                                                cursorAsVerticalBar ? barCursorVOffset : charCursorVOffset);

                if (cursorAsVerticalBar)
                    g.FillRectangle(theme.CursorColor, new RectangleF()
                    {
                        X = cursorArea.X - (0.125f * cursorArea.Width) - (0.5f * theme.BarCursorLineWidth),
                        Y = cursorArea.Y,
                        Width = theme.BarCursorLineWidth,
                        Height = cursorArea.Height,
                    });
                else
                    drawChar(charCursorChar, g, theme.CursorColor, cursorArea);
            }
        }

        private RectangleF fracOffsetCharArea(RectangleF charArea, float fracWidth, float fracHeight)
        {
            return offsetCharArea(charArea, fracWidth * charArea.Width, fracHeight * charArea.Height);
        }

        private RectangleF offsetCharArea(RectangleF charArea, float x, float y)
        {
            return new RectangleF()
            {
                X = charArea.X + x,
                Y = charArea.Y + y,
                Width = charArea.Width,
                Height = charArea.Height,
            };
        }

        private char pilcrow = '\xB6';

        private void drawChar(char ch, Graphics g, Brush brush, RectangleF charArea)
        {
            if (ch == '\n')
                ch = pilcrow;

            g.DrawString(ch.ToString(), theme.TypingFont, brush, charArea, SingleCharStringFormat);
        }

        private void drawText(string text, Graphics g, Brush brush, RectangleF typingArea)
        {
            if (visibleNewlines)
                text = text.Replace("\n", string.Format("{0}\n", pilcrow));

            g.DrawString(text, theme.TypingFont, brush, typingArea, TextStringFormat);
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

            Region[] regions = g.MeasureCharacterRanges(text, theme.TypingFont, typingArea, stringFormat);

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

        #endregion


        #region Key Processing

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (PracticeMode)
            {
                Keys keyMinusShift = keyData & ~Keys.Shift;

                if (keyMinusShift.IsOneOf(Keys.Left, Keys.Right, Keys.Up, Keys.Down))
                    return true;

                if (keyMinusShift.IsOneOf(Keys.Return, Keys.Tab, Keys.Space))
                {
                    OnKeyPress(new KeyPressEventArgs((char)keyMinusShift));
                    return true;
                }
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void TypistForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control || e.Alt)
            {
                if (e.Control)
                {
                    if (e.KeyCode == Keys.P)
                        PracticeMode = !PracticeMode;
                    else if (e.KeyCode == Keys.M)
                    {
                        PracticeMode = false;
                        WindowState = FormWindowState.Minimized;
                    }
                }

                if (PracticeMode)
                    e.SuppressKeyPress = true;
            }
            else if (e.KeyCode == Keys.Escape)
            {
                if (PracticeMode)
                    PracticeMode = false;

                e.SuppressKeyPress = true;
            }
        }

        private void TypistForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (rightAfterImport &&
                e.KeyChar != ' ' && e.KeyChar != '\b' &&
                !char.IsControl(e.KeyChar))
                PracticeMode = true;

            if (PracticeMode)
            {
                timeOfLastCharTyped = DateTime.Now;

                if (e.KeyChar == '\b' && (!allowBackspace || TypedText.Length == 0))
                {
                    playBeep();
                    return;
                }

                TypedText.ProcessKey(e.KeyChar);

                displayErrorCount();

                if (IsFinished)
                    PracticeMode = false;

                picTyping.Refresh();
            }
        }

        private DateTime timeOfLastCharTyped;

        #endregion


        #region Timer and Statistics

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

                if ((pauseAfterElapsed > 0 && DateTime.Now - timeOfLastCharTyped >= new TimeSpan(0, 0, pauseAfterElapsed)) ||
                    (pauseOnMinimize && WindowState == FormWindowState.Minimized))
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

        #endregion
    }
}

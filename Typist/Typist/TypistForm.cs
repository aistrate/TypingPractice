using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Media;
using System.Windows.Forms;
using Typist.Appearance;
using System.Configuration;
using System.Text;

namespace Typist
{
    public partial class TypistForm : Form
    {
        #region Flags and Settings

        private const bool allowBackspace = true;
        private const bool visibleNewlines = false;
        private const bool countWhitespaceAsWordChars = true;
        private const bool countErrorsAsWordChars = true;

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

        private Theme theme =
            new Theme(Theme.Default)
            {
                //FontName = FontNames.FixedWidth.CourierNew,
                //FontSize = 14,
                //FontStyle = FontStyle.Bold,
                //ErrorBackColor = Brushes.White,
                BeepOnError = false,
            };

        #endregion


        #region General Behavior

        public TypistForm(string filePath)
        {
            InitializeComponent();

            initializeTypingBox();

            if (Properties.Settings.Default.IsMaximized)
                WindowState = FormWindowState.Maximized;

            if (Properties.Settings.Default.WindowX == 0 && Properties.Settings.Default.WindowY == 0 &&
                Properties.Settings.Default.WindowWidth == 0 && Properties.Settings.Default.WindowHeight == 0)
                StartPosition = FormStartPosition.CenterScreen;
            else
            {
                Location = new Point(Properties.Settings.Default.WindowX, Properties.Settings.Default.WindowY);
                Size = new Size(Properties.Settings.Default.WindowWidth, Properties.Settings.Default.WindowHeight);
            }

            ImportedText = new TextBuffer("", countWhitespaceAsWordChars);
            PracticeMode = false;

            ImportFile(filePath);
        }

        private void initializeTypingBox()
        {
            picTyping.MarginLeft = marginLeft;
            picTyping.MarginRight = marginRight;
            picTyping.MarginTop = marginTop;
            picTyping.MarginBottom = marginBottom;

            picTyping.Theme = theme;

            picTyping.CursorAsVerticalBar = cursorAsVerticalBar;
            picTyping.CharCursorChar = charCursorChar;

            picTyping.BarCursorVOffset = barCursorVOffset;
            picTyping.CharCursorVOffset = charCursorVOffset;
            picTyping.ErrorBackgroundVOffset = errorBackgroundVOffset;

            picTyping.VisibleNewlines = visibleNewlines;
            picTyping.ShowCursorWhenPaused = showCursorWhenPaused;

            picTyping.DrawingCursor += new System.ComponentModel.CancelEventHandler(picTyping_DrawingCursor);
        }

        private void TypistForm_Load(object sender, EventArgs e)
        {
        }

        private void picTyping_DrawingCursor(object sender, System.ComponentModel.CancelEventArgs e)
        {
            bool drawCursor = (PracticeMode || showCursorWhenPaused && IsPaused) &&
                              !IsFinished;

            e.Cancel = !drawCursor;
        }

        protected TextBuffer ImportedText
        {
            get { return importedText; }
            private set
            {
                importedText = value;

                TypedText = new TextBuffer(importedText, countWhitespaceAsWordChars, countErrorsAsWordChars);

                picTyping.ImportedText = value;
                picTyping.TypedText = TypedText;

                if (theme.BeepOnError)
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
                                          IsImported && !string.IsNullOrEmpty(importedFileName) ? importedFileName + " - " : "",
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
                ImportFile(dlgImport.FileName);
        }

        public void ImportFile(string filePath)
        {
            try
            {
                if (string.IsNullOrEmpty(filePath))
                    return;

                importedFileName = new FileInfo(filePath).Name;

                using (StreamReader sr = new StreamReader(filePath, Encoding.Default))
                    ImportedText = new TextBuffer(sr.ReadToEnd(), countWhitespaceAsWordChars);

                stopwatch.Reset();

                rightAfterImport = true;
                PracticeMode = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Typist", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void TypistForm_SizeChanged(object sender, EventArgs e)
        {
            if (pauseOnMinimize && WindowState == FormWindowState.Minimized)
                PracticeMode = false;

            presaveWindowPosition(false, true);
        }

        private void TypistForm_Move(object sender, EventArgs e)
        {
            presaveWindowPosition(true, false);
        }

        private void presaveWindowPosition(bool location, bool size)
        {
            if (WindowState == FormWindowState.Normal)
            {
                if (location)
                {
                    Properties.Settings.Default.WindowX = Location.X;
                    Properties.Settings.Default.WindowY = Location.Y;
                }

                if (size)
                {
                    Properties.Settings.Default.WindowWidth = Size.Width;
                    Properties.Settings.Default.WindowHeight = Size.Height;
                }
            }

            if (WindowState != FormWindowState.Minimized)
                Properties.Settings.Default.IsMaximized = WindowState == FormWindowState.Maximized;
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            if (askBeforeCloseDuringPractice && IsStarted)
            {
                PracticeMode = false;

                if (MessageBox.Show("Practice session is in progress. Are you sure you want to quit?", "Typist",
                                    MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1)
                                    == DialogResult.No)
                {
                    e.Cancel = true;
                    return;
                }
            }

            presaveWindowPosition(true, true);
            Properties.Settings.Default.Save();

            base.OnClosing(e);
        }

        private void TypistForm_Paint(object sender, PaintEventArgs e)
        {
            repaintNeeded = true;
        }

        private bool repaintNeeded = false;

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

        protected void TypedText_Error(object sender, EventArgs e)
        {
            playBeep();
        }

        private void playBeep()
        {
            if (theme.BeepOnError)
                SystemSounds.Beep.Play();
        }

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

                if (pauseAfterElapsed > 0 && DateTime.Now - timeOfLastCharTyped >= new TimeSpan(0, 0, pauseAfterElapsed))
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

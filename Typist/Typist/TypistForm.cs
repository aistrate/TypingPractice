using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Windows.Forms;
using Typist.Appearance;

namespace Typist
{
    public partial class TypistForm : Form
    {
        #region Flags and Settings

        private const bool pauseOnMinimize = true;
        private const bool pauseOnDeactivate = false;

        private const bool cursorAsVerticalBar = true;
        private const char charCursorChar = '_';

        private const float barCursorVOffset = 0;
        private const float charCursorVOffset = 0;
        private const float errorBackgroundVOffset = 0;

        private const int marginLeft = 1;
        private const int marginRight = 1;
        private const int marginTop = 2;
        private const int marginBottom = 2;

        private const float largeFontScalingFactor = 1f;

        private const bool loadStoredTypingFont = true;
        private static readonly FontInfo defaultTypingFont = Fonts.GenericMonospace;

        private static readonly Theme theme = new Theme(Theme.Default);

        #endregion


        #region General Behavior

        public TypistForm(string filePath)
        {
            InitializeComponent();

            loadWindowPosition();
            loadTypingFont();
            loadUserSettings();

            initializeTypingBox();
            initializeContextMenuStrip();
            initializeSettingsDialog();

            ImportedText = new TextBuffer("", userSettings.CountWhitespaceAsWordChars, userSettings.RemoveMultipleWhitespace);
            PracticeMode = false;

            //if (string.IsNullOrEmpty(filePath))
            //    filePath = @"C:\Documents and Settings\Adrian\Desktop\TypingPracticeTexts\SingleParagraph\European Wildcat.txt";
            //filePath = @"C:\Documents and Settings\Adrian\Desktop\TypingPracticeTexts\Wikipedia\Done\Aluminium.txt";

            ImportFile(filePath);
        }

        private void initializeTypingBox()
        {
            picTyping.Theme = theme;

            picTyping.TextMargin = new Padding(marginLeft, marginTop, marginRight, marginBottom);

            picTyping.CursorAsVerticalBar = cursorAsVerticalBar;
            picTyping.CharCursorChar = charCursorChar;

            picTyping.BarCursorVOffset = barCursorVOffset;
            picTyping.CharCursorVOffset = charCursorVOffset;
            picTyping.ErrorBackgroundVOffset = errorBackgroundVOffset;

            picTyping.VisibleNewlines = userSettings.VisibleNewlines;
        }

        private void loadWindowPosition()
        {
            if (Properties.Settings.Default.WindowIsMaximized)
                WindowState = FormWindowState.Maximized;

            if (Properties.Settings.Default.WindowX == 0 && Properties.Settings.Default.WindowY == 0 &&
                Properties.Settings.Default.WindowWidth == 0 && Properties.Settings.Default.WindowHeight == 0)
                StartPosition = FormStartPosition.CenterScreen;
            else
            {
                Location = new Point(Properties.Settings.Default.WindowX, Properties.Settings.Default.WindowY);
                Size = new Size(Properties.Settings.Default.WindowWidth, Properties.Settings.Default.WindowHeight);
            }
        }

        private void TypistForm_Load(object sender, EventArgs e)
        {
        }

        private void picTyping_DrawingCursor(object sender, CancelEventArgs e)
        {
            bool drawCursor = (PracticeMode || userSettings.ShowCursorWhenPaused && IsPaused) &&
                              !IsFinished;

            e.Cancel = !drawCursor;
        }

        private void picTyping_StatusChanged(object sender, StatusChangedEventArgs e)
        {
            lblStatusBar.Text = e.StatusMessage;
        }

        protected TextBuffer ImportedText
        {
            get { return importedText; }
            private set
            {
                importedText = value;

                TypedText = new TextBuffer(importedText,
                                           userSettings.CountWhitespaceAsWordChars,
                                           userSettings.CountErrorsAsWordChars);

                picTyping.ImportedText = value;
                picTyping.TypedText = TypedText;

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

                btnStart.Enabled = IsImported;
                btnStart.Text =
                    practiceMode ? "Pause" :
                    IsStarted ? "Resume" :
                    "Start";

                pauseToolStripMenuItem.Enabled = IsImported;
                pauseToolStripMenuItem.Text = btnStart.Text + " Typing Practice";

                this.Text = string.Format("{0}Typist{1}",
                                          IsImported && !string.IsNullOrEmpty(importedFileName) ? importedFileName + " - " : "",
                                          IsPaused ? (IsFinished ? " (Finished)" : " (Paused)") : "");

                if (practiceMode)
                    lblStatusBar.Text = "Typing...";
                else if (IsPaused)
                    lblStatusBar.Text = IsFinished ? "Finished" : "Paused";

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
            importFile();
        }

        public void ImportFile(string filePath)
        {
            try
            {
                if (string.IsNullOrEmpty(filePath))
                    return;

                FileInfo fileInfo = new FileInfo(filePath);
                importedFileName = fileInfo.Name;

                using (StreamReader sr = new StreamReader(filePath, Encoding.Default))
                    ImportedText = new TextBuffer(sr.ReadToEnd(),
                                                  userSettings.CountWhitespaceAsWordChars,
                                                  userSettings.RemoveMultipleWhitespace);

                stopwatch.Reset();

                rightAfterImport = true;
                PracticeMode = false;

                lblStatusBar.Text = string.Format("Imported: {0}", fileInfo.FullName);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Typist", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private string importedFileName = "";

        private void btnStart_Click(object sender, EventArgs e)
        {
            pauseResume();
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
                Properties.Settings.Default.WindowIsMaximized = WindowState == FormWindowState.Maximized;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (userSettings.AskBeforeCloseDuringPractice && IsStarted && !IsFinished)
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
            presaveTypingFont();
            presaveUserSettings();

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
                    switch (e.KeyCode)
                    {
                        case Keys.P:
                            pauseResume();
                            break;
                        case Keys.M:
                            pauseAndMinimize();
                            break;
                        case Keys.I:
                            importFile();
                            break;
                        case Keys.X:
                            changeSettings();
                            break;

                        case Keys.F:
                            changeFont();
                            break;
                        case Keys.S:
                            saveAsCustomFont();
                            break;
                        case Keys.T:
                            viewCustomFont();
                            break;

                        case Keys.N:
                            moveToFont(e.Shift ? -1 : +1);
                            break;
                    }
                }

                if (PracticeMode)
                    e.SuppressKeyPress = true;
            }
            else if (e.KeyCode == Keys.Escape)
                e.SuppressKeyPress = true;
        }

        private void TypistForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (rightAfterImport &&
                e.KeyChar != ' ' && e.KeyChar != '\b' &&
                !char.IsControl(e.KeyChar))
                PracticeMode = true;

            if (PracticeMode)
            {
                lblStatusBar.Text = "Typing...";

                timeOfLastCharTyped = DateTime.Now;

                if (e.KeyChar == '\b' && (!userSettings.AllowBackspace || TypedText.Length == 0))
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
            if (userSettings.BeepOnError)
                SystemSounds.Beep.Play();
        }

        #endregion


        #region Fonts

        private void loadTypingFont()
        {
            if (loadStoredTypingFont &&
                !string.IsNullOrEmpty(Properties.Settings.Default.TypingFontName) &&
                Properties.Settings.Default.TypingFontSize > 0 &&
                Properties.Settings.Default.TypingFontUnit > 0)
            {
                FontInfo fontInfo = new FontInfo(Properties.Settings.Default.TypingFontName,
                                                 Properties.Settings.Default.TypingFontSize,
                                                 (FontStyle)Properties.Settings.Default.TypingFontStyle,
                                                 (GraphicsUnit)Properties.Settings.Default.TypingFontUnit);

                CustomFont = fontInfo.Font;
            }
            else
                CustomFont = defaultTypingFont.Font;
        }

        private void presaveTypingFont()
        {
            if (loadStoredTypingFont)
            {
                Properties.Settings.Default.TypingFontName = CustomFont.FontFamily.Name;
                Properties.Settings.Default.TypingFontSize = CustomFont.Size;
                Properties.Settings.Default.TypingFontUnit = (int)CustomFont.Unit;
                Properties.Settings.Default.TypingFontStyle = (int)CustomFont.Style;
            }
        }

        protected class PredefinedFont
        {
            public int Index;
            public FontInfo FontInfo;
            public int IndexInAvailables;
        }

        protected PredefinedFont[] PredefinedFonts
        {
            get
            {
                if (predefinedFonts == null)
                {
                    predefinedFonts = Fonts.Small.Concat(Fonts.Large.Select(f => f.ScaleBy(largeFontScalingFactor)))
                                                 .Select(f => new PredefinedFont()
                                                 {
                                                     FontInfo = f,
                                                     IndexInAvailables = -1,
                                                 })
                                                 .ToArray();

                    for (int i = 0, k = 1; i < predefinedFonts.Length; i++)
                    {
                        predefinedFonts[i].Index = i + 1;

                        if (predefinedFonts[i].FontInfo.IsAvailable)
                            predefinedFonts[i].IndexInAvailables = k++;
                    }
                }

                return predefinedFonts;
            }
        }
        private PredefinedFont[] predefinedFonts;

        protected Font[] AvailableFonts
        {
            get
            {
                if (availableFonts == null)
                    availableFonts = new Font[] { null }
                                            .Concat(PredefinedFonts.Where(f => f.FontInfo.IsAvailable)
                                                                   .Select(f => f.FontInfo.Font))
                                            .ToArray();

                return availableFonts;
            }
        }
        private Font[] availableFonts;

        protected Font CustomFont
        {
            get { return AvailableFonts[0]; }
            set
            {
                AvailableFonts[0] = value;
                CurrentFontIndex = 0;

                viewCustomFontToolStripMenuItem.Text = string.Format("Custom Font ({0})",
                                                                     FontInfo.GetDescription(value, "{0}, {1}"));
            }
        }

        protected int CurrentFontIndex
        {
            get { return currentFontIndex; }
            set
            {
                getMenuItem(currentFontIndex).Checked = false;

                currentFontIndex = value;

                if (currentFontIndex >= AvailableFonts.Length)
                    currentFontIndex = 0;
                else if (currentFontIndex < 0)
                    currentFontIndex = AvailableFonts.Length - 1;

                picTyping.TypingFont = AvailableFonts[currentFontIndex];
                picTyping.BarCursorLineWidth = AvailableFonts[currentFontIndex].SizeInPoints < 14.5 ? 2 : 3;

                lblStatusBar.Text = string.Format("{0}: {1}",
                                                  currentFontIndex == 0 ?
                                                        "Custom Font" :
                                                        string.Format("Predefined Font ({0})",
                                                                      PredefinedFonts.First(f => f.IndexInAvailables == currentFontIndex)
                                                                                     .Index),
                                                  FontInfo.GetDescription(picTyping.TypingFont));

                getMenuItem(currentFontIndex).Checked = true;

                picTyping.Invalidate();
            }
        }
        private int currentFontIndex = 0;

        private ToolStripMenuItem getMenuItem(int availableFontIndex)
        {
            if (availableFontIndex == 0)
                return viewCustomFontToolStripMenuItem;
            else
                return predefinedFontsToolStripMenuItem.DropDownItems
                                                       .OfType<ToolStripMenuItem>()
                                                       .First(item => (int)item.Tag == availableFontIndex);
        }

        private void openFontDialog()
        {
            CurrentFontIndex = CurrentFontIndex;

            dlgFontDialog.Font = picTyping.TypingFont;

            if (dlgFontDialog.ShowDialog() == DialogResult.OK)
                CustomFont = dlgFontDialog.Font;
        }

        private void dlgFontDialog_Apply(object sender, EventArgs e)
        {
            CustomFont = dlgFontDialog.Font;
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

                if (userSettings.PauseAfterElapsed > 0 &&
                    DateTime.Now - timeOfLastCharTyped >= new TimeSpan(0, 0, userSettings.PauseAfterElapsed))
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


        #region Context Menu

        private void initializeContextMenuStrip()
        {
            var predefinedFontEventHandler = new EventHandler(predefinedFontMenuItem_Click);

            predefinedFontsToolStripMenuItem.DropDownItems.AddRange(
                PredefinedFonts.Select(f =>
                {
                    var predefinedFontMenuItem = new ToolStripMenuItem(f.FontInfo.OriginalDescription)
                    {
                        Enabled = f.FontInfo.IsAvailable,
                        Tag = f.IndexInAvailables,
                    };

                    predefinedFontMenuItem.Click += predefinedFontEventHandler;

                    return predefinedFontMenuItem;
                })
                             .ToArray());

            predefinedFontsToolStripMenuItem.DropDownItems.Insert(Fonts.Small.Length, new ToolStripSeparator());
        }

        private void predefinedFontMenuItem_Click(object sender, EventArgs e)
        {
            var predefinedFontMenuItem = (ToolStripMenuItem)sender;

            PracticeMode = false;
            CurrentFontIndex = (int)predefinedFontMenuItem.Tag;
        }

        private void pauseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pauseResume();
        }

        private void pauseAndMinimizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pauseAndMinimize();
        }

        private void importFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            importFile();
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            changeSettings();
        }

        private void changeFontToolStripMenuItem_Click(object sender, EventArgs e)
        {
            changeFont();
        }

        private void saveAsCustomFontToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveAsCustomFont();
        }

        private void viewCustomFontToolStripMenuItem_Click(object sender, EventArgs e)
        {
            viewCustomFont();
        }

        private void previousFontToolStripMenuItem_Click(object sender, EventArgs e)
        {
            moveToFont(-1);
        }

        private void nextFontToolStripMenuItem_Click(object sender, EventArgs e)
        {
            moveToFont(+1);
        }

        private void mnuContextMenu_Closed(object sender, ToolStripDropDownClosedEventArgs e)
        {
            picTyping.Invalidate();
        }

        private void predefinedFontsToolStripMenuItem_DropDownClosed(object sender, EventArgs e)
        {
            picTyping.Invalidate();
        }

        private void pauseResume()
        {
            PracticeMode = !PracticeMode;
        }

        private void pauseAndMinimize()
        {
            PracticeMode = false;
            WindowState = FormWindowState.Minimized;
        }

        private void importFile()
        {
            PracticeMode = false;
            if (dlgImport.ShowDialog() == DialogResult.OK)
                ImportFile(dlgImport.FileName);
        }

        private void changeSettings()
        {
            PracticeMode = false;
            openSettingsDialog();
        }

        private void changeFont()
        {
            PracticeMode = false;
            openFontDialog();
        }

        private void saveAsCustomFont()
        {
            CustomFont = AvailableFonts[CurrentFontIndex];
        }

        private void viewCustomFont()
        {
            CurrentFontIndex = 0;
        }

        private void moveToFont(int inc)
        {
            PracticeMode = false;
            CurrentFontIndex += inc;
        }

        #endregion


        #region User Settings

        private UserSettings userSettings;

        private void loadUserSettings()
        {
            userSettings = new UserSettings()
            {
                BeepOnError = Properties.Settings.Default.UserSettings_BeepOnError,
                AllowBackspace = Properties.Settings.Default.UserSettings_AllowBackspace,
                VisibleNewlines = Properties.Settings.Default.UserSettings_VisibleNewlines,
                RemoveMultipleWhitespace = Properties.Settings.Default.UserSettings_RemoveMultipleWhitespace,
                CountWhitespaceAsWordChars = Properties.Settings.Default.UserSettings_CountWhitespaceAsWordChars,
                CountErrorsAsWordChars = Properties.Settings.Default.UserSettings_CountErrorsAsWordChars,
                AskBeforeCloseDuringPractice = Properties.Settings.Default.UserSettings_AskBeforeCloseDuringPractice,
                ShowCursorWhenPaused = Properties.Settings.Default.UserSettings_ShowCursorWhenPaused,
                PauseAfterElapsed = Properties.Settings.Default.UserSettings_PauseAfterElapsed,
            };
        }

        private void presaveUserSettings()
        {
            Properties.Settings.Default.UserSettings_BeepOnError = userSettings.BeepOnError;
            Properties.Settings.Default.UserSettings_AllowBackspace = userSettings.AllowBackspace;
            Properties.Settings.Default.UserSettings_VisibleNewlines = userSettings.VisibleNewlines;
            Properties.Settings.Default.UserSettings_RemoveMultipleWhitespace = userSettings.RemoveMultipleWhitespace;
            Properties.Settings.Default.UserSettings_CountWhitespaceAsWordChars = userSettings.CountWhitespaceAsWordChars;
            Properties.Settings.Default.UserSettings_CountErrorsAsWordChars = userSettings.CountErrorsAsWordChars;
            Properties.Settings.Default.UserSettings_AskBeforeCloseDuringPractice = userSettings.AskBeforeCloseDuringPractice;
            Properties.Settings.Default.UserSettings_ShowCursorWhenPaused = userSettings.ShowCursorWhenPaused;
            Properties.Settings.Default.UserSettings_PauseAfterElapsed = userSettings.PauseAfterElapsed;
        }

        private SettingsDialog dlgSettingsDialog;

        private void initializeSettingsDialog()
        {
            dlgSettingsDialog = new SettingsDialog();
            dlgSettingsDialog.Move += new EventHandler(dlgSettingsDialog_Move);
        }

        private void dlgSettingsDialog_Move(object sender, EventArgs e)
        {
            picTyping.Invalidate();
        }

        private void openSettingsDialog()
        {
            dlgSettingsDialog.UserSettings = userSettings;

            if (dlgSettingsDialog.ShowDialog() == DialogResult.OK)
            {
                userSettings = dlgSettingsDialog.UserSettings;

                ImportedText.CountWhitespaceAsWordChars = userSettings.CountWhitespaceAsWordChars;
                TypedText.CountWhitespaceAsWordChars = userSettings.CountWhitespaceAsWordChars;
                TypedText.CountErrorsAsWordChars = userSettings.CountErrorsAsWordChars;
                displayWPM();

                picTyping.VisibleNewlines = userSettings.VisibleNewlines;
                picTyping.Invalidate();
            }

            dlgSettingsDialog.StartPosition = FormStartPosition.Manual;
        }

        #endregion
    }
}

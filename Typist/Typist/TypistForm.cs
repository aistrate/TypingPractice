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
using Typist.TextBuffers;

namespace Typist
{
    public partial class TypistForm : Form
    {
        #region Flags and Settings

        private const bool pauseOnMinimize = true;
        private const bool pauseOnDeactivate = false;

        private const bool cursorAsVerticalBar = true;
        private const char charCursorChar = '_';

        private const float barCursorRelativeWidth = 0.125f;

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

            ImportedText = new ReadOnlyTypingBuffer("",
                                                    userSettings.VisibleNewlines,
                                                    userSettings.RemoveEndOfLineSpaces,
                                                    userSettings.RemoveMultipleWhitespace);

            PracticeMode = false;

            ImportFile(!string.IsNullOrEmpty(filePath) ? filePath : LastImportedFile);

            loadStatisticsMode();
        }

        private void initializeTypingBox()
        {
            picTyping.Theme = theme;

            picTyping.WordWrap = userSettings.WordWrap;

            picTyping.TextMargin = new Padding(marginLeft, marginTop, marginRight, marginBottom);

            picTyping.CursorAsVerticalBar = cursorAsVerticalBar;
            picTyping.CharCursorChar = charCursorChar;

            picTyping.BarCursorRelativeWidth = barCursorRelativeWidth;

            picTyping.BarCursorVOffset = barCursorVOffset;
            picTyping.CharCursorVOffset = charCursorVOffset;
            picTyping.ErrorBackgroundVOffset = errorBackgroundVOffset;
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
            lblStatusBarMain.Text = e.StatusMessage;
        }

        private void picTyping_HorizontalVisibleRegionChanged(object sender, VisibleRegionChangedEventArgs e)
        {
            firstVisibleColumn = e.FirstVisibleIndex;

            setScrollBarRegion(scrHTyping, e);
        }
        private int firstVisibleColumn;

        private void scrHTyping_Scroll(object sender, ScrollEventArgs e)
        {
            e.NewValue = firstVisibleColumn;
        }

        private void picTyping_VerticalVisibleRegionChanged(object sender, VisibleRegionChangedEventArgs e)
        {
            firstVisibleRow = e.FirstVisibleIndex;

            setScrollBarRegion(scrVTyping, e);
        }
        private int firstVisibleRow;

        private void scrVTyping_Scroll(object sender, ScrollEventArgs e)
        {
            e.NewValue = firstVisibleRow;
        }

        private void setScrollBarRegion(ScrollBar scrollBar, VisibleRegionChangedEventArgs e)
        {
            if (e.FirstVisibleIndex == 0 && e.LastVisibleIndex == e.TotalLength - 1)
                scrollBar.Enabled = false;
            else
            {
                scrollBar.Enabled = true;

                scrollBar.Minimum = 0;
                scrollBar.Maximum = e.TotalLength - 1;

                scrollBar.LargeChange = e.LastVisibleIndex - e.FirstVisibleIndex + 1;
                scrollBar.Value = e.FirstVisibleIndex;
            }
        }

        private void picTyping_CursorPositionChanged(object sender, CursorPositionChangedEventArgs e)
        {
            cursorRow = e.Row;
            cursorColumn = e.Column;

            if (StatisticsMode == StatisticsModes.RowColumn)
                displayProgressStatistics();
        }
        private int cursorRow;
        private int cursorColumn;

        protected ReadOnlyTypingBuffer ImportedText
        {
            get { return importedText; }
            private set
            {
                importedText = value;

                TypedText = new ReadWriteTypingBuffer(importedText);

                picTyping.ImportedText = value;
                picTyping.TypedText = TypedText;

                TypedText.Error += new EventHandler(TypedText_Error);
            }
        }
        private ReadOnlyTypingBuffer importedText;

        protected ReadWriteTypingBuffer TypedText { get; private set; }

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
                    lblStatusBarMain.Text = "Typing...";
                else if (IsPaused)
                    lblStatusBarMain.Text = IsFinished ? "Finished" : "Paused";

                lblTrafficLight.Image =
                    practiceMode ? Typist.Properties.Resources.Green :
                    IsPaused     ? (IsFinished ? Typist.Properties.Resources.Red :
                                                 Typist.Properties.Resources.Yellow) :
                                   Typist.Properties.Resources.Gray;

                if (practiceMode)
                    rightAfterImport = false;

                picTyping.Invalidate();

                timeOfLastCharTyped = DateTime.Now;

                IsStopwatchRunning = practiceMode;

                displayTimeElapsed();
                displayWPM();
                displayErrorCount();
                displayProgressStatistics();

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
            LastImportedFile = "";

            try
            {
                if (string.IsNullOrEmpty(filePath))
                    return;

                FileInfo fileInfo = new FileInfo(filePath);
                importedFileName = fileInfo.Name;

                using (StreamReader sr = new StreamReader(filePath, Encoding.Default))
                    ImportedText = new ReadOnlyTypingBuffer(sr.ReadToEnd(),
                                                            userSettings.VisibleNewlines,
                                                            userSettings.RemoveEndOfLineSpaces,
                                                            userSettings.RemoveMultipleWhitespace);

                LastImportedFile = fileInfo.FullName;

                lblStatusBarMain.Text = "Imported";

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
            pauseResume();
        }

        private void TypistForm_Deactivate(object sender, EventArgs e)
        {
            if (pauseOnDeactivate && PracticeMode)
                PracticeMode = false;
        }

        private void TypistForm_SizeChanged(object sender, EventArgs e)
        {
            picTyping.Invalidate();

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
            presaveStatisticsMode();
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
                if (e.Control && !e.Alt)
                {
                    switch (e.KeyCode)
                    {
                        case Keys.P:
                            pauseResume();
                            break;
                        case Keys.M:
                            pauseAndMinimize();
                            break;
                        case Keys.O:
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

                        case Keys.Oemplus:
                            if (e.Shift)
                                changeFontSize(+1);
                            break;
                        case Keys.OemMinus:
                            if (!e.Shift)
                                changeFontSize(-1);
                            break;
                        case Keys.B:
                            toggleFontStyle(FontStyle.Bold);
                            break;
                        case Keys.I:
                            toggleFontStyle(FontStyle.Italic);
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
                lblStatusBarMain.Text = "Typing...";

                timeOfLastCharTyped = DateTime.Now;

                if (e.KeyChar == '\b' && (!userSettings.AllowBackspace || TypedText.Length == 0))
                {
                    playBeep();
                    return;
                }

                TypedText.ProcessKey(e.KeyChar);

                displayErrorCount();
                displayProgressStatistics();

                if (IsFinished)
                    PracticeMode = false;

                picTyping.Invalidate();
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

                lblStatusBarMain.Text = string.Format("{0}: {1}",
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

        private void changeFontSize(int delta)
        {
            PracticeMode = false;

            Font font = picTyping.TypingFont;

            float size = font.Unit == GraphicsUnit.Point ? Math.Max(6, font.Size + 0.25f * delta) :
                                                           Math.Max(1, font.Size + delta);

            CustomFont = new Font(font.Name, size, font.Style, font.Unit);
        }

        private void toggleFontStyle(FontStyle styleFlag)
        {
            Font font = picTyping.TypingFont;

            FontStyle newStyle = font.Style ^ styleFlag;

            if (font.FontFamily.IsStyleAvailable(newStyle))
                CustomFont = new Font(font.Name, font.Size, newStyle, font.Unit);
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

                int wordCharCount = TypedText.RecordedKeys.Count(userSettings.CountWhitespaceAsWordChars,
                                                                 userSettings.CountErrorsAsWordChars);
                double wordCount = (double)wordCharCount / 5.0;

                lblWPM.Text = string.Format("{0:#0} wpm",
                                            elapsedMinutes != 0.0 ?
                                                wordCount / elapsedMinutes :
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

        private void displayProgressStatistics()
        {
            int progress = IsImported ? 100 * TypedText.Length / ImportedText.Length : 0;

            pgsTypingProgress.Value = progress;

            switch (StatisticsMode)
            {
                case StatisticsModes.Percentage:
                    btnStatistics.Text = string.Format("{0} %", progress);
                    break;
                case StatisticsModes.TypedByTotal:
                    btnStatistics.Text = string.Format("{0} / {1}", TypedText.Length, ImportedText.Length);
                    break;
                case StatisticsModes.Total:
                    btnStatistics.Text = string.Format("{0}", ImportedText.Length);
                    break;
                case StatisticsModes.Typed:
                    btnStatistics.Text = string.Format("{0}", TypedText.Length);
                    break;
                case StatisticsModes.TypedKeys:
                    btnStatistics.Text = string.Format("{0}", TypedText.RecordedKeys.Length);
                    break;
                case StatisticsModes.RowColumn:
                    btnStatistics.Text = string.Format("{0}, {1}", cursorRow, cursorColumn);
                    break;
                default:
                    btnStatistics.Text = "";
                    break;
            }
        }

        private void btnStatistics_ButtonClick(object sender, EventArgs e)
        {
            StatisticsMode = (StatisticsModes)(((int)StatisticsMode + 1) % StatisticsMenuItems.Length);
        }

        private void btnStatisticsMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem selectedItem = (ToolStripMenuItem)sender;

            StatisticsMode = (StatisticsModes)int.Parse((string)selectedItem.Tag);
        }

        protected StatisticsModes StatisticsMode
        {
            get { return statisticsMode; }
            set
            {
                statisticsMode = value;

                foreach (var item in StatisticsMenuItems)
                    item.Checked = false;

                StatisticsMenuItems[(int)statisticsMode].Checked = true;

                displayProgressStatistics();
            }
        }
        private StatisticsModes statisticsMode;

        public enum StatisticsModes
        {
            Percentage = 0,
            TypedByTotal = 1,
            Total = 2,
            Typed = 3,
            TypedKeys = 4,
            RowColumn = 5,
        }

        protected ToolStripMenuItem[] StatisticsMenuItems
        {
            get
            {
                if (statisticsMenuItems == null)
                    statisticsMenuItems = new ToolStripMenuItem[]
                    {
                        toolStripPercentage,
                        toolStripTypedByTotal,
                        toolStripTotal,
                        toolStripTyped,
                        toolStripTypedKeys,
                        toolStripRowColumn,
                    };

                return statisticsMenuItems;
            }
        }
        private ToolStripMenuItem[] statisticsMenuItems;

        private void loadStatisticsMode()
        {
            int statisticsMode = Properties.Settings.Default.StatisticsMode;

            if (statisticsMode >= StatisticsMenuItems.Length)
                statisticsMode = 0;

            StatisticsMode = (StatisticsModes)statisticsMode;
        }

        private void presaveStatisticsMode()
        {
            Properties.Settings.Default.StatisticsMode = (int)StatisticsMode;
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
                WordWrap = Properties.Settings.Default.UserSettings_WordWrap,
                VisibleNewlines = Properties.Settings.Default.UserSettings_VisibleNewlines,
                RemoveEndOfLineSpaces = Properties.Settings.Default.UserSettings_RemoveEndOfLineSpaces,
                RemoveMultipleWhitespace = Properties.Settings.Default.UserSettings_RemoveMultipleWhitespace,
                RememberLastImportedFile = Properties.Settings.Default.UserSettings_RememberLastImportedFile,
                HideStatisticsWhileTyping = Properties.Settings.Default.UserSettings_HideStatisticsWhileTyping,
                CountWhitespaceAsWordChars = Properties.Settings.Default.UserSettings_CountWhitespaceAsWordChars,
                CountErrorsAsWordChars = Properties.Settings.Default.UserSettings_CountErrorsAsWordChars,
                AskBeforeCloseDuringPractice = Properties.Settings.Default.UserSettings_AskBeforeCloseDuringPractice,
                ShowCursorWhenPaused = Properties.Settings.Default.UserSettings_ShowCursorWhenPaused,
                PauseAfterElapsed = Properties.Settings.Default.UserSettings_PauseAfterElapsed,
            };

            LastImportedFile = userSettings.RememberLastImportedFile ? Properties.Settings.Default.LastImportedFile : "";
        }

        private void presaveUserSettings()
        {
            Properties.Settings.Default.UserSettings_BeepOnError = userSettings.BeepOnError;
            Properties.Settings.Default.UserSettings_AllowBackspace = userSettings.AllowBackspace;
            Properties.Settings.Default.UserSettings_WordWrap = userSettings.WordWrap;
            Properties.Settings.Default.UserSettings_VisibleNewlines = userSettings.VisibleNewlines;
            Properties.Settings.Default.UserSettings_RemoveEndOfLineSpaces = userSettings.RemoveEndOfLineSpaces;
            Properties.Settings.Default.UserSettings_RemoveMultipleWhitespace = userSettings.RemoveMultipleWhitespace;
            Properties.Settings.Default.UserSettings_RememberLastImportedFile = userSettings.RememberLastImportedFile;
            Properties.Settings.Default.UserSettings_HideStatisticsWhileTyping = userSettings.HideStatisticsWhileTyping;
            Properties.Settings.Default.UserSettings_CountWhitespaceAsWordChars = userSettings.CountWhitespaceAsWordChars;
            Properties.Settings.Default.UserSettings_CountErrorsAsWordChars = userSettings.CountErrorsAsWordChars;
            Properties.Settings.Default.UserSettings_AskBeforeCloseDuringPractice = userSettings.AskBeforeCloseDuringPractice;
            Properties.Settings.Default.UserSettings_ShowCursorWhenPaused = userSettings.ShowCursorWhenPaused;
            Properties.Settings.Default.UserSettings_PauseAfterElapsed = userSettings.PauseAfterElapsed;

            Properties.Settings.Default.LastImportedFile = userSettings.RememberLastImportedFile ? LastImportedFile : "";
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

                ImportedText.ExpandNewlines = userSettings.VisibleNewlines;

                displayWPM();

                picTyping.WordWrap = userSettings.WordWrap;

                picTyping.Invalidate();
            }

            dlgSettingsDialog.StartPosition = FormStartPosition.Manual;
        }

        protected string LastImportedFile { get; set; }

        #endregion
    }
}

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
            TypedText = new StringBuilder();
        }

        private void TypistForm_Load(object sender, EventArgs e)
        {
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

                if (value)
                    stopwatch.Start();
                else
                    stopwatch.Stop();

                tmrTimer.Enabled = value;
            }
        }

        private Stopwatch stopwatch = new Stopwatch();


        private void btnImport_Click(object sender, EventArgs e)
        {
            PracticeMode = false;

            if (ofdImport.ShowDialog() == DialogResult.OK)
            {
                using (StreamReader sr = new StreamReader(ofdImport.FileName))
                    ImportedText = sr.ReadToEnd();

                TypedText = new StringBuilder();
                pbTyping.Refresh();

                txtImportedText.Text = ImportedText;
                txtImportedText.Select(0, 0);

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
            pbTyping.Invalidate();
        }

        private void TypistForm_Activated(object sender, EventArgs e)
        {
            pbTyping.Invalidate();
        }

        private bool controlKeyPressed = false;

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (PracticeMode)
            {
                Keys keyMinusShift = keyData & ~Keys.Shift;

                if (keyMinusShift == Keys.Tab)
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

            if (afterImport)
                PracticeMode = true;

            if (PracticeMode)
            {
                if (keyChar == '\b')
                {
                    if (TypedText.Length > 0)
                        TypedText.Remove(TypedText.Length - 1, 1);
                }
                else
                {
                    string str;
                    switch (keyChar)
                    {
                        case '\r':
                            str = "\n";
                            break;
                        default:
                            str = keyChar.ToString();
                            break;
                    }

                    TypedText.Append(str);
                }

                pbTyping.Refresh();
            }
        }

        private void tmrTimer_Tick(object sender, EventArgs e)
        {
            displayTime();
        }

        private void displayTime()
        {
            lblTime.Text = string.Format("{0:00}:{1:00}",
                                         stopwatch.Elapsed.Minutes,
                                         stopwatch.Elapsed.Seconds);
        }
    }
}

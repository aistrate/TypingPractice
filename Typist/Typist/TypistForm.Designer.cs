namespace Typist
{
    partial class TypistForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TypistForm));
            this.btnImport = new System.Windows.Forms.Button();
            this.dlgImport = new System.Windows.Forms.OpenFileDialog();
            this.btnStart = new System.Windows.Forms.Button();
            this.lblTime = new System.Windows.Forms.Label();
            this.tmrTimer = new System.Windows.Forms.Timer(this.components);
            this.lblWPM = new System.Windows.Forms.Label();
            this.tblTopLine = new System.Windows.Forms.TableLayoutPanel();
            this.tblBottomLine = new System.Windows.Forms.TableLayoutPanel();
            this.lblErrorCount = new System.Windows.Forms.Label();
            this.lblAccuracy = new System.Windows.Forms.Label();
            this.dlgFontDialog = new System.Windows.Forms.FontDialog();
            this.lblStatusBar = new System.Windows.Forms.Label();
            this.mnuContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.pauseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pauseAndMinimizeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.changeFontToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsCustomFontToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewCustomFontToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.previousFontToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.nextFontToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.predefinedFontsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.picTyping = new Typist.TypingBox();
            this.mnuContextMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picTyping)).BeginInit();
            this.SuspendLayout();
            // 
            // btnImport
            // 
            this.btnImport.Location = new System.Drawing.Point(0, 1);
            this.btnImport.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(101, 25);
            this.btnImport.TabIndex = 0;
            this.btnImport.Text = "Import...";
            this.btnImport.UseMnemonic = false;
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // dlgImport
            // 
            this.dlgImport.DefaultExt = "txt";
            this.dlgImport.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            // 
            // btnStart
            // 
            this.btnStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStart.Enabled = false;
            this.btnStart.Location = new System.Drawing.Point(361, 1);
            this.btnStart.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(101, 25);
            this.btnStart.TabIndex = 1;
            this.btnStart.Text = "Start";
            this.btnStart.UseMnemonic = false;
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // lblTime
            // 
            this.lblTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTime.Location = new System.Drawing.Point(295, 6);
            this.lblTime.Margin = new System.Windows.Forms.Padding(0);
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size(59, 14);
            this.lblTime.TabIndex = 5;
            this.lblTime.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // tmrTimer
            // 
            this.tmrTimer.Enabled = true;
            this.tmrTimer.Tick += new System.EventHandler(this.tmrTimer_Tick);
            // 
            // lblWPM
            // 
            this.lblWPM.Location = new System.Drawing.Point(103, 6);
            this.lblWPM.Margin = new System.Windows.Forms.Padding(0);
            this.lblWPM.Name = "lblWPM";
            this.lblWPM.Size = new System.Drawing.Size(65, 14);
            this.lblWPM.TabIndex = 6;
            this.lblWPM.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // tblTopLine
            // 
            this.tblTopLine.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tblTopLine.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Outset;
            this.tblTopLine.ColumnCount = 1;
            this.tblTopLine.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblTopLine.Location = new System.Drawing.Point(0, 26);
            this.tblTopLine.Name = "tblTopLine";
            this.tblTopLine.RowCount = 1;
            this.tblTopLine.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblTopLine.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 1F));
            this.tblTopLine.Size = new System.Drawing.Size(463, 2);
            this.tblTopLine.TabIndex = 9;
            // 
            // tblBottomLine
            // 
            this.tblBottomLine.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tblBottomLine.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Outset;
            this.tblBottomLine.ColumnCount = 1;
            this.tblBottomLine.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblBottomLine.Location = new System.Drawing.Point(0, 548);
            this.tblBottomLine.Name = "tblBottomLine";
            this.tblBottomLine.RowCount = 1;
            this.tblBottomLine.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblBottomLine.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 1F));
            this.tblBottomLine.Size = new System.Drawing.Size(463, 2);
            this.tblBottomLine.TabIndex = 10;
            // 
            // lblErrorCount
            // 
            this.lblErrorCount.BackColor = System.Drawing.SystemColors.Control;
            this.lblErrorCount.Location = new System.Drawing.Point(171, 6);
            this.lblErrorCount.Margin = new System.Windows.Forms.Padding(0);
            this.lblErrorCount.Name = "lblErrorCount";
            this.lblErrorCount.Size = new System.Drawing.Size(65, 14);
            this.lblErrorCount.TabIndex = 10;
            this.lblErrorCount.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblAccuracy
            // 
            this.lblAccuracy.BackColor = System.Drawing.SystemColors.Control;
            this.lblAccuracy.Location = new System.Drawing.Point(239, 6);
            this.lblAccuracy.Margin = new System.Windows.Forms.Padding(0);
            this.lblAccuracy.Name = "lblAccuracy";
            this.lblAccuracy.Size = new System.Drawing.Size(59, 14);
            this.lblAccuracy.TabIndex = 11;
            this.lblAccuracy.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // dlgFontDialog
            // 
            this.dlgFontDialog.AllowScriptChange = false;
            this.dlgFontDialog.FixedPitchOnly = true;
            this.dlgFontDialog.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dlgFontDialog.FontMustExist = true;
            this.dlgFontDialog.ShowApply = true;
            this.dlgFontDialog.ShowEffects = false;
            this.dlgFontDialog.Apply += new System.EventHandler(this.dlgFontDialog_Apply);
            // 
            // lblStatusBar
            // 
            this.lblStatusBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblStatusBar.BackColor = System.Drawing.SystemColors.Control;
            this.lblStatusBar.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatusBar.Location = new System.Drawing.Point(0, 551);
            this.lblStatusBar.Name = "lblStatusBar";
            this.lblStatusBar.Size = new System.Drawing.Size(462, 15);
            this.lblStatusBar.TabIndex = 12;
            // 
            // mnuContextMenu
            // 
            this.mnuContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.pauseToolStripMenuItem,
            this.pauseAndMinimizeToolStripMenuItem,
            this.settingsToolStripMenuItem,
            this.toolStripSeparator3,
            this.changeFontToolStripMenuItem,
            this.saveAsCustomFontToolStripMenuItem,
            this.viewCustomFontToolStripMenuItem,
            this.toolStripSeparator1,
            this.previousFontToolStripMenuItem,
            this.nextFontToolStripMenuItem,
            this.predefinedFontsToolStripMenuItem});
            this.mnuContextMenu.Name = "contextMenuStrip1";
            this.mnuContextMenu.Size = new System.Drawing.Size(276, 214);
            this.mnuContextMenu.Closed += new System.Windows.Forms.ToolStripDropDownClosedEventHandler(this.mnuContextMenu_Closed);
            // 
            // pauseToolStripMenuItem
            // 
            this.pauseToolStripMenuItem.Name = "pauseToolStripMenuItem";
            this.pauseToolStripMenuItem.ShortcutKeyDisplayString = "Ctrl+P";
            this.pauseToolStripMenuItem.Size = new System.Drawing.Size(275, 22);
            this.pauseToolStripMenuItem.Text = "Pause Typing Practice";
            this.pauseToolStripMenuItem.Click += new System.EventHandler(this.pauseToolStripMenuItem_Click);
            // 
            // pauseAndMinimizeToolStripMenuItem
            // 
            this.pauseAndMinimizeToolStripMenuItem.Name = "pauseAndMinimizeToolStripMenuItem";
            this.pauseAndMinimizeToolStripMenuItem.ShortcutKeyDisplayString = "Ctrl+M";
            this.pauseAndMinimizeToolStripMenuItem.Size = new System.Drawing.Size(275, 22);
            this.pauseAndMinimizeToolStripMenuItem.Text = "Pause and Minimize";
            this.pauseAndMinimizeToolStripMenuItem.Click += new System.EventHandler(this.pauseAndMinimizeToolStripMenuItem_Click);
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.ShortcutKeyDisplayString = "Ctrl+X";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(275, 22);
            this.settingsToolStripMenuItem.Text = "Settings...";
            this.settingsToolStripMenuItem.Click += new System.EventHandler(this.settingsToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(272, 6);
            // 
            // changeFontToolStripMenuItem
            // 
            this.changeFontToolStripMenuItem.Name = "changeFontToolStripMenuItem";
            this.changeFontToolStripMenuItem.ShortcutKeyDisplayString = "Ctrl+F";
            this.changeFontToolStripMenuItem.Size = new System.Drawing.Size(275, 22);
            this.changeFontToolStripMenuItem.Text = "Font Properties...";
            this.changeFontToolStripMenuItem.Click += new System.EventHandler(this.changeFontToolStripMenuItem_Click);
            // 
            // saveAsCustomFontToolStripMenuItem
            // 
            this.saveAsCustomFontToolStripMenuItem.Name = "saveAsCustomFontToolStripMenuItem";
            this.saveAsCustomFontToolStripMenuItem.ShortcutKeyDisplayString = "Ctrl+S";
            this.saveAsCustomFontToolStripMenuItem.Size = new System.Drawing.Size(275, 22);
            this.saveAsCustomFontToolStripMenuItem.Text = "Save As Custom Font";
            this.saveAsCustomFontToolStripMenuItem.Click += new System.EventHandler(this.saveAsCustomFontToolStripMenuItem_Click);
            // 
            // viewCustomFontToolStripMenuItem
            // 
            this.viewCustomFontToolStripMenuItem.Name = "viewCustomFontToolStripMenuItem";
            this.viewCustomFontToolStripMenuItem.ShortcutKeyDisplayString = "Ctrl+C";
            this.viewCustomFontToolStripMenuItem.Size = new System.Drawing.Size(275, 22);
            this.viewCustomFontToolStripMenuItem.Text = "Custom Font";
            this.viewCustomFontToolStripMenuItem.Click += new System.EventHandler(this.viewCustomFontToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(272, 6);
            // 
            // previousFontToolStripMenuItem
            // 
            this.previousFontToolStripMenuItem.Name = "previousFontToolStripMenuItem";
            this.previousFontToolStripMenuItem.ShortcutKeyDisplayString = "Ctrl+Shift+N";
            this.previousFontToolStripMenuItem.Size = new System.Drawing.Size(275, 22);
            this.previousFontToolStripMenuItem.Text = "Previous Predefined Font";
            this.previousFontToolStripMenuItem.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.previousFontToolStripMenuItem.Click += new System.EventHandler(this.previousFontToolStripMenuItem_Click);
            // 
            // nextFontToolStripMenuItem
            // 
            this.nextFontToolStripMenuItem.Name = "nextFontToolStripMenuItem";
            this.nextFontToolStripMenuItem.ShortcutKeyDisplayString = "Ctrl+N";
            this.nextFontToolStripMenuItem.Size = new System.Drawing.Size(275, 22);
            this.nextFontToolStripMenuItem.Text = "Next Predefined Font";
            this.nextFontToolStripMenuItem.Click += new System.EventHandler(this.nextFontToolStripMenuItem_Click);
            // 
            // predefinedFontsToolStripMenuItem
            // 
            this.predefinedFontsToolStripMenuItem.Name = "predefinedFontsToolStripMenuItem";
            this.predefinedFontsToolStripMenuItem.Size = new System.Drawing.Size(275, 22);
            this.predefinedFontsToolStripMenuItem.Text = "Predefined Fonts";
            this.predefinedFontsToolStripMenuItem.DropDownClosed += new System.EventHandler(this.predefinedFontsToolStripMenuItem_DropDownClosed);
            // 
            // picTyping
            // 
            this.picTyping.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.picTyping.BackColor = System.Drawing.Color.White;
            this.picTyping.BarCursorVOffset = 0F;
            this.picTyping.CharCursorVOffset = 0F;
            this.picTyping.ContextMenuStrip = this.mnuContextMenu;
            this.picTyping.ErrorBackgroundVOffset = 0F;
            this.picTyping.ImportedText = null;
            this.picTyping.Location = new System.Drawing.Point(0, 29);
            this.picTyping.Name = "picTyping";
            this.picTyping.Size = new System.Drawing.Size(462, 519);
            this.picTyping.TabIndex = 2;
            this.picTyping.TabStop = false;
            this.picTyping.Theme = null;
            this.picTyping.TypedText = null;
            this.picTyping.TypingFont = null;
            this.picTyping.DrawingCursor += new System.ComponentModel.CancelEventHandler(this.picTyping_DrawingCursor);
            // 
            // TypistForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(462, 566);
            this.Controls.Add(this.lblStatusBar);
            this.Controls.Add(this.tblBottomLine);
            this.Controls.Add(this.lblAccuracy);
            this.Controls.Add(this.lblErrorCount);
            this.Controls.Add(this.tblTopLine);
            this.Controls.Add(this.lblWPM);
            this.Controls.Add(this.lblTime);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.picTyping);
            this.Controls.Add(this.btnImport);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.MinimumSize = new System.Drawing.Size(470, 150);
            this.Name = "TypistForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Typist";
            this.Deactivate += new System.EventHandler(this.TypistForm_Deactivate);
            this.Load += new System.EventHandler(this.TypistForm_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.TypistForm_Paint);
            this.SizeChanged += new System.EventHandler(this.TypistForm_SizeChanged);
            this.Activated += new System.EventHandler(this.TypistForm_Activated);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TypistForm_KeyPress);
            this.Move += new System.EventHandler(this.TypistForm_Move);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TypistForm_KeyDown);
            this.mnuContextMenu.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picTyping)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnImport;
        private System.Windows.Forms.OpenFileDialog dlgImport;
        private TypingBox picTyping;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Label lblTime;
        private System.Windows.Forms.Timer tmrTimer;
        private System.Windows.Forms.Label lblWPM;
        private System.Windows.Forms.TableLayoutPanel tblTopLine;
        private System.Windows.Forms.Label lblErrorCount;
        private System.Windows.Forms.Label lblAccuracy;
        private System.Windows.Forms.FontDialog dlgFontDialog;
        private System.Windows.Forms.TableLayoutPanel tblBottomLine;
        private System.Windows.Forms.Label lblStatusBar;
        private System.Windows.Forms.ContextMenuStrip mnuContextMenu;
        private System.Windows.Forms.ToolStripMenuItem pauseToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem changeFontToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem previousFontToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem nextFontToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsCustomFontToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem predefinedFontsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pauseAndMinimizeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewCustomFontToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
    }
}


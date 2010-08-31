namespace Typist
{
    partial class SettingsDialog
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
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.chkBeepOnError = new System.Windows.Forms.CheckBox();
            this.chkAllowBackspace = new System.Windows.Forms.CheckBox();
            this.chkVisibleNewlines = new System.Windows.Forms.CheckBox();
            this.chkCountWhitespaceAsWordChars = new System.Windows.Forms.CheckBox();
            this.chkCountErrorsAsWordChars = new System.Windows.Forms.CheckBox();
            this.chkAskBeforeCloseDuringPractice = new System.Windows.Forms.CheckBox();
            this.chkShowCursorWhenPaused = new System.Windows.Forms.CheckBox();
            this.chkPauseAfterElapsed = new System.Windows.Forms.CheckBox();
            this.txtPauseAfterElapsed = new System.Windows.Forms.TextBox();
            this.lblSeconds = new System.Windows.Forms.Label();
            this.chkRemoveMultipleWhitespace = new System.Windows.Forms.CheckBox();
            this.chkWordWrap = new System.Windows.Forms.CheckBox();
            this.chkRemoveEndOfLineSpaces = new System.Windows.Forms.CheckBox();
            this.chkRememberLastImportedFile = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnPredefChars = new System.Windows.Forms.Button();
            this.btnPredefCode = new System.Windows.Forms.Button();
            this.btnPredefArticle = new System.Windows.Forms.Button();
            this.btnPredefDebug = new System.Windows.Forms.Button();
            this.btnPredefRelease = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Location = new System.Drawing.Point(151, 352);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 100;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(232, 352);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 101;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // chkBeepOnError
            // 
            this.chkBeepOnError.AutoSize = true;
            this.chkBeepOnError.Location = new System.Drawing.Point(12, 64);
            this.chkBeepOnError.Name = "chkBeepOnError";
            this.chkBeepOnError.Size = new System.Drawing.Size(90, 17);
            this.chkBeepOnError.TabIndex = 0;
            this.chkBeepOnError.Text = "Beep on error";
            this.chkBeepOnError.UseVisualStyleBackColor = false;
            // 
            // chkAllowBackspace
            // 
            this.chkAllowBackspace.AutoSize = true;
            this.chkAllowBackspace.Location = new System.Drawing.Point(12, 87);
            this.chkAllowBackspace.Name = "chkAllowBackspace";
            this.chkAllowBackspace.Size = new System.Drawing.Size(107, 17);
            this.chkAllowBackspace.TabIndex = 1;
            this.chkAllowBackspace.Text = "Allow backspace";
            this.chkAllowBackspace.UseVisualStyleBackColor = false;
            // 
            // chkVisibleNewlines
            // 
            this.chkVisibleNewlines.AutoSize = true;
            this.chkVisibleNewlines.Location = new System.Drawing.Point(12, 133);
            this.chkVisibleNewlines.Name = "chkVisibleNewlines";
            this.chkVisibleNewlines.Size = new System.Drawing.Size(120, 17);
            this.chkVisibleNewlines.TabIndex = 3;
            this.chkVisibleNewlines.Text = "Show newlines as ¶";
            this.chkVisibleNewlines.UseVisualStyleBackColor = false;
            // 
            // chkCountWhitespaceAsWordChars
            // 
            this.chkCountWhitespaceAsWordChars.AutoSize = true;
            this.chkCountWhitespaceAsWordChars.Location = new System.Drawing.Point(12, 225);
            this.chkCountWhitespaceAsWordChars.Name = "chkCountWhitespaceAsWordChars";
            this.chkCountWhitespaceAsWordChars.Size = new System.Drawing.Size(204, 17);
            this.chkCountWhitespaceAsWordChars.TabIndex = 7;
            this.chkCountWhitespaceAsWordChars.Text = "Count whitespace as word characters";
            this.chkCountWhitespaceAsWordChars.UseVisualStyleBackColor = false;
            // 
            // chkCountErrorsAsWordChars
            // 
            this.chkCountErrorsAsWordChars.AutoSize = true;
            this.chkCountErrorsAsWordChars.Location = new System.Drawing.Point(12, 248);
            this.chkCountErrorsAsWordChars.Name = "chkCountErrorsAsWordChars";
            this.chkCountErrorsAsWordChars.Size = new System.Drawing.Size(176, 17);
            this.chkCountErrorsAsWordChars.TabIndex = 8;
            this.chkCountErrorsAsWordChars.Text = "Count errors as word characters";
            this.chkCountErrorsAsWordChars.UseVisualStyleBackColor = false;
            // 
            // chkAskBeforeCloseDuringPractice
            // 
            this.chkAskBeforeCloseDuringPractice.AutoSize = true;
            this.chkAskBeforeCloseDuringPractice.Location = new System.Drawing.Point(12, 271);
            this.chkAskBeforeCloseDuringPractice.Name = "chkAskBeforeCloseDuringPractice";
            this.chkAskBeforeCloseDuringPractice.Size = new System.Drawing.Size(186, 17);
            this.chkAskBeforeCloseDuringPractice.TabIndex = 9;
            this.chkAskBeforeCloseDuringPractice.Text = "Ask before closing during practice";
            this.chkAskBeforeCloseDuringPractice.UseVisualStyleBackColor = false;
            // 
            // chkShowCursorWhenPaused
            // 
            this.chkShowCursorWhenPaused.AutoSize = true;
            this.chkShowCursorWhenPaused.Location = new System.Drawing.Point(12, 294);
            this.chkShowCursorWhenPaused.Name = "chkShowCursorWhenPaused";
            this.chkShowCursorWhenPaused.Size = new System.Drawing.Size(152, 17);
            this.chkShowCursorWhenPaused.TabIndex = 10;
            this.chkShowCursorWhenPaused.Text = "Show cursor when paused";
            this.chkShowCursorWhenPaused.UseVisualStyleBackColor = false;
            // 
            // chkPauseAfterElapsed
            // 
            this.chkPauseAfterElapsed.AutoSize = true;
            this.chkPauseAfterElapsed.Location = new System.Drawing.Point(12, 317);
            this.chkPauseAfterElapsed.Name = "chkPauseAfterElapsed";
            this.chkPauseAfterElapsed.Size = new System.Drawing.Size(80, 17);
            this.chkPauseAfterElapsed.TabIndex = 11;
            this.chkPauseAfterElapsed.Text = "Pause after";
            this.chkPauseAfterElapsed.UseVisualStyleBackColor = false;
            this.chkPauseAfterElapsed.CheckedChanged += new System.EventHandler(this.chkPauseAfterElapsed_CheckedChanged);
            // 
            // txtPauseAfterElapsed
            // 
            this.txtPauseAfterElapsed.Location = new System.Drawing.Point(90, 315);
            this.txtPauseAfterElapsed.MaxLength = 4;
            this.txtPauseAfterElapsed.Name = "txtPauseAfterElapsed";
            this.txtPauseAfterElapsed.ShortcutsEnabled = false;
            this.txtPauseAfterElapsed.Size = new System.Drawing.Size(36, 20);
            this.txtPauseAfterElapsed.TabIndex = 12;
            this.txtPauseAfterElapsed.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtPauseAfterElapsed.Leave += new System.EventHandler(this.txtPauseAfterElapsed_Leave);
            this.txtPauseAfterElapsed.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtPauseAfterElapsed_KeyPress);
            // 
            // lblSeconds
            // 
            this.lblSeconds.AutoSize = true;
            this.lblSeconds.Location = new System.Drawing.Point(126, 318);
            this.lblSeconds.Name = "lblSeconds";
            this.lblSeconds.Size = new System.Drawing.Size(103, 13);
            this.lblSeconds.TabIndex = 110;
            this.lblSeconds.Text = "seconds of inactivity";
            // 
            // chkRemoveMultipleWhitespace
            // 
            this.chkRemoveMultipleWhitespace.AutoSize = true;
            this.chkRemoveMultipleWhitespace.Location = new System.Drawing.Point(12, 179);
            this.chkRemoveMultipleWhitespace.Name = "chkRemoveMultipleWhitespace";
            this.chkRemoveMultipleWhitespace.Size = new System.Drawing.Size(235, 17);
            this.chkRemoveMultipleWhitespace.TabIndex = 5;
            this.chkRemoveMultipleWhitespace.Text = "Remove multiple whitespace when importing";
            this.chkRemoveMultipleWhitespace.UseVisualStyleBackColor = false;
            // 
            // chkWordWrap
            // 
            this.chkWordWrap.AutoSize = true;
            this.chkWordWrap.Location = new System.Drawing.Point(12, 110);
            this.chkWordWrap.Name = "chkWordWrap";
            this.chkWordWrap.Size = new System.Drawing.Size(78, 17);
            this.chkWordWrap.TabIndex = 2;
            this.chkWordWrap.Text = "Word wrap";
            this.chkWordWrap.UseVisualStyleBackColor = false;
            // 
            // chkRemoveEndOfLineSpaces
            // 
            this.chkRemoveEndOfLineSpaces.AutoSize = true;
            this.chkRemoveEndOfLineSpaces.Location = new System.Drawing.Point(12, 156);
            this.chkRemoveEndOfLineSpaces.Name = "chkRemoveEndOfLineSpaces";
            this.chkRemoveEndOfLineSpaces.Size = new System.Drawing.Size(229, 17);
            this.chkRemoveEndOfLineSpaces.TabIndex = 4;
            this.chkRemoveEndOfLineSpaces.Text = "Remove end-of-line spaces when importing";
            this.chkRemoveEndOfLineSpaces.UseVisualStyleBackColor = false;
            // 
            // chkRememberLastImportedFile
            // 
            this.chkRememberLastImportedFile.AutoSize = true;
            this.chkRememberLastImportedFile.Location = new System.Drawing.Point(12, 202);
            this.chkRememberLastImportedFile.Name = "chkRememberLastImportedFile";
            this.chkRememberLastImportedFile.Size = new System.Drawing.Size(155, 17);
            this.chkRememberLastImportedFile.TabIndex = 6;
            this.chkRememberLastImportedFile.Text = "Remember last imported file";
            this.chkRememberLastImportedFile.UseVisualStyleBackColor = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 13);
            this.label1.TabIndex = 111;
            this.label1.Text = "Predefined:";
            // 
            // btnPredefChars
            // 
            this.btnPredefChars.Location = new System.Drawing.Point(12, 28);
            this.btnPredefChars.Name = "btnPredefChars";
            this.btnPredefChars.Size = new System.Drawing.Size(55, 23);
            this.btnPredefChars.TabIndex = 112;
            this.btnPredefChars.Text = "Chars";
            this.btnPredefChars.UseVisualStyleBackColor = true;
            this.btnPredefChars.Click += new System.EventHandler(this.btnPredefChars_Click);
            // 
            // btnPredefCode
            // 
            this.btnPredefCode.Location = new System.Drawing.Point(126, 28);
            this.btnPredefCode.Name = "btnPredefCode";
            this.btnPredefCode.Size = new System.Drawing.Size(55, 23);
            this.btnPredefCode.TabIndex = 113;
            this.btnPredefCode.Text = "Code";
            this.btnPredefCode.UseVisualStyleBackColor = true;
            this.btnPredefCode.Click += new System.EventHandler(this.btnPredefCode_Click);
            // 
            // btnPredefArticle
            // 
            this.btnPredefArticle.Location = new System.Drawing.Point(69, 28);
            this.btnPredefArticle.Name = "btnPredefArticle";
            this.btnPredefArticle.Size = new System.Drawing.Size(55, 23);
            this.btnPredefArticle.TabIndex = 114;
            this.btnPredefArticle.Text = "Article";
            this.btnPredefArticle.UseVisualStyleBackColor = true;
            this.btnPredefArticle.Click += new System.EventHandler(this.btnPredefArticle_Click);
            // 
            // btnPredefDebug
            // 
            this.btnPredefDebug.Location = new System.Drawing.Point(250, 28);
            this.btnPredefDebug.Name = "btnPredefDebug";
            this.btnPredefDebug.Size = new System.Drawing.Size(55, 23);
            this.btnPredefDebug.TabIndex = 116;
            this.btnPredefDebug.Text = "Debug";
            this.btnPredefDebug.UseVisualStyleBackColor = true;
            this.btnPredefDebug.Click += new System.EventHandler(this.btnPredefDebug_Click);
            // 
            // btnPredefRelease
            // 
            this.btnPredefRelease.Location = new System.Drawing.Point(193, 28);
            this.btnPredefRelease.Name = "btnPredefRelease";
            this.btnPredefRelease.Size = new System.Drawing.Size(55, 23);
            this.btnPredefRelease.TabIndex = 115;
            this.btnPredefRelease.Text = "Release";
            this.btnPredefRelease.UseVisualStyleBackColor = true;
            this.btnPredefRelease.Click += new System.EventHandler(this.btnPredefRelease_Click);
            // 
            // SettingsDialog
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(319, 387);
            this.Controls.Add(this.btnPredefDebug);
            this.Controls.Add(this.btnPredefRelease);
            this.Controls.Add(this.btnPredefArticle);
            this.Controls.Add(this.btnPredefCode);
            this.Controls.Add(this.btnPredefChars);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.chkRememberLastImportedFile);
            this.Controls.Add(this.chkRemoveEndOfLineSpaces);
            this.Controls.Add(this.chkWordWrap);
            this.Controls.Add(this.chkRemoveMultipleWhitespace);
            this.Controls.Add(this.lblSeconds);
            this.Controls.Add(this.txtPauseAfterElapsed);
            this.Controls.Add(this.chkPauseAfterElapsed);
            this.Controls.Add(this.chkShowCursorWhenPaused);
            this.Controls.Add(this.chkAskBeforeCloseDuringPractice);
            this.Controls.Add(this.chkCountErrorsAsWordChars);
            this.Controls.Add(this.chkCountWhitespaceAsWordChars);
            this.Controls.Add(this.chkVisibleNewlines);
            this.Controls.Add(this.chkAllowBackspace);
            this.Controls.Add(this.chkBeepOnError);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SettingsDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Settings";
            this.Load += new System.EventHandler(this.SettingsDialog_Load);
            this.VisibleChanged += new System.EventHandler(this.SettingsDialog_VisibleChanged);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.CheckBox chkBeepOnError;
        private System.Windows.Forms.CheckBox chkAllowBackspace;
        private System.Windows.Forms.CheckBox chkVisibleNewlines;
        private System.Windows.Forms.CheckBox chkCountWhitespaceAsWordChars;
        private System.Windows.Forms.CheckBox chkCountErrorsAsWordChars;
        private System.Windows.Forms.CheckBox chkAskBeforeCloseDuringPractice;
        private System.Windows.Forms.CheckBox chkShowCursorWhenPaused;
        private System.Windows.Forms.CheckBox chkPauseAfterElapsed;
        private System.Windows.Forms.TextBox txtPauseAfterElapsed;
        private System.Windows.Forms.Label lblSeconds;
        private System.Windows.Forms.CheckBox chkRemoveMultipleWhitespace;
        private System.Windows.Forms.CheckBox chkWordWrap;
        private System.Windows.Forms.CheckBox chkRemoveEndOfLineSpaces;
        private System.Windows.Forms.CheckBox chkRememberLastImportedFile;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnPredefChars;
        private System.Windows.Forms.Button btnPredefCode;
        private System.Windows.Forms.Button btnPredefArticle;
        private System.Windows.Forms.Button btnPredefDebug;
        private System.Windows.Forms.Button btnPredefRelease;
    }
}
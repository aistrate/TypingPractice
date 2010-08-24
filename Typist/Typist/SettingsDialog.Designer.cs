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
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Location = new System.Drawing.Point(111, 254);
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
            this.btnCancel.Location = new System.Drawing.Point(192, 254);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 101;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // chkBeepOnError
            // 
            this.chkBeepOnError.AutoSize = true;
            this.chkBeepOnError.Location = new System.Drawing.Point(12, 12);
            this.chkBeepOnError.Name = "chkBeepOnError";
            this.chkBeepOnError.Size = new System.Drawing.Size(90, 17);
            this.chkBeepOnError.TabIndex = 0;
            this.chkBeepOnError.Text = "Beep on error";
            this.chkBeepOnError.UseVisualStyleBackColor = false;
            // 
            // chkAllowBackspace
            // 
            this.chkAllowBackspace.AutoSize = true;
            this.chkAllowBackspace.Location = new System.Drawing.Point(12, 35);
            this.chkAllowBackspace.Name = "chkAllowBackspace";
            this.chkAllowBackspace.Size = new System.Drawing.Size(107, 17);
            this.chkAllowBackspace.TabIndex = 1;
            this.chkAllowBackspace.Text = "Allow backspace";
            this.chkAllowBackspace.UseVisualStyleBackColor = false;
            // 
            // chkVisibleNewlines
            // 
            this.chkVisibleNewlines.AutoSize = true;
            this.chkVisibleNewlines.Location = new System.Drawing.Point(12, 81);
            this.chkVisibleNewlines.Name = "chkVisibleNewlines";
            this.chkVisibleNewlines.Size = new System.Drawing.Size(120, 17);
            this.chkVisibleNewlines.TabIndex = 2;
            this.chkVisibleNewlines.Text = "Show newlines as ¶";
            this.chkVisibleNewlines.UseVisualStyleBackColor = false;
            // 
            // chkCountWhitespaceAsWordChars
            // 
            this.chkCountWhitespaceAsWordChars.AutoSize = true;
            this.chkCountWhitespaceAsWordChars.Location = new System.Drawing.Point(12, 127);
            this.chkCountWhitespaceAsWordChars.Name = "chkCountWhitespaceAsWordChars";
            this.chkCountWhitespaceAsWordChars.Size = new System.Drawing.Size(204, 17);
            this.chkCountWhitespaceAsWordChars.TabIndex = 4;
            this.chkCountWhitespaceAsWordChars.Text = "Count whitespace as word characters";
            this.chkCountWhitespaceAsWordChars.UseVisualStyleBackColor = false;
            // 
            // chkCountErrorsAsWordChars
            // 
            this.chkCountErrorsAsWordChars.AutoSize = true;
            this.chkCountErrorsAsWordChars.Location = new System.Drawing.Point(12, 150);
            this.chkCountErrorsAsWordChars.Name = "chkCountErrorsAsWordChars";
            this.chkCountErrorsAsWordChars.Size = new System.Drawing.Size(176, 17);
            this.chkCountErrorsAsWordChars.TabIndex = 5;
            this.chkCountErrorsAsWordChars.Text = "Count errors as word characters";
            this.chkCountErrorsAsWordChars.UseVisualStyleBackColor = false;
            // 
            // chkAskBeforeCloseDuringPractice
            // 
            this.chkAskBeforeCloseDuringPractice.AutoSize = true;
            this.chkAskBeforeCloseDuringPractice.Location = new System.Drawing.Point(12, 173);
            this.chkAskBeforeCloseDuringPractice.Name = "chkAskBeforeCloseDuringPractice";
            this.chkAskBeforeCloseDuringPractice.Size = new System.Drawing.Size(186, 17);
            this.chkAskBeforeCloseDuringPractice.TabIndex = 6;
            this.chkAskBeforeCloseDuringPractice.Text = "Ask before closing during practice";
            this.chkAskBeforeCloseDuringPractice.UseVisualStyleBackColor = false;
            // 
            // chkShowCursorWhenPaused
            // 
            this.chkShowCursorWhenPaused.AutoSize = true;
            this.chkShowCursorWhenPaused.Location = new System.Drawing.Point(12, 196);
            this.chkShowCursorWhenPaused.Name = "chkShowCursorWhenPaused";
            this.chkShowCursorWhenPaused.Size = new System.Drawing.Size(152, 17);
            this.chkShowCursorWhenPaused.TabIndex = 7;
            this.chkShowCursorWhenPaused.Text = "Show cursor when paused";
            this.chkShowCursorWhenPaused.UseVisualStyleBackColor = false;
            // 
            // chkPauseAfterElapsed
            // 
            this.chkPauseAfterElapsed.AutoSize = true;
            this.chkPauseAfterElapsed.Location = new System.Drawing.Point(12, 219);
            this.chkPauseAfterElapsed.Name = "chkPauseAfterElapsed";
            this.chkPauseAfterElapsed.Size = new System.Drawing.Size(80, 17);
            this.chkPauseAfterElapsed.TabIndex = 8;
            this.chkPauseAfterElapsed.Text = "Pause after";
            this.chkPauseAfterElapsed.UseVisualStyleBackColor = false;
            this.chkPauseAfterElapsed.CheckedChanged += new System.EventHandler(this.chkPauseAfterElapsed_CheckedChanged);
            // 
            // txtPauseAfterElapsed
            // 
            this.txtPauseAfterElapsed.Location = new System.Drawing.Point(90, 217);
            this.txtPauseAfterElapsed.MaxLength = 4;
            this.txtPauseAfterElapsed.Name = "txtPauseAfterElapsed";
            this.txtPauseAfterElapsed.ShortcutsEnabled = false;
            this.txtPauseAfterElapsed.Size = new System.Drawing.Size(36, 20);
            this.txtPauseAfterElapsed.TabIndex = 9;
            this.txtPauseAfterElapsed.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtPauseAfterElapsed.Leave += new System.EventHandler(this.txtPauseAfterElapsed_Leave);
            this.txtPauseAfterElapsed.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtPauseAfterElapsed_KeyPress);
            // 
            // lblSeconds
            // 
            this.lblSeconds.AutoSize = true;
            this.lblSeconds.Location = new System.Drawing.Point(127, 220);
            this.lblSeconds.Name = "lblSeconds";
            this.lblSeconds.Size = new System.Drawing.Size(103, 13);
            this.lblSeconds.TabIndex = 110;
            this.lblSeconds.Text = "seconds of inactivity";
            // 
            // chkRemoveMultipleWhitespace
            // 
            this.chkRemoveMultipleWhitespace.AutoSize = true;
            this.chkRemoveMultipleWhitespace.Location = new System.Drawing.Point(12, 104);
            this.chkRemoveMultipleWhitespace.Name = "chkRemoveMultipleWhitespace";
            this.chkRemoveMultipleWhitespace.Size = new System.Drawing.Size(235, 17);
            this.chkRemoveMultipleWhitespace.TabIndex = 3;
            this.chkRemoveMultipleWhitespace.Text = "Remove multiple whitespace when importing";
            this.chkRemoveMultipleWhitespace.UseVisualStyleBackColor = false;
            // 
            // chkWordWrap
            // 
            this.chkWordWrap.AutoSize = true;
            this.chkWordWrap.Location = new System.Drawing.Point(12, 58);
            this.chkWordWrap.Name = "chkWordWrap";
            this.chkWordWrap.Size = new System.Drawing.Size(78, 17);
            this.chkWordWrap.TabIndex = 111;
            this.chkWordWrap.Text = "Word wrap";
            this.chkWordWrap.UseVisualStyleBackColor = false;
            // 
            // SettingsDialog
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(279, 289);
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
    }
}
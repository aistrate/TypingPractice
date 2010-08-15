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
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Location = new System.Drawing.Point(148, 102);
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
            this.btnCancel.Location = new System.Drawing.Point(229, 102);
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
            this.chkAllowBackspace.TabIndex = 102;
            this.chkAllowBackspace.Text = "Allow backspace";
            this.chkAllowBackspace.UseVisualStyleBackColor = false;
            // 
            // chkVisibleNewlines
            // 
            this.chkVisibleNewlines.AutoSize = true;
            this.chkVisibleNewlines.Location = new System.Drawing.Point(12, 58);
            this.chkVisibleNewlines.Name = "chkVisibleNewlines";
            this.chkVisibleNewlines.Size = new System.Drawing.Size(100, 17);
            this.chkVisibleNewlines.TabIndex = 103;
            this.chkVisibleNewlines.Text = "Visible newlines";
            this.chkVisibleNewlines.UseVisualStyleBackColor = false;
            // 
            // SettingsDialog
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(316, 137);
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
    }
}
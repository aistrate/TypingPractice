namespace SourceCodeTextCreator
{
    partial class SourceCodeTextCreator
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtInputFile = new System.Windows.Forms.TextBox();
            this.txtOutputFolder = new System.Windows.Forms.TextBox();
            this.btnBrowseInputFile = new System.Windows.Forms.Button();
            this.btnOutputFolder = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.txtLinesPerFile = new System.Windows.Forms.TextBox();
            this.cbRemoveLineComments = new System.Windows.Forms.CheckBox();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.comLineCommentStartChars = new System.Windows.Forms.ComboBox();
            this.cbRemoveLineCommentsAtEndOfLine = new System.Windows.Forms.CheckBox();
            this.comBlockCommentStartChars = new System.Windows.Forms.ComboBox();
            this.cbRemoveBlockComments = new System.Windows.Forms.CheckBox();
            this.comBlockCommentEndChars = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cbRemoveLiterateComments = new System.Windows.Forms.CheckBox();
            this.comStringDelimiter = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Input File:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 39);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(74, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Output Folder:";
            // 
            // txtInputFile
            // 
            this.txtInputFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtInputFile.Location = new System.Drawing.Point(91, 12);
            this.txtInputFile.Name = "txtInputFile";
            this.txtInputFile.Size = new System.Drawing.Size(531, 20);
            this.txtInputFile.TabIndex = 2;
            // 
            // txtOutputFolder
            // 
            this.txtOutputFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtOutputFolder.Location = new System.Drawing.Point(91, 36);
            this.txtOutputFolder.Name = "txtOutputFolder";
            this.txtOutputFolder.Size = new System.Drawing.Size(531, 20);
            this.txtOutputFolder.TabIndex = 3;
            this.txtOutputFolder.Text = "C:\\Documents and Settings\\Adrian\\Desktop\\TypingPracticeTexts\\Code\\New";
            // 
            // btnBrowseInputFile
            // 
            this.btnBrowseInputFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowseInputFile.Location = new System.Drawing.Point(628, 10);
            this.btnBrowseInputFile.Name = "btnBrowseInputFile";
            this.btnBrowseInputFile.Size = new System.Drawing.Size(100, 23);
            this.btnBrowseInputFile.TabIndex = 4;
            this.btnBrowseInputFile.Text = "Browse...";
            this.btnBrowseInputFile.UseVisualStyleBackColor = true;
            this.btnBrowseInputFile.Click += new System.EventHandler(this.btnBrowseInputFile_Click);
            // 
            // btnOutputFolder
            // 
            this.btnOutputFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOutputFolder.Location = new System.Drawing.Point(628, 34);
            this.btnOutputFolder.Name = "btnOutputFolder";
            this.btnOutputFolder.Size = new System.Drawing.Size(100, 23);
            this.btnOutputFolder.TabIndex = 5;
            this.btnOutputFolder.Text = "Browse...";
            this.btnOutputFolder.UseVisualStyleBackColor = true;
            this.btnOutputFolder.Click += new System.EventHandler(this.btnOutputFolder_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 175);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(72, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Lines per File:";
            // 
            // txtLinesPerFile
            // 
            this.txtLinesPerFile.Location = new System.Drawing.Point(111, 172);
            this.txtLinesPerFile.Name = "txtLinesPerFile";
            this.txtLinesPerFile.Size = new System.Drawing.Size(56, 20);
            this.txtLinesPerFile.TabIndex = 9;
            this.txtLinesPerFile.Text = "50";
            this.txtLinesPerFile.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // cbRemoveLineComments
            // 
            this.cbRemoveLineComments.AutoSize = true;
            this.cbRemoveLineComments.Checked = true;
            this.cbRemoveLineComments.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbRemoveLineComments.Location = new System.Drawing.Point(13, 72);
            this.cbRemoveLineComments.Name = "cbRemoveLineComments";
            this.cbRemoveLineComments.Size = new System.Drawing.Size(198, 17);
            this.cbRemoveLineComments.TabIndex = 10;
            this.cbRemoveLineComments.Text = "Remove line comments starting with:";
            this.cbRemoveLineComments.UseVisualStyleBackColor = true;
            this.cbRemoveLineComments.CheckedChanged += new System.EventHandler(this.cbRemoveLineComments_CheckedChanged);
            // 
            // btnGenerate
            // 
            this.btnGenerate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGenerate.Location = new System.Drawing.Point(628, 170);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(100, 23);
            this.btnGenerate.TabIndex = 11;
            this.btnGenerate.Text = "Generate";
            this.btnGenerate.UseVisualStyleBackColor = true;
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // comLineCommentStartChars
            // 
            this.comLineCommentStartChars.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comLineCommentStartChars.FormattingEnabled = true;
            this.comLineCommentStartChars.Items.AddRange(new object[] {
            "//",
            "--",
            "#",
            "#(?!\\S)"});
            this.comLineCommentStartChars.Location = new System.Drawing.Point(227, 69);
            this.comLineCommentStartChars.Name = "comLineCommentStartChars";
            this.comLineCommentStartChars.Size = new System.Drawing.Size(100, 23);
            this.comLineCommentStartChars.TabIndex = 12;
            this.comLineCommentStartChars.Text = "//";
            // 
            // cbRemoveLineCommentsAtEndOfLine
            // 
            this.cbRemoveLineCommentsAtEndOfLine.AutoSize = true;
            this.cbRemoveLineCommentsAtEndOfLine.Checked = true;
            this.cbRemoveLineCommentsAtEndOfLine.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbRemoveLineCommentsAtEndOfLine.Location = new System.Drawing.Point(425, 72);
            this.cbRemoveLineCommentsAtEndOfLine.Name = "cbRemoveLineCommentsAtEndOfLine";
            this.cbRemoveLineCommentsAtEndOfLine.Size = new System.Drawing.Size(200, 17);
            this.cbRemoveLineCommentsAtEndOfLine.TabIndex = 13;
            this.cbRemoveLineCommentsAtEndOfLine.Text = "Remove line comments at end of line";
            this.cbRemoveLineCommentsAtEndOfLine.UseVisualStyleBackColor = true;
            // 
            // comBlockCommentStartChars
            // 
            this.comBlockCommentStartChars.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comBlockCommentStartChars.FormattingEnabled = true;
            this.comBlockCommentStartChars.Items.AddRange(new object[] {
            "/*",
            "\\{-(?!#)",
            "\"\"\"",
            "=begin"});
            this.comBlockCommentStartChars.Location = new System.Drawing.Point(227, 95);
            this.comBlockCommentStartChars.Name = "comBlockCommentStartChars";
            this.comBlockCommentStartChars.Size = new System.Drawing.Size(100, 23);
            this.comBlockCommentStartChars.TabIndex = 15;
            this.comBlockCommentStartChars.Text = "/\\*";
            // 
            // cbRemoveBlockComments
            // 
            this.cbRemoveBlockComments.AutoSize = true;
            this.cbRemoveBlockComments.Checked = true;
            this.cbRemoveBlockComments.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbRemoveBlockComments.Location = new System.Drawing.Point(13, 98);
            this.cbRemoveBlockComments.Name = "cbRemoveBlockComments";
            this.cbRemoveBlockComments.Size = new System.Drawing.Size(208, 17);
            this.cbRemoveBlockComments.TabIndex = 14;
            this.cbRemoveBlockComments.Text = "Remove block comments starting with:";
            this.cbRemoveBlockComments.UseVisualStyleBackColor = true;
            this.cbRemoveBlockComments.CheckedChanged += new System.EventHandler(this.cbRemoveBlockComments_CheckedChanged);
            // 
            // comBlockCommentEndChars
            // 
            this.comBlockCommentEndChars.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comBlockCommentEndChars.FormattingEnabled = true;
            this.comBlockCommentEndChars.Items.AddRange(new object[] {
            "*/",
            "(?<!#)-}",
            "\"\"\"",
            "=end"});
            this.comBlockCommentEndChars.Location = new System.Drawing.Point(425, 95);
            this.comBlockCommentEndChars.Name = "comBlockCommentEndChars";
            this.comBlockCommentEndChars.Size = new System.Drawing.Size(100, 23);
            this.comBlockCommentEndChars.TabIndex = 16;
            this.comBlockCommentEndChars.Text = "\\*/";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(334, 99);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(85, 13);
            this.label4.TabIndex = 17;
            this.label4.Text = "and ending with:";
            // 
            // cbRemoveLiterateComments
            // 
            this.cbRemoveLiterateComments.AutoSize = true;
            this.cbRemoveLiterateComments.Checked = true;
            this.cbRemoveLiterateComments.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbRemoveLiterateComments.Enabled = false;
            this.cbRemoveLiterateComments.Location = new System.Drawing.Point(13, 124);
            this.cbRemoveLiterateComments.Name = "cbRemoveLiterateComments";
            this.cbRemoveLiterateComments.Size = new System.Drawing.Size(151, 17);
            this.cbRemoveLiterateComments.TabIndex = 18;
            this.cbRemoveLiterateComments.Text = "Remove literate comments";
            this.cbRemoveLiterateComments.UseVisualStyleBackColor = true;
            // 
            // comStringDelimiter
            // 
            this.comStringDelimiter.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comStringDelimiter.FormattingEnabled = true;
            this.comStringDelimiter.Items.AddRange(new object[] {
            "\"",
            "\'"});
            this.comStringDelimiter.Location = new System.Drawing.Point(111, 146);
            this.comStringDelimiter.Name = "comStringDelimiter";
            this.comStringDelimiter.Size = new System.Drawing.Size(100, 23);
            this.comStringDelimiter.TabIndex = 20;
            this.comStringDelimiter.Text = "\"";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(10, 150);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(78, 13);
            this.label5.TabIndex = 21;
            this.label5.Text = "String delimiter:";
            // 
            // SourceCodeTextCreator
            // 
            this.AcceptButton = this.btnGenerate;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(742, 204);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.comStringDelimiter);
            this.Controls.Add(this.cbRemoveLiterateComments);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.comBlockCommentEndChars);
            this.Controls.Add(this.comBlockCommentStartChars);
            this.Controls.Add(this.cbRemoveBlockComments);
            this.Controls.Add(this.cbRemoveLineCommentsAtEndOfLine);
            this.Controls.Add(this.comLineCommentStartChars);
            this.Controls.Add(this.btnGenerate);
            this.Controls.Add(this.cbRemoveLineComments);
            this.Controls.Add(this.txtLinesPerFile);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnOutputFolder);
            this.Controls.Add(this.btnBrowseInputFile);
            this.Controls.Add(this.txtOutputFolder);
            this.Controls.Add(this.txtInputFile);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.MaximumSize = new System.Drawing.Size(4000, 238);
            this.MinimumSize = new System.Drawing.Size(650, 238);
            this.Name = "SourceCodeTextCreator";
            this.Text = "Source Code Text Creator";
            this.Load += new System.EventHandler(this.SourceCodeTextCreator_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtInputFile;
        private System.Windows.Forms.TextBox txtOutputFolder;
        private System.Windows.Forms.Button btnBrowseInputFile;
        private System.Windows.Forms.Button btnOutputFolder;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtLinesPerFile;
        private System.Windows.Forms.CheckBox cbRemoveLineComments;
        private System.Windows.Forms.Button btnGenerate;
        private System.Windows.Forms.ComboBox comLineCommentStartChars;
        private System.Windows.Forms.CheckBox cbRemoveLineCommentsAtEndOfLine;
        private System.Windows.Forms.ComboBox comBlockCommentStartChars;
        private System.Windows.Forms.CheckBox cbRemoveBlockComments;
        private System.Windows.Forms.ComboBox comBlockCommentEndChars;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox cbRemoveLiterateComments;
        private System.Windows.Forms.ComboBox comStringDelimiter;
        private System.Windows.Forms.Label label5;
    }
}


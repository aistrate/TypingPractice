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
            this.picTyping = new System.Windows.Forms.PictureBox();
            this.btnStart = new System.Windows.Forms.Button();
            this.lblTime = new System.Windows.Forms.Label();
            this.tmrTimer = new System.Windows.Forms.Timer(this.components);
            this.lblWPM = new System.Windows.Forms.Label();
            this.tblLine = new System.Windows.Forms.TableLayoutPanel();
            this.lblErrorCount = new System.Windows.Forms.Label();
            this.lblAccuracy = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.picTyping)).BeginInit();
            this.SuspendLayout();
            // 
            // btnImport
            // 
            this.btnImport.Location = new System.Drawing.Point(0, 1);
            this.btnImport.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(100, 25);
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
            // picTyping
            // 
            this.picTyping.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.picTyping.BackColor = System.Drawing.Color.White;
            this.picTyping.Location = new System.Drawing.Point(0, 29);
            this.picTyping.Name = "picTyping";
            this.picTyping.Size = new System.Drawing.Size(461, 737);
            this.picTyping.TabIndex = 2;
            this.picTyping.TabStop = false;
            this.picTyping.Resize += new System.EventHandler(this.picTyping_Resize);
            this.picTyping.Paint += new System.Windows.Forms.PaintEventHandler(this.picTyping_Paint);
            // 
            // btnStart
            // 
            this.btnStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStart.Enabled = false;
            this.btnStart.Location = new System.Drawing.Point(361, 1);
            this.btnStart.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(100, 25);
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
            this.lblTime.Size = new System.Drawing.Size(60, 14);
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
            // tblLine
            // 
            this.tblLine.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tblLine.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Outset;
            this.tblLine.ColumnCount = 1;
            this.tblLine.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblLine.Location = new System.Drawing.Point(0, 26);
            this.tblLine.Name = "tblLine";
            this.tblLine.RowCount = 1;
            this.tblLine.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblLine.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 1F));
            this.tblLine.Size = new System.Drawing.Size(463, 2);
            this.tblLine.TabIndex = 9;
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
            this.lblAccuracy.Size = new System.Drawing.Size(60, 14);
            this.lblAccuracy.TabIndex = 11;
            this.lblAccuracy.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // TypistForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(462, 766);
            this.Controls.Add(this.lblAccuracy);
            this.Controls.Add(this.lblErrorCount);
            this.Controls.Add(this.tblLine);
            this.Controls.Add(this.lblWPM);
            this.Controls.Add(this.lblTime);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.picTyping);
            this.Controls.Add(this.btnImport);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.MinimumSize = new System.Drawing.Size(470, 150);
            this.Name = "TypistForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Typist";
            this.Deactivate += new System.EventHandler(this.TypistForm_Deactivate);
            this.Load += new System.EventHandler(this.TypistForm_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.TypistForm_Paint);
            this.Activated += new System.EventHandler(this.TypistForm_Activated);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TypistForm_KeyPress);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TypistForm_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.picTyping)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnImport;
        private System.Windows.Forms.OpenFileDialog dlgImport;
        private System.Windows.Forms.PictureBox picTyping;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Label lblTime;
        private System.Windows.Forms.Timer tmrTimer;
        private System.Windows.Forms.Label lblWPM;
        private System.Windows.Forms.TableLayoutPanel tblLine;
        private System.Windows.Forms.Label lblErrorCount;
        private System.Windows.Forms.Label lblAccuracy;
    }
}


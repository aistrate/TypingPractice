﻿namespace Typist
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
            this.btnImport = new System.Windows.Forms.Button();
            this.ofdImport = new System.Windows.Forms.OpenFileDialog();
            this.pbTyping = new System.Windows.Forms.PictureBox();
            this.btnStart = new System.Windows.Forms.Button();
            this.lblTime = new System.Windows.Forms.Label();
            this.tmrTimer = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.pbTyping)).BeginInit();
            this.SuspendLayout();
            // 
            // btnImport
            // 
            this.btnImport.Location = new System.Drawing.Point(2, 7);
            this.btnImport.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(100, 25);
            this.btnImport.TabIndex = 0;
            this.btnImport.Text = "Import...";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // ofdImport
            // 
            this.ofdImport.DefaultExt = "txt";
            this.ofdImport.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            // 
            // pbTyping
            // 
            this.pbTyping.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pbTyping.BackColor = System.Drawing.Color.White;
            this.pbTyping.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pbTyping.Location = new System.Drawing.Point(2, 35);
            this.pbTyping.Name = "pbTyping";
            this.pbTyping.Size = new System.Drawing.Size(623, 524);
            this.pbTyping.TabIndex = 1;
            this.pbTyping.TabStop = false;
            this.pbTyping.Resize += new System.EventHandler(this.pbTyping_Resize);
            this.pbTyping.Paint += new System.Windows.Forms.PaintEventHandler(this.pbTyping_Paint);
            // 
            // btnStart
            // 
            this.btnStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStart.Enabled = false;
            this.btnStart.Location = new System.Drawing.Point(525, 7);
            this.btnStart.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(100, 25);
            this.btnStart.TabIndex = 2;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // lblTime
            // 
            this.lblTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTime.Location = new System.Drawing.Point(459, 12);
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size(59, 14);
            this.lblTime.TabIndex = 5;
            this.lblTime.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // tmrTimer
            // 
            this.tmrTimer.Tick += new System.EventHandler(this.tmrTimer_Tick);
            // 
            // TypistForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(628, 561);
            this.Controls.Add(this.lblTime);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.pbTyping);
            this.Controls.Add(this.btnImport);
            this.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.MinimumSize = new System.Drawing.Size(350, 500);
            this.Name = "TypistForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Typist";
            this.Load += new System.EventHandler(this.TypistForm_Load);
            this.Activated += new System.EventHandler(this.TypistForm_Activated);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TypistForm_KeyPress);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TypistForm_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.pbTyping)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnImport;
        private System.Windows.Forms.OpenFileDialog ofdImport;
        private System.Windows.Forms.PictureBox pbTyping;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Label lblTime;
        private System.Windows.Forms.Timer tmrTimer;
    }
}


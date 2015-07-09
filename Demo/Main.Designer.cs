namespace Demo
{
    partial class Main
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
            this.textBoxLog = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.playButton = new System.Windows.Forms.Button();
            this.startButton = new System.Windows.Forms.Button();
            this.textBoxRec = new System.Windows.Forms.TextBox();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBoxLog
            // 
            this.textBoxLog.BackColor = System.Drawing.Color.White;
            this.textBoxLog.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxLog.Dock = System.Windows.Forms.DockStyle.Right;
            this.textBoxLog.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxLog.Location = new System.Drawing.Point(491, 0);
            this.textBoxLog.Multiline = true;
            this.textBoxLog.Name = "textBoxLog";
            this.textBoxLog.ReadOnly = true;
            this.textBoxLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxLog.Size = new System.Drawing.Size(111, 344);
            this.textBoxLog.TabIndex = 9;
            this.textBoxLog.WordWrap = false;
            // 
            // panel1
            // 
            this.panel1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.panel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panel1.Controls.Add(this.playButton);
            this.panel1.Controls.Add(this.startButton);
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(300, 100);
            this.panel1.TabIndex = 10;
            // 
            // playButton
            // 
            this.playButton.Location = new System.Drawing.Point(113, 13);
            this.playButton.Name = "playButton";
            this.playButton.Size = new System.Drawing.Size(75, 23);
            this.playButton.TabIndex = 1;
            this.playButton.Text = "Play";
            this.playButton.UseVisualStyleBackColor = true;
            this.playButton.Click += new System.EventHandler(this.launchPlaying);
            // 
            // startButton
            // 
            this.startButton.Location = new System.Drawing.Point(13, 13);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(75, 23);
            this.startButton.TabIndex = 0;
            this.startButton.Text = "Start";
            this.startButton.UseMnemonic = false;
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.launchRecording);
            // 
            // textBoxRec
            // 
            this.textBoxRec.BackColor = System.Drawing.Color.White;
            this.textBoxRec.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxRec.Dock = System.Windows.Forms.DockStyle.Right;
            this.textBoxRec.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxRec.Location = new System.Drawing.Point(380, 0);
            this.textBoxRec.Multiline = true;
            this.textBoxRec.Name = "textBoxRec";
            this.textBoxRec.ReadOnly = true;
            this.textBoxRec.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxRec.Size = new System.Drawing.Size(111, 344);
            this.textBoxRec.TabIndex = 11;
            this.textBoxRec.WordWrap = false;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(602, 344);
            this.Controls.Add(this.textBoxRec);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.textBoxLog);
            this.Name = "Main";
            this.Text = "Mouse and Keyboard Hooks Demo";
            this.Load += new System.EventHandler(this.Main_Load);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxLog;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.Button playButton;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox textBoxRec;

    }
}


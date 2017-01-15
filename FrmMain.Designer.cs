using System.Windows.Forms;

namespace SharpNeatLander
{
    partial class FrmMain
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
            this.picBox = new System.Windows.Forms.PictureBox();
            this.btnLearn = new System.Windows.Forms.Button();
            this.btnRunBest = new System.Windows.Forms.Button();
            this.btnReset = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnStopEA = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnStopRunning = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.picBox)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // picBox
            // 
            this.picBox.BackColor = System.Drawing.Color.Black;
            this.picBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.picBox.Location = new System.Drawing.Point(0, 0);
            this.picBox.Name = "picBox";
            this.picBox.Size = new System.Drawing.Size(849, 551);
            this.picBox.TabIndex = 0;
            this.picBox.TabStop = false;
            // 
            // btnLearn
            // 
            this.btnLearn.Location = new System.Drawing.Point(9, 6);
            this.btnLearn.Name = "btnLearn";
            this.btnLearn.Size = new System.Drawing.Size(75, 23);
            this.btnLearn.TabIndex = 1;
            this.btnLearn.Text = "Start EA";
            this.btnLearn.UseVisualStyleBackColor = true;
            this.btnLearn.Click += new System.EventHandler(this.btnLearn_Click);
            // 
            // btnRunBest
            // 
            this.btnRunBest.Location = new System.Drawing.Point(9, 77);
            this.btnRunBest.Name = "btnRunBest";
            this.btnRunBest.Size = new System.Drawing.Size(75, 23);
            this.btnRunBest.TabIndex = 1;
            this.btnRunBest.Text = "Run Best";
            this.btnRunBest.UseVisualStyleBackColor = true;
            this.btnRunBest.Click += new System.EventHandler(this.btnRunBest_Click);
            // 
            // btnReset
            // 
            this.btnReset.Location = new System.Drawing.Point(9, 166);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(75, 23);
            this.btnReset.TabIndex = 1;
            this.btnReset.Text = "Reset";
            this.btnReset.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnStopEA);
            this.panel1.Controls.Add(this.btnLearn);
            this.panel1.Controls.Add(this.btnExit);
            this.panel1.Controls.Add(this.btnReset);
            this.panel1.Controls.Add(this.btnStopRunning);
            this.panel1.Controls.Add(this.btnRunBest);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(757, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(92, 551);
            this.panel1.TabIndex = 2;
            // 
            // btnStopEA
            // 
            this.btnStopEA.Location = new System.Drawing.Point(10, 35);
            this.btnStopEA.Name = "btnStopEA";
            this.btnStopEA.Size = new System.Drawing.Size(75, 23);
            this.btnStopEA.TabIndex = 1;
            this.btnStopEA.Text = "Stop EA";
            this.btnStopEA.UseVisualStyleBackColor = true;
            this.btnStopEA.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnExit
            // 
            this.btnExit.Location = new System.Drawing.Point(10, 355);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(75, 23);
            this.btnExit.TabIndex = 1;
            this.btnExit.Text = "Exit";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnStopRunning
            // 
            this.btnStopRunning.Location = new System.Drawing.Point(9, 106);
            this.btnStopRunning.Name = "btnStopRunning";
            this.btnStopRunning.Size = new System.Drawing.Size(75, 23);
            this.btnStopRunning.TabIndex = 1;
            this.btnStopRunning.Text = "Stop";
            this.btnStopRunning.UseVisualStyleBackColor = true;
            this.btnStopRunning.Click += new System.EventHandler(this.btnStopRunning_Click);
            // 
            // FrmMain
            // 
            this.ClientSize = new System.Drawing.Size(849, 551);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.picBox);
            this.Name = "FrmMain";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmMain_FormClosing);
            this.Load += new System.EventHandler(this.FrmMain_Load);
            ((System.ComponentModel.ISupportInitialize)(this.picBox)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private PictureBox picBox;
        private Button btnLearn;
        private Button btnRunBest;
        private Button btnReset;
        private Button btnExit;
        private Panel panel1;
        #endregion
    }
}


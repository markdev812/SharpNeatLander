﻿using System;
using System.Collections.Concurrent;
using System.Drawing;
using System.Windows.Forms;

namespace SharpNeatLander
{

    public partial class FrmMain : Form
    {
        public double ViewScale => 0.6;
        public int ViewHeight => picBox.Height;
        public int ViewWidth => picBox.Width;


        private Button btnStopEA;
        private Button btnStopRunning;


        public static FrmMain Instance = null;

        LanderWorld _world = new LanderWorld();

        public FrmMain()
        {
            InitializeComponent();
        }

        private void FrmMain_Load(object sender, System.EventArgs e)
        {
            Instance = this;
            picBox.Paint += new PaintEventHandler(this.picBox1_Paint);
        }

        private void picBox1_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            foreach (var obj in _world.RenderList)
            {
                obj.Render(g);
            }
        }
        public Point WorldToView(Vector2 pos)
        {

            return new Point((int)Math.Round(pos.X * ViewScale), ViewHeight - (int)Math.Round(pos.Y * ViewScale));

        }

        public void UpdateView()
        {
            picBox.Invalidate();
        }
        private void btnLearn_Click(object sender, System.EventArgs e)
        {
            _world.StartLearning();
        }






        private void btnStop_Click(object sender, EventArgs e)
        {
            _world.StopLearning();
        }


        private void btnRunBest_Click(object sender, EventArgs e)
        {
            _world.StartRunning();
        }



        private void btnStopRunning_Click(object sender, EventArgs e)
        {
            _world.StopRunning();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {

            Close();
        }

        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            _world.StopLearning();
            _world.StopRunning();
        }
    }
}

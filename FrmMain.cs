using SharpNeat.Core;
using SharpNeat.EvolutionAlgorithms;
using SharpNeat.Genomes.Neat;
using SharpNeat.Phenomes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace SharpNeatLander
{

    public partial class FrmMain : Form
    {


        private static SimpleExperiment _experiment;
        private Button btnStopEA;
        private Button btnStopRunning;
        static NeatEvolutionAlgorithm<NeatGenome> _ea;
        private bool _running;
        List<Lander> renderList = new List<Lander>();

        public FrmMain()
        {
            InitializeComponent();
        }

        private void FrmMain_Load(object sender, System.EventArgs e)
        {

            picBox.Paint += new PaintEventHandler(this.picBox1_Paint);
        }

        private void picBox1_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            foreach (var obj in renderList)
            {
                obj.Render(g);
            }
        }

        private void btnLearn_Click(object sender, System.EventArgs e)
        {
            // Experiment classes encapsulate much of the nuts and bolts of setting up a NEAT search.
            _experiment = new SimpleExperiment();

            //create the EA with simple defaults
            _ea = _experiment.CreateSimpleEA("lander", 5, 2, GetFitness);

            _ea.UpdateEvent += ea_UpdateEvent;

            ////start learning
            _ea.StartContinue();
        }


        public static double GetFitness(IBlackBox box)
        {
            Lander ship = new Lander();
            ship.Start();

            for (int i = 1; i <= 100; i++)
            {
                ship.Compute(box);
                //Ship.Thrust = 3.42;

                ship.Update(1);//0.25);
                //if (playMode)
                //    Console.WriteLine($"S:{i,-5}  X:{ship.Position.X,6:F1}  A:{ship.Position.Y,6:F1}  R:{ship.Rotation,6:F1}  Vx:{ship.Velocity.X,6:F1} Vy:{ship.Velocity.Y,6:F1} F:{ship.Fuel,6:F1}  T:{ship.Thrust,6:F1}");

                //did we hit the ground?
                if (ship.Position.Y <= 0)
                {

                    break;
                }


            }

            return ship.GetFitness();
        }



        static void ea_UpdateEvent(object sender, EventArgs e)
        {
            Debug.WriteLine($"gen={_ea.CurrentGeneration:N0} bestFitness={_ea.Statistics._maxFitness:N6}");
            // Save the best genome to file
            _experiment.SaveChamp(_ea.CurrentChampGenome);

        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            _ea.Stop();
            while (_ea.RunState == RunState.Running)
                Thread.Sleep(1000);
        }

        private Thread t;
        private void btnRunBest_Click(object sender, EventArgs e)
        {
            t = new Thread(RunBest);
            t.Start();
        }

        void RunBest()
        {
            _experiment = new SimpleExperiment();
            _ea = _experiment.CreateSimpleEA("lander", 5, 2, GetFitness);

            IBlackBox bestLander = _experiment.GetChamp();
            Lander ship = new Lander();
            ship.Start();

            _running = true;

            Stopwatch sw = new Stopwatch();
            sw.Start();
            //add obj to render list
            renderList.Add(ship);
            double prevTime = 0, currTime = 0;
            while (_running)
            {
                ship.Compute(bestLander);

                currTime = (double)sw.ElapsedMilliseconds / 1000.0;
                double deltaTime = currTime - prevTime;
                ship.Update(deltaTime);

                prevTime = currTime;

                Debug.WriteLine($"S:{deltaTime,-5:F2}  X:{ship.Position.X,6:F1}  A:{ship.Position.Y,6:F1}  R:{ship.Rotation,6:F1}  Vx:{ship.Velocity.X,6:F1} Vy:{ship.Velocity.Y,6:F1} F:{ship.Fuel,6:F1}  T:{ship.Thrust,6:F1}");

                Thread.Sleep(100);
            }
            renderList.Remove(ship);
        }

        private void btnStopRunning_Click(object sender, EventArgs e)
        {
            _running = false;
        }
    }
}

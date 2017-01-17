using SharpNeat.Core;
using SharpNeat.EvolutionAlgorithms;
using SharpNeat.Genomes.Neat;
using SharpNeat.Phenomes;
using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace SharpNeatLander
{

    public partial class FrmMain : Form
    {

        public const int NUM_INPUTS = 2;
        public const int NUM_OUTPUTS = 2;

        public static double FixedDeltaTime = 0.1;
        public static double ViewScale = 0.6;

        private static SimpleExperiment _experiment;
        private Button btnStopEA;
        private Button btnStopRunning;
        static NeatEvolutionAlgorithm<NeatGenome> _ea;
        private bool _running;
        private static readonly ConcurrentBag<Lander> _renderList = new ConcurrentBag<Lander>();
        public static FrmMain Instance = null;

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
            foreach (var obj in _renderList)
            {
                obj.Render(g);
            }
        }
        public Point WorldToView(Vector2 pos)
        {

            return new Point((int)Math.Round(pos.X * ViewScale), picBox.Height - (int)Math.Round(pos.Y * ViewScale));

        }
        private void btnLearn_Click(object sender, System.EventArgs e)
        {
            // Experiment classes encapsulate much of the nuts and bolts of setting up a NEAT search.
            _experiment = new SimpleExperiment();

            //create the EA with simple defaults
            _ea = _experiment.CreateSimpleEA("lander", NUM_INPUTS, NUM_OUTPUTS, Evaluate);

            _ea.UpdateEvent += ea_UpdateEvent;

            ////start learning
            _ea.StartContinue();
        }


        public static double Evaluate(IBlackBox box)
        {
            World w = new World();

            double fitness = 0;

            for (int i = 0; i < 10; i++)
            {
                double f = w.RunTrial(box);
                if (f > fitness)
                    fitness = f;

            }

            return fitness;
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
            _ea = _experiment.CreateSimpleEA("lander", NUM_INPUTS, NUM_OUTPUTS, Evaluate);

            IBlackBox bestLander = _experiment.GetChamp();
            Lander ship = new Lander();
            ship.Start();

            _running = true;

            Stopwatch sw = new Stopwatch();
            sw.Start();
            //add obj to render list
            _renderList.Add(ship);

            while (_running)
            {
                ship.Compute(bestLander);


                ship.Update(FixedDeltaTime);
                picBox.Invalidate();

                Debug.WriteLine($"S:{FixedDeltaTime,-5:F2}  X:{ship.Position.X,6:F1}  A:{ship.Position.Y,6:F1}  R:{ship.Rotation,6:F1}  Vx:{ship.Velocity.X,6:F1} Vy:{ship.Velocity.Y,6:F1} F:{ship.Fuel,6:F1}  T:{ship.Thrust,6:F1}");

                if (ship.Crashed || ship.Landed)
                    _running = false;

                Thread.Sleep((int)(FixedDeltaTime * 1000.0));
            }
            Lander dummy;
            _renderList.TryTake(out dummy);
        }

        private void btnStopRunning_Click(object sender, EventArgs e)
        {
            _running = false;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            _running = false;
            Close();
        }

        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            _running = false;
        }
    }
}

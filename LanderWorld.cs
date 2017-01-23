using SharpNeat.Core;
using SharpNeat.EvolutionAlgorithms;
using SharpNeat.Genomes.Neat;
using SharpNeat.Phenomes;
using System;
using System.Diagnostics;
using System.Threading;

namespace SharpNeatLander
{
    public class LanderWorld : INeatWorld
    {
        public double FixedDeltaTime => 0.1;


        public const string NAME = "lander";
        public const int NUM_INPUTS = 4;
        public const int NUM_OUTPUTS = 2;


        public double Width => 1000.0;
        public double Height => 1000.0;

        private static SimpleExperiment _experiment;
        static NeatEvolutionAlgorithm<NeatGenome> _ea;

        private Thread _runBestThread;
        private bool _running;



        public void StartLearning()
        {
            // Experiment classes encapsulate much of the nuts and bolts of setting up a NEAT search.
            _experiment = new SimpleExperiment();

            //create the EA with simple defaults
            _ea = _experiment.CreateSimpleEA(NAME, NUM_INPUTS, NUM_OUTPUTS, Evaluate);

            _ea.UpdateEvent += ea_UpdateEvent;

            ////start learning
            _ea.StartContinue();
        }

        static void ea_UpdateEvent(object sender, EventArgs e)
        {
            Debug.WriteLine($"gen={_ea.CurrentGeneration:N0} bestFitness={_ea.Statistics._maxFitness:N6}");
            // Save the best genome to file
            _experiment.SaveChamp(_ea.CurrentChampGenome);

        }

        public void StopLearning()
        {
            _ea.Stop();
            while (_ea.RunState == RunState.Running)
                Thread.Sleep(1000);
        }

        public static double Evaluate(IBlackBox box)
        {
            LanderWorld w = new LanderWorld();

            double fitness = 0;

            //run the trial several times and keep the best fitness
            for (int i = 0; i < 10; i++)
            {
                double f = w.RunTrial(box);
                if (f > fitness)
                    fitness = f;

            }
            //Console.WriteLine($"Eval best: {fitness}");
            return fitness;
        }
        /// <summary>
        /// Instantiate one unit and run it through a few updates.
        /// </summary>
        /// <param name="box">The phenome to run against</param>
        /// <returns>The units fitness (0-1)</returns>
        public double RunTrial(IBlackBox box)
        {
            LanderUnit ship = new LanderUnit();
            ship.Start(this);

            //run simulation for a few frames
            int i = 0;
            for (i = 0; i < 200; i++)
            {

                ship.Compute(box);


                ship.Update(FixedDeltaTime); //0.25);

                //    Console.WriteLine($"S:{i,-5}  X:{ship.Position.X,6:F1}  A:{ship.Position.Y,6:F1}  R:{ship.Rotation,6:F1}  Vx:{ship.Velocity.X,6:F1} Vy:{ship.Velocity.Y,6:F1} F:{ship.Fuel,6:F1}  T:{ship.Thrust,6:F1}");
                
                if (ship.Landed || ship.Crashed)
                    break;


            }
            //Console.WriteLine($"Frames: {i}");
            return ship.GetFitness();
        }
        public void StartRunning()
        {
            _running = false;
            _runBestThread = new Thread(DoRunBest);
            _runBestThread.Start();
        }

        public void StopRunning()
        {
            _running = false;
        }
        void DoRunBest()
        {
            _experiment = new SimpleExperiment();
            _ea = _experiment.CreateSimpleEA(NAME, NUM_INPUTS, NUM_OUTPUTS, Evaluate);

            IBlackBox bestLander = _experiment.GetChamp();
            LanderUnit ship = new LanderUnit();
            ship.Start(this);

            _running = true;

            Stopwatch sw = new Stopwatch();
            sw.Start();
            //add obj to render list
            //
            FrmMain.RenderList.Add(ship);

            while (_running)
            {
                ship.Compute(bestLander);


                ship.Update(FixedDeltaTime);
                FrmMain.Instance.UpdateView();

                Debug.WriteLine($"S:{FixedDeltaTime,-5:F2}  X:{ship.Position.X,6:F1}  A:{ship.Position.Y,6:F1}  R:{ship.Rotation,6:F1}  Vx:{ship.Velocity.X,6:F1} Vy:{ship.Velocity.Y,6:F1} F:{ship.Fuel,6:F1}  T:{ship.Thrust,6:F1}");

                if (ship.Crashed || ship.Landed)
                    _running = false;

                Thread.Sleep((int)(FixedDeltaTime * 1000.0));
            }
            LanderUnit dummy;
            FrmMain.RenderList.TryTake(out dummy);
        }

    }

}

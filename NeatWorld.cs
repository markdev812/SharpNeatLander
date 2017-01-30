using SharpNeat.Core;
using SharpNeat.EvolutionAlgorithms;
using SharpNeat.Genomes.Neat;
using SharpNeat.Phenomes;
using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading;

namespace SharpNeatLander
{
    public class NeatWorld
    {
        public static string Name;
        public static int NumInputs;
        public static int NumOutputs;


        public double FixedDeltaTime => 0.1;
        public virtual double Width => 1000.0;
        public virtual double Height => 1000.0;

        public readonly ConcurrentBag<NeatUnit> RenderList = new ConcurrentBag<NeatUnit>();



        private static SimpleExperiment _experiment;
        public NeatEvolutionAlgorithm<NeatGenome> _ea;

        private Thread _runBestThread;
        private bool _running;

        public NeatWorld(string name, int numInputs, int numOutputs)
        {
            Name = name;
            NumInputs = numInputs;
            NumOutputs = numOutputs;

        }
        public static double Evaluate(IBlackBox box)
        {
            NeatWorld w = new NeatWorld(Name, NumInputs, NumOutputs);

            double fitness = 0;

            //run the trial several times and keep the best fitness
            for (int i = 0; i < 10; i++)
            {
                double f = w.RunTrial(box, 100);
                if (f > fitness)
                    fitness = f;

            }
            //Console.WriteLine($"Eval best: {fitness}");
            return fitness;
        }

        public void StartLearning()
        {
            // Experiment classes encapsulate much of the nuts and bolts of setting up a NEAT search.
            _experiment = new SimpleExperiment();

            //create the EA with simple defaults
            _ea = _experiment.CreateSimpleEA(Name, NumInputs, NumOutputs, Evaluate);

            _ea.UpdateEvent += ea_UpdateEvent;

            ////start learning
            _ea.StartContinue();
        }

        void ea_UpdateEvent(object sender, EventArgs e)
        {
            Debug.WriteLine($"gen={_ea.CurrentGeneration:N0} bestFitness={_ea.Statistics._maxFitness:N6}");
            // Save the best genome to file
            _experiment.SaveChamp(_ea.CurrentChampGenome);

        }

        public void StopLearning()
        {
            if (_ea != null)
            {
                _ea.Stop();

                while (_ea.RunState == RunState.Running)
                    Thread.Sleep(1000);
            }
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

        /// <summary>
        /// Instantiate one unit and run it through a few updates.
        /// </summary>
        /// <param name="box">The phenome to run against</param>
        /// <param name="maxFrames">Maximum number of update iterations</param>
        /// <returns>The units fitness (0-1)</returns>
        public double RunTrial(IBlackBox box, int maxFrames)
        {
            NeatUnit unit = NeatUnit.Create(Name);
            unit.Start(this);

            //run simulation for a few frames
            int i = 0;
            for (i = 0; i < maxFrames; i++)
            {

                unit.Compute(box);


                bool done = unit.Update(FixedDeltaTime); //0.25);

                //    Console.WriteLine($"S:{i,-5}  X:{ship.Position.X,6:F1}  A:{ship.Position.Y,6:F1}  R:{ship.Rotation,6:F1}  Vx:{ship.Velocity.X,6:F1} Vy:{ship.Velocity.Y,6:F1} F:{ship.Fuel,6:F1}  T:{ship.Thrust,6:F1}");

                if (done)
                    break;


            }
            //Console.WriteLine($"Frames: {i}");
            return unit.GetFitness();
        }


        public void DoRunBest()
        {
            _experiment = new SimpleExperiment();
            _ea = _experiment.CreateSimpleEA(Name, NumInputs, NumOutputs, Evaluate);

            IBlackBox champ = _experiment.GetChamp();

            NeatUnit unit = NeatUnit.Create(Name);
            unit.Start(this);

            _running = true;

            Stopwatch sw = new Stopwatch();
            sw.Start();
            //add obj to render list
            //
            RenderList.Add(unit);

            while (_running)
            {
                unit.Compute(champ);


                bool done = unit.Update(FixedDeltaTime);
                unit.PrintStats();

                if (done)
                    _running = false;

                FrmMain.Instance.UpdateView();

                Thread.Sleep((int)(FixedDeltaTime * 1000.0));
            }
            NeatUnit dummy;
            RenderList.TryTake(out dummy);
        }
    }

}

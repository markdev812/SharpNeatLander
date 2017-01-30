using SharpNeat.Core;
using SharpNeat.EvolutionAlgorithms;
using SharpNeat.Genomes.Neat;
using SharpNeat.Phenomes;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
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



        protected static SimpleExperiment _experiment;
        protected NeatEvolutionAlgorithm<NeatGenome> _ea;

        private static Dictionary<IBlackBox, NeatWorld> _boxWorldMap = new Dictionary<IBlackBox, NeatWorld>();

        private Thread _runBestThread;
        private bool _running;

        public NeatWorld(string name, int numInputs, int numOutputs)
        {
            Name = name;
            NumInputs = numInputs;
            NumOutputs = numOutputs;

        }

        public static NeatWorld Create(string name)
        {
            if (name == "lander") return new SimWorld("lander", 4, 2);
            //if (name == "tictactoe") return new TurnBasedWorld("tictactoe", 9, 1);

            return null;

        }

        public static double DoEvaluate(IBlackBox box)
        {
            NeatWorld w = null;
            if (_boxWorldMap.ContainsKey(box))
            {
                w = _boxWorldMap[box];
            }
            else
            {
                w = NeatWorld.Create(Name);
            }

            if (w != null)
                return w.Evaluate(box);

            return 0;
        }

        public void StartLearning()
        {
            // Experiment classes encapsulate much of the nuts and bolts of setting up a NEAT search.
            _experiment = new SimpleExperiment();

            //create the EA with simple defaults
            _ea = _experiment.CreateSimpleEA(Name, NumInputs, NumOutputs, DoEvaluate);

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
            _runBestThread = new Thread(() => RunBest());
            _runBestThread.Start();
        }

        public void StopRunning()
        {
            _running = false;
        }

        public void RunBest()
        {
            _experiment = new SimpleExperiment();
            _experiment.CreateSimpleEA(Name, NumInputs, NumOutputs, null);

            IBlackBox box = _experiment.GetChamp();

            Run(box);

        }

        public virtual double Evaluate(IBlackBox box)
        {
            throw new NotImplementedException();

        }
        ///// <summary>
        ///// Instantiate one unit and run it through a few updates.
        ///// </summary>
        ///// <param name="box">The phenome to run against</param>
        ///// <param name="maxFrames">Maximum number of update iterations</param>
        ///// <returns>The units fitness (0-1)</returns>
        //public virtual double RunTrial(IBlackBox box, int maxFrames)
        //{
        //    throw new NotImplementedException();
        //}

        public virtual void Run(IBlackBox box)
        {

            throw new NotImplementedException();
        }



    }

}

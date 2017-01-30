using SharpNeat.Phenomes;
using System.Diagnostics;
using System.Threading;

namespace SharpNeatLander
{
    public class SimWorld : NeatWorld
    {


        private Thread _runBestThread;
        private bool _running;

        public SimWorld(string name, int numInputs, int numOutputs) :
            base(name, numInputs, numOutputs)
        {

        }
        public override double Evaluate(IBlackBox box)
        {

            double fitness = 0;

            //run the trial several times and keep the best fitness
            for (int i = 0; i < 10; i++)
            {
                double f = RunTrial(box, 100);
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


        public override void Run(IBlackBox box)
        {


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
                unit.Compute(box);


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

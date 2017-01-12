using SharpNeat.Core;
using SharpNeat.EvolutionAlgorithms;
using SharpNeat.Genomes.Neat;
using SharpNeat.Phenomes;
using System;
using System.Threading;

namespace SharpNeatLander
{
    class Program
    {


        private static SimpleExperiment _experiment;

        static NeatEvolutionAlgorithm<NeatGenome> _ea;

        static void Main()//string[] args)
        {


            // Experiment classes encapsulate much of the nuts and bolts of setting up a NEAT search.
            _experiment = new SimpleExperiment();

            //create the EA with simple defaults
            _ea = _experiment.CreateSimpleEA("lander", 5, 2, GetFitness);

            _ea.UpdateEvent += ea_UpdateEvent;

            ////start learning
            Console.WriteLine("Learning... (press any key to stop)");
            _ea.StartContinue();


            Console.ReadKey();
            _ea.Stop();
            while (_ea.RunState == RunState.Running)
                Thread.Sleep(1000);

            //test the NN

            IBlackBox bestLander = _experiment.GetChamp();
            Lander.RunSimulation(bestLander, true);

            Console.WriteLine("Press any key");
            Console.ReadKey();
        }

        /// <summary>
        /// This gets called in parallel for each genome to be evaluated
        /// </summary>
        /// <param name="box"></param>
        /// <returns></returns>
        public static double GetFitness(IBlackBox box)
        {
            return Lander.RunSimulation(box);

        }






        static void ea_UpdateEvent(object sender, EventArgs e)
        {
            Console.WriteLine($"gen={_ea.CurrentGeneration:N0} bestFitness={_ea.Statistics._maxFitness:N6}");
            // Save the best genome to file
            _experiment.SaveChamp(_ea.CurrentChampGenome);

        }
    }

}

using log4net.Config;
using SharpNeat.Core;
using SharpNeat.EvolutionAlgorithms;
using SharpNeat.Genomes.Neat;
using System;
using System.Threading;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using SharpNeat.Phenomes;

namespace SharpNeatLander
{
    class Program
    {
        public const double StartingAltitude = 2400;
        public const double StartingFuel = 500;
        public const double Gravity = -3.711;
        public const double TerminalVel = -200;


        static IGenomeFactory<NeatGenome> _genomeFactory;
        static List<NeatGenome> _genomeList;
        static NeatEvolutionAlgorithm<NeatGenome> _ea;

        static void Main()//string[] args)
        {

            

            //double fitness = RunSimulation();//(box, true);
            //Console.WriteLine($"{fitness}");
            //Console.ReadKey();
            //return;


            // Initialise log4net (log to console).
            XmlConfigurator.Configure(new FileInfo("log4net.properties"));

            // Experiment classes encapsulate much of the nuts and bolts of setting up a NEAT search.
            LanderExperiment experiment = new LanderExperiment();

            // Load XML config file for the experiment
            XmlDocument xmlConfig = new XmlDocument();
            xmlConfig.Load("lander.config.xml");

            //init the experiment using config file
            experiment.Initialize("Lander", xmlConfig.DocumentElement);

            // Create a genome factory with our neat genome parameters object and the appropriate number of input and output neuron genes.
            _genomeFactory = experiment.CreateGenomeFactory();

            // Create an initial population of randomly generated genomes.
            int populationSize = 150;
            _genomeList = _genomeFactory.CreateGenomeList(populationSize, 0);

            Console.WriteLine($"Created [{populationSize}] random genomes.");

            //create evolutionary algorithm
            _ea = experiment.CreateEvolutionAlgorithm(_genomeFactory, _genomeList);
            _ea.UpdateEvent += ea_UpdateEvent;

            //start learning
            Console.WriteLine("Starting...");
            _ea.StartContinue();

            //to save current genome
            //XmlWriterSettings xwSettings = new XmlWriterSettings();
            //xwSettings.Indent = true;
            //using (XmlWriter xw = XmlWriter.Create(cmdArgs[1], xwSettings))
            //{
            //    experiment.SavePopulation(xw, _genomeList);
            //}

            //to save best genome
            //XmlWriterSettings xwSettings = new XmlWriterSettings();
            //xwSettings.Indent = true;
            //using (XmlWriter xw = XmlWriter.Create(FILENAME, xwSettings))
            //{
            //    experiment.SavePopulation(xw, new NeatGenome[] { _ea.CurrentChampGenome });
            //}



            //to pause/stop execution
            //_ea.RequestPauseAndWait();


            Console.ReadKey();
            _ea.Stop();
            while( _ea.RunState == RunState.Running)
                Thread.Sleep(1000);

            //test the NN
            var genomeDecoder = experiment.CreateGenomeDecoder();

            NeatGenome genome = null;
            using (XmlReader xr = XmlReader.Create("lander_champ.xml"))
            {
                genome = NeatGenomeXmlIO.ReadCompleteGenomeList(xr, false, (NeatGenomeFactory)_genomeFactory)[0];
            }
            // Decode the best genome into a phenome (neural network).
            var box = genomeDecoder.Decode(genome);// CurrentChampGenome);

            RunSimulation(box, true);

            Console.WriteLine("Press any key");
            Console.ReadKey();
        }

        public static double RunSimulation(IBlackBox box, bool report=false)
        {
            ISignalArray inputArr = box.InputSignalArray;
            ISignalArray outputArr = box.OutputSignalArray;

            //run the simulation
            Lander Ship = new Lander();
            Ship.Init(StartingAltitude, StartingFuel, Gravity);

            for (int i = 1; i <= 100; i++)
            {
                //set inputs
                inputArr[0] = Ship.Altitude / StartingAltitude;
                inputArr[1] = Ship.Velocity / TerminalVel;
                inputArr[2] = Ship.Fuel / StartingFuel;
                //inputArr[3] = (double)i / 100.0;

                box.Activate();

                Ship.Thrust = Math.Round(outputArr[0] * 4); //Math.Floor(outputArr[0] * 4.0);
                //Ship.Thrust = 3.42;

                Ship.Update(1);
                if(report)
                  Console.WriteLine($"{i,-5}{Ship.Altitude,8:F1}{Ship.Velocity,8:F1}{Ship.Fuel,8:F1}{Ship.Thrust,8:F1}");

                //did we hit the ground?
                if (Ship.Altitude <= 0)
                    break;


            }
            return CalcFitness(Ship);
        }

        static double CalcFitness(Lander l)
        {
            //  fitness = fuel remaining
            //  height > 1  didn't land  fitness -= HeightAboveSurface
            //  height <= 1  landed
            //      V < -40 m/s  on touchdown  fitness = 0 
            //      V > -40  fitness += MaxVel - V   

            double nFuel = l.Fuel / StartingFuel;
            double nVelocity = l.Velocity / TerminalVel;
            double nAltitude = l.Altitude / StartingAltitude;

            double fitness = 0;
            //small reward for lower velocity
            fitness = (1 - nVelocity) * 5;
             //fitness += (1 - nAltitude)* 10;
            if (l.Altitude <= 2) //landed
            {
                if (l.Velocity > -40) //no crash
                {
 
                    //big reward for safe landing
                    fitness += 100;
                    //fuel savings bonus
                    fitness += nFuel * 10;

                }

            }

            return fitness > 0 ? fitness : 0;
        }




        static void ea_UpdateEvent(object sender, EventArgs e)
        {
            Console.WriteLine($"gen={_ea.CurrentGeneration:N0} bestFitness={_ea.Statistics._maxFitness:N6}");
            // Save the best genome to file
            var doc = NeatGenomeXmlIO.SaveComplete(new List<NeatGenome>() { _ea.CurrentChampGenome }, false);
            doc.Save("lander_champ.xml");
        }
    }

}

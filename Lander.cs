using SharpNeat.Phenomes;
using System;

namespace SharpNeatLander
{
    class Lander
    {
        public const double StartingAltitude = 2400;
        public const double StartingFuel = 200;
        public const double Gravity = -3.711;
        public const double TerminalVel = -200;
        public const double CrashSpeed = -40;

        public double Altitude { get; private set; }
        public double Velocity { get; private set; }
        public double Fuel { get; private set; }
        public double Thrust { get; set; }

        private double _gravity;


        public void Init(double startingHeight, double startingFuel, double gravity)
        {
            Altitude = startingHeight;
            Fuel = startingFuel;
            _gravity = gravity;
        }
        public void Update(double deltaTime)
        {
            if (Fuel <= 0)
                Thrust = 0;
            Fuel -= Thrust * deltaTime;
            Velocity += (Thrust + _gravity) * deltaTime;

            if (Velocity < TerminalVel)
                Velocity = TerminalVel;

            Altitude += Velocity * deltaTime;
            if (Altitude < 0)
                Altitude = 0;

        }
        public static double RunSimulation(IBlackBox box, bool playMode = false)
        {
            ISignalArray inputArr = box.InputSignalArray;
            ISignalArray outputArr = box.OutputSignalArray;

            //run the simulation
            Lander ship = new Lander();
            ship.Init(StartingAltitude, StartingFuel, Gravity);

            for (int i = 1; i <= 100; i++)
            {
                //set inputs
                inputArr[0] = ship.Altitude / StartingAltitude;
                inputArr[1] = ship.Velocity / TerminalVel;
                inputArr[2] = ship.Fuel / StartingFuel;
                //inputArr[3] = (double)i / 100.0;

                box.Activate();

                //if (outputArr[0] > 0.5)
                //    ship.Thrust = 10;
                ship.Thrust = Math.Round(outputArr[0] * 4); //Math.Floor(outputArr[0] * 4.0);
                //Ship.Thrust = 3.42;

                ship.Update(1);//0.25);
                if (playMode)
                    Console.WriteLine($"{i,-5}{ship.Altitude,8:F1}{ship.Velocity,8:F1}{ship.Fuel,8:F1}{ship.Thrust,8:F1}");

                //did we hit the ground?
                if (ship.Altitude <= 0)
                {

                    break;
                }


            }

            return ship.CalcFitness();
        }

        double NormalizedFitness(double x, double origRangeA, double origRangeB, double desiredRangeA, double desiredRangeB)
        {
            //A,B = original range
            //r1,r2 = desired range
            //y = 1 + (x-A)*(r2-r1)/(B-A)
            var fitness = 1 + (x - origRangeA) * (desiredRangeB - desiredRangeA) / (origRangeB - origRangeA);
            return fitness;
        }


        public double CalcFitness()
        {


            double f = NormalizedFitness(Fuel, 1, StartingFuel, 1, 10);
            double v = NormalizedFitness(Velocity, TerminalVel, CrashSpeed, 1, 10);
            //double a = NormalizedFitness(Altitude, 1, StartingAltitude, 10, 1);

            double fitness = f + v;
            if (Velocity > CrashSpeed)
            {
                f = NormalizedFitness(Fuel, 1, StartingFuel, 1, 100);
                v = NormalizedFitness(Velocity, CrashSpeed, -1, 1, 20);
                fitness += f + v;
                //fitness += 100;
            }
            //double nFuel = Fuel / StartingFuel;
            //double nVelocity = Velocity / TerminalVel;
            //double nAltitude = Altitude / StartingAltitude;

            //double fitness = 0;
            ////fitness = (1 - nAltitude);
            //fitness += (CrashSpeed / Velocity) * 10;
            ////fitness += nFuel * 5;
            //if (Altitude <= 2) //landed
            //{
            //    //small reward for lower velocity
            //    //fitness += (1 - nVelocity) * 5;
            //    if (Velocity > CrashSpeed) //no crash
            //    {

            //        //big reward for safe landing
            //        fitness += 100;
            //        //fuel savings bonus
            //        fitness += nFuel * 10;

            //    }

            //}
            //Console.WriteLine($"{Altitude,8:F1}{Velocity,8:F1}{Fuel,8:F1}{Thrust,8:F1}");
            return fitness > 0 ? fitness : 0;
        }
    }
}

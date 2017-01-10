using SharpNeat.Phenomes;
using System;

namespace SharpNeatLander
{
    class Lander
    {
        public const double StartingAltitude = 2400;
        public const double StartingFuel = 200;
        public readonly Vector2 Gravity = new Vector2(0, -3.711);

        public const double TerminalVel = -200;
        public const double CrashSpeed = -40;

        public Vector2 Position = new Vector2(0, 0);
        public double Rotation { get; set; }
        //public double Altitude { get; private set; }
        public Vector2 Velocity = new Vector2(0, 0);
        public double Fuel { get; private set; }
        public double Thrust { get; private set; }



        public void Start()
        {
            Position.Y = StartingAltitude;
            Fuel = StartingFuel;


            //CalcFitness(40, 0, 200, 20, 100);
        }
        public void Update(double deltaTime)
        {
            if (Fuel <= 0)
                Thrust = 0;

            Fuel -= Thrust * deltaTime;

            Vector2 thrustVec = Vector2.Up * Thrust;

            thrustVec.Rotate(Rotation);

            thrustVec += Gravity;

            Velocity += thrustVec * deltaTime;

            if (Velocity.Y < TerminalVel)
                Velocity.Y = TerminalVel;

            Position += Velocity * deltaTime;


            if (Position.Y < 0)
                Position.Y = 0;

        }
        public static double RunSimulation(IBlackBox box, bool playMode = false)
        {
            ISignalArray inputArr = box.InputSignalArray;
            ISignalArray outputArr = box.OutputSignalArray;

            //run the simulation
            Lander ship = new Lander();
            ship.Start();

            for (int i = 1; i <= 100; i++)
            {
                //set inputs
                inputArr[0] = ship.Position.Y / StartingAltitude;
                inputArr[1] = ship.Velocity.Y / TerminalVel;
                inputArr[2] = ship.Fuel / StartingFuel;
                //inputArr[3] = (double)i / 100.0;

                box.Activate();

                //if (outputArr[0] > 0.5)
                //    ship.Thrust = 10;
                ship.Thrust = Math.Round(outputArr[0] * 4); //Math.Floor(outputArr[0] * 4.0);
                //Ship.Thrust = 3.42;

                ship.Update(1);//0.25);
                if (playMode)
                    Console.WriteLine($"T:{i,-5}A:{ship.Position.Y,8:F1}R:{ship.Rotation,8:F1}V:{ship.Velocity.Y,8:F1}F:{ship.Fuel,8:F1}T:{ship.Thrust,8:F1}");

                //did we hit the ground?
                if (ship.Position.Y <= 0)
                {

                    break;
                }


            }

            return ship.GetFitness();
        }

        double StandardizedRange(double x, double origRangeA, double origRangeB, double desiredRangeA, double desiredRangeB)
        {
            //A,B = original range
            //r1,r2 = desired range
            //y = 1 + (x-A)*(r2-r1)/(B-A)
            var fitness = 1 + (x - origRangeA) * (desiredRangeB - desiredRangeA) / (origRangeB - origRangeA);
            return fitness;
        }
        /// <summary>
        /// Calculate a fitness by normalizing and scaling the value by a "weight", centered around a desired "best" value
        /// </summary>
        /// <param name="x">incoming value</param>
        /// <param name="min">value range mininum</param>
        /// <param name="max">value range maximum</param>
        /// <param name="best">best value for maximum fitness</param>
        /// <param name="weight">importance of this value compared to others</param>
        /// <returns></returns>
        public double NormalizeFitness(double x, double min, double max, double best, double weight)
        {
            if (x < min || x > max)
                return 0;

            var offset = best - x;
            var range = max - min;
            var v = 1 + offset * 1 / range;
            var fit = v * weight;
            return fit;
        }


        public double GetFitness()
        {


            //double v = NormalizeFitness(Velocity.Y, TerminalVel, 0, CrashSpeed + 1, 100);

            double f = NormalizeFitness(Fuel, 0, StartingFuel, StartingFuel, 10);
            double v = NormalizeFitness(Velocity.Y, TerminalVel, 0, CrashSpeed + 1, 10);
            //double a = NormalizedFitness(Altitude, 1, StartingAltitude, 10, 1);

            double fitness = f + v;
            if (Position.Y < 2 && Velocity.Y > CrashSpeed) //safe landing?
            {
                fitness += 100;
                //big bonus for fuel savings
                //fitness += NormalizeFitness(Fuel, 0, StartingFuel, StartingFuel, 100); //StandardizedRange(Fuel, 1, StartingFuel, 1, 100);
                //bonus for soft landing
                //v = CalcFitness(Velocity, CrashSpeed + 1, -1, -2, 50);  // StandardizedRange(Velocity, CrashSpeed, -1, 1, 20);
                //fitness += f;// + v;
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

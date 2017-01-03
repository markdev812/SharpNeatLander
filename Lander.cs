using System;
using SharpNeat.Core;
using SharpNeat.Phenomes;

namespace SharpNeatLander
{
    class Lander
    {
		public const double StartingAltitude = 2400;
		public const double StartingFuel = 500;
		public const double Gravity = -3.711;
		public const double TerminalVel = -200;

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
            Fuel -= Thrust;
            Velocity += (Thrust + _gravity) * deltaTime;

            if (Velocity < TerminalVel)
                Velocity = TerminalVel;

            Altitude += Velocity * deltaTime;

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

				ship.Thrust = Math.Round(outputArr[0] * 4); //Math.Floor(outputArr[0] * 4.0);
															//Ship.Thrust = 3.42;

				ship.Update(1);
				if (playMode)
					Console.WriteLine($"{i,-5}{ship.Altitude,8:F1}{ship.Velocity,8:F1}{ship.Fuel,8:F1}{ship.Thrust,8:F1}");

				//did we hit the ground?
				if (ship.Altitude <= 0)
					break;


			}

			return ship.CalcFitness();
		}

		public double CalcFitness()
		{
			//  fitness = fuel remaining
			//  height > 1  didn't land  fitness -= HeightAboveSurface
			//  height <= 1  landed
			//      V < -40 m/s  on touchdown  fitness = 0 
			//      V > -40  fitness += MaxVel - V   

			double nFuel = Fuel / StartingFuel;
			double nVelocity = Velocity / TerminalVel;
			double nAltitude = Altitude / StartingAltitude;

			double fitness = 0;
			//small reward for lower velocity
			fitness = (1 - nVelocity) * 5;
			//fitness += (1 - nAltitude)* 10;
			if (Altitude <= 2) //landed
			{
				if (Velocity > -40) //no crash
				{

					//big reward for safe landing
					fitness += 100;
					//fuel savings bonus
					fitness += nFuel * 10;

				}

			}

			return fitness > 0 ? fitness : 0;
		}
	}
}

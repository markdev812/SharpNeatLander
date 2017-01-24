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
    public class LanderWorld : NeatWorld
    {

        /// <summary>
        /// Instantiate one unit and run it through a few updates.
        /// </summary>
        /// <param name="box">The phenome to run against</param>
        /// <returns>The units fitness (0-1)</returns>
        public override double RunTrial(IBlackBox box)
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

 
 

    }

}

using SharpNeat.Phenomes;

namespace SharpNeatLander
{
    public class World
    {
        public double RunTrial(IBlackBox box)
        {
            Lander ship = new Lander();
            ship.Start();

            //run simulation for a few frames
            for (int i = 0; i < 500; i++)
            {

                ship.Compute(box);


                ship.Update(FrmMain.FixedDeltaTime); //0.25);
                //if (playMode)
                //    Console.WriteLine($"S:{i,-5}  X:{ship.Position.X,6:F1}  A:{ship.Position.Y,6:F1}  R:{ship.Rotation,6:F1}  Vx:{ship.Velocity.X,6:F1} Vy:{ship.Velocity.Y,6:F1} F:{ship.Fuel,6:F1}  T:{ship.Thrust,6:F1}");
                //double f = ship.GetFitness();
                //if (f > fitness)
                //    fitness = f;
                if (ship.Landed || ship.Crashed)
                    break;


            }
            return ship.GetFitness();
        }

    }

}

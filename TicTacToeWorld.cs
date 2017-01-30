using SharpNeat.Phenomes;

namespace SharpNeatLander
{
    public class TicTacToeWorld : NeatWorld
    {
        public TicTacToeWorld(string name, int numInputs, int numOutputs) : base(name, numInputs, numOutputs)
        {
        }

        public override double Evaluate(IBlackBox box)
        {
            double fitness = 0;
            //create 2 players

            NeatUnit p1 = NeatUnit.Create("tictactoe");
            p1.Start(this);
            NeatUnit p2 = NeatUnit.Create("tictactoe");
            p2.Start(this);

            bool done = false;
            while (!done)
            {
                p1.Compute(box);
                p1.Update(FixedDeltaTime);
                p2.Compute(box);  //*** is it OK to use the same box for both?
                p2.Update(FixedDeltaTime);

                //*** CHECK FOR WIN/LOSE/DRAW
                //if (p1win) return p1.GetFitness();
                //if (p2win) return p2.GetFitness();
                //if (draw) return 0;

            }
            //run until win or draw

            //return winner fitness
            return fitness;

        }

        public override void Run(IBlackBox box)
        {
            //run until win or draw
        }
    }
}

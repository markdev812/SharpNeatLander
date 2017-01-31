using SharpNeat.Phenomes;
using System.Drawing;

namespace SharpNeatLander
{
    public abstract class NeatUnit
    {
        public static NeatUnit Create(string name)
        {
            if (name == "lander") return new LanderUnit();
            //if (name == "tictactoe") return new TicTacToeUnit();

            return null;

        }
        public abstract void Start(NeatWorld world);
        public abstract bool Update(double deltaTime);
        public abstract void Compute(IBlackBox box);
        public abstract double GetFitness();
        public abstract void Render(Graphics g);
        public virtual void PrintStats() { }

    }
}

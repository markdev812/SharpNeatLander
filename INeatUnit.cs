using SharpNeat.Phenomes;
using System.Drawing;

namespace SharpNeatLander
{
    interface INeatUnit
    {
        void Start();
        void Update(double deltaTime);
        void Compute(IBlackBox box);
        double GetFitness();
        void Render(Graphics g);


    }
}

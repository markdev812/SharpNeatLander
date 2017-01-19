using SharpNeat.Phenomes;
using System.Drawing;

namespace SharpNeatLander
{
    public interface INeatUnit
    {
        void Start(INeatWorld world);
        void Update(double deltaTime);
        void Compute(IBlackBox box);
        double GetFitness();
        void Render(Graphics g);


    }
}

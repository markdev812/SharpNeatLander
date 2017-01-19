using SharpNeat.Phenomes;
using System.Drawing;

namespace SharpNeatLander
{
    public interface INeatWorld
    {
        double FixedDeltaTime { get; }
        double ViewScale { get; }
        int ViewWidth { get; }
        int ViewHeight { get; }
        Point WorldToView(Vector2 pos);

        void StartLearning();
        void StopLearning();
        void StartRunning();
        void StopRunning();

        /// <summary>
        /// Instantiate one INeatUnit and run it through a few compute/update cycles using the supplied phenome.
        /// Return the unit's fitness normalized 0-1
        /// </summary>
        /// <param name="box"></param>
        /// <returns></returns>
        double RunTrial(IBlackBox box);
    }
}

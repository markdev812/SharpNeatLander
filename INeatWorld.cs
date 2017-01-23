using SharpNeat.Phenomes;

namespace SharpNeatLander
{
    public interface INeatWorld
    {
        double FixedDeltaTime { get; }

        double Width { get; }
        double Height { get; }

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

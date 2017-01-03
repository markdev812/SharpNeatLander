using SharpNeat.Core;
using SharpNeat.Phenomes;

namespace SharpNeatLander
{
	public delegate double FitnessFunction(IBlackBox box);

	/// <summary>

	/// </summary>
	public class SimpleEvaluator : IPhenomeEvaluator<IBlackBox>
	{
		private readonly bool _hasStopFitness;
		private readonly double _stopFitness;
        ulong _evalCount;
        bool _stopConditionSatisfied;

		private readonly FitnessFunction _fitnessFunction;

		public SimpleEvaluator(FitnessFunction f)
		{
			_fitnessFunction = f;
			_hasStopFitness = false;
		}
		public SimpleEvaluator(FitnessFunction f, double stopFitness)
	    {
		    _fitnessFunction = f;
		    _stopFitness = stopFitness;
			_hasStopFitness = true;
	    }
      
        /// <summary>
        /// Gets the total number of evaluations that have been performed.
        /// </summary>
        public ulong EvaluationCount => _evalCount;

		/// <summary>
        /// Gets a value indicating whether some goal fitness has been achieved and that
        /// the evolutionary algorithm/search should stop. This property's value can remain false
        /// to allow the algorithm to run indefinitely.
        /// </summary>
        public bool StopConditionSatisfied => _stopConditionSatisfied;

		/// <summary>
        /// Evaluate the provided IBlackBox against the problem domain and return its fitness score.
        /// </summary>
        public FitnessInfo Evaluate(IBlackBox box)
        {
            double fitness = 0;
	       
            _evalCount++;
			
            box.ResetState();


            if(_fitnessFunction != null)
				fitness = _fitnessFunction(box);

            if (_hasStopFitness && fitness >= _stopFitness)
            {
                _stopConditionSatisfied = true;
            }

            return new FitnessInfo(fitness, fitness);
        }

        /// <summary>
        /// Reset the internal state of the evaluation scheme if any exists.
        /// </summary>
        public void Reset()
        {
        }

      
    }
}
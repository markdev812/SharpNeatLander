using SharpNeat.Core;
using SharpNeat.Decoders;
using SharpNeat.Decoders.Neat;
using SharpNeat.DistanceMetrics;
using SharpNeat.Domains;
using SharpNeat.EvolutionAlgorithms;
using SharpNeat.EvolutionAlgorithms.ComplexityRegulation;
using SharpNeat.Genomes.Neat;
using SharpNeat.Phenomes;
using SharpNeat.SpeciationStrategies;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml;

namespace SharpNeatLander
{
	/// <summary>
	/// INeatExperiment for the XOR logic gate problem domain. 
	/// </summary>
	public class SimpleExperiment //: IGuiNeatExperiment
	{
		//private static readonly ILog __log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

		public IGenomeFactory<NeatGenome> GenomeFactory;
		public List<NeatGenome> GenomeList;

		NeatEvolutionAlgorithmParameters _eaParams;
		NeatGenomeParameters _neatGenomeParams;
		string _name;
		int _populationSize;
		int _specieCount;
		NetworkActivationScheme _activationScheme;
		string _complexityRegulationStr;
		int? _complexityThreshold;
		string _description;
		ParallelOptions _parallelOptions;

		private FitnessFunction _fitnessFunction;
		private double _stopFitness;

		private string _champFile;

		/// <summary>
		/// Gets the name of the experiment.
		/// </summary>
		public string Name => _name;

		/// <summary>
		/// Gets human readable explanatory text for the experiment.
		/// </summary>
		public string Description => _description;

		/// <summary>
		/// Gets the number of inputs required by the network/black-box that the underlying problem domain is based on.
		/// </summary>
		public int InputCount { get; private set; }

		/// <summary>
		/// Gets the number of outputs required by the network/black-box that the underlying problem domain is based on.
		/// </summary>
		public int OutputCount { get; private set; }

		/// <summary>
		/// Gets the default population size to use for the experiment.
		/// </summary>
		public int DefaultPopulationSize => _populationSize;

		/// <summary>
		/// Gets the NeatEvolutionAlgorithmParameters to be used for the experiment. Parameters on this object can be 
		/// modified. Calls to CreateEvolutionAlgorithm() make a copy of and use this object in whatever state it is in 
		/// at the time of the call.
		/// </summary>
		public NeatEvolutionAlgorithmParameters NeatEvolutionAlgorithmParameters => _eaParams;

		/// <summary>
		/// Gets the NeatGenomeParameters to be used for the experiment. Parameters on this object can be modified. Calls
		/// to CreateEvolutionAlgorithm() make a copy of and use this object in whatever state it is in at the time of the call.
		/// </summary>
		public NeatGenomeParameters NeatGenomeParameters => _neatGenomeParams;

		/// <summary>
		/// Initialize the experiment with simple defaults params.
		/// </summary>
		public void Initialize(string name, int inputCount, int outputCount)
		{
			InputCount = inputCount;
			OutputCount = outputCount;

			_name = name;
			_populationSize = 100;
			_specieCount = 10;
			_activationScheme = NetworkActivationScheme.CreateAcyclicScheme();
			_complexityRegulationStr = "Absolute";
			_complexityThreshold = 10;
			_description = name;
			_parallelOptions = new ParallelOptions();

			_eaParams = new NeatEvolutionAlgorithmParameters { SpecieCount = _specieCount };
			_neatGenomeParams = new NeatGenomeParameters { FeedforwardOnly = _activationScheme.AcyclicNetwork };
		}
		/// <summary>
		/// Initialize the experiment with some optional XML configuration data.
		/// </summary>
		//public void Initialize(string name, XmlElement xmlConfig)
		//{
		//	_name = name;
		//	_populationSize = XmlUtils.GetValueAsInt(xmlConfig, "PopulationSize");
		//	_specieCount = XmlUtils.GetValueAsInt(xmlConfig, "SpecieCount");
		//	_activationScheme = ExperimentUtils.CreateActivationScheme(xmlConfig, "Activation");
		//	_complexityRegulationStr = XmlUtils.TryGetValueAsString(xmlConfig, "ComplexityRegulationStrategy");
		//	_complexityThreshold = XmlUtils.TryGetValueAsInt(xmlConfig, "ComplexityThreshold");
		//	_description = XmlUtils.TryGetValueAsString(xmlConfig, "Description");
		//	_parallelOptions = ExperimentUtils.ReadParallelOptions(xmlConfig);

		//	_eaParams = new NeatEvolutionAlgorithmParameters();
		//	_eaParams.SpecieCount = _specieCount;
		//	_neatGenomeParams = new NeatGenomeParameters();
		//	_neatGenomeParams.FeedforwardOnly = _activationScheme.AcyclicNetwork;
		//}


		///
		public NeatEvolutionAlgorithm<NeatGenome> CreateSimpleEA(string name, int inputCount, int outputCount, FitnessFunction fitnessFunction, double stopFitness = -1)
		{
			_stopFitness = stopFitness;
			_fitnessFunction = fitnessFunction;
			//init with simple defaults
			Initialize(name, inputCount, outputCount);

			_champFile = $"{name}.champ.xml";

			// Create a genome factory with our neat genome parameters object and the appropriate number of input and output neuron genes.
			GenomeFactory = CreateGenomeFactory();

			// Create an initial population of randomly generated genomes.

			GenomeList = GenomeFactory.CreateGenomeList(_populationSize, 0);

			//create evolutionary algorithm
			return CreateEvolutionAlgorithm(GenomeFactory, GenomeList);
		}

		/// <summary>
		/// Load the champion xml file, decode and return the genome 
		/// </summary>
		public IBlackBox GetChamp()
		{
			var genomeDecoder = CreateGenomeDecoder();

			NeatGenome genome = null;
			using (XmlReader xr = XmlReader.Create(_champFile))
			{
				genome = NeatGenomeXmlIO.ReadCompleteGenomeList(xr, false, (NeatGenomeFactory)GenomeFactory)[0];
			}
			// Decode the best genome into a phenome (neural network).
			return genomeDecoder.Decode(genome);// CurrentChampGenome);
		}

		/// <summary>
		/// Save the genome to the champion file
		/// </summary>
		/// <param name="genome"></param>
		public void SaveChamp(NeatGenome genome)
		{
			var doc = NeatGenomeXmlIO.SaveComplete(new List<NeatGenome>() { genome }, false);
			doc.Save(_champFile);
		}
		/// <summary>
		/// Load a population of genomes from an XmlReader and returns the genomes in a new list.
		/// The genome factory for the genomes can be obtained from any one of the genomes.
		/// </summary>
		public List<NeatGenome> LoadPopulation(XmlReader xr)
		{
			NeatGenomeFactory genomeFactory = (NeatGenomeFactory)CreateGenomeFactory();
			return NeatGenomeXmlIO.ReadCompleteGenomeList(xr, false, genomeFactory);
		}

		/// <summary>
		/// Save a population of genomes to an XmlWriter.
		/// </summary>
		public void SavePopulation(XmlWriter xw, IList<NeatGenome> genomeList)
		{
			// Writing node IDs is not necessary for NEAT.
			NeatGenomeXmlIO.WriteComplete(xw, genomeList, false);
		}

		/// <summary>
		/// Create a genome decoder for the experiment.
		/// </summary>
		public IGenomeDecoder<NeatGenome, IBlackBox> CreateGenomeDecoder()
		{
			return new NeatGenomeDecoder(_activationScheme);
		}

		/// <summary>
		/// Create a genome factory for the experiment.
		/// Create a genome factory with our neat genome parameters object and the appropriate number of input and output neuron genes.
		/// </summary>
		public IGenomeFactory<NeatGenome> CreateGenomeFactory()
		{
			return new NeatGenomeFactory(InputCount, OutputCount, _neatGenomeParams);
		}

		/// <summary>
		/// Create and return a NeatEvolutionAlgorithm object ready for running the NEAT algorithm/search. Various sub-parts
		/// of the algorithm are also constructed and connected up.
		/// Uses the experiments default population size defined in the experiment's config XML.
		/// </summary>
		public NeatEvolutionAlgorithm<NeatGenome> CreateEvolutionAlgorithm()
		{
			return CreateEvolutionAlgorithm(_populationSize);
		}

		/// <summary>
		/// Create and return a NeatEvolutionAlgorithm object ready for running the NEAT algorithm/search. Various sub-parts
		/// of the algorithm are also constructed and connected up.
		/// This overload accepts a population size parameter that specifies how many genomes to create in an initial randomly
		/// generated population.
		/// </summary>
		public NeatEvolutionAlgorithm<NeatGenome> CreateEvolutionAlgorithm(int populationSize)
		{
			// Create a genome factory with our neat genome parameters object and the appropriate number of input and output neuron genes.
			IGenomeFactory<NeatGenome> genomeFactory = CreateGenomeFactory();

			// Create an initial population of randomly generated genomes.
			List<NeatGenome> genomeList = genomeFactory.CreateGenomeList(populationSize, 0);

			// Create evolution algorithm.
			return CreateEvolutionAlgorithm(genomeFactory, genomeList);
		}

		/// <summary>
		/// Create and return a NeatEvolutionAlgorithm object ready for running the NEAT algorithm/search. Various sub-parts
		/// of the algorithm are also constructed and connected up.
		/// This overload accepts a pre-built genome population and their associated/parent genome factory.
		/// </summary>
		public NeatEvolutionAlgorithm<NeatGenome> CreateEvolutionAlgorithm(IGenomeFactory<NeatGenome> genomeFactory, List<NeatGenome> genomeList)
		{
			// Create distance metric. Mismatched genes have a fixed distance of 10; for matched genes the distance is their weight difference.
			IDistanceMetric distanceMetric = new ManhattanDistanceMetric(1.0, 0.0, 10.0);
			ISpeciationStrategy<NeatGenome> speciationStrategy = new ParallelKMeansClusteringStrategy<NeatGenome>(distanceMetric, _parallelOptions);

			// Create complexity regulation strategy.
			IComplexityRegulationStrategy complexityRegulationStrategy = ExperimentUtils.CreateComplexityRegulationStrategy(_complexityRegulationStr, _complexityThreshold);

			// Create the evolution algorithm.
			NeatEvolutionAlgorithm<NeatGenome> ea = new NeatEvolutionAlgorithm<NeatGenome>(_eaParams, speciationStrategy, complexityRegulationStrategy);

			// Create IBlackBox evaluator.
			SimpleEvaluator evaluator;
			if (_stopFitness >= 0)
				evaluator = new SimpleEvaluator(_fitnessFunction, _stopFitness);
			else
				evaluator = new SimpleEvaluator(_fitnessFunction);


			// Create genome decoder.
			IGenomeDecoder<NeatGenome, IBlackBox> genomeDecoder = CreateGenomeDecoder();

			// Create a genome list evaluator. This packages up the genome decoder with the genome evaluator.
			IGenomeListEvaluator<NeatGenome> innerEvaluator = new ParallelGenomeListEvaluator<NeatGenome, IBlackBox>(genomeDecoder, evaluator, _parallelOptions);

			// Wrap the list evaluator in a 'selective' evaluator that will only evaluate new genomes. That is, we skip re-evaluating any genomes
			// that were in the population in previous generations (elite genomes). This is determined by examining each genome's evaluation info object.
			IGenomeListEvaluator<NeatGenome> selectiveEvaluator = new SelectiveGenomeListEvaluator<NeatGenome>(
																					innerEvaluator,
																					SelectiveGenomeListEvaluator<NeatGenome>.CreatePredicate_OnceOnly());
			// Initialize the evolution algorithm.
			ea.Initialize(selectiveEvaluator, genomeFactory, genomeList);

			// Finished. Return the evolution algorithm
			return ea;
		}


	}
}
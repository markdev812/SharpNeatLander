using SharpNeat.Phenomes;

namespace SharpNeatLander
{
    public class TicTacToePlayer : IPlayer
    {



        public int PlayerType { get; set; }     //0=none,  1 = X,  2 = O
        private IBlackBox _brain;
        private int[] _gb;


        public TicTacToePlayer(IBlackBox brain, int[] gameBoard, int playerType)
        {
            _gb = gameBoard;
            _brain = brain;
            PlayerType = playerType;
        }
        //public override void Start(NeatWorld world)
        //{
        //    _world = world;

        //    if (first)
        //    {
        //        PlayerType = 1;
        //    }
        //    else
        //    {
        //        PlayerType = 2;
        //    }
        //    first = false;
        //}


        public void MakeMove()
        {

            ISignalArray inputArr = _brain.InputSignalArray;
            ISignalArray outputArr = _brain.OutputSignalArray;

            _brain.ResetState();


            //one input for each square on the board
            for (int i = 0; i < 9; i++)
            {
                inputArr[i] = _gb[i];
            }

            _brain.Activate();

            //_desiredMove = Mathf.FloorToInt(outputArr[0] * 8.0);

            //take the best of the 9 outputs
            int move = -1;
            double maxScore = 0;
            for (int i = 0; i < 9; i++)
            {
                if (!TicTacToeWorld.IsLegalMove(_gb, i))
                    continue;
                double score = outputArr[i];
                if (score > maxScore)
                {
                    move = i;
                    maxScore = score;
                }
            }

            if (move >= 0)
                _gb[move] = PlayerType;


        }


        public void Compute(IBlackBox box)
        {








        }


    }
}

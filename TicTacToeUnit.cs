using SharpNeat.Phenomes;
using System.Diagnostics;
using System.Drawing;

namespace SharpNeatLander
{
    class TicTacToeUnit : NeatUnit
    {
        private NeatWorld _world;

        public static int[] GB = null;                  //0 1 2
                                                        //3 4 5
                                                        //6 7 8


        private static bool first = true;
        public int PlayerType { get; set; }     //0=none,  1 = X,  2 = O

        private int _desiredMove;
        private int xwins, owins = 0;


        public override void Start(NeatWorld world)
        {
            _world = world;
            GB = new int[9];

            if (first)
            {
                PlayerType = 1;
            }
            else
            {
                PlayerType = 2;
            }
            first = false;
        }


        public override bool Update(double deltaTime)
        {

            //update will make one move for this player


            //perform our move (X)
            if (IsLegalMove(_desiredMove))
            {

                GB[_desiredMove] = PlayerType;



            }
            return GetWinner() == PlayerType;
        }
        public override void PrintStats()
        {
            Debug.WriteLine($"{GB[0]} {GB[1]} {GB[2]}");
            Debug.WriteLine($"{GB[3]} {GB[4]} {GB[5]}");
            Debug.WriteLine($"{GB[6]} {GB[7]} {GB[8]}");
            Debug.WriteLine("");
            Debug.WriteLine($"X: {xwins} O: {owins}");
        }

        public override void Compute(IBlackBox box)
        {
            ISignalArray inputArr = box.InputSignalArray;
            ISignalArray outputArr = box.OutputSignalArray;

            //one input for each square on the board
            for (int i = 0; i < 9; i++)
            {
                inputArr[i] = GB[i];
            }


            box.Activate();

            _desiredMove = Mathf.FloorToInt(outputArr[0] * 8.0);

        }

        private bool IsLegalMove(int index)
        {
            return GB[index] == 0; //unoccupied?
        }

        private int Sq(int v)
        {
            return v * v;
        }
        private int GetWinner()
        {
            //0 1 2
            //3 4 5
            //6 7 8

            //check rows
            int v1 = Sq(GB[0]) + Sq(GB[1]) + Sq(GB[2]);
            int v2 = Sq(GB[3]) + Sq(GB[4]) + Sq(GB[5]);
            int v3 = Sq(GB[6]) + Sq(GB[7]) + Sq(GB[8]);
            if (v1 == 3 || v2 == 3 || v3 == 3)
                return 1;  //X won
            if (v1 == 12 || v2 == 12 || v3 == 12)
                return 2;  //O won

            //check columns
            v1 = Sq(GB[0]) + Sq(GB[3]) + Sq(GB[6]);
            v2 = Sq(GB[1]) + Sq(GB[4]) + Sq(GB[7]);
            v3 = Sq(GB[2]) + Sq(GB[5]) + Sq(GB[8]);
            if (v1 == 3 || v2 == 3 || v3 == 3)
                return 1;  //X won
            if (v1 == 12 || v2 == 12 || v3 == 12)
                return 2;  //O won          

            //check diag
            int d1 = Sq(GB[0]) + Sq(GB[4]) + Sq(GB[8]);
            int d2 = Sq(GB[2]) + Sq(GB[4]) + Sq(GB[6]);
            if (d1 == 3 || d2 == 3)
                return 1;  //X won
            if (d1 == 12 || d2 == 12)
                return 2;  //O won           

            return 0;
        }
        public override double GetFitness()
        {
            return GetWinner() == PlayerType ? 1.0 : 0;
        }

        public override void Render(Graphics g)
        {
            //throw new NotImplementedException();
        }
    }
}

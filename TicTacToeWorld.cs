using SharpNeat.Phenomes;
using System.Diagnostics;

namespace SharpNeatLander
{
    public class TicTacToeWorld : NeatWorld
    {
        public TicTacToeWorld(string name, int numInputs, int numOutputs) : base(name, numInputs, numOutputs)
        {


        }
        public static bool IsLegalMove(int[] GB, int index)
        {
            return GB[index] == 0; //unoccupied?
        }

        private static int Sq(int v)
        {
            return v * v;
        }
        public static int GetWinner(int[] GB)
        {
            //0 1 2
            //3 4 5
            //6 7 8
            bool p1 = false, p2 = false;


            //check rows
            int v1 = Sq(GB[0]) + Sq(GB[1]) + Sq(GB[2]);
            int v2 = Sq(GB[3]) + Sq(GB[4]) + Sq(GB[5]);
            int v3 = Sq(GB[6]) + Sq(GB[7]) + Sq(GB[8]);
            if (v1 == 3 || v2 == 3 || v3 == 3)
                p1 = true;  //X won
            if (v1 == 12 || v2 == 12 || v3 == 12)
                p2 = true;  //O won

            //check columns
            v1 = Sq(GB[0]) + Sq(GB[3]) + Sq(GB[6]);
            v2 = Sq(GB[1]) + Sq(GB[4]) + Sq(GB[7]);
            v3 = Sq(GB[2]) + Sq(GB[5]) + Sq(GB[8]);
            if (v1 == 3 || v2 == 3 || v3 == 3)
                p1 = true;  //X won
            if (v1 == 12 || v2 == 12 || v3 == 12)
                p2 = true;  //O won          

            //check diag
            int d1 = Sq(GB[0]) + Sq(GB[4]) + Sq(GB[8]);
            int d2 = Sq(GB[2]) + Sq(GB[4]) + Sq(GB[6]);
            if (d1 == 3 || d2 == 3)
                p1 = true;  //X won
            if (d1 == 12 || d2 == 12)
                p2 = true;  //O won           

            int result = 0; // 0=none 1=player1 won, 2=player2 won, 3=draw
            if (p1 && p2)
                result = 3;
            else if (p1)
                result = 1;
            else if (p2)
                result = 2;

            return result;
        }

        public double PlayGame(int[] GB, TicTacToePlayer p1, TicTacToePlayer p2)
        {
            //reset game board
            for (int i = 0; i < 9; i++)
            {
                GB[i] = 0;
            }

            for (int i = 0; i < 9; i++)
            {

                p1.MakeMove();
                p2.MakeMove();
                if (i >= 3)  //start checking for winner after 3 moves
                {
                    int result = GetWinner(GB);
                    if (result == 3) //draw
                        return 1;
                    if (result == 1) //p1 won
                        return 10;
                    if (result == 2) //p1 lost
                        return 0;
                }
            }
            return 0;
        }

        public override double Evaluate(IBlackBox box)
        {

            int[] GB = new int[9];

            //create 2 players
            //we are only evaluating player 1

            var p1 = new TicTacToePlayer(box, GB, 1);
            var p2 = new TicTacToePlayer(box, GB, 2); //*** this could be random or optimal player also

            double fitness = 0;
            for (int i = 0; i < 50; i++)
            {


                fitness += PlayGame(GB, p1, p2);

            }
            //PrintStats(GB);
            return fitness;

        }

        public override void Run(IBlackBox box)
        {
            int[] GB = new int[9];


            var p1 = new TicTacToePlayer(box, GB, 1);
            var p2 = new TicTacToePlayer(box, GB, 2);

            PlayGame(GB, p1, p2);

            PrintStats(GB);


        }
        public void PrintStats(int[] GB)
        {
            Debug.WriteLine($"{GB[0]} {GB[1]} {GB[2]}");
            Debug.WriteLine($"{GB[3]} {GB[4]} {GB[5]}");
            Debug.WriteLine($"{GB[6]} {GB[7]} {GB[8]}");
            Debug.WriteLine("");

        }
    }
}

using System;
using System.Threading;

namespace SharpNeatLander
{
    class TicTacToeRandomPlayer : IPlayer
    {

        public int PlayerType { get; set; }     //0=none,  1 = X,  2 = O
        private int[] _gb;
        public TicTacToeRandomPlayer(int[] gameBoard, int playerType)
        {
            _gb = gameBoard;
            PlayerType = playerType;
        }

        public void MakeMove()
        {
            Thread.Sleep(10); //make sure random seed has changed
            Random random = new Random();

            if (TicTacToeWorld.MovesAvailable(_gb) == false)
                return;

            int move = random.Next(9);

            while (_gb[move] != 0)
            {
                move = random.Next(9);

            }


            if (move >= 0)
                _gb[move] = PlayerType; ;
        }
    }
}

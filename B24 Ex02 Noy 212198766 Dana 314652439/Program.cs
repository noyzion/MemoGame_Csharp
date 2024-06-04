using Ex02;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B24_Ex02_Noy_212198766_Dana_314652439
{
    public class Program
    {
        public static void Main()
        {
            UserInterfaceController user = new UserInterfaceController();
            GameBoard<char> board = new GameBoard<char>();
            user.GetPlayerName();
            user.GetBoardBounds();
            int colum, row;
            user.GetNextCard(out colum,out row);
            board.SetValidBounds(4, 4);
            char[] array = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H' };
            board.GetValuesForTheBoard(array);
            board.GameMemoryBoard = (board.Height, board.Width);
            board.UpdateBoard(2, 3);
            Console.WriteLine(board.BuildBoard());

        }
    }
}

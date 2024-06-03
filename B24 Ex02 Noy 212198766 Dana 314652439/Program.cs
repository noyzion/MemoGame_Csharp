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
            GameBoard<char> board = new GameBoard<char>();
            board.setValidBounds(4, 4);
            char[] array = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H' };
            board.getValuesForTheBoard(array);
            board.GameMemoryBoard = (board.Height, board.Width);
            board.PrintBoard();
        }
    }
}

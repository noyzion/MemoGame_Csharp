using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace B24_Ex02_Noy_212198766_Dana_314652439
{
    internal class GameLogic<T>
    {

        public void CheckPlayerTurn(Player i_Player,T i_CardOneValue, T i_CardTwoValue)
        {
            if (Equals(i_CardOneValue, i_CardTwoValue))
            {
                i_Player.IsMyTurn = true;
                i_Player.Score++;
            }
            else
            {
                i_Player.IsMyTurn = false;
            }
        }


    }
}

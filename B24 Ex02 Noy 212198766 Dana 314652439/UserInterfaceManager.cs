using Ex02;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B24_Ex02_Noy_212198766_Dana_314652439
{
    public class UserInterfaceManager
    {
        UserInterfaceController UIController = new UserInterfaceController();
        GameLogic<char> m_GameLogic = new GameLogic<char>();
        public void RunGame()
        {
            eGameConfig gameStaus = eGameConfig.CountinueGame;
            UIController.FirstPlayer.Name = UIController.GetPlayerName();
            int ComputerOrHuman = UIController.GetAndCheckIfSecondPlayerCompOrHuman();

            if (ComputerOrHuman == (int)eGameConfig.Human)
            {
                UIController.SecondPlayer.Name = UIController.GetPlayerName();
            }
            else
            {
                UIController.SecondPlayer.Name = "Computer";
            }

            UIController.GetAndCheckBoardBounds();
            char[] array = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H' };
            UIController.GameBoard.GetValuesForTheBoard(array);
            UIController.GameBoard.FillBoardWithValues();
            Console.WriteLine(UIController.BuildBoard());

            while (gameStaus != eGameConfig.EndGame)
            {
                UIController.FirstPlayer.IsMyTurn = true;
                while (UIController.FirstPlayer.IsMyTurn)
                {
                    gameStaus = PlayerTurn(UIController.FirstPlayer);
                }
                UIController.SecondPlayer.IsMyTurn = true;
                while (UIController.SecondPlayer.IsMyTurn)
                {
                    gameStaus = PlayerTurn(UIController.SecondPlayer);
                }
                gameStaus = UIController.GameBoard.IsBoardFull();
            }


        }

        public eGameConfig PlayerTurn(Player i_player)
        {
            eGameConfig fullBoard = UIController.GameBoard.IsBoardFull();

            if (fullBoard == eGameConfig.CountinueGame)
            {
                i_player.FirstCard = UIController.GetNextCard(i_player.Name);
                Ex02.ConsoleUtils.Screen.Clear();

                UIController.GameBoard.UpdateBoard(i_player.FirstCard, true);
                Console.WriteLine(UIController.BuildBoard());

                i_player.SecondCard = UIController.GetNextCard(i_player.Name);

                Ex02.ConsoleUtils.Screen.Clear();
                UIController.GameBoard.UpdateBoard(i_player.SecondCard, true);



                char firstCardValue = UIController.GameBoard.GetValueFromCellInBoard(i_player.FirstCard);
                char secondCardValue = UIController.GameBoard.GetValueFromCellInBoard(i_player.SecondCard);
                m_GameLogic.CheckPlayerTurn(i_player, firstCardValue, secondCardValue);

                if (i_player.IsMyTurn)
                {
                    Console.WriteLine(UIController.BuildBoard());
                }
                else
                {
                    Console.WriteLine(UIController.BuildBoard());
                    System.Threading.Thread.Sleep(2000);
                    UIController.GameBoard.UpdateBoard(i_player.FirstCard, false);
                    UIController.GameBoard.UpdateBoard(i_player.SecondCard, false);
                    Ex02.ConsoleUtils.Screen.Clear();
                    Console.WriteLine(UIController.BuildBoard());
                    i_player.IsMyTurn = false;
                }
            }
            return fullBoard;
        }
    }
}

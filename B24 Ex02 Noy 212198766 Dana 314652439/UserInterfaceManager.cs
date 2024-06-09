
using System.Linq;
using System.Security.Cryptography;

namespace Exercise02
{
    public class UserInterfaceManager
    {
        UserInterfaceController UIController = new UserInterfaceController();
        GameLogic m_GameLogic = new GameLogic();
        public void RunGame()
        {
            eGameConfig gameStatus = eGameConfig.CountinueGame;
            Player firstPlayer = new Player();
            firstPlayer.Name = UIController.GetPlayerName();
            int ComputerOrHuman = UIController.GetAndCheckIfSecondPlayerCompOrHuman();

            Player secondPlayer = new Player();
            if (ComputerOrHuman == (int)eGameConfig.Human)
            {
                secondPlayer.Name = UIController.GetPlayerName();
                if (firstPlayer.Name == secondPlayer.Name)
                {
                    secondPlayer.Name += "2";
                }
            }
            else
            {
                secondPlayer.Name = "computer";
            }

            UIController.GetAndCheckBoardBounds(out int width, out int height);

            GameBoard memoGameBoard = new GameBoard(width, height);

            while (memoGameBoard.CheckParityBounds() != eErrorType.NoError)
            {
                UIController.PrintError(eErrorType.OddSize);
                UIController.GetAndCheckBoardBounds(out width, out height);
                memoGameBoard = new GameBoard(width, height);
            }

            char[] valuesForTheBoard = UIController.ShuffleCharValuesForTheBoard(memoGameBoard);
            UIController.MatchLogicalValueToChar(memoGameBoard, valuesForTheBoard);
            UIController.PrintBoard(memoGameBoard);

            while (gameStatus != eGameConfig.EndGame)
            {
                firstPlayer.IsMyTurn = true;
                while (firstPlayer.IsMyTurn && gameStatus != eGameConfig.EndGame)
                {
                    gameStatus = PlayerTurn(memoGameBoard, firstPlayer);
                }
                secondPlayer.IsMyTurn = true;
                while (secondPlayer.IsMyTurn && gameStatus != eGameConfig.EndGame)
                {
                    gameStatus = PlayerTurn(memoGameBoard, secondPlayer);
                }
                gameStatus = memoGameBoard.IsBoardFull();
            }

            UIController.PrintWinner(firstPlayer, secondPlayer);
        }

        public eGameConfig PlayerTurn(GameBoard i_MemoGameBoard, Player i_Player)
        {
            i_Player.FirstCard.Initialize();
            i_Player.SecondCard.Initialize();


            eGameConfig fullBoard = i_MemoGameBoard.IsBoardFull();

            if (fullBoard == eGameConfig.CountinueGame)
            {

                if (i_Player.Name == "computer")
                {
                    i_Player.ComputerChooseCards(i_MemoGameBoard, m_GameLogic);
                    i_MemoGameBoard.UpdateBoard(i_Player.FirstCard, true);
                    i_MemoGameBoard.UpdateBoard(i_Player.SecondCard, true);

                }
                else
                {
                    i_Player.FirstCard = UIController.GetNextCard(i_MemoGameBoard, i_Player.Name);
                    Ex02.ConsoleUtils.Screen.Clear();

                    i_MemoGameBoard.UpdateBoard(i_Player.FirstCard, true);
                    UIController.PrintBoard(i_MemoGameBoard);

                    i_Player.SecondCard = UIController.GetNextCard(i_MemoGameBoard, i_Player.Name);

                    i_MemoGameBoard.UpdateBoard(i_Player.SecondCard, true);
                }

                Ex02.ConsoleUtils.Screen.Clear();
                m_GameLogic.CheckCardsAndReplaceTurn(i_MemoGameBoard, i_Player);

                if (i_Player.IsMyTurn)
                {
                    UIController.PrintBoard(i_MemoGameBoard);
                    if (i_Player.Name == "computer")
                    {
                        System.Threading.Thread.Sleep(2000);
                    }
                }
                else
                {
                    UIController.PrintBoard(i_MemoGameBoard);
                    System.Threading.Thread.Sleep(2000);
                    i_MemoGameBoard.UpdateBoard(i_Player.FirstCard, false);
                    i_MemoGameBoard.UpdateBoard(i_Player.SecondCard, false);
                    Ex02.ConsoleUtils.Screen.Clear();
                    UIController.PrintBoard(i_MemoGameBoard);
                    i_Player.IsMyTurn = false;
                }

            }

            return fullBoard;
        }

    }

}
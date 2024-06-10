﻿
using System;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Security.Cryptography;

namespace Exercise02
{
    public class UserInterfaceManager
    {
        UserInterfaceController UIController = new UserInterfaceController();
        GameLogic m_GameLogic = new GameLogic();

        public void RunGame()
        {
            eGameConfig gameStatus = eGameConfig.AnotherGame;
            Player firstPlayer = initializePlayer(UIController.GetPlayerName());
            Player secondPlayer = initializeSecondPlayer();

            if (firstPlayer.Name == secondPlayer.Name)
            {
                secondPlayer.Name += "2";
            }

            while (gameStatus == eGameConfig.AnotherGame)
            {
                GameBoard memoGameBoard = createGameBoard();

                UIController.PrintBoard(memoGameBoard);
                gameStatus = playGame(firstPlayer, secondPlayer, memoGameBoard);
                if (gameStatus == eGameConfig.CountinueGame || gameStatus == eGameConfig.BoardFull)
                {
                    UIController.PrintWinner(firstPlayer, secondPlayer);
                    gameStatus = (eGameConfig)UIController.AskIfThePlayerWantAnotherGame();
                    Ex02.ConsoleUtils.Screen.Clear();
                }
            }
        }

        private GameBoard createGameBoard()
        {
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
            return memoGameBoard;
        }

        private Player initializePlayer(string i_PlayerName)
        {
            Player player = new Player();
            player.Name = i_PlayerName;
            return player;
        }

        private Player initializeSecondPlayer()
        {
            Player secondPlayer = new Player();
            int ComputerOrHuman = UIController.GetAndCheckIfSecondPlayerCompOrHuman();

            if (ComputerOrHuman == (int)eGameConfig.Human)
            {
                secondPlayer.Name = UIController.GetPlayerName();
            }
            else
            {
                secondPlayer.Name = "computer";
            }
            return secondPlayer;
        }

        private eGameConfig playGame(Player i_FirstPlayer, Player i_SecondPlayer, GameBoard i_MemoGameBoard)
        {
            eGameConfig gameStatus = eGameConfig.CountinueGame;

            while (gameStatus != eGameConfig.EndGame && gameStatus != eGameConfig.BoardFull)
            {
                i_FirstPlayer.IsMyTurn = true;
                while (gameStatus != eGameConfig.EndGame && i_FirstPlayer.IsMyTurn)
                {
                    gameStatus = PlayerTurn(i_FirstPlayer, i_MemoGameBoard);
                }
                i_SecondPlayer.IsMyTurn = true;
                while ( gameStatus != eGameConfig.EndGame &&
                        gameStatus != eGameConfig.BoardFull && i_SecondPlayer.IsMyTurn)
                {
                    gameStatus = PlayerTurn(i_SecondPlayer, i_MemoGameBoard);
                }
                if (gameStatus != eGameConfig.EndGame)
                {
                    gameStatus = i_MemoGameBoard.IsBoardFull();
                }
            }
            
            return gameStatus;
        }

        public eGameConfig PlayerTurn(Player i_Player, GameBoard i_MemoGameBoard)
        {
            i_Player.FirstCard.Initialize();
            i_Player.SecondCard.Initialize();

            eGameConfig gameStatus = i_MemoGameBoard.IsBoardFull();

            if (gameStatus == eGameConfig.CountinueGame)
            {
                if (i_Player.Name == "computer")
                {
                    performComputerTurn(i_Player, i_MemoGameBoard);
                }
                else
                {
                   gameStatus = performHumanTurn(i_Player, i_MemoGameBoard);
                }
                if (gameStatus == eGameConfig.CountinueGame)
                {
                    Ex02.ConsoleUtils.Screen.Clear();
                    m_GameLogic.CheckCardsAndReplaceTurn(i_Player, i_MemoGameBoard);
                    UIController.PrintBoard(i_MemoGameBoard);
                    System.Threading.Thread.Sleep(2000);
                    if (!i_Player.IsMyTurn)
                    {
                        closePlayerCards(i_Player, i_MemoGameBoard);
                    }
                }
            }

            return gameStatus;
        }

        private void performComputerTurn(Player i_Player, GameBoard i_MemoGameBoard)
        {
            i_Player.ComputerChooseCards(i_MemoGameBoard, m_GameLogic);
            i_MemoGameBoard.UpdateBoard(i_Player.FirstCard, true);
            i_MemoGameBoard.UpdateBoard(i_Player.SecondCard, true);
        }

        private eGameConfig performHumanTurn(Player i_Player, GameBoard i_MemoGameBoard)
        {
            eGameConfig gameStatus = eGameConfig.CountinueGame;

            i_Player.FirstCard = UIController.GetNextCard(i_MemoGameBoard, i_Player.Name);
            if (i_Player.FirstCard == null)
            {
                gameStatus = eGameConfig.EndGame;
            }
            else
            { 
                Ex02.ConsoleUtils.Screen.Clear();
                i_MemoGameBoard.UpdateBoard(i_Player.FirstCard, true);
                UIController.PrintBoard(i_MemoGameBoard);
                i_Player.SecondCard = UIController.GetNextCard(i_MemoGameBoard, i_Player.Name);
                if (i_Player.SecondCard != null)
                {
                    i_MemoGameBoard.UpdateBoard(i_Player.SecondCard, true);
                }
                else
                {
                    gameStatus = eGameConfig.EndGame;
                }
            }

            return gameStatus;
        }
        private void closePlayerCards(Player i_Player, GameBoard i_MemoGameBoard)
        {
            i_MemoGameBoard.UpdateBoard(i_Player.FirstCard, false);
            i_MemoGameBoard.UpdateBoard(i_Player.SecondCard, false);
            Ex02.ConsoleUtils.Screen.Clear();
            UIController.PrintBoard(i_MemoGameBoard);
        }
    }
}
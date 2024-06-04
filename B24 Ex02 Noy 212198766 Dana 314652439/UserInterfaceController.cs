using B24_Ex02_Noy_212198766_Dana_314652439;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex02
{
    public class UserInterfaceController
    {
        private const int k_MaxBoardSize = 6;
        private const int k_MinBoardSize = 4;
        GameBoard<char> m_MemoGameBoard = new GameBoard<char>();
        GameLogic<char> m_GameLogic = new GameLogic<char>();
        Player m_FirstPlayer = new Player();
        Player m_SecondPlayer = new Player();

        public string GetPlayerName()
        {
            Console.Write("Please enter your name: ");
            string playerName = Console.ReadLine();
            return playerName;
        }
        public void GetBoardBounds()
        {
            Console.Write("Please enter the board width: ");
            string boardWidth = Console.ReadLine();
            Console.Write("Please enter the board height: ");
            string boardHeight = Console.ReadLine();
            if (CheckIfIntegers(boardWidth, boardHeight) != eErrorType.NoError)
            {
                PrintError(eErrorType.NotAnInteger);
                GetBoardBounds();
            }
            m_MemoGameBoard.Width = int.Parse(boardWidth);
            m_MemoGameBoard.Height = int.Parse(boardHeight);
            if (CheckValidBounds(m_MemoGameBoard.Width, m_MemoGameBoard.Height) != eErrorType.NoError)
            {
                PrintError(eErrorType.OutOfBounds);
                GetBoardBounds();
            }
            if (m_MemoGameBoard.CheckParityBounds() != eErrorType.NoError)
            {
                PrintError(eErrorType.OddSize);
                GetBoardBounds();
            }
        }
        public eErrorType CheckIfIntegers(string i_Width, string i_Height)
        {
            eErrorType errorType = eErrorType.NoError;
            if (!(int.TryParse(i_Width, out int boardWidth) && int.TryParse(i_Height, out int boardHeight)))
            {
                errorType = eErrorType.NotAnInteger;
            }
            return errorType;
        }
        public eErrorType CheckValidBounds(int i_Width, int i_Height)
        {
            eErrorType errorType = eErrorType.NoError;

            if (i_Width < k_MinBoardSize || i_Height < k_MinBoardSize ||
                i_Width > k_MaxBoardSize || i_Height > k_MaxBoardSize)
            {
                errorType = eErrorType.OutOfBounds;
            }

            return errorType;
        }

        public void PrintError(eErrorType i_ErrorType)
        {
            switch (i_ErrorType)
            {
                case eErrorType.FullCell:
                    Console.WriteLine("Try again. This cell is already full.");
                    break;
                case eErrorType.NoSuchCell:
                    Console.WriteLine("Try again. This is not a valid");
                    break;
                case eErrorType.NotAnInteger:
                    Console.WriteLine("Try again. You need to enter a number");
                    break;
                case eErrorType.OddSize:
                    Console.WriteLine("Try again. The board should be an even size.");
                    break;
                case eErrorType.OutOfBounds:
                    Console.WriteLine("Try again. You are out of bounds.");
                    break;
                case eErrorType.InvalidInput:
                    System.Console.WriteLine("Invalid input. Try again.");
                    break;
                case eErrorType.NotALetter:
                    System.Console.WriteLine("Invalid input. First character " +
                                       "should be a letter between A-Z. Try Again.");
                    break;

            }

        }

        public int GetAndCheckIfSecondPlayerCompOrHuman()
        {

            Console.WriteLine("Would you like to compete against another human " +
                               "or against the computer?");
            Console.WriteLine("(1) Computer");
            Console.WriteLine("(2) Human");
            int playerOrComp = int.Parse(Console.ReadLine());
            if (!IsOneOrTwoValidCheck(playerOrComp))
            {
                PrintError(eErrorType.InvalidInput);
                playerOrComp = GetAndCheckIfSecondPlayerCompOrHuman();
            }
            return playerOrComp;
        }

        public bool IsOneOrTwoValidCheck(int i_userInput)
        {
            return (i_userInput != 1 && i_userInput != 2);
        }

        public int AskIfThePlayerWantAnotherGame()
        {
            Console.WriteLine("Would you like to play another game?");
            Console.WriteLine("(1) yes");
            Console.WriteLine("(2) no");
            int anotherGame = int.Parse(Console.ReadLine());
            if (!IsOneOrTwoValidCheck(anotherGame))
            {
                PrintError(eErrorType.InvalidInput);
                anotherGame = AskIfThePlayerWantAnotherGame();
            }
            return anotherGame;
        }

        public int[] GetNextCard()
        {
            int[] cardValues = new int[1];
            Console.WriteLine("It's your turn!");
            Console.Write("Please enter the next card: ");
            string card = Console.ReadLine();
            if (!IsValidCard(card))
            {
                GetNextCard();
            }
            cardValues[0] = char.Parse(card[0].ToString()) - 'A';
            cardValues[1] = int.Parse(card[1].ToString());
            if (m_MemoGameBoard.IsCellIsValid(cardValues) != eErrorType.NoError)
            {
                GetNextCard();
            }
            return cardValues; 
        }
        public bool IsValidCard(string i_card)
        {
            bool isValid = true;
            if (i_card[0] < 'A' || i_card[0] > 'Z')
            {
                PrintError(eErrorType.NotALetter);
                isValid = false;
            }
            bool checkIfNumber = int.TryParse(i_card[1].ToString(), out int numberInCard);
            if (numberInCard < 1 && checkIfNumber)
            {
                PrintError(eErrorType.NotAnInteger);
                isValid = false;
            }
            return isValid;

        }

        public void RunGame()
        {
            m_FirstPlayer.Name = GetPlayerName();
            int ComputerOrHuman = GetAndCheckIfSecondPlayerCompOrHuman();

            if (ComputerOrHuman == (int)eGameConfig.Computer)
            {
                m_SecondPlayer.Name = "Computer";
            }
            else
            {
                m_SecondPlayer.Name = GetPlayerName();
            }

            GetBoardBounds();
            Console.WriteLine(m_MemoGameBoard.BuildBoard());
            m_FirstPlayer.FirstCard = GetNextCard();
            m_FirstPlayer.SecondCard = GetNextCard();
            char firstCardValue = m_MemoGameBoard.GetValueFromCellInBoard(m_FirstPlayer.FirstCard);
            char secondCardValue = m_MemoGameBoard.GetValueFromCellInBoard(m_FirstPlayer.SecondCard);
            m_GameLogic.CheckPlayerTurn(m_FirstPlayer,firstCardValue, secondCardValue);



        }
    }

}
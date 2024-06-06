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
        Player m_FirstPlayer = new Player();
        Player m_SecondPlayer = new Player();

        public Player FirstPlayer { get { return m_FirstPlayer; } set { FirstPlayer = value; } }
        public Player SecondPlayer { get { return m_SecondPlayer; } set { SecondPlayer = value; } }
        public GameBoard<char> GameBoard { get { return m_MemoGameBoard; } } 
        public string GetPlayerName()
        {
            Console.Write("Please enter your name: ");
            string playerName = Console.ReadLine();
            return playerName;
        }
        public void GetAndCheckBoardBounds()
        {
            Console.Write("Please enter the board width: ");
            string boardWidth = Console.ReadLine();
            Console.Write("Please enter the board height: ");
            string boardHeight = Console.ReadLine();
            if (CheckIfIntegers(boardWidth, boardHeight) != eErrorType.NoError)
            {
                PrintError(eErrorType.NotAnInteger);
                GetAndCheckBoardBounds();
            }
            m_MemoGameBoard.Width = int.Parse(boardWidth);
            m_MemoGameBoard.Height = int.Parse(boardHeight);
            if (CheckValidBounds(m_MemoGameBoard.Width, m_MemoGameBoard.Height) != eErrorType.NoError)
            {
                PrintError(eErrorType.OutOfBounds);
                GetAndCheckBoardBounds();
            }
            if (m_MemoGameBoard.CheckParityBounds() != eErrorType.NoError)
            {
                PrintError(eErrorType.OddSize);
                GetAndCheckBoardBounds();
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
        public int[] GetNextCard(string i_name)
        {
            int[] cardValues = new int[2];
            bool validInput = false;

            Console.WriteLine(i_name + " It's your turn!");
            while (!validInput)
            {
                Console.Write("Please enter the next card: ");
                string card = Console.ReadLine();

                if (IsValidCard(card))
                {
                    cardValues[0] = char.Parse(card[0].ToString()) - 'A';
                    cardValues[1] = int.Parse(card.Substring(1)) - 1;

                    eErrorType isCellValid = m_MemoGameBoard.IsCellIsValid(cardValues);
                    if (isCellValid == eErrorType.NoError)
                    {
                        validInput = true;
                    }
                    else
                    {
                        PrintError(isCellValid);
                    }
                }
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
            if (numberInCard < 1 || !checkIfNumber)
            {
                PrintError(eErrorType.NotAnInteger);
                isValid = false;
            }
            return isValid;

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
        public bool IsOneOrTwoValidCheck(int i_userInput)
        {
            return (i_userInput == 1 || i_userInput == 2);
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

        public void BuildBoard()
        {
            StringBuilder boardBase = new StringBuilder();
            boardBase.Append("    ");
            for (int i = 0; i < m_MemoGameBoard.Width; i++)
            {
                boardBase.Append((char)('A' + i) + "   ");
            }
            boardBase.AppendLine();
            boardBase.Append("  ");
            boardBase.Append('=', k_MinBoardSize * m_MemoGameBoard.Width + 1).AppendLine();

            for (int i = 0; i < m_MemoGameBoard.Height; i++)
            {
                boardBase.Append(i + 1 + " ");
                for (int j = 0; j < m_MemoGameBoard.Width; j++)
                {
                    boardBase.Append('|');
                    int[] card = { j, i };
                    if (m_MemoGameBoard.IsCellIsOpen(card))
                    {
                        boardBase.Append(" ");
                        boardBase.Append(m_MemoGameBoard.GetValueFromCellInBoard(card));
                        boardBase.Append(" ");
                    }
                    else
                    {
                        boardBase.Append("   ");
                    }
                }
                boardBase.Append('|');
                boardBase.AppendLine();
                boardBase.Append("  ");
                boardBase.Append('=', k_MinBoardSize * m_MemoGameBoard.Width + 1).AppendLine();
            }
            PrintPlayersScore();
            Console.WriteLine(boardBase.ToString());
            
        }

        public void PrintPlayersScore()
        {
            Console.Write("{0} score: {1}", FirstPlayer.Name, FirstPlayer.Score);
            Console.Write("     ");
            Console.WriteLine("{0} score: {1}", SecondPlayer.Name, SecondPlayer.Score);
        }

        public void PrintWinner()
        {
            PrintPlayersScore();
            if(FirstPlayer.Score > SecondPlayer.Score)
            {
                Console.WriteLine("Congrats! {0} you are the winner!", FirstPlayer.Name);
            }
            else if (FirstPlayer.Score < SecondPlayer.Score)
            {
                Console.WriteLine("Congrats! {0} you are the winner!", SecondPlayer.Name);
            }
            else
            {
                Console.WriteLine("Congrats for you both! Its a tie!");
            }
        }
    }


}
using System;
using System.Text;

namespace Exercise02
{
    public class UserInterfaceController
    {
        private const int k_MaxBoardSize = 6;
        private const int k_MinBoardSize = 4;
        char[] m_MatchingBoardValues;

        public char[] ShuffleCharValuesForTheBoard(GameBoard i_MemoGameBoard)
        {
            Random random = new Random();
            char[] BoardValues = new char[(i_MemoGameBoard.Width * i_MemoGameBoard.Height) / 2];

            for (int i = 0; i < BoardValues.Length; i++)
            {

                char nextLetter = (char)random.Next('A', 'Z');

                while (isCharEqualToCardInArr(BoardValues, i, nextLetter))
                {
                    nextLetter = (char)random.Next('A', 'Z');
                }

                BoardValues[i] = nextLetter;
            }

            return BoardValues;
        }

        private bool isCharEqualToCardInArr(char[] i_BoardValues, int i_CurrentSize, 
                                            char i_RandomChar)
        {
            bool isEqual = false;
            for (int j = 0; j < i_CurrentSize; j++)
            {
                if (i_BoardValues[j] == i_RandomChar)
                {
                    isEqual = true;
                }
            }
            return isEqual;
        }

        public void MatchLogicalValueToChar(GameBoard i_MemoGameBoard, char[] i_BoardValues)
        {
            int amountOfValues = (i_MemoGameBoard.Width * i_MemoGameBoard.Height) / 2;
            int valuesIndex = 0;
            m_MatchingBoardValues = new char[amountOfValues];

            for (int i = 0; i < i_MemoGameBoard.Height && valuesIndex < amountOfValues; i++)
            {
                for (int j = 0; j < i_MemoGameBoard.Width && valuesIndex < amountOfValues; j++)
                {
                    int[] card = { j, i };
                    int LogicalValueFromCellInBoard = i_MemoGameBoard.GetValueFromCellInBoard(card);

                    if (m_MatchingBoardValues[LogicalValueFromCellInBoard] == '\0')
                    {
                        m_MatchingBoardValues[LogicalValueFromCellInBoard] = i_BoardValues[valuesIndex];
                        valuesIndex++;
                    }
                }
            }
        }

        public string GetPlayerName()
        {
            Console.Write("Please enter your name: ");
            string playerName = Console.ReadLine();
            return playerName;
        }

        public void GetAndCheckBoardBounds(out int o_Width, out int o_Height)
        {
            o_Width = 0;
            o_Height = 0;
            bool validInput = false;

            while (!validInput)
            {
                Console.Write("Please enter the board width: ");
                string boardWidth = Console.ReadLine();
                Console.Write("Please enter the board height: ");
                string boardHeight = Console.ReadLine();

                if (checkIfIntegers(boardWidth, boardHeight))
                {
                    o_Width = int.Parse(boardWidth);
                    o_Height = int.Parse(boardHeight);
                    validInput = isValidBoardSize(o_Width, o_Height);
                }
            }
        }

        public int GetAndCheckIfSecondPlayerCompOrHuman()
        {
            Console.WriteLine("Would you like to compete against another human " +
                               "or against the computer?");
            Console.WriteLine("(1) Computer");
            Console.WriteLine("(2) Human");
            bool checkIfNumber = int.TryParse(Console.ReadLine().ToString(), out int playerOrComp);

            if (!isOneOrTwoValidCheck(playerOrComp) || !checkIfNumber)
            {
                PrintError(eErrorType.InvalidInput);
                playerOrComp = GetAndCheckIfSecondPlayerCompOrHuman();
            }

            return playerOrComp;
        }

        public int[] GetNextCard(GameBoard i_MemoGameBoard, string i_PlayerName)
        {
            int[] cardValues = new int[2];
            bool validInput = false;

            Console.WriteLine(i_PlayerName + " It's your turn!");
            while (!validInput)
            {
                Console.Write("Please enter the next card: ");
                string card = Console.ReadLine();

                if (card[0] == (char)eGameConfig.QuitGame || card[1] == (char)eGameConfig.QuitGame)
                {
                    Console.WriteLine("Quiting game!");
                    cardValues = null;
                    validInput = true;
                }
                else
                {
                    if (isValidCard(card))
                    {
                        cardValues[0] = char.Parse(card[0].ToString()) - 'A';
                        cardValues[1] = int.Parse(card.Substring(1)) - 1;

                        eErrorType isCellValid = i_MemoGameBoard.IsCellIsValid(cardValues);
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
            }

            return cardValues;
        }

        private bool isValidCard(string i_Card)
        {
            bool isValid = true;
            if (i_Card.Length != 2)
            {
                PrintError(eErrorType.InvalidInput);
                isValid = false;
            }
            else
            {
                if (i_Card[0] < 'A' || i_Card[0] > 'Z')
                {
                    PrintError(eErrorType.NotALetter);
                    isValid = false;
                }
                bool checkIfNumber = int.TryParse(i_Card[1].ToString(), out int numberInCard);
                if (numberInCard < 1 || !checkIfNumber)
                {
                    PrintError(eErrorType.NotAnInteger);
                    isValid = false;
                }
            }
            return isValid;

        }

        private bool checkIfIntegers(string i_Width, string i_Height)
        {
            bool areIntegers = int.TryParse(i_Width, out _) && int.TryParse(i_Height, out _);
            if (!areIntegers)
            {
                PrintError(eErrorType.NotAnInteger);
            }
            return areIntegers;
        }

        private bool isValidBoardSize(int i_Width, int i_Height)
        {
            bool isValidBoardSize = true;

            if (i_Width < k_MinBoardSize || i_Height < k_MinBoardSize
                || i_Width > k_MaxBoardSize || i_Height > k_MaxBoardSize)
            {
                isValidBoardSize = false;
                PrintError(eErrorType.OutOfBounds);
            }

            return isValidBoardSize;
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

        private bool isOneOrTwoValidCheck(int i_userInput)
        {
            return (i_userInput == 1 || i_userInput == 2);
        }

        public int AskIfThePlayerWantAnotherGame()
        {
            Console.WriteLine("Would you like to play another game?");
            Console.WriteLine("(1) yes");
            Console.WriteLine("(2) no");
            bool checkIfNumber = int.TryParse(Console.ReadLine().ToString(), out int anotherGame);

            if (!isOneOrTwoValidCheck(anotherGame) || !checkIfNumber)
            {
                PrintError(eErrorType.InvalidInput);
                anotherGame = AskIfThePlayerWantAnotherGame();
            }

            return anotherGame;
        }

        public void PrintBoard(GameBoard i_MemoGameBoard)
        {
            StringBuilder boardBase = new StringBuilder();
            boardBase.Append("    ");
            for (int i = 0; i < i_MemoGameBoard.Width; i++)
            {
                boardBase.Append((char)('A' + i) + "   ");
            }
            boardBase.AppendLine();
            boardBase.Append("  ");
            boardBase.Append('=', k_MinBoardSize * i_MemoGameBoard.Width + 1).AppendLine();

            for (int i = 0; i < i_MemoGameBoard.Height; i++)
            {
                boardBase.Append(i + 1 + " ");
                for (int j = 0; j < i_MemoGameBoard.Width; j++)
                {
                    boardBase.Append('|');
                    int[] card = { j, i };
                    if (i_MemoGameBoard.IsCellIsOpen(card))
                    {
                        boardBase.Append(" ");
                        int logicalValue = i_MemoGameBoard.GetValueFromCellInBoard(card);
                        boardBase.Append(m_MatchingBoardValues[logicalValue]);
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
                boardBase.Append('=', k_MinBoardSize * i_MemoGameBoard.Width + 1).AppendLine();
            }
            Console.WriteLine(boardBase.ToString());
        }

        public void PrintPlayersScore(Player i_FirstPlayer, Player i_SecondPlayer)
        {
            Console.Write("{0} score: {1}", i_FirstPlayer.Name, i_FirstPlayer.Score);
            Console.Write("     ");
            Console.WriteLine("{0} score: {1}", i_SecondPlayer.Name, i_SecondPlayer.Score);
        }

        public void PrintWinner(Player i_FirstPlayer, Player i_SecondPlayer)
        {
            PrintPlayersScore(i_FirstPlayer, i_SecondPlayer);
            if (i_FirstPlayer.Score > i_SecondPlayer.Score)
            {
                Console.WriteLine("Congrats! {0} you are the winner!", i_FirstPlayer.Name);
            }
            else if (i_FirstPlayer.Score < i_SecondPlayer.Score)
            {
                Console.WriteLine("Congrats! {0} you are the winner!", i_SecondPlayer.Name);
            }
            else
            {
                Console.WriteLine("Congrats for you both! Its a tie!");
            }
        }
    }
}
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
                while (IsCharEqualToCardInArr(BoardValues, i, nextLetter))
                {
                    nextLetter = (char)random.Next('A', 'Z');
                }
                BoardValues[i] = nextLetter;


            }
            return BoardValues;

        }

        public bool IsCharEqualToCardInArr(char[] i_BoardValues, int i_CurrentSize, char i_RandomChar)
        {
            bool isEqual = false;
            for(int j = 0; j < i_CurrentSize; j++)
            {
                if (i_BoardValues[j] == i_RandomChar)
                {
                    isEqual = true;
                }
            }
            return isEqual;
        }

        public void MatchLogicalValueToChar(GameBoard i_MemoGameBoard,char[] i_BoardValues)
        {
            int amountOfValues = (i_MemoGameBoard.Width * i_MemoGameBoard.Height) / 2;
            m_MatchingBoardValues = new char[amountOfValues];
            int valuesIndex = 0;
            for (int i = 0; i < i_MemoGameBoard.Height && valuesIndex < amountOfValues; i++)
            {
                for (int j = 0; j < i_MemoGameBoard.Width && valuesIndex < amountOfValues; j++)
                {
                    int[] card = { j, i };
                    int LogicalValueFromCellInBoard = i_MemoGameBoard.GetValueFromCellInBoard(card);
                    if (m_MatchingBoardValues[LogicalValueFromCellInBoard] == '\0' )
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
            Console.Write("Please enter the board width: ");
            string boardWidth = Console.ReadLine();
            Console.Write("Please enter the board height: ");
            string boardHeight = Console.ReadLine();
            if (CheckIfIntegers(boardWidth, boardHeight) != eErrorType.NoError)
            {
                PrintError(eErrorType.NotAnInteger);
                GetAndCheckBoardBounds(out o_Width,out o_Height);
            }
            o_Width = int.Parse(boardWidth);
            o_Height = int.Parse(boardHeight);
            if (CheckValidBounds(o_Width, o_Height) != eErrorType.NoError)
            {
                PrintError(eErrorType.OutOfBounds);
                GetAndCheckBoardBounds(out o_Width, out o_Height);
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
        public int[] GetNextCard(GameBoard i_MemoGameBoard,string i_name)
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
        public void PrintWinner(Player i_FirstPlayer,Player i_SecondPlayer)
        {
            PrintPlayersScore(i_FirstPlayer,i_SecondPlayer);
            if(i_FirstPlayer.Score > i_SecondPlayer.Score)
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
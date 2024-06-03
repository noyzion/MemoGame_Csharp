using Ex02;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace B24_Ex02_Noy_212198766_Dana_314652439
{
 
    public class GameBoard<T>
    {
        private const int k_MaxBoardSize = 6;
        private const int k_MinBoardSize = 4;
        private Card[,] m_GameMemoryBoard;
        private int m_Width;
        private int m_Height;
        private T[] m_ValuesForTheBoard;
        private int[] m_CounterValuesFromTheBoard;

        private class Card
        {
            private T m_CardValue;
            private bool m_IsCardOpen; //True = open, False = close

            public T CardValue { get { return m_CardValue; } set { m_CardValue = value; } }
            public bool IsCardOpen { get { return m_IsCardOpen; } set { m_IsCardOpen = value; } }

        }
        public void getValuesForTheBoard(T[] i_ValuesForTheBoard)
        {
            m_ValuesForTheBoard = i_ValuesForTheBoard;
            m_CounterValuesFromTheBoard = new int[m_Width * m_Height / 2];

        }
        public int Width { get { return m_Width; } }
        public int Height { get { return m_Height; } }
        public void setValidBounds(int i_Width, int i_Height)
        {
            m_Width = i_Width;
            m_Height = i_Height;
        }

        public (int height, int width) GameMemoryBoard
        {
            get { return (m_Height, m_Width); }
            set
            {
                m_Height = value.height;
                m_Width = value.width;
                m_GameMemoryBoard = new Card[value.height, value.width];
                FillBoardWithValues();
            }
        }

        public void FillBoardWithValues()
        {
            m_GameMemoryBoard = new Card[m_Height, m_Width];

            Random random = new Random();

            for (int i = 0; i < m_Height; i++)
            {
                for (int j = 0; j < m_Width; j++)
                {
                    int nextIndex = random.Next(m_ValuesForTheBoard.Length);
                    while (m_CounterValuesFromTheBoard[nextIndex] == 2)
                    {
                        nextIndex = random.Next(m_ValuesForTheBoard.Length);
                    }
                    m_CounterValuesFromTheBoard[nextIndex]++;
                    m_GameMemoryBoard[i, j] = new Card
                    {
                        CardValue = m_ValuesForTheBoard[nextIndex],
                        IsCardOpen = false
                    };
                }
            }
        }

        public bool CheckValidBounds(int i_Width, int i_Height, ref eErrorType o_Error)
        {
            bool isValid = true;
            if (i_Width < k_MinBoardSize || i_Height < k_MinBoardSize ||
                i_Width > k_MaxBoardSize || i_Height > k_MaxBoardSize)
            {
                o_Error = eErrorType.OutOfBounds;
                isValid = false;
            }
            if ((i_Width * i_Height) % 2 == 0)
            {
                o_Error = eErrorType.OddSize;
                isValid = false;
            }
            return isValid;
        }
        public void PrintBoard(T i_CardValue,int i_row,int i_col)
        {
            StringBuilder board = new StringBuilder();
            board.Append("    ");
            for (int i = 0; i < m_Width; i++)
            {
                board.Append((char)('A' + i) + "   ");
            }
            board.AppendLine();
            board.Append("  ");
            board.Append('=', k_MinBoardSize * m_Width + 1).AppendLine();

            for (int i = 1; i <= m_Height; i++)
            {
                board.Append(i + " ");
                for(int j=0; j<=m_Width; j++)
                {
                    board.Append('|');
                    board.Append("   ");
                }
                board.AppendLine();
                board.Append("  ");
                board.Append('=', k_MinBoardSize * m_Width + 1).AppendLine();
            }

            Console.WriteLine(board);

        }

        public eErrorType IsCellIsValid(int i_Row, int i_Col)
        {
            eErrorType errorType = eErrorType.NoError;
            if (!IsCellIsOpen(i_Row, i_Col))
            {
                errorType = eErrorType.FullCell;
            }
            else if (!IsCellIsInBounds(i_Row, i_Col))
            {
                errorType = eErrorType.NoSuchCell;
            }
            return errorType;

        }
        public bool IsCellIsOpen(int i_Row,int i_Col)
        {
            return m_GameMemoryBoard[i_Row, i_Col].IsCardOpen;
        }

        public bool IsCellIsInBounds(int i_Row, int i_Col)
        {
            char widthLetter = (char)('A' + m_Width);
            return i_Col < 'A' && i_Col > widthLetter && i_Row < 1 && i_Row > m_Height;
        }
    }
}
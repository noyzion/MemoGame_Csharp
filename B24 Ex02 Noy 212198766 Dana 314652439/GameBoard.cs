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

        private int k_LengthOfSeprator = 4;
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

        public eErrorType CheckParityBounds()
        {
            eErrorType error = eErrorType.NoError;
            if ((m_Width * m_Height) % 2 != 0)
            {
                error = eErrorType.OddSize;
            }
            return error;
        }
        public void GetValuesForTheBoard(T[] i_ValuesForTheBoard)
        {
            m_ValuesForTheBoard = i_ValuesForTheBoard;
            m_CounterValuesFromTheBoard = new int[m_Width * m_Height / 2];

        }
        public int Width { get { return m_Width; } set { m_Width = value; } }
        public int Height { get { return m_Height; } set { m_Height = value; } }
        public (int height, int width) GameMemoryBoard
        {
            get { return (m_Height, m_Width); }
            set
            {
                m_Height = value.height;
                m_Width = value.width;
                m_GameMemoryBoard = new Card[value.height, value.width];
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

        public void UpdateBoard(int[] i_Card ,bool i_IsCardOpen)
        {
            m_GameMemoryBoard[i_Card[1], i_Card[0]].IsCardOpen = i_IsCardOpen;
        }
        public StringBuilder BuildBoard()
        {
            StringBuilder boardBase = new StringBuilder();
            boardBase.Append("    ");
            for (int i = 0; i < m_Width; i++)
            {
                boardBase.Append((char)('A' + i) + "   ");
            }
            boardBase.AppendLine();
            boardBase.Append("  ");
            boardBase.Append('=', k_LengthOfSeprator * m_Width + 1).AppendLine();

            for (int i = 0; i < m_Height; i++)
            {
                boardBase.Append(i + 1 + " ");
                for (int j = 0; j < m_Width; j++)
                {
                    boardBase.Append('|');
                    if (m_GameMemoryBoard[i, j].IsCardOpen)
                    {
                        boardBase.Append(" ");
                        boardBase.Append(m_GameMemoryBoard[i, j].CardValue);
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
                boardBase.Append('=', k_LengthOfSeprator * m_Width + 1).AppendLine();
            }
            return boardBase;
        }

        public eErrorType IsCellIsValid(int[] i_Card)
        {
            eErrorType errorType = eErrorType.NoError;
            if  (!IsCellIsInBounds(i_Card))
            {
                errorType = eErrorType.OutOfBounds;
            }
            else if (!IsCellIsOpen(i_Card))
            {
                errorType = eErrorType.NoSuchCell;
            }
            return errorType;
        }

        public bool IsCellIsOpen(int[] i_Card)
        {
            return m_GameMemoryBoard[i_Card[1], i_Card[0]].IsCardOpen;
        }

        public bool IsCellIsInBounds(int[] i_Card)
        {
            char widthLetter = (char)('A' + m_Width);
            return i_Card[0] < 'A' && i_Card[0] > widthLetter && i_Card[1] < 1 && i_Card[1] > m_Height;
        }


        public T GetValueFromCellInBoard(int[] i_Card)
        {
            return m_GameMemoryBoard[i_Card[1], i_Card[0]].CardValue;
        }

        public eGameConfig IsBoardFull()
        {
            eGameConfig isBoardFull = eGameConfig.EndGame;
            for (int i = 0; i < m_Height; i++)
            {
                for (int j = 0; j < m_Width; j++)
                {
                    if (!m_GameMemoryBoard[i, j].IsCardOpen)
                    {
                        isBoardFull = eGameConfig.CountinueGame;
                    }
                }
            }
            return isBoardFull;
        }
    }
}
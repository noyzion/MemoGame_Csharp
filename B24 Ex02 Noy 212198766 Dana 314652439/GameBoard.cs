using System;

namespace Exercise02
{
    public class GameBoard
    {
        private Cell[,] m_GameMemoryBoard;
        private int m_Width;
        private int m_Height;

        private struct Cell
        {
            private int m_LogivalValue;
            private bool m_IsCardOpen; //True = open, False = close

            public int CardValue { get { return m_LogivalValue; } set { m_LogivalValue = value; } }

            public bool IsCardOpen { get { return m_IsCardOpen; } set { m_IsCardOpen = value; } }
        }

        public int Width { get { return m_Width; } set { m_Width = value; } }
        public int Height { get { return m_Height; } set { m_Height = value; } }
        public (int i_Height, int i_Width) GameMemoryBoard
        {
            get { return (m_Height, m_Width); }
            set
            {
                m_Height = value.i_Height;
                m_Width = value.i_Width;
                m_GameMemoryBoard = new Cell[value.i_Height, value.i_Width];
            }
        }

        public GameBoard(int i_Width, int i_Height)
        {
            m_Width = i_Width;
            m_Height = i_Height;
            if (CheckParityBounds() == eErrorType.NoError)
            {
                m_GameMemoryBoard = new Cell[Height, Width];
                FillBoardWithLogicValues();
            }
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

        public void FillBoardWithLogicValues()
        {
            int[] counterValuesFromTheBoard = new int[m_Height * m_Width / 2];
            Random random = new Random();

            for (int i = 0; i < m_Height; i++)
            {
                for (int j = 0; j < m_Width; j++)
                {
                    int nextValue = random.Next(counterValuesFromTheBoard.Length);

                    while (counterValuesFromTheBoard[nextValue] == 2)
                    {
                        nextValue = random.Next(counterValuesFromTheBoard.Length);
                    }

                    counterValuesFromTheBoard[nextValue]++;
                    m_GameMemoryBoard[i, j] = new Cell
                    {
                        CardValue = nextValue,
                        IsCardOpen = false
                    };
                }
            }
        }

        public void UpdateBoard(int[] i_Card, bool i_IsCardOpen)
        {
            m_GameMemoryBoard[i_Card[1], i_Card[0]].IsCardOpen = i_IsCardOpen;
        }

        public eErrorType IsCellIsValid(int[] i_Card)
        {
            eErrorType errorType = eErrorType.NoError;

            if (IsCellNotInBounds(i_Card))
            {
                errorType = eErrorType.OutOfBounds;
            }
            else if (IsCellIsOpen(i_Card))
            {
                errorType = eErrorType.NoSuchCell;
            }

            return errorType;
        }

        public bool IsCellIsOpen(int[] i_Card)
        {
            return m_GameMemoryBoard[i_Card[1], i_Card[0]].IsCardOpen;
        }

        public bool IsCellNotInBounds(int[] i_Card)
        {
            return (i_Card[0] < 0 || i_Card[0] >= m_Width ||
                    i_Card[1] < 0 || i_Card[1] >= m_Height);
        }

        public int GetValueFromCellInBoard(int[] i_Card)
        {
            return m_GameMemoryBoard[i_Card[1], i_Card[0]].CardValue;
        }

        public eGameConfig IsBoardFull()
        {
            eGameConfig isBoardFull = eGameConfig.BoardFull;

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

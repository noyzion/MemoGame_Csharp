

using System;
using System.Collections.Generic;


namespace Exercise02
{
    public class Player
    {
        private string m_Name;
        private int m_Score;
        private bool m_IsMyTurn;
        private int[] m_FirstCard = new int[2];
        private int[] m_SecondCard = new int[2];

        public string Name { get { return m_Name; } set { m_Name = value; } }
        public int Score { get { return m_Score; } set { m_Score = value; } }
        public bool IsMyTurn { get { return m_IsMyTurn; } set { m_IsMyTurn = value; } }
        public int[] FirstCard { get { return m_FirstCard; } set { m_FirstCard = value; } }
        public int[] SecondCard { get { return m_SecondCard; } set { m_SecondCard = value; } }
        public void ComputerChooseCards(GameBoard i_Board,
                                        GameLogic i_gameLogic)
        {
            int rememberListIndex = 0;
            bool foundCard = false;
            while (rememberListIndex < i_gameLogic.RememberValues.Capacity && !foundCard)
            {
                if (i_gameLogic.RememberValues[rememberListIndex] != null)
                {
                    if (i_gameLogic.RememberValues[rememberListIndex].HowManyTimesOpened == 2)
                    {
                        FirstCard = i_gameLogic.RememberValues[rememberListIndex].CardOneCoordinate;
                        SecondCard = i_gameLogic.RememberValues[rememberListIndex].CardTwoCoordinate;
                        foundCard = true;
                    }
                }
                rememberListIndex++;
            }
            if (!foundCard)
            {
                Random random = new Random();
                
                //error - fixed we need to random from the bounds
                m_FirstCard[0] = random.Next(i_Board.Height);
                m_FirstCard[1] = random.Next(i_Board.Width);
                while (i_Board.IsCellIsOpen(m_FirstCard))
                {
                    m_FirstCard[0] = random.Next(i_Board.Height);
                    m_FirstCard[1] = random.Next(i_Board.Width);
                }
                i_gameLogic.UpdateRememberList(m_FirstCard, i_Board.GetValueFromCellInBoard(m_FirstCard));


                if (i_gameLogic.RememberValues[i_Board.GetValueFromCellInBoard(m_FirstCard)].HowManyTimesOpened == 2)
                {
                    SecondCard = i_gameLogic.RememberValues[i_Board.GetValueFromCellInBoard(m_FirstCard)].CardTwoCoordinate;
                    i_gameLogic.RememberValues[i_Board.GetValueFromCellInBoard(m_FirstCard)].HowManyTimesOpened++; //update to 3 - means both opene
                }
                else
                {
                    m_SecondCard[0] = random.Next(i_Board.Height);
                    m_SecondCard[1] = random.Next(i_Board.Width);
                    while (i_Board.IsCellIsOpen(m_SecondCard))
                    {
                        m_SecondCard[0] = random.Next(i_Board.Height);
                        m_SecondCard[1] = random.Next(i_Board.Width);
                    }
                    i_gameLogic.UpdateRememberList(m_SecondCard, i_Board.GetValueFromCellInBoard(m_SecondCard));

                }
            }
        }

    }
}
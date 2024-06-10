

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
        public void ComputerChooseCards(GameBoard i_Board, GameLogic gameLogic)
        {
            bool foundCards = false;
            foreach (KeyValuePair<int, List<int[]>> pair in gameLogic.RememberValues)
            {
                if (pair.Value.Count == 2 && !foundCards)
                {
                    m_FirstCard = gameLogic.RememberValues[pair.Key][0];
                    m_SecondCard = gameLogic.RememberValues[pair.Key][1];
                    foundCards = true;
                }
            }
            if (!foundCards)
            {
                chooseRandomCard(i_Board, m_FirstCard);
                 gameLogic.UpdateRememberValues(m_FirstCard, i_Board.GetValueFromCellInBoard(m_FirstCard));
                if (gameLogic.RememberValues[i_Board.GetValueFromCellInBoard(m_FirstCard)].Count == 2)
                {
                    m_SecondCard = gameLogic.RememberValues[i_Board.GetValueFromCellInBoard(m_FirstCard)][0];
                }
                else
                {
                    chooseRandomCard(i_Board, m_SecondCard);
                    while (m_SecondCard[0] == m_FirstCard[0] && m_SecondCard[1] == m_FirstCard[1])
                    {
                        chooseRandomCard(i_Board, m_SecondCard);
                    }
                    gameLogic.UpdateRememberValues(m_SecondCard, i_Board.GetValueFromCellInBoard(m_SecondCard));
                }
            }
        }


        private void chooseRandomCard(GameBoard i_Board, int[] i_Card)
        {
            Random random = new Random();
            bool isOpenCell = true;
            while (isOpenCell)
            {
                i_Card[0] = random.Next(i_Board.Width);
                i_Card[1] = random.Next(i_Board.Height);
                isOpenCell = i_Board.IsCellIsOpen(i_Card);
            }
        }
    }
}

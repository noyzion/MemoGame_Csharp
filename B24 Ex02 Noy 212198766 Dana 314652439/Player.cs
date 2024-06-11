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

        public void ComputerChooseCards(GameBoard i_Board, GameLogic i_GameLogic)
        {
            bool foundCards = findMatchingCards(i_GameLogic);

            if (!foundCards)
            {
                chooseRandomCards(i_Board);
                int logicalValueFirstCard = i_Board.GetValueFromCellInBoard(m_FirstCard);

                i_GameLogic.UpdateRememberValues(m_FirstCard, logicalValueFirstCard);
                if (i_GameLogic.RememberValues[logicalValueFirstCard].Count == 2)
                {
                    m_SecondCard = i_GameLogic.RememberValues[logicalValueFirstCard][0];
                }
                else
                {
                    chooseRandomDifferentCard(i_Board, i_GameLogic);
                    int logicalValueSecondCard = i_Board.GetValueFromCellInBoard(m_SecondCard);

                    i_GameLogic.UpdateRememberValues(m_SecondCard, logicalValueSecondCard);
                }
            }
        }

        private bool findMatchingCards(GameLogic i_GameLogic)
        {
            foreach (KeyValuePair<int, List<int[]>> pair in i_GameLogic.RememberValues)
            {
                if (pair.Value.Count == 2)
                {
                    m_FirstCard = pair.Value[0];
                    m_SecondCard = pair.Value[1];
                    return true;
                }
            }

            return false;
        }

        private void chooseRandomCards(GameBoard i_Board)
        {
            chooseRandomCard(i_Board, m_FirstCard);
        }

        private void chooseRandomDifferentCard(GameBoard i_Board, GameLogic i_GameLogic)
        {
            bool isCardExist = true;

            chooseRandomCard(i_Board, m_SecondCard);
            while (checkIfCoordinateAreEqual(m_FirstCard, m_SecondCard)
                || isCardExist)
            {
                chooseRandomCard(i_Board, m_SecondCard);
                int logicalValue = i_Board.GetValueFromCellInBoard(m_SecondCard);

                if (i_GameLogic.RememberValues.ContainsKey(logicalValue))
                {
                    isCardExist = i_GameLogic.CardExistsInList(m_SecondCard,
                    i_GameLogic.RememberValues[logicalValue]);
                }
            }
        }

        private bool checkIfCoordinateAreEqual(int[] i_CardOne, int[] i_CardTwo)
        {
            return i_CardOne[0] == i_CardTwo[0] && i_CardOne[1] == i_CardTwo[1];
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

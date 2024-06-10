using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Exercise02
{
    public class GameLogic
    {
        private Dictionary<int, List<int[]>> m_RememberValues = new Dictionary<int, List<int[]>>();

        public Dictionary<int, List<int[]>>  RememberValues { get { return m_RememberValues; } }

        public void UpdateRememberValues( int[] i_Card, int i_LogicalValue)
        {
            bool cardExist = false;

            if (m_RememberValues.ContainsKey(i_LogicalValue))
            {
                List<int[]> cardList = m_RememberValues[i_LogicalValue];
                foreach (int[] existingCard in cardList)
                {
                    if (!cardExist)
                    {
                        cardExist = (existingCard[0] == i_Card[0] && existingCard[1] == i_Card[1]);
                    }
                }
                if (!cardExist)
                {
                    cardList.Add(i_Card);
                }
            }
            else
            {
                List<int[]> cardCoordinates = new List<int[]>();
                cardCoordinates.Add(i_Card);
                m_RememberValues.Add(i_LogicalValue, cardCoordinates);
            }

        }

        public void CheckCardsAndReplaceTurn(Player i_Player, GameBoard i_Board)
        {
            int logicalValueFirstCard = i_Board.GetValueFromCellInBoard(i_Player.FirstCard);
            UpdateRememberValues(i_Player.FirstCard, logicalValueFirstCard);

            int logicalValueSecondCard = i_Board.GetValueFromCellInBoard(i_Player.SecondCard);
            UpdateRememberValues(i_Player.SecondCard, logicalValueSecondCard);

            if(logicalValueFirstCard == logicalValueSecondCard)
            {
                m_RememberValues.Remove(logicalValueSecondCard);
                i_Player.IsMyTurn = true;
                i_Player.Score++;
            }
            else
            {
                i_Player.IsMyTurn = false;

            }

        }
    }
}

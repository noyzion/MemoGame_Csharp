using System.Collections.Generic;

namespace Exercise02
{
    public class GameLogic
    {
        private List<RememberValue> m_RememberValues = new List<RememberValue>();
        public class RememberValue
        {
            private int[] m_CardOneCoordinate = new int[2];
            private int[] m_CardTwoCoordinate = new int[2];
            private int m_HowManyTimesOpened;

            public RememberValue(int[] i_CardOneCoordinate, int[] i_CardTwoCoordinate, int i_HowManyTimesOpened)
            {
                m_CardOneCoordinate = i_CardOneCoordinate;
                m_CardTwoCoordinate = i_CardTwoCoordinate;
                m_HowManyTimesOpened = i_HowManyTimesOpened;
            }
            public int[] CardOneCoordinate { get { return m_CardOneCoordinate; } set { m_CardOneCoordinate = value; } }
            public int[] CardTwoCoordinate { get { return m_CardTwoCoordinate; } set { m_CardTwoCoordinate = value; } }
            public int HowManyTimesOpened { get { return m_HowManyTimesOpened; } set { m_HowManyTimesOpened = value; } }
        }

        public List<RememberValue> RememberValues { get { return m_RememberValues; } }
        public void UpdateRememberList(int[] i_Card, int i_logicalValueCard)
        {

            if (m_RememberValues.Capacity <= i_logicalValueCard)
            {
                m_RememberValues.Capacity = i_logicalValueCard + 1;
                while (m_RememberValues.Count <= i_logicalValueCard)
                {
                    m_RememberValues.Add(null);
                }
                RememberValue rememberValue = new RememberValue(i_Card, null, 1);
                m_RememberValues[i_logicalValueCard] = rememberValue;
            }
            else
            {
                if (m_RememberValues[i_logicalValueCard] == null)
                {
                    RememberValue rememberValue = new RememberValue(i_Card, null, 1);
                    m_RememberValues[i_logicalValueCard] = rememberValue;

                }
                else if (m_RememberValues[i_logicalValueCard].HowManyTimesOpened == 1)
                {
                    if (m_RememberValues[i_logicalValueCard].CardOneCoordinate[0] != i_Card[0]
                        || m_RememberValues[i_logicalValueCard].CardOneCoordinate[1] != i_Card[1])
                    {
                        m_RememberValues[i_logicalValueCard].HowManyTimesOpened++;
                        m_RememberValues[i_logicalValueCard].CardTwoCoordinate = i_Card;
                    }
                }

            }
        }


        public void CheckCardsAndReplaceTurn(GameBoard i_MemoGameBoard, Player i_Player)
        {
            int logicalValueFirstCard = i_MemoGameBoard.GetValueFromCellInBoard(i_Player.FirstCard);
            UpdateRememberList(i_Player.FirstCard, logicalValueFirstCard);

            int logicalValueSecondCard = i_MemoGameBoard.GetValueFromCellInBoard(i_Player.SecondCard);
            UpdateRememberList(i_Player.SecondCard, logicalValueSecondCard);

            if (logicalValueFirstCard == logicalValueSecondCard)
            {
                RememberValues[logicalValueFirstCard].HowManyTimesOpened++; //updated to 3
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

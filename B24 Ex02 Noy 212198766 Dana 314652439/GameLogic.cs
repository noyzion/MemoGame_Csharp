


namespace Exercise02
{
    internal class GameLogic
    {

        public void CheckCardsAndReplaceTurn(GameBoard i_MemoGameBoard, Player i_Player)
        {
            int logicalValueFirstCard = i_MemoGameBoard.GetValueFromCellInBoard(i_Player.FirstCard);
            int logicalValueSecondCard = i_MemoGameBoard.GetValueFromCellInBoard(i_Player.SecondCard);

            if (logicalValueFirstCard == logicalValueSecondCard)
            {
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex02
{
    public class UserInterfaceController
    {
        public string GetPlayerName()
        {
            Console.WriteLine("Please enter your name: ");
            string playerName = Console.ReadLine();
            return playerName;
        }

        public int GetIsSecondPlayerCompOrHuman()
        {
            Console.WriteLine("Would you like to compete against another human " +
                               "player or against the computer?");
            Console.WriteLine("If your choice is human, please press 1");
            Console.WriteLine("If your choice is computer, please press 2");
            int playerOrComp = int.Parse(Console.ReadLine());
                if (!IsOneOrTwoValidCheck(playerOrComp))
                {
                    System.Console.WriteLine("Invalid input. Try again.");
                playerOrComp = GetIsSecondPlayerCompOrHuman();
                }
            return playerOrComp;
        }

        public bool IsOneOrTwoValidCheck(int i_userInput)
        {
            return (i_userInput != 1 && i_userInput != 2);
        }


        public int AskIfThePlayerWantAnotherGame()
        {
            Console.WriteLine("Would you like to play another game?");
            Console.WriteLine("If your choice is yes, please press 1");
            Console.WriteLine("If your choice is no, please press 2");
            int anotherGame = int.Parse(Console.ReadLine());
            if (!IsOneOrTwoValidCheck(anotherGame))
            {
                System.Console.WriteLine("Invalid input. Try again.");
                anotherGame = GetIsSecondPlayerCompOrHuman();
            }
            return anotherGame;
        }


        public void GetNextCard(out char o_Column, out int o_Row)
        {
            Console.WriteLine("It's your turn!");
            Console.Write("Please enter the next card: ");
            string card = Console.ReadLine();
            if (!IsValidCard(card))
            {
                GetNextCard(out o_Column,out o_Row);
            }
            o_Column = card[0];
            o_Row = int.Parse(card[1].ToString());
        }

        public bool IsValidCard(string i_card)
        {
            bool isValid = true;
            if (i_card[0] < 'A' || i_card[0] > 'Z')
            {
                System.Console.WriteLine("Invalid input. First character " +
                                         "should be a letter between A-Z. Try Again.");

                isValid = false;
            }
            bool checkIfNumber = int.TryParse(i_card[1].ToString(), out int numberInCard);
            if (numberInCard < 1 && checkIfNumber)
            {
                System.Console.WriteLine("Invalid input. First character should be a" +
                                         "positive number. Try Again.");
                isValid = false;
            }
            return isValid;

        }
    }
}

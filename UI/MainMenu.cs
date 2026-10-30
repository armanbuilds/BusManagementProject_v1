using System;
using System.Collections.Generic;
using System.Text;

namespace Project_02_nov.UI
{
    internal class MainMenu
    {

        public static int DisplayMainMenu()
        {
            ConsoleUtility.DisplayIntroHeader();
            Console.WriteLine("********************** USER MENU ********************");
            Console.WriteLine("1. Admin Menu");
            Console.WriteLine("2. Passenger Menu");
            Console.WriteLine("3. Exit");
            Console.Write("Choose any option: ");


            int choice;
            if (int.TryParse(Console.ReadLine(), out choice))
            {
                return choice;
            }
            return -1;


        }
    }
}

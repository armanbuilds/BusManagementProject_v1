using System;
using System.Collections.Generic;
using System.Text;

namespace Project_02_nov.UI
{
    internal class ConsoleUtility
    {

        public static void DisplayIntroHeader()
        {
            Console.WriteLine("-------------------------------------------------------------");
            Console.WriteLine("------------------ BUSES MANAGEMENT SYSTEM ------------------");
            Console.WriteLine("-------------------------------------------------------------");
        }


        public static void ClearScreen()
        {
            Console.Clear();
        }

        public static void PauseScreen()
        {
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        public static void ErrorMessage()
        {
            Console.WriteLine("INVALID OPTION !");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();

        }
    }
}

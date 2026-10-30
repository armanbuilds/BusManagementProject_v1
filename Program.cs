using Project_02_nov.UI;
using System.Xml.Serialization;

namespace Project_02_nov
{
    internal class Program
    {
        static void Main(string[] args)
        {
           while(true)
            {
                ConsoleUtility.ClearScreen ();
                int choice = MainMenu.DisplayMainMenu();

                switch(choice)
                {
                    case 1:
                        AdminUi.DisplayAdminMenu();
                        break;

                    case 2:
                        PassengerUi.DisplayPassengerMenu();
                        break;


                    case 3:
                        Console.WriteLine("EXITING THE SYSTEM!.THANKS FOR USING THIS SOFTWARE");
                        return;

                    default:
                        ConsoleUtility.ErrorMessage();
                        break;



                }

            }

           
        }
    }
}

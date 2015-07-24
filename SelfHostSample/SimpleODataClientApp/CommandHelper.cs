using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleODataClientApp
{
    public class CommandHelper
    {
        public static void ShowMenu()
        {
            Console.Clear();
            Console.WriteLine("**********************************");
            Console.WriteLine("[1]. Query Entities");
            Console.WriteLine("[2]. Query Single Entity");
            Console.WriteLine("[3]. Query Single Entity with navigation property");
            Console.WriteLine("[4]. Add a link");
           // Console.WriteLine("[5]. Delete a link");
            Console.WriteLine("[9]. Quit");
            Console.WriteLine("**********************************");
        }

        public static int GetUserInput()
        {
            ShowMenu();
            string input = Console.ReadLine();
            int a;
            if (int.TryParse(input, out a))
            {
                return a;
            }

            Console.WriteLine("[Wrong] Invalid input.");
            return -1;
        }
    }
}

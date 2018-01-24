using Microsoft.Owin.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ODataOWin
{
    class Program
    {
        static void Main(string[] args)
        {
            using (WebApp.Start<Startup1>("http://localhost:12345"))
            {
                Console.WriteLine("Query at http://localhost:12345 \n" +
                    "Press [enter] to quit ...");
                Console.ReadLine();
            }
        }
    }
}

using Microsoft.Owin.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApi7xOWin
{
    class Program
    {
        static void Main(string[] args)
        {
            string baseUri = "http://localhost:53234";

            using (WebApp.Start<Startup>(url: baseUri))
            {
                Console.WriteLine("Service is running http://localhost:53234. Press any key to quit ....");
                Console.ReadKey();
            }
        }
    }
}

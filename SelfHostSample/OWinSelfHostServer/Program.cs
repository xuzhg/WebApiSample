using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Owin.Hosting;

namespace OWinSelfHostServer
{
    class Program
    {
        static void Main(string[] args)
        {
            string baseUri = "http://localhost:54321";

            using (WebApp.Start<Startup>(url: baseUri))
            {
                Console.WriteLine("Service is running http://localhost:54321. Press any key to quit....");
                Console.ReadKey();
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Owin.Hosting;

namespace ReadEdmxSelfHostOWin
{
    class Program
    {
        static void Main(string[] args)
        {
            string baseUri = "http://localhost:12345";

            using (WebApp.Start<StartUp>(url: baseUri))
            {
                Console.WriteLine("Service is running http://localhost:12345. Press any key to quit....");
                Console.ReadKey();
            }
        }
    }
}

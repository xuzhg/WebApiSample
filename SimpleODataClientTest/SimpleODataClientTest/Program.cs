using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simple.OData.Client;

namespace SimpleODataClientTest
{
    class Program
    {
        static void Main(string[] args)
        {
            QueryPeople();

            Console.WriteLine("OK");
            Console.ReadKey();
        }

        private static async void QueryPeople()
        {
            var client = new ODataClient("http://services.odata.org/v4/TripPinServiceRW/");
            var people = await client.For<People>().FindEntriesAsync();

            int itemIndex= 1;
            foreach (var p in people)
            {
                Console.WriteLine("People[" + itemIndex++ + "]:");
                Console.WriteLine("\tFirstName: " + p.FirstName);
                Console.WriteLine("\tLastName: " + p.LastName);
                Console.Write("\tEmails: { ");
                Console.Write(String.Join(",", p.Emails));
                Console.WriteLine(" }");
            }

            Console.WriteLine("==> Finished");

/* it returns:
OK
People[1]:
        FirstName: Russell
        LastName: Whyte
        Emails: { Russell@example.com,Russell@contoso.com }
People[2]:
        FirstName: Scott
        LastName: Ketchum
        Emails: { Scott@example.com }
People[3]:
        FirstName: Ronald
        LastName: Mundy
        Emails: { Ronald@example.com,Ronald@contoso.com }
People[4]:
        FirstName: Javier
        LastName: Alfred
        Emails: { Javier@example.com,Javier@contoso.com }
People[5]:
        FirstName: Willie
        LastName: Ashmore
        Emails: { Willie@example.com,Willie@contoso.com }
People[6]:
        FirstName: Vincent
        LastName: Calabrese
        Emails: { Vincent@example.com,Vincent@contoso.com }
People[7]:
        FirstName: Clyde
        LastName: Guess
        Emails: { Clyde@example.com }
People[8]:
        FirstName: Keith
        LastName: Pinckney
        Emails: { Keith@example.com,Keith@contoso.com }
==> Finished
*/
        }
    }
}

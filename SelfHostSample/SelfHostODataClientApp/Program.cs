using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SelfHostODataClientApp.Default;
using SelfHostODataClientApp.SelfHostServer.Models;

namespace SelfHostODataClientApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Container container = new Container(new Uri("http://localhost.fiddler:12345/odata/"));

            var customers = container.Customers.ToList();

            foreach (var customer in customers)
            {
                Console.WriteLine(FormatCustomer(customer));
            }

            // call function
            Console.WriteLine("Call function: ");
            var result = container.Customers.ByKey(1).RefreshCustomer().Expand("Category");
            var customer1 = result.GetValueAsync().Result;
            // var customer1 = result.GetValue();
            Console.WriteLine(FormatCustomer(customer1));

            result = container.Customers.ByKey(2).RefreshCustomer2().Expand("Category");
            customer1 = result.GetValueAsync().Result;
            Console.WriteLine(FormatCustomer(customer1));

            Console.ReadKey();
        }

        private static string FormatCustomer(Customer customer)
        {
            return "" + customer.CustomerId + ":" + customer.CustomerName + " likes " + customer.FavoriteColor +
                   " living in " + FormatAddress(customer.Location);
        }

        private static string FormatAddress(Address address)
        {
            return "{ " + address.Country + "," + address.City + " }";
        }
    }
}

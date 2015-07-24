using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SelfHostServer.Models;
using Simple.OData.Client;

namespace SimpleODataClientApp
{
    // some test codes, back up here.
    class SimpleClientTest
    {
        private static string _baseUri = "http://localhost:12345/odata/";

        // Be caution when you are using async/await to call the 
        // 1. QueryCustomersTyped(...)
        // 2. QueryCustomersUnTyped(...)
        // it maybe failed because you are using the same client.
        private static /*async*/ void QueryCustomersUnTyped(ODataClient client)
        {
            // "Un-typed syntax"
            Console.WriteLine("\n[Un-typed Syntax]");
            var customers = /*await*/ client.For("Customers").FindEntriesAsync().Result;

            // customers is a instance of WhereSelectListIterator, a list
            int n = 1;
            string indent = "";
            foreach (var customer in customers)
            {
                PrintItems("Customer #" + n++, indent, customer);
            }
        }

        private static /*async*/ void QueryCustomersTyped(ODataClient client)
        {
            // if you use async/await, please create a new client.
            //var newclient = new ODataClient("http://localhost:12345/odata/");

            // "typed syntax"
            Console.WriteLine("\n[Typed Syntax]");
            var customers = /*await*/ client.For<Customer>().FindEntriesAsync().Result;

            foreach (var customer in customers)
            {
                Console.WriteLine("{0}: {1} likes {2}", customer.CustomerId, customer.CustomerName, customer.FavoriteColor);
            }
        }

        private static async void QuerySingleCustomer(int customerId)
        {
            Console.WriteLine("\n[UnTyped Syntax - Query single customer]");

            var client = new ODataClient(_baseUri);

            var customer = await client.For("Customers").Key(customerId).FindEntryAsync();

            PrintItems("customer #" + customerId, "", customer);
        }

        private static async void QueryCustomersTypedWithExpand()
        {
            var client = new ODataClient(_baseUri);

            var customers = await client.For<Customer>().Expand(x => x.Category).FindEntriesAsync();

            foreach (var customer in customers)
            {
                Console.Write("{0}: {1} likes {2}", customer.CustomerId, customer.CustomerName, customer.FavoriteColor);

                if (customer.Category != null)
                {
                    Console.Write(", Category ( {0}, {1} )", customer.Category.CategoryId, customer.Category.CategoryType);
                }

                Console.WriteLine();
            }
        }

        private static /*async*/ void AddLinkEntryTest()
        {
            int customerId = 2;

            Category newCategory = new Category();
            newCategory.CategoryId = 888;
            newCategory.CategoryType = "Manager";

            var client = new ODataClient(_baseUri);
            /*await*/
            client.For<Customer>().Key(customerId).LinkEntryAsync(x => x.Category, newCategory);
        }

        private static void PrintItems(string name, string indent, IDictionary<string, object> dics)
        {
            Console.WriteLine(indent + "Properties of : " + name);
            indent += "    ";
            foreach (var dic in dics)
            {
                if (dic.Value is IDictionary<string, object>)
                {
                    PrintItems(dic.Key, indent, dic.Value as IDictionary<string, object>);
                }
                else
                {
                    Console.WriteLine(indent + dic.Key + " : " + dic.Value);
                }
            }
        }
    }
}

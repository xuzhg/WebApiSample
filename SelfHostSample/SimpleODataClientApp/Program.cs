using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Simple.OData.Client;
using SelfHostServer.Models;

namespace SimpleODataClientApp
{
    class Program
    {
        private static string _baseUri = "http://localhost:12345/odata/";
        static void Main(string[] args)
        {
            bool quit = false;
            for(;!quit;)
            {
                int input = CommandHelper.GetUserInput();
                switch (input)
                {
                    case 1: // Query Entities
                        QueryEntitySet();
                        break;

                    case 2: // Query single entity
                        QuerySingleEntity();
                        break;

                    case 3: // Query with expand
                        QueryEntitiesWithExpand();

                        break;
                    case 4: // add a link
                        AddCustomerCategoryLinkEntry();

                        Thread.Sleep(5000);
                        break;

                    case 5: // post an order
                        PostAnOrder();
                        Thread.Sleep(5000);
                        break;

                    case 6:
                        break;

                    case 9: //quit
                        quit = true;
                        break;
                }

                if (quit == true)
                {
                    break;
                }

                Console.Write("\n\nTry again [y/n]:");
                string userInput = Console.ReadLine();
                userInput = userInput.ToLowerInvariant();
                if (userInput != "y")
                {
                    break;
                }
            }

            /*
            var client = new ODataClient("http://localhost:12345/odata/");

            QueryCustomersTypedWithExpand();

            QueryCustomersTyped(client);

            QueryCustomersUnTyped(client);

            QuerySingleCustomer(3);

            AddCustomerCategoryLinkEntry();

            AddLinkEntryTest();

            QueryCustomersTypedWithExpand();

            Console.ReadKey();*/
        }

        private static /*async*/ void QueryEntitySet()
        {
            Console.Write("Please input the entity set name: [Customers/Orders/Categories]:");
            string entityset = Console.ReadLine();

            if (new string[] {"Customers", "Orders", "Categories"}.All(e => e != entityset))
            {
                Console.WriteLine("Invalid entity set names.");
                return;
            }

            var client = new ODataClient(_baseUri);

            Console.WriteLine("\n[Un-typed Syntax]");
            var entities = /*await*/ client.For(entityset).FindEntriesAsync().Result;
            // customers is a instance of WhereSelectListIterator, a list
            int n = 1;
            string indent = "";
            foreach (var entity in entities)
            {
                PrintItems("Entity Set #" + n++, indent, entity);
            }

            Console.WriteLine("\n[Typed Syntax]");
            switch (entityset)
            {
                case "Customers":
                    var customers = /*await*/ client.For<Customer>().FindEntriesAsync().Result;

                    foreach (var customer in customers)
                    {
                        Console.WriteLine("{0}: {1} likes {2}", customer.CustomerId, customer.CustomerName,
                            customer.FavoriteColor);
                    }
                    break;
                case "Orders":
                    var orders = /*await*/ client.For<Order>().FindEntriesAsync().Result;

                    foreach (var order in orders)
                    {
                        Console.WriteLine("{0}: {1}", order.OrderId, order.OrderName);
                    }
                    break;
                case "Categories":
                    var categories = /*await*/ client.For<Category>().FindEntriesAsync().Result;

                    foreach (var category in categories)
                    {
                        Console.WriteLine("{0}: {1}", category.CategoryId, category.CategoryType);
                    }
                    break;
            }
        }

        private static /*async*/ void QuerySingleEntity()
        {
            Console.Write("Please input the entity set name: [Customers/Orders/Categories]:");
            string entityset = Console.ReadLine();

            if (new string[] { "Customers", "Orders", "Categories" }.All(e => e != entityset))
            {
                Console.WriteLine("Invalid entity set names.");
                return;
            }

            Console.Write("Please input the key:");
            string keyInput = Console.ReadLine();
            int key;
            if (!int.TryParse(keyInput, out key))
            {
                Console.WriteLine("Invalid key input.");
                return;
            }

            var client = new ODataClient(_baseUri);

            Console.WriteLine("\n[Un-typed Syntax]");
            var entity = /*await*/ client.For(entityset).Key(key).FindEntryAsync().Result;
            PrintItems("Entity", "", entity);


            Console.WriteLine("\n[Typed Syntax]");
            switch (entityset)
            {
                case "Customers":
                    var customer = /*await*/ client.For<Customer>().Key(key).FindEntryAsync().Result;

                    Console.WriteLine("{0}: {1} likes {2}", customer.CustomerId, customer.CustomerName,
                        customer.FavoriteColor);
                    break;
                case "Orders":
                    var order = /*await*/ client.For<Order>().Key(key).FindEntryAsync().Result;

                    Console.Write("{0}: {1}", order.OrderId, order.OrderName);

                    if (order.Properties != null)
                    {
                        // It seems can't query the dynamic properties from "Properties" dictionary
                        foreach (var prop in order.Properties)
                        {
                            Console.Write(",{0}:{1}", prop.Key, prop.Value);
                        }
                    }
                    Console.WriteLine();
                    break;
                case "Categories":
                    var category = /*await*/ client.For<Category>().Key(key).FindEntryAsync().Result;

                    Console.WriteLine("{0}: {1}", category.CategoryId, category.CategoryType);
                    break;
            }
        }

        private static /*async*/ void QueryEntitiesWithExpand()
        {
            var client = new ODataClient(_baseUri);

            var customers = /*await*/ client.For<Customer>().Expand(x => x.Category).Expand(x => x.Orders).FindEntriesAsync().Result;

            foreach (var customer in customers)
            {
                Console.Write("{0}: {1} likes {2}", customer.CustomerId, customer.CustomerName, customer.FavoriteColor);

                if (customer.Category != null)
                {
                    Console.Write(", Category ( {0}, {1} )", customer.Category.CategoryId, customer.Category.CategoryType);
                }

                if (customer.Orders != null)
                {
                    Console.Write(",(");
                    foreach (var order in customer.Orders)
                    {
                        Console.Write("{0}: {1} ", order.OrderId, order.OrderName);
                    }
                    Console.Write(")");
                }

                Console.WriteLine();
            }
        }

        private static async void AddCustomerCategoryLinkEntry()
        {
            var client = new ODataClient(_baseUri);

            Customer newCustomer = new Customer
            {
                //CustomerId = 9,
                CustomerName = "John",
                FavoriteColor = Color.Yellow,
                Location = new Address
                {
                    City = "London",
                    Country = "EN"
                }
            };

            var customer = await client.For<Customer>().Set(newCustomer).InsertEntryAsync();

            Category newCategory = new Category
            {
                //CategoryId = 99999,
                CategoryType = "Manager"
            };

            var category = await client.For<Category>().Set(newCategory).InsertEntryAsync();

            await client.For<Customer>().Key(customer).LinkEntryAsync(x => x.Category, category);

            Console.WriteLine("\nAdded a new customer, new Category, link with customer and category.\n");
        }

        private static async void PostAnOrder()
        {
            var client = new ODataClient(_baseUri);

            // With dynamic properties
            var order =
                await
                    client.For("Orders")
                        .Set(new {OrderId = 9, OrderName = "New Order", MyProperty = "Dynamic Property", GuidProperty = Guid.NewGuid()})
                        .InsertEntryAsync();

            Console.WriteLine("Post order result (un-typed) : ");
            PrintItems("order", "", order);

            var result = await client.For<Order>().Set(new {OrderId = 9, OrderName = "New Order", Birthday = DateTimeOffset.Now})
                .InsertEntryAsync();

            Console.WriteLine("\nPost order result (typed): ");
            Console.Write("\n{0}: {1}", result.OrderId, result.OrderName);
            if (result.Properties != null)
            {
                // It seems can't query the dynamic properties from "Properties" dictionary
                foreach (var prop in result.Properties)
                {
                    Console.Write(",{0}:{1}", prop.Key, prop.Value);
                }
            }
            Console.WriteLine();
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

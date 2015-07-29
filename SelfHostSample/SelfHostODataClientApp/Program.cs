using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.OData.Client;
using SelfHostODataClientApp.Default;
using SelfHostODataClientApp.SelfHostServer.Models;

namespace SelfHostODataClientApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Container container = new Container(new Uri("http://localhost.fiddler:12345/odata/"));
            //Container container = new Container(new Uri("http://localhost:12345/odata/"));

            // This global configuration is necessary to overwrite other query result.
            // especially for $expand.
            container.MergeOption = MergeOption.OverwriteChanges;

            Console.WriteLine("\nQuery without expand:");
            var customers = container.Customers.ToList();
            foreach (var customer in customers)
            {
                Console.WriteLine(FormatCustomer(customer));
            }
            Console.WriteLine("\nQuery with expand:");

            // test the expand
            var customersWithExpand = container.Customers.Expand(c => c.Orders).Expand(c => c.Category);
            foreach (var customer in customersWithExpand)
            {
                Console.WriteLine(FormatCustomer(customer));
                container.Detach(customer.Orders);
            }

            // call function
            Console.WriteLine("\nCall function: ");
            var result = container.Customers.ByKey(1).RefreshCustomer().Expand("Category");
            var customer1 = result.GetValueAsync().Result;
            // var customer1 = result.GetValue();

            // Caution: It's strange that the customer1 will have the `Orders` value. because OData client will track/cache the
            // return entities if you query them before.
            // Workaround: 1. you (the client) ignore the returned `Orders`
            //             2. Every time, create a new container.
            Console.WriteLine(FormatCustomer(customer1));

            var result2 = container.Customers.ByKey(2).RefreshCustomer2().Expand("Category").Expand("Orders");
            //var result2 = container.Customers.ByKey(2).RefreshCustomer2().Expand("Category").Expand("Orders");
            var customer2 = result2.GetValueAsync().Result;
            Console.WriteLine(FormatCustomer(customer2));

            Console.ReadKey();
        }

        private static string FormatCustomer(Customer customer)
        {
            StringBuilder sb = new StringBuilder("  ");
            sb.Append(customer.CustomerId)
                .Append("):")
                .Append(customer.CustomerName)
                .Append(", ")
                .Append(customer.FavoriteColor)
                .Append(",")
                .Append(FormatAddress(customer.Location))
                .Append(",")
                .Append(FormatCategory(customer.Category))
                .Append(",")
                .Append(FormatOrders(customer.Orders));

            return sb.ToString();
        }

        private static string FormatAddress(Address address)
        {
            if (address != null)
            {
                return "{ " + address.Country + "," + address.City + " }";
            }

            return string.Empty;
        }

        private static string FormatCategory(Category category)
        {
            if (category != null)
            {
                return "{ " + category.CategoryId + "," + category.CategoryType + " }";
            }

            return string.Empty;
        }

        private static string FormatOrders(DataServiceCollection<Order> orders)
        {
            if (orders == null || !orders.Any())
            {
                return string.Empty;
            }

            StringBuilder sb = new StringBuilder("[");
            foreach (var order in orders)
            {
                sb.Append("{").Append(order.OrderId).Append(",").Append(order.OrderName).Append("}");
            }
            sb.Append("]");

            return sb.ToString();
        }
    }
}

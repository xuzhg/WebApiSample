using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfHostServer.Models
{
    public class DataSource
    {
        public static IList<Customer> Customers { get; set; }

        public static IList<Order> Orders { get; set; }

        public static IList<Category> Categories { get; set; }

        static DataSource()
        {
            Categories = Enumerable.Range(1, 5).Select(e =>
                new Category
                {
                    CategoryId = 100 + e,
                    CategoryType = e % 2 == 0 ? "VIP" : "General"
                }).ToList();

            Orders = Enumerable.Range(1, 10).Select(e =>
                new Order
                {
                    OrderId = 10 + e,
                    OrderName = "Order #" + e,
                }).ToList();

            Customers = Enumerable.Range(1, 5).Select(e =>
                new Customer
                {
                    CustomerId = e,
                    CustomerName = new[] {"Mike", "Peter", "Sam", "John", "Tony"}[e - 1],
                    FavoriteColor = new[] {Color.Red, Color.Black, Color.Green, Color.Pink, Color.Purple}[e-1],
                    Location = new Address
                    {
                        Country = e % 2 == 0 ? "US" : "CN",
                        City = new[] {"Shanghai", "Redmond", "Beijing", "New York", "Suzhou"}[e - 1],
                    },
                    Category = Categories[e - 1]
                }).ToList();

            int n = 0;
            foreach (var customer in Customers)
            {
                customer.Orders = new List<Order>();
                for (int i = 0; i < 2; i++)
                {
                    customer.Orders.Add(Orders[n++]);
                }
            }
        }
    }
}

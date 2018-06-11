using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BasicEFCoreTest.Models
{
    public static class DataSource
    {
        public static IList<Customer> GetCustomers()
        {
            Customer customerA = new Customer
            {
                Name = "Sam",
                Age = 18,
                FavoriateColor = Color.Red,
                HomeAddress = new Address
                {
                    City = "Redmond",
                    Street = "156 AVE NE"
                },
                Order = new Order
                {
                    Price = 101m
                },
            };

            Customer customerB = new Customer
            {
                Name = "Peter",
                Age = 19,
                FavoriateColor = Color.Red,
                HomeAddress = new Address
                {
                    City = "Bellevue",
                    Street = "Main St NE"
                },
                Order = new Order
                {
                    Price = 104m
                }
            };

            return new List<Customer>
            {
                customerA,
                customerB
            };
        }
    }
}

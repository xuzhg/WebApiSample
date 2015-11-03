using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdmModelLib
{
    public class DataSource
    {
        private static IList<Customer> _customers;
        private static IList<Order> _orders;
        public static IList<Customer> Customers
        {
            get
            {
                if (_customers == null)
                {
                    Generate();
                }

                return _customers;
            }
        }

        public static IList<Order> Orders
        {
            get
            {
                if (_orders == null)
                {
                    Generate();
                }

                return _orders;
            }
        }

        private static void Generate()
        {
            int orderId = 1;
            _customers = Enumerable.Range(1, 5).Select(e => new Customer
            {
                Id = e,
                Name = "Customer_" + e,
                Orders = Enumerable.Range(1, e).Select(f => new Order
                {
                    Id = orderId,
                    Title = "Order_" + orderId++
                }).ToList()
            }).ToList();

            var orders = new List<Order>();
            foreach (var customer in _customers)
            {
                orders.AddRange(customer.Orders);
            }

            _orders = orders;
        }
    }
}

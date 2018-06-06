using System.Collections.Generic;
using System.Linq;
using BasicWebApiSample.Models;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;

namespace BasicWebApiSample.Controllers
{
    public class CustomersController : ODataController
    {
        private readonly CustomerOrderContext _context;

        public CustomersController(CustomerOrderContext context)
        {
            _context = context;
            Generate(_context);
        }

        /*
        [EnableQuery]
        public IActionResult Get()
        {
            return Ok(_context.Customers);
        }*/

        [EnableQuery]
        public IQueryable<Customer> Get()
        {
           // var a = _context.Customers.AsQueryable().Sum(i => i.Id);

            if (Request.Path.Value.Contains("inmem"))
            {
                return GetCustomers().AsQueryable();
            }
            else
            {
                return _context.Customers;
            }
        }

        [EnableQuery]
        public IActionResult Get(int key)
        {
            return Ok(_context.Customers.FirstOrDefault(c => c.Id == key));
        }

        private static IList<Customer> _customers;
        public static IList<Customer> GetCustomers()
        {
            if (_customers != null)
            {
                return _customers;
            }
            Customer customerA = new Customer
            {
                Id = 1,
                Name = "Customer A",
                FavoriateColor = Color.Red,
                HomeAddress = new Address
                {
                    City = "Redmond",
                    Street = "156 AVE NE"
                },
                Order = new Order
                {
                    Id = 101,
                    Price = 101m
                },
                Orders = Enumerable.Range(1, 3).Select(e => new Order
                {
                    Id = 10 + e,
                    Price = 10.8m * e
                }).ToList()
            };

            Customer customerB = new Customer
            {
                Id = 2,
                Name = "Customer B",
                FavoriateColor = Color.Red,
                HomeAddress = new Address
                {
                    City = "Bellevue",
                    Street = "Main St NE"
                },
                Order = new Order
                {
                    Id = 102,
                    Price = 104m
                },
                Orders = Enumerable.Range(1, 4).Select(e => new Order
                {
                    Id = 20 + e,
                    Price = 28.8m / e
                }).ToList()
            };

            _customers = new List<Customer>
            {
                customerA,
                customerB
            };

            return _customers;
        }

        public static void Generate(CustomerOrderContext context)
        {
            if (context.Customers.Any())
            {
                return;
            }

            var customers = GetCustomers();

            foreach (var c in customers)
            {
                foreach (var o in c.Orders)
                {
                    context.Orders.Add(o);
                }

                context.Customers.Add(c);
            }
            context.SaveChanges();
        }
    }
}

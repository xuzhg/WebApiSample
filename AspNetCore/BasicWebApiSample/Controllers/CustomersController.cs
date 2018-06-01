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

        [EnableQuery]
        public IActionResult Get()
        {
            return Ok(_context.Customers);
        }

        [EnableQuery]
        public IActionResult Get(int key)
        {
            return Ok(_context.Customers.FirstOrDefault(c => c.Id == key));
        }

        public static void Generate(CustomerOrderContext context)
        {
            if (context.Customers.Any())
            {
                return;
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
                Orders = Enumerable.Range(1, 3).Select(e => new Order
                {
                    Id = 10 + e,
                    Price = 10.8m * e
                }).ToList()
            };

            context.Customers.Add(customerA);
            foreach(var o in customerA.Orders)
            {
                context.Orders.Add(o);
            }

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
                Orders = Enumerable.Range(1, 4).Select(e => new Order
                {
                    Id = 20 + e,
                    Price = 28.8m / e
                }).ToList()
            };

            context.Customers.Add(customerB);
            foreach (var o in customerB.Orders)
            {
                context.Orders.Add(o);
            }

            context.SaveChanges();
        }
    }
}

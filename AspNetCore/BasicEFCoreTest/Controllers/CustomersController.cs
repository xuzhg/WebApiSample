using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BasicEFCoreTest.Models;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;

namespace BasicEFCoreTest.Controllers
{
    public class CustomersController : ODataController
    {
        private CustomerOrderContext _db;

        public CustomersController(CustomerOrderContext context)
        {
            _db = context;

            if (context.Database.EnsureCreated())
            {
                if (context.Customers.Count() == 0)
                {
                    foreach(var customer in DataSource.GetCustomers())
                    {
                        context.Customers.Add(customer);
                        context.Orders.Add(customer.Order);
                    }

                    context.SaveChanges();
                }
            }
        }

        [EnableQuery]
        public IActionResult Get()
        {
            var query = _db.Customers.Select(x => new Customer
            {
                Age = x.Age,
                FavoriateColor = x.FavoriateColor,
                FirstName = x.FirstName,
                HomeAddress = x.HomeAddress,
                Id = x.Id,
                LastName = x.LastName,
                Order = x.Order,
                UserName = x.UserName
            });

            return Ok(query);
        }
    }
}

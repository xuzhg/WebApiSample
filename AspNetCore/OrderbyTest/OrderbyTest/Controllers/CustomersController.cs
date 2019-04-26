using System.Collections.Generic;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;
using OrderbyTest.Models;

namespace OrderbyTest.Controllers
{
    public class CustomersController : ODataController
    {
        private IList<Customer> _customers = new List<Customer>
        {
            new Customer
            {
                Id = 1,
                Name = "abc",
                Locations = new List<Address>
                {
                    new Address { Street = "148th AVE"},
                    new Address { Street = "136th AVE"},
                },
                Orders = new List<Order>
                {
                    new Order { Id = 11, Name = "OrderXyz" },
                    new Order { Id = 12, Name = "OrderIjk" }
                }
            },
            new SpecialCustomer
            {
                Id = 2,
                Name = "efj",
                Token = "token5",
                Locations = new List<Address>
                {
                    new Address { Street = "8th AVE"},
                    new Address { Street = "88th AVE"},
                },
                Orders = new List<Order>
                {
                    new Order { Id = 21, Name = "OrderEfj"},
                    new Order { Id = 22, Name = "OrderHik"}
                }
            },
            new SpecialCustomer
            {
                Id = 3,
                Name = "aaa",
                Token = "token3",
                Locations = new List<Address>
                {
                    new Address { Street = "5th AVE"},
                    new Address { Street = "98th AVE"}
                },
                Orders = new List<Order>
                {
                    new SpecialOrder { Id = 31, Name = "hfj", Price = 9 },
                    new SpecialOrder { Id = 32, Name = "aik", Price = 8 }
                }
            }
        };

        [EnableQuery]
        public IActionResult Get()
        {
            return Ok(_customers);
        }
    }
}

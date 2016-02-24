using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.OData;
using CustomSerializerSample.Models;

namespace CustomSerializerSample.Controllers
{
    public class CustomersController : ODataController
    {
        [EnableQuery]
        public IHttpActionResult Get()
        {
            return Ok(_customers);
        }

        [EnableQuery]
        public IHttpActionResult Get(int key)
        {
            Customer c = _customers.FirstOrDefault(e => e != null && e.Id == key);
            if (c == null)
            {
                return NotFound();
            }

            return Ok(c);
        }

        private static IList<Customer> _customers;

        static CustomersController()
        {
            int oId = 1;
            _customers = Enumerable.Range(1, 5).Select(e => new Customer
            {
                Id = e,
                Name = "Name#" + e,
                Orders = Enumerable.Range(1, 3).Select(o => new Order
                {
                    Id = 100 + oId++
                }).ToList()
            }).ToList();

            // Add a null customer
            _customers.Add(null);

            // add a null order in a customer
            Customer customer = new Customer
            {
                Id = 9,
                Name = "Sam",
                Orders = new List<Order>
                {
                    new Order {Id = 100 + oId++},
                    null,
                    new Order {Id = 100 + oId++},
                }
            };
            _customers.Add(customer);
        }
    }
}
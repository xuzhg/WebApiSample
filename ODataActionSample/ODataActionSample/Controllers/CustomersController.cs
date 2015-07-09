using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.OData;

namespace ODataActionSample.Controllers
{
    public class CustomersController : ODataController
    {
        private static readonly IList<Customer> _customers;

        static CustomersController()
        {
            string[] names = {"John", "Mike", "Sam", "Mark", "Tony"};
            _customers = Enumerable.Range(1, 5).Select(e =>
                new Customer
                {
                    Id = e,
                    Name = names[e-1],
                    Salary = 5.7 * e,
                    Orders = Enumerable.Range(1, e).Select(f =>
                        new Order
                        {
                            OrderId = e + f,
                            Price = 2.3*e + f,
                        }).ToList()
                }).ToList();
        }


        [EnableQuery]
        public IHttpActionResult Get()
        {
            return Ok(_customers);
        }

        [HttpPost]
        public IHttpActionResult Post(Customer customer)
        {
            int key = _customers.Count();
            customer.Id = key + 1;
            _customers.Add(customer);
            return Created(customer);
        }
    }
}

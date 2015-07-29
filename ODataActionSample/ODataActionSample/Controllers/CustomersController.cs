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
                    Properties = new Dictionary<string, object>
                    {
                        {"Email", "abs@microsoft.com"},
                        {"Age", e + 20}
                    },
                    Orders = Enumerable.Range(1, 6-e).Select(f =>
                        new Order
                        {
                            OrderId = e + f,
                            Price = 2.3*e + f,
                        }).ToList()
                }).ToList();

            Address address = new Address
            {
                City = "Redmond",
                Properties = new Dictionary<string, object>
                {
                    {"Street", "Microsoft Road"} // dynamic property
                }
            };

            _customers[1].Properties.Add("Address", address);

            address = new Address
            {
                City = "Shanghai",
                Properties = new Dictionary<string, object>
                {
                    {"Postcode", 201101} // dynamic property
                }
            };

            _customers[3].Properties.Add("Address", address);
        }

        [EnableQuery]
        public IHttpActionResult Get()
        {
            return Ok(_customers);
        }

        [HttpPost]
        public IHttpActionResult Post(Customer customer)
        {/*
            int key = _customers.Count();

            if (_customers.Any(c => c.Id == customer.Id))
            {
                customer.Id = key + 1;
            }*/

            _customers.Add(customer);
            return Created(customer);
        }

        public IHttpActionResult Patch(int key, Delta<Customer> patch)
        {
            Customer customer = _customers.FirstOrDefault(c => c.Id == key);
            if (customer == null)
            {
                return NotFound();
            }

            patch.Patch(customer);
            return Updated(customer);
        }

        public IHttpActionResult Put(int key, Customer changedCustomer)
        {
            Customer customer = _customers.FirstOrDefault(c => c.Id == key);
            if (customer == null)
            {
                return NotFound();
            }

            _customers.Remove(customer);
            _customers.Add(changedCustomer);
            return Updated(changedCustomer); // Updated(customer);
        }


        // be caution: if you want to the action, please modify the Web.config
        // just make path="*"
        [HttpPost]
        public bool IsEmailAvailable(ODataActionParameters parameters)
        {
            object value;
            if (parameters.TryGetValue("email", out value))
            {
                string email = value as string;
                if (email != null)
                {
                    // just for test
                    if (email == "saxu@microsoft.com")
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        [HttpPost]
        public IHttpActionResult IsEmailAvailable(int key, ODataActionParameters parameters)
        {
            // Just for test
            if (key != 3)
            {
                return NotFound();
            }

            object value;
            if (parameters.TryGetValue("email", out value))
            {
                string email = value as string;
                if (email != null)
                {
                    if (email == "saxu@microsoft.com")
                    {
                        return Ok("Your input email is :" + email);
                    }
                }
            }

            return Ok(false);
        }
    }
}

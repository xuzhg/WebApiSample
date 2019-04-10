using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LocalDateTimeSample.Models;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;

namespace LocalDateTimeSample.Controllers
{
    public class CustomersController : ODataController
    {
        private readonly IList<Customer> _inMemoryCustomers = new List<Customer>
        {
            new Customer { Id = 1, Name = "Sam", ReleaseDate = new DateTime(1999, 1, 1), ModifiedDate = new DateTime(2001, 12, 23)},
            new Customer { Id = 2, Name = "Peter", ReleaseDate = new DateTime(1998, 11, 1), ModifiedDate = new DateTime(1888, 2, 3)},
            new Customer { Id = 3, Name = "Kone", ReleaseDate = new DateTime(2009, 1, 21), ModifiedDate = null},
            new Customer { Id = 4, Name = "Kerry", ReleaseDate = new DateTime(2019, 3, 4), ModifiedDate = new DateTime(1998, 3, 5)}
        };

        public CustomersController()
        {
        }

        [EnableQuery]
        public IActionResult Get()
        {
            return Ok(_inMemoryCustomers);
        }

        [EnableQuery]
        public IActionResult Post([FromBody]Customer customer)
        {
            return Created(customer);
        }
    }
}

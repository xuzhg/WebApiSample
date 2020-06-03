using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Issue2180.Controllers
{
    public class CustomersController : ODataController
    {
        [HttpGet]
        [EnableQuery]
        public IActionResult Get()
        {
            var customers = new List<Customer>
            {
                new Customer
                {
                    Id = 1,
                    Color = Color.Red
                },
                new Customer
                {
                    Id = 2,
                    Color = Color.Blue
                },
                new Customer
                {
                    Id = 3,
                    Color = Color.Green
                },
            };

            return Ok(customers);
        }
    }
}

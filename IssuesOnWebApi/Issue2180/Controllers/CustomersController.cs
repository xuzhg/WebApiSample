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
        private AppDbContext _context;
        public CustomersController(AppDbContext context)
        {
            _context = context;

            // this line is necessary otherwise
            // the Database is not existed so 
            // Microsoft.Data.SqlClient.SqlException (0x80131904): Cannot open database "WebApiIssue2080Test" requested by the login. The login failed.
            // Login failed for user 'FAREAST\saxu'.
            context.Database.EnsureCreated();

            if (!context.Customers.Any())
            {
                context.Customers.AddRange(new List<Customer>
                {
                    new Customer
                    {
                        //Id = 1,
                        Color = Color.Red
                    },
                    new Customer
                    {
                        //Id = 2,
                        Color = Color.Blue
                    },
                    new Customer
                    {
                        //Id = 3,
                        Color = Color.Green
                    },
                });
                context.SaveChanges();
            }
        }


        [HttpGet]
        [EnableQuery]
        public IActionResult Get()
        {
            //var customers = new List<Customer>
            //{
            //    new Customer
            //    {
            //        Id = 1,
            //        Color = Color.Red
            //    },
            //    new Customer
            //    {
            //        Id = 2,
            //        Color = Color.Blue
            //    },
            //    new Customer
            //    {
            //        Id = 3,
            //        Color = Color.Green
            //    },
            //};

            //return Ok(customers);

            return Ok(_context.Customers);
        }
    }
}

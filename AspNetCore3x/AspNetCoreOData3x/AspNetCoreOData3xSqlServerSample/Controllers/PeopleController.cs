using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreOData3xSqlServerSample.Models;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AspNetCoreOData3xSqlServerSample.Controllers
{
    public class PeopleController : ODataController
    {
        private readonly ApplicationDbContext _db;
        public PeopleController(ApplicationDbContext applicationDbContext)
        {
            _db = applicationDbContext;

            if (_db.People.Any())
            {
                _db.People.Add(new Person
                {
                    FirstName = "a",
                    LastName = "b",
                    Car = new Car { Name = "dada" },
                    Cars = new List<Car> { new Car { Name = "sdad" } }
                });

                _db.SaveChanges();
            }
        }

        [EnableQuery]
        public IActionResult Get()
        {
            return Ok(_db.People);
        }
    }
}

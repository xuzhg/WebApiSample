using CreateNewTypeSample.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace CreateNewTypeSample.Controllers
{
    public class PeopleController : ODataController
    {
        [HttpGet]
        public IActionResult Get()
        {
            IList<Person> people = new List<Person>
            {
                new Person
                {
                    Id = 1,
                    NormalTemp = new Temperature(30.17, TemperatureKind.Celsius),
                    Dist = new Distance { Value = 9, Unit = DistanceUnit.M }
                },
                new Person
                {
                    Id = 2,
                    NormalTemp = new Temperature(10.57, TemperatureKind.Fahrenheit),
                    Dist = new Distance { Value = 19, Unit = DistanceUnit.FT }
                }
            };

            return Ok(people);
        }

        [HttpPost]
        public IActionResult Post([FromBody]Person p)
        {
            // send a request with body like:
            /*
            {
                "Id": 3,
                "NormalTemp": "10.70℉"
            }
            */

            return Created(p);
        }
    }
}
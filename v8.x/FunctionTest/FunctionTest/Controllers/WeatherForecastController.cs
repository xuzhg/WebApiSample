using FunctionTest.Extensions;
using FunctionTest.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace FunctionTest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpGet]
        [ODataFunction("odata")]
        public IActionResult MyFunction([FromRoute]int k, [FromBody]Detail detail)
        {
            detail.KFromFunctionCall = k;
            return Ok(detail);
        }
    }

    public class CustomersController : ODataController
    {
        [HttpGet]
        public IActionResult MyFunction2(int k, [FromBody]Detail d)
        {
            d.KFromFunctionCall = k;
            return Ok(d);
        }

        [HttpGet]
        public IActionResult MyFunction3(int k, [FromODataUri] Detail d)
        {
            d.KFromFunctionCall = k;
            return Ok(d);
        }
    }
}

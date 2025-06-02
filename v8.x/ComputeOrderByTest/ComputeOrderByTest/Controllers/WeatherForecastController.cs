using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;

namespace ComputeOrderByTest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        internal static readonly string[] Summaries = new[]
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
    }

    public class CustomersController : Controller
    {
        internal static readonly int[] Ages =
            [34, 54, 10, 8, 19, 22, 43, 11];

        [HttpGet]
        [EnableQuery]
        public IEnumerable<Customer> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new Customer
            {
                Id = index,
                Name = WeatherForecastController.Summaries[index - 1],
                Age = Ages[index - 1]
            })
            .ToArray();
        }
    }
}

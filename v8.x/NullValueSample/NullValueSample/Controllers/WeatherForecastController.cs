using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace NullValueSample.Controllers
{
    public class HandleCustomersController : ODataController
    {
        private readonly ApplicationDbContext _context;

        public HandleCustomersController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("empty/Customers")]
        [EnableQuery]
        public IActionResult GetEmpty()
        {
            return Ok(_context.Customers);
        }

        [HttpGet("null/Customers")]
        [EnableQuery(HandleNullPropagation = HandleNullPropagationOption.True)]
        public IActionResult GetNull()
        {
            return Ok(_context.Customers);
        }
    }

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
    }
}

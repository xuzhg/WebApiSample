using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;

namespace ODataDynamicPropertiesQuery.Controllers
{
    [ApiController]
    [Route("odata")]
    public class WeatherForecastController : ODataController
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private ApplicationDbContext _context;
        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, ApplicationDbContext dbContext)
        {
            _logger = logger;
            _context = dbContext;
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

        [HttpGet("Findings")]
        [EnableQuery]
        public IActionResult GetFinding(ODataQueryOptions<Finding> queryOptions)
        {
            IQueryable<Finding> qry = _context.Identities
                .Select(a => new Finding
                {
                    Id = a.FindingId,
                    Name = a.Name
                });

            var lst = qry.ToList().Where(q => (string)q.DynamicProperties["FindingType"] == "Base");

            var qry3 = qry.Where(q => (string)q.DynamicProperties["FindingType"] == "Base").ToList();
            var lst3 = qry3.ToList();

            IQueryable<Finding> qry2 = (IQueryable<Finding>)queryOptions.ApplyTo(qry);
            List<Finding> lst2 = qry2.ToList();
            return Ok(lst2);
        }
    }
}

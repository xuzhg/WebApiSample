using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Query.Expressions;
using Microsoft.AspNetCore.OData.Query.Wrapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace CustomizeSelectExpand.Controllers
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
        private JsonSerializerOptions _jsonOptions;

        private static IList<WeatherForecast> _weatherForecasts;
        static WeatherForecastController()
        {
            var rng = new Random();
            _weatherForecasts = Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Id = index,
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)],
                Departs = Enumerable.Range(1, 5).Select(k => new Department
                {
                    Id = k,
                    Name = Summaries[rng.Next(Summaries.Length)]
                }).ToList()
            })
            .ToList();
        }

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IOptions<JsonOptions> jsonOptions)
        {
            _logger = logger;
            _jsonOptions = jsonOptions.Value.JsonSerializerOptions;
        }

        [HttpGet]
        // [EnableQuery] // if you have ODataQueryOptions as parameters, don't enable it.
        public IActionResult Get(ODataQueryOptions<WeatherForecast> query)
        {
            if (query == null)
            {
                var rng = new Random();
                return Ok(Enumerable.Range(1, 5).Select(index => new WeatherForecast
                {
                    Date = DateTime.Now.AddDays(index),
                    TemperatureC = rng.Next(-20, 55),
                    Summary = Summaries[rng.Next(Summaries.Length)]
                })
                .ToArray());
            }
            else
            {
                IQueryable queryable = _weatherForecasts.AsQueryable();
                var queried = query.ApplyTo(queryable);
                var elementType = queried.ElementType;
                if (typeof(ISelectExpandWrapper).IsAssignableFrom(elementType))
                {
                    // you can serialize it as array for test.
                    string a = Serialize(queried);

                    IList<IDictionary<string, object>> list = new List<IDictionary<string, object>>();
                    foreach (var item in queried)
                    {
                        var wrapper = item as ISelectExpandWrapper;
                        var dic = wrapper.ToDictionary();
                        list.Add(dic);
                    }

                    return Ok(list);
                }
                else
                {
                    return Ok(queried.Cast<WeatherForecast>());
                }
            }
        }

        private string Serialize(IQueryable queryable)
        {
            // Should include the SelectExpandWrapperConverter in the json options.
            // You can introduce it by calling AddOData
            // or directly create a serializerOptions and add converter into converters.
            var s = JsonSerializer.Serialize(queryable, _jsonOptions);
            return s;
        }
    }

}

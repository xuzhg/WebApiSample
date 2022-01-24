using AddTypeAnnotationExtensions.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;

namespace AddTypeAnnotationExtensions.Controllers
{
    public class CustomersController : Controller
    {
        private static readonly IList<Customer> _customers = new List<Customer>
        {
            new Customer
            {
                Id = 1,
                Name = "Bracing",
                Dynamics = new Dictionary<string, object>
                {
                    { "DynamicLong", 44L },
                    { "DynamicBool", false},
                    { "DynamicString", "abc" },
                }
            },
            new Customer
            {
                Id = 2,
                Name = "Chilly",
                Dynamics = new Dictionary<string, object>
                {
                    { "DynamicGuid", Guid.NewGuid() },
                    { "DynamicByte", (byte)5 },
                }
            }
        };

        private readonly ILogger<CustomersController> _logger;

        public CustomersController(ILogger<CustomersController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [EnableQuery]
        public IActionResult Get()
        {
            return Ok(_customers);
        }
    }
}

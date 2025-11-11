using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Routing.Attributes;

namespace JustRoutingSamples.Controllers
{
    public class MeController : Controller
    {
        [HttpGet]
        public string GetOrders() => "Hello from /me endpoint";

        [ODataAttributeRouting] // this line is required to enable OData attribute routing, or derive from ODataController which has it by default
        [HttpGet("odata/me/Orders({key})")]
        [HttpGet("odata/me/Orders/{key}")] // support both (key) and /key style
        public string GetOrders(int key) => $"Hello from /me/orders({key}) endpoint";
    }
}

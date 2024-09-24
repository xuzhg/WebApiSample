using DynamicRouteSample.Extensions;
using DynamicRouteSample.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Abstracts;
using Microsoft.AspNetCore.OData.Extensions;
using Microsoft.AspNetCore.OData.Query;

namespace DynamicRouteSample.Controllers
{
    public class AnyNameForController : Controller
    {
        [HttpGet]
        [EnableQuery]
        [ODataRoute("{entityset}")]
        // It's for all routes (v1 and v2)
        public IActionResult Get(string entitySet)
        {
            IODataFeature feature = Request.ODataFeature();
            string prefix = feature.RoutingConventionsStore["odata_prefix"] as string;

            if (entitySet.Equals("customers", StringComparison.OrdinalIgnoreCase))
            {
                return Ok(DataSource.Customers);
            }
            else if (entitySet.Equals("orders", StringComparison.OrdinalIgnoreCase))
            {
                return Ok(DataSource.Orders);
            }
            else if (entitySet.Equals("people", StringComparison.OrdinalIgnoreCase))
            {
                return Ok(prefix == "v1" ? DataSource.PeopleV1 : DataSource.PeopleV2);
            }
            else
            {
                return NotFound($"Cannot find Data source '{entitySet}'");
            }
        }

        [HttpGet]
        [EnableQuery]
        [ODataRoute("{entityset}({key})")]
        // It's for all routes (v1 and v2)
        public IActionResult Get(string entitySet, int key)
        {
            IODataFeature feature = Request.ODataFeature();
            string prefix = feature.RoutingConventionsStore["odata_prefix"] as string;

            object data = null;
            if (entitySet.Equals("customers", StringComparison.OrdinalIgnoreCase))
            {
                data = DataSource.Customers.FirstOrDefault(c => c.Id == key);
            }
            else if (entitySet.Equals("orders", StringComparison.OrdinalIgnoreCase))
            {
                data = DataSource.Orders.FirstOrDefault(c => c.Id == key);
            }
            else if (entitySet.Equals("people", StringComparison.OrdinalIgnoreCase))
            {
                data = prefix == "v1" ? DataSource.PeopleV1.FirstOrDefault(c => c.Id == key) : DataSource.PeopleV2.FirstOrDefault(c => c.Id == key);
            }
            else
            {
                return NotFound($"Cannot find Data source '{entitySet}'");
            }

            if (data == null)
            {
                return NotFound($"Cannot find entity using key '{key}' from Data source '{entitySet}'");
            }

            return Ok(data);
        }

        [HttpGet]
        [ODataRoute("v1", "customers/{key}/Address")]
        public IActionResult GetAddress(int key)
        {
            Customer data = DataSource.Customers.FirstOrDefault(c => c.Id == key);
            if (data == null)
            {
                return NotFound($"Cannot find customer using key {key}");
            }

            return Ok(data.Address);
        }

        [HttpGet]
        [ODataRoute("v2", "{entityset}({key})/{property}")]
        public IActionResult Get(string entitySet, int key, string property)
        {
            object data = null;
            if (entitySet.Equals("orders", StringComparison.OrdinalIgnoreCase))
            {
                data = DataSource.Orders.FirstOrDefault(c => c.Id == key);
            }
            else if (entitySet.Equals("people", StringComparison.OrdinalIgnoreCase))
            {
                data = DataSource.PeopleV2.FirstOrDefault(c => c.Id == key);
            }
            else
            {
                return NotFound($"Cannot find Data source '{entitySet}'");
            }

            if (data == null)
            {
                return NotFound($"Cannot find Data source {entitySet} using property {property} on key {key}");
            }

            Type type = data.GetType();
            var propertyInfo = type.GetProperties().FirstOrDefault(p => p.Name.Equals(property, StringComparison.OrdinalIgnoreCase));
            if (propertyInfo == null)
            {
                return NotFound($"Cannot find Data source {entitySet} using property {property} on key {key}");
            }

            object propertyValue = propertyInfo.GetValue(data);
            return Ok(propertyValue);
        }

        [HttpGet]
        [ODataRoute("$metadata")]
        public IActionResult GetMetadata([FromServices]IODataModelProvider modelProvider)
        {
            IODataFeature feature = Request.ODataFeature();
            string prefix = feature.RoutingConventionsStore["odata_prefix"] as string;

            return Ok(modelProvider.GetEdmModel(prefix));
        }
    }
}

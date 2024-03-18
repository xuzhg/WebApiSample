using GenericControllerSample.Extensions;
using GenericControllerSample.Models;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Deltas;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Results;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace GenericControllerSample.Controllers
{
    [GenericControllerRouteConvention]
    public class ODataController<T> : ODataController where T : class
    {
        private static IList<Customer> _customers = new List<Customer>();
        private static IList<Order> _orders = new List<Order>();

        [HttpGet]
        [EnableQuery]
        public IQueryable<T> Get()
        {
            return (new List<T>()).AsQueryable();
        }

        [HttpGet]
        [EnableQuery]
        public SingleResult<T> Get([FromODataUri] int key)
        {
            if (typeof(T) == typeof(Customer))
            {
                return new SingleResult<T>(new[] { new Customer { Id = key } }.Cast<T>().AsQueryable());
            }
            else if (typeof(T) == typeof(Order))
            {
                return new SingleResult<T>(new[] { new Order { Id = key } }.Cast<T>().AsQueryable());
            }

            return SingleResult.Create(Get());
        }

        [HttpPost]
        public ActionResult Post([FromBody] T entity)
        {
            if (typeof(T) == typeof(Customer))
            {
                Customer customer = entity as Customer;
                customer.Id = _customers.Max(c => c.Id) + 1;
                _customers.Add(customer);
                return Created<Customer>(customer);
            }
            else if (typeof(T) == typeof(Order))
            {
                throw new NotImplementedException();
                // return new SingleResult<T>(new[] { new Order { Id = key } }.Cast<T>().AsQueryable());
            }

            return NoContent();
        }

        [HttpPatch]
        public ActionResult Patch([FromODataUri] int key, [FromBody] Delta<T> delta)
        {
            var test = delta;
            return NoContent();
        }

        [HttpDelete]
        public ActionResult Delete([FromODataUri] int key)
        {
            return NoContent();

        }

        [HttpPost]
        [HttpPut]
        public async Task<IActionResult> CreateLinkAsync(int key, string entityset, string navigationProperty, [FromBody]Uri link)
        {
            await Task.CompletedTask;
            return Ok($"CreateLinkAsync,key={key}, entityset={entityset}, navigationProperty={navigationProperty}, Link={link}");
        }
    }
}

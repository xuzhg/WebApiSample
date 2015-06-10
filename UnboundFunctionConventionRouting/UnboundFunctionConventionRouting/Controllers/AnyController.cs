using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.OData;
using System.Web.OData.Routing;
using UnboundFunctionConventionRouting.Models;
using UnboundNS;

namespace UnboundFunctionConventionRouting.Controllers
{
    public class AnyController : ODataController
    {
        [HttpGet]
        [ODataRoute("RetrieveCustomersByFilter(name={name},lastName={lastName},customerType={customerType})")]
        [EnableQuery]
        public IHttpActionResult UnboundFunction([FromODataUri]string name, [FromODataUri] string lastName,
            [FromODataUri] CustomerType customerType)
        {
            // add your business here.

            // I create a list for test
            Customer[] customers =
            {
                new Customer {Id = 1, Name = "John", LastName = "Alex",  CustomerType = CustomerType.Vip},
                new Customer {Id = 2, Name = "John", LastName = "Alex",  CustomerType = CustomerType.Vip},
                new Customer {Id = 3, Name = "John", LastName = "Alex",  CustomerType = CustomerType.Normal},
                new Customer {Id = 4, Name = "John", LastName = "Alex",  CustomerType = CustomerType.Vip},
                new Customer {Id = 5, Name = "Mike", LastName = "Alex",  CustomerType = CustomerType.Vip},
                new Customer {Id = 6, Name = "Peter", LastName = "Alex", CustomerType = CustomerType.Normal}
            };

            var found = customers.Where(c => c.Name == name && c.LastName == lastName && c.CustomerType == customerType);
            return Ok(found);
        }
    }
}

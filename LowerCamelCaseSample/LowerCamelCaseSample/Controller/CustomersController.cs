using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.OData;

namespace LowerCamelCaseSample.Controller
{
    public class CustomersController : ODataController
    {
        [EnableQuery]
        public IHttpActionResult Get()
        {
            return Ok(_customers);
        }

        private static IList<Customer> _customers;

        static CustomersController()
        {
            _customers = Enumerable.Range(1, 5).Select(s => new Customer
            {
                Id = s,
                Order = new Order {Id = 10 + s},
                Color = s < 2 ? Color.Red : s < 4 ? Color.Blue : Color.Green,
                Status = s < 2 ? ServiceStatus.Active : s < 4 ? ServiceStatus.Sleep : ServiceStatus.ShutDown
            }).ToList();
        }
    }
}

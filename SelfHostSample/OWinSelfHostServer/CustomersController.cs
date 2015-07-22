using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.OData;

namespace OWinSelfHostServer
{
    public class CustomersController : ODataController
    {
        private static IList<Customer> _customers = Enumerable.Range(1, 5).Select(e =>
            new Customer
            {
                Id = e,
                Name = "Name #" + e
            }).ToList();
            
        [EnableQuery]
        public IHttpActionResult Get()
        {
            return Ok(_customers);
        }

        public IHttpActionResult Get(int key)
        {
            Customer customer = _customers.FirstOrDefault(e => e.Id == key);
            if (customer == null)
            {
                return NotFound();
            }

            return Ok(customer);
        }
    }
}

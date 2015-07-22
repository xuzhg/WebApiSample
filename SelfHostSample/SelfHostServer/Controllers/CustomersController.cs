using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.OData;
using SelfHostServer.Models;

namespace SelfHostServer.Controllers
{
    public class CustomersController : ODataController
    {
        [EnableQuery]
        public IHttpActionResult Get()
        {
            return Ok(DataSource.Customers);
        }

        public IHttpActionResult Get(int key)
        {
            Customer customer = DataSource.Customers.FirstOrDefault(e => e.CustomerId == key);
            if (customer == null)
            {
                return NotFound();
            }

            return Ok(customer);
        }
    }
}

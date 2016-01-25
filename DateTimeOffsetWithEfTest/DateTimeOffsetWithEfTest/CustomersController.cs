using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.OData;

namespace DateTimeOffsetWithEfTest
{
    public class CustomersController : ODataController
    {
        private CustomerContext _db = new CustomerContext();

        [EnableQuery]
        public IHttpActionResult Get()
        {
            return Ok(_db.Customers);
        }
    }
}

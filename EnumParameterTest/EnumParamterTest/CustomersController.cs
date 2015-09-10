using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.OData;
using System.Web.OData.Routing;

namespace EnumParamterTest
{
    public class CustomersController : ODataController
    {
        [HttpGet]
        [ODataRoute("GetCountByCategory(category={category})")]
        public IHttpActionResult GetCountByCategory([FromODataUri] Category category)
        {
            return Ok(category);
        }
    }
}

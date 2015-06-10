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
    public class CustomersController : ODataController
    {
        [EnableQuery]
        public IHttpActionResult Get()
        {
            return Ok(new[] { new Customer { Id = 1, Name = "John", LastName = "Alex", CustomerType = CustomerType.Vip} });
        }

        [HttpGet]
        [ODataRoute("UnboundFunction(p1={intP},p2={strP},location={addP})")]
        public IHttpActionResult UnboundFunction([FromODataUri] int intP, [FromODataUri] string strP,
            [FromODataUri] Address addP)
        {
            StringBuilder sb = new StringBuilder("UnboundFunction: ");
            sb.Append("p1:").Append(intP).Append("|");
            sb.Append("p2:").Append(strP).Append("|");
            sb.Append("location:{City:").Append(addP.City).Append("}");

            return Ok(sb.ToString());
        }
    }
}

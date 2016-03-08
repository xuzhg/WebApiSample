using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.OData;
using System.Web.OData.Routing;

namespace ContainmentApplication
{
    public class CustomersController : ODataController
    {
        [HttpGet]
        [EnableQuery]
        public IHttpActionResult GetInfos([FromODataUri] int key)
        {
            return Ok("GetInfos(" + key + ")"); 
        }

        [HttpGet]
        [EnableQuery]
        [ODataRoute("Customers({customerId})/Infos({infoId})")]
        public IHttpActionResult GetSingleInfo([FromODataUri] int customerId, [FromODataUri]int infoId)
        {
            return Ok("GetSingleInfo(" + customerId + ")(" + infoId + ")");
        }

        [HttpPut]
        [EnableQuery]
        [ODataRoute("Customers({customerId})/Infos({infoId})")]
        public IHttpActionResult PutInfo([FromODataUri] int customerId, [FromODataUri]int infoId, Delta<CustomerInfo> patch)
        {
            return Ok("PutInfo(" + customerId + ")(" + infoId + ")");
        }

        [HttpDelete]
        [EnableQuery]
        [ODataRoute("Customers({customerId})/Infos({infoId})")]
        public IHttpActionResult DeleteInfo([FromODataUri] int customerId, [FromODataUri]int infoId)
        {
            return Ok("DeleteInfo(" + customerId + ")(" + infoId + ")");
        }
    }
}

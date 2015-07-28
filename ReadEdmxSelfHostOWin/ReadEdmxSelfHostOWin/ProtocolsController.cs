using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.OData;
using System.Web.OData.Routing;

namespace ReadEdmxSelfHostOWin
{
    public class ProtocolsController : ODataController
    {
        [EnableQuery]
        public IHttpActionResult Get()
        {
            return Ok(Enumerable.Range(1, 5).Select(e => new Protocol {Id = new Guid()}));
        }

        [HttpPatch]
        [ODataRoute("Protocols({protocolKey})/Fields({fieldKey})")]
        public IHttpActionResult PatchField([FromODataUri] Guid protocolKey, [FromODataUri] Guid fieldKey,
            Delta<Field> patch)
        {
            return Ok("OK");
        }
    }
}

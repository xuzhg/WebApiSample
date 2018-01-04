using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.OData;
using System.Web.OData.Routing;
using Xunit;

namespace Microsoft.AspNet.OData.Test.GeneralPropertyAccess
{
    public class LotOfPropertiesEntitiesController : ODataController
    {
        [HttpGet]
        public IHttpActionResult ReturnPropertyValue(int key, string propertyName)
        {
            return Ok(propertyName);
        }
    }
}

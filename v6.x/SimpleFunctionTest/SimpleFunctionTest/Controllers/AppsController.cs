using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.OData;

namespace SimpleFunctionTest.Controllers
{
    public class AppsController : ODataController
    {
        [HttpGet]
        public IHttpActionResult Get([FromODataUri]int key)
        {
            return Ok(key);
        }

        [HttpGet]
        public HttpResponseMessage Download([FromODataUri]int key, [FromODataUri]string p)
        {
            var r = new HttpResponseMessage(System.Net.HttpStatusCode.OK);
            var c = "Download (p=" + p + ") in Apps of key =" + key;
            r.Content = new StringContent(c);
            return r;
        }
    }
}

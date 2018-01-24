using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.OData;

namespace ODataOWin
{
    public class ensController : ODataController
    {
        [EnableQuery]
        public IHttpActionResult F8(int key)
        {
            return Ok(key);
        }

        [EnableQuery]
        public IHttpActionResult Post(Application app)
        {
            return Created(app);
        }
    }
}

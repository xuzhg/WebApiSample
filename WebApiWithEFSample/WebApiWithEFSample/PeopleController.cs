using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.OData;

namespace WebApiWithEFSample
{
    public class PeopleController : ODataController
    {
        private WebApiWithEfContext db = new WebApiWithEfContext();

        [EnableQuery]
        public IHttpActionResult Get()
        {
            return Ok(db.People);
        }

        [EnableQuery]
        public IHttpActionResult Post(Person person)
        {
            if (!ModelState.IsValid || person == null)
            {
                return BadRequest();
            }

            db.People.Add(person);
            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}

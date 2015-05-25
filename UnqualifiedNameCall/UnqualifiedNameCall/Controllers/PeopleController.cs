using System.Collections.Generic;
using System.Web.Http;
using System.Web.OData;
using System.Web.OData.Query;
using System.Web.OData.Routing;
using Ga;

namespace UnqualifiedNameCall.Controllers
{
    [ODataRoutePrefix("People")]
    public class PeopleController : ODataController
    {
        [HttpGet]
        [EnableQuery(PageSize = 20, AllowedQueryOptions = AllowedQueryOptions.All)]
        [ODataRoute("Ga.DoIt()")]
        public IHttpActionResult DoIt()
        {
            var people = new List<Person>
            {
                new Person {Id = 1},
                new Person {Id = 11}
            };
            return Ok(people);
        }
    }
}

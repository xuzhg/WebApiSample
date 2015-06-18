using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Results;
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

        public IHttpActionResult Get(ODataQueryOptions<Person> queryOptions)
        {
            ODataQuerySettings settings = new ODataQuerySettings
            {
                PageSize = 2
            };
            
            IEnumerable<Person> persons = BuildPersons();
            var result = queryOptions.ApplyTo(persons.AsQueryable(), settings).AsQueryable();

            // return Ok(result);
            return Ok(result, result.GetType());
        }

        private static IEnumerable<Person> BuildPersons()
        {
            return Enumerable.Range(1, 10).Select(e =>
                new Person
                {
                    Id = e,
                    Name = "People #2"
                });
        }

        private IHttpActionResult Ok(object content, Type type)
        {
            var resultType = typeof(OkNegotiatedContentResult<>).MakeGenericType(type);
            return Activator.CreateInstance(resultType, content, this) as IHttpActionResult;
        }
    }
}

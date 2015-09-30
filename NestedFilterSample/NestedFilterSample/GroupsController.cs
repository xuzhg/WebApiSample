using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.OData;
using Microsoft.OData.Core;

namespace NestedFilterSample
{
    public class GroupsController : ODataController
    {
        private static int QueryId = 100;
        private static IList<Group> _groups = Enumerable.Range(1, 5).Select(e => new Group
        {
            Id = e,
            Name = "Group " + e,
            IsHidden = e%2 == 0,
            IsShared = e%2 != 0,
            Queries = Enumerable.Range(1, 2).Select(f => new Query
            {
                Id = QueryId,
                Name = "Query " + QueryId++,
                IsPinned = f%2 == 0
            }).ToList()
        }).ToList();

        [EnableQuery]
        public IHttpActionResult Get()
        {
            return Ok(_groups);
        }
    }
}

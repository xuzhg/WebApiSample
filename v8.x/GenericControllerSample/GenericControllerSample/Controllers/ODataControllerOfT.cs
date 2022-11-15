using GenericControllerSample.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace GenericControllerSample.Controllers
{
    [GenericControllerRouteConvention]
    public class ODataController<T> : ODataController
    {
        [HttpGet]
        [EnableQuery]
        public IQueryable<T> Get()
        {
            return (new List<T>()).AsQueryable();
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

using ODataDyncRouteSample.Models;

namespace ODataDyncRouteSample.Controllers
{
    public class CommsEntityController<T> : ODataController
        where T : CommsEntity, new()
    {
        [EnableQuery]
        public Task<ActionResult<IQueryable<T>>> GetAsync(ODataQueryOptions<T> queryOptions)
        {
            List<CommsEntity> result = new List<CommsEntity>();
            result.Add(new AzureUpdate
            {
                Id = "1",
                Title = "Azure Update 1",
                Description = "Azure Update 1 Description",
            });

            IQueryable<T> queryable = result.AsQueryable().Cast<T>();
            ActionResult<IQueryable<T>> actionResult = Ok(queryable);

            return Task.FromResult(actionResult);
        }
    }
}

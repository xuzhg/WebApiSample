using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace GenericControllerSample.Controllers;

public class ATFEntitySetController<TEntity, TId> : ODataController
{
}

public partial class EngagementEntitySetController<TEntity, TId> : ATFEntitySetController<TEntity, TId>
        where TEntity : class
{
    [HttpPost]
    public async Task<IActionResult> BulkTaskOperationsAsync([FromODataUri] int id, ODataActionParameters parameters)
    {
        await Task.CompletedTask;
        return Ok(id);
    }

    [HttpGet]
    public async Task<IActionResult> EdmFunctionOperationsAsync([FromODataUri] int id, int p1)
    {
        await Task.CompletedTask;
        return Ok($"id={id},p1={p1}");
    }

    [HttpPost]
    public async Task<IActionResult> CollectionActionOperations(ODataActionParameters parameters)
    {
        await Task.CompletedTask;
        return Ok("CollectionActionOperations");
    }

    [HttpGet]
    public async Task<IActionResult> CollectionFunctionOperations(int p1)
    {
        await Task.CompletedTask;
        return Ok($"CollectionFunctionOperations,p1={p1}");
    }
}
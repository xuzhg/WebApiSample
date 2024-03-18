using GenericControllerSample.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace GenericControllerSample.Controllers;

/// <summary>
/// The customer controller.
/// </summary>
public class CustomersController<Customer> : ODataController<Customer>
    where Customer : class
{
    [HttpGet("api/odata/customers/GetLatestCustomer(key={key})")]
    public async Task<ActionResult<int>> GetLatestCustomer([FromRoute] int key)
    {
        await Task.CompletedTask;
        return Ok(key);
    }
}


public class HandleOtherController : ODataController
{
    private static IList<Annotation> _Annotations = new List<Annotation>();
    private static int _Id = 888;

    [HttpPost("api/odata/annotations")]
    public async Task<ActionResult<int>> PostToAnnotations([FromBody] Annotation annotation)
    {
        annotation.Id = _Id++;
        return Created(annotation);
    }
}
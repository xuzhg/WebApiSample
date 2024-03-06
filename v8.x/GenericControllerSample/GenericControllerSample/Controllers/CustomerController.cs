using Microsoft.AspNetCore.Mvc;

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

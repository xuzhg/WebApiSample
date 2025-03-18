using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace issue1444_CustomSerializerProvider.Controllers;

public class ODataTestController : ODataController
{
    [HttpGet("odata/GetTest()")]
    public IActionResult GetTest()
    {
        var a = new Dictionary<int, List<int>>();   
        a.Add(10, new List<int>() { 20, 20, 20 });
        a.Add(20, new List<int>() { 30, 30, 30 });

        return Ok(new TestInfoDto() { GroupTypes = a });
    }
}
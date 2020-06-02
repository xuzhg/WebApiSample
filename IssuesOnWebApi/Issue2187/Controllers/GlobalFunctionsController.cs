using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Issue2187.Controllers
{
    public class GlobalFunctionsController : ODataController
    {
        [HttpGet]
        [ODataRoute("SayAnything(value={anyValueNameHere},repeat={anyRepeatNameHere})")]
        public IActionResult SayAnything([FromODataUri] string anyValueNameHere, [FromODataUri] int anyRepeatNameHere)
        {
            // Breakpoint anywhere.  Function never gets called.
            string result = string.Join(" ... ", Enumerable.Range(1, anyRepeatNameHere).Select(i => anyValueNameHere));
            return Content(result, "text/plain; charset=utf-8");
        }

    }
}

using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntitySetWithSpaceRouting.Controllers
{
    public class OrderDetailsController : ODataController
    {
        [EnableQuery]
        public IActionResult Get()
        {
            return Ok("Here's any things");
        }
    }
}

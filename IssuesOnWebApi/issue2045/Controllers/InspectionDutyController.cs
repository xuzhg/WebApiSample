using issue2045.Models;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace issue2045.Controllers
{
    // the controller should name as "EntitySetName" + Controller in convention routing
    // owing the attribute routing doesn't care about the controller name, so use the convention name can support both
    public class InspectionDutyController : ODataController
    {
        [HttpGet]
        [EnableQuery]
        // ~/InspectionDuty
        public IActionResult Get()
        {
            return Ok(new[] { new InspectionDuty { Id = 1 } });
        }

        [HttpGet]
        [EnableQuery]
        // ~/InspectionDuty/{id}
        public IActionResult Get(int key)
        {
            return Ok(new InspectionDuty { Id = key } );
        }

        [HttpPost]
        [ODataRoute("InspectionDuty/Default.SingleChange")] // This is not required, it supports convention routing
        public IActionResult SingleChange(ODataActionParameters parameters)
        {
            return ReturnAction(parameters);
        }

        [HttpPost]
        [ODataRoute("UnboundSingleChange")] // This is REQUIRED
        public IActionResult UnboundSingleChange(ODataActionParameters parameters)
        {
            // You can renamed the action name as any action name
            return ReturnAction(parameters);
        }

        private IActionResult ReturnAction(ODataActionParameters parameters)
        {
            if (parameters == null)
            {
                return Ok("Null parameters");
            }

            if (parameters.ContainsKey("Comment") && parameters.ContainsKey("Change"))
            {
                return Ok("Received both parameters.");
            }

            if (parameters.ContainsKey("Comment"))
            {
                return Ok(parameters["Comment"]);
            }

            if (parameters.ContainsKey("Change"))
            {
                return Ok(parameters["Change"]);
            }

            return Ok("Empty Parameters");
        }
    }
}

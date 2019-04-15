using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;

namespace OptionalParameterSample.Controllers
{
    public class UsersController : ODataController
    {
        // GET api/values
        [HttpGet]
        public IActionResult getAssignedAppTiles(int key, string param, bool? includeOfficeFirstParty)
        {
            return Ok($"User Key = {key}, param = {param}, includeOfficeFirstParty = {includeOfficeFirstParty}");
        }
    }
}

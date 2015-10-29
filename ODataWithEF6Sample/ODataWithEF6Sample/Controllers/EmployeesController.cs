using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.OData;
using ODataWithEF6Sample.Models;

namespace ODataWithEF6Sample.Controllers
{
    public class EmployeesController : ODataController
    {
        private ODataWithEf6SampleContext db = new ODataWithEf6SampleContext();

        [EnableQuery]
        public IHttpActionResult Get([FromODataUri] int key)
        {
            return Ok(db.Employees.FirstOrDefault(e => e.Id == key));
        }
    }
}

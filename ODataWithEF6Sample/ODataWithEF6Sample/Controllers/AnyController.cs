using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Web;
using System.Web.Http;
using System.Web.OData;
using System.Web.OData.Routing;
using ODataWithEF6Sample.Models;

namespace ODataWithEF6Sample.Controllers
{
    public class AnyController : ODataController
    {
        [HttpPost]
        [ODataRoute("ResetEF")]
        public IHttpActionResult ResetEF()
        {
            ODataWithEf6SampleContext db = new ODataWithEf6SampleContext();

            if (db.Employees.Any())
            {
                return Ok("Already created.");
            }

            // Address
            var addresses = Enumerable.Range(1, 5).Select(e => new Address
            {
                Id = e,
                AddressLine1 = "Line_1Of_" + e,
                AddressLine2 = "Line_2Of_" + e,
            }).ToList();

            foreach (var address in addresses)
            {
                db.Addresses.Add(address);
            }

            // Message
            var messages = Enumerable.Range(1, 5).Select(e => new Message
            {
                Id = e,
                Name = "Msg_Of_" + e
            }).ToList();

            foreach (var message in messages)
            {
                db.Messages.Add(message);
            }

            // Employee
            var employees = Enumerable.Range(1, 5).Select(e => new Employee
            {
                Id = e,
                Name = "Employee_" + e,
                AddressId = addresses[e - 1].Id,
                Address = addresses[e - 1],
                Messages = new List<Message>()
            }).ToList();

            foreach (var employee in employees)
            {
                //employee.Messages.Add();
                db.Employees.Add(employee);
            }

            db.SaveChangesAsync();
            return Ok("Done.");
        }
    }
}
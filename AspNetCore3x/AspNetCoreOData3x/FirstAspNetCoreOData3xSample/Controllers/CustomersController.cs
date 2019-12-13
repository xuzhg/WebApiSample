// Copyright (c) Microsoft Corporation.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using FirstAspNetCoreOData3xSample.Models;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FirstAspNetCoreOData3xSample.Controllers
{
    public class CustomersController : ODataController
    {
        private readonly CustomerOrderContext _context;

        public CustomersController(CustomerOrderContext context)
        {
            _context = context;

            if (_context.Customers.Count() == 0)
            {
                IList<Customer> customers = new List<Customer>
                {
                    new Customer
                    {
                        Name = "Jonier",
                        ByteValue = 8,
                        Data = new byte[] { 1, 2, 3 },
                        HomeAddress = new Address { City = "Redmond", Street = "156 AVE NE"},
                        Order = new Order { Title = "104m" }
                    },
                    new Customer
                    {
                        Name = "Sam",
                        ByteValue = 18,
                        Data = new byte[] { 4, 5, 6 },
                        HomeAddress = new Address { City = "Bellevue", Street = "Main St NE"},
                        Order = new Order { Title = "Zhang" }
                    },
                    new Customer
                    {
                        Name = "Peter",
                        ByteValue = 28,
                        Data = new byte[] { 7, 8, 9 },
                        HomeAddress = new Address {  City = "Hollewye", Street = "Main St NE"},
                        Order = new Order { Title = "Jichan" }
                    },
                };

                foreach (var customer in customers)
                {
                    _context.Customers.Add(customer);
                    _context.Orders.Add(customer.Order);
                }

                _context.SaveChanges();
            }
        }

        [EnableQuery]
        public IActionResult Get()
        {
            // Be noted: without the NoTracking setting, the query for $select=HomeAddress with throw exception:
            // A tracking query projects owned entity without corresponding owner in result. Owned entities cannot be tracked without their owner...
            _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            return Ok(_context.Customers);
        }

        [EnableQuery]
        public IActionResult Get(int key)
        {
            return Ok(_context.Customers.FirstOrDefault(c => c.Id == key));
        }
    }
}

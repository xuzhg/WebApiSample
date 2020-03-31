// Copyright (c) Microsoft Corporation.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.

using System.Linq;
using AspNetCore3xEndpointSample.Models;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AspNetCore3xEndpointSample.Controllers
{
    public class ProductsController : ODataController
    {
        private readonly ProductDepartmentContext _context;

        public ProductsController(ProductDepartmentContext context)
        {
            _context = context;

            if (_context.Products.Count() == 0)
            {
                Product p = new Product
                {
                    ProductId = 1,
                    Depid = "133",
                    ProductName = "test"
                };
                _context.Products.Add(p);

                Department d = new Department
                {
                    Depid = "14",
                    DepName = "abc"
                };
                _context.Departments.Add(d);
                _context.SaveChanges();
            }
        }

        [EnableQuery]
        public IActionResult Get()
        {
            return Ok(_context.Products);
        }

        [EnableQuery]
        public IActionResult Get(int key)
        {
            return Ok(_context.Products.FirstOrDefault(c => c.ProductId == key));
        }
    }

    public class DepartmentsController : ODataController
    {
        private readonly ProductDepartmentContext _context;

        public DepartmentsController(ProductDepartmentContext context)
        {
            _context = context;

            if (_context.Products.Count() == 0)
            {
                Product p = new Product
                {
                    ProductId = 1,
                    Depid = "133",
                    ProductName = "test"
                };
                _context.Products.Add(p);

                Department d = new Department
                {
                    Depid = "14",
                    DepName = "abc"
                };
                _context.Departments.Add(d);
                _context.SaveChanges();
            }
        }

        [EnableQuery]
        public IActionResult Get()
        {
            // Be noted: without the NoTracking setting, the query for $select=HomeAddress with throw exception:
            // A tracking query projects owned entity without corresponding owner in result. Owned entities cannot be tracked without their owner...
            _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            return Ok(_context.Departments);
        }
    }
}

// Copyright (c) Microsoft Corporation.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using FirstAspNetCoreOData3xSample.Models;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;

namespace FirstAspNetCoreOData3xSample.Controllers
{
    public class ProductsController : ODataController
    {
        // In memory
        private static IList<Product> _products;

        public ProductsController()
        {
            if (_products == null)
            {
                _products = new List<Product>
                {
                    new Product
                    {
                        Title = "Product_1",
                        EMails = new string[] { "e1@abc.com", "e2@xyz.com" },
                        ByteValue = 8,
                        Data = new byte[] { 1, 2, 3 },
                        HomeAddress = new Address { City = "Redmond", Street = "156 AVE NE"},
                        Category = new Category { Id = 11, Title = "104m" }
                    },
                    new Product
                    {
                        Title = "Product_2",
                        EMails = new string[] { "cd1@abc.com", "ad2@xyz.com" },
                        ByteValue = 18,
                        Data = new byte[] { 4, 5, 6 },
                        HomeAddress = new Address { City = "Bellevue", Street = "Main St NE"},
                        Category = new Category { Id = 12, Title = "Zhang" }
                    },
                    new Product
                    {
                        Title = "Product_3",
                        EMails = new string[] { "fe1@abc.com", "fa2@xyz.com" },
                        ByteValue = 28,
                        Data = new byte[] { 7, 8, 9 },
                        HomeAddress = new Address {  City = "Hollewye", Street = "Main St NE"},
                        Category = new Category { Id = 13, Title = "Jian" }
                    },
                };

            }
        }

        [EnableQuery]
        public IActionResult Get()
        {
            return Ok(_products);
        }

        [EnableQuery]
        public IActionResult Get(int key)
        {
            return Ok(_products.FirstOrDefault(c => c.Id == key));
        }
    }
}

// Copyright (c) Microsoft Corporation.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.

using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace AspNetCore3xEndpointSample.Models
{
    public class ProductDepartmentContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Department> Departments { get; set; }

        public ProductDepartmentContext(DbContextOptions<ProductDepartmentContext> options) : base(options)
        {
        }
    }

    public static class DbInitializer
    {
        public static void Initialize(ProductDepartmentContext context)
        {
            context.Database.EnsureCreated();

            // Look for any students.
            if (context.Products.Any())
            {
                return;   // DB has been seeded
            }

            var departments = new Department[]
            {
                new Department
                {
                    DepName="Chemistry"
                },
                new Department
                {
                    DepName="Microeconomics"
                }
            };

            foreach (Department c in departments)
            {
                context.Departments.Add(c);
            }

            var departList = new List<Department>
            {
                new Department
                {
                    DepName="ChemList"
                },
                new Department
                {
                    DepName="MicroeList"
                }
            };

            foreach (Department c in departList)
            {
                context.Departments.Add(c);
            }

            var departCollect = new Collection<Department>
            {
                new Department
                {
                    DepName="ChemList"
                },
                new Department
                {
                    DepName="MicroeList"
                }
            };

            foreach (Department c in departCollect)
            {
                context.Departments.Add(c);
            }

            //var departEnum = Enumerable.Range(1, 3).Select(e =>
            //    new Department
            //   {
            //       DepName = "abc " + e
            //   });

            var products = new Product[]
            {
                new Product
                {
                    ProductName="Alexander",
                    Department = departments[0],
                    DepartList = departList,
                    DepartCollection = departCollect,
             //       DepartEnumer = departEnum
                },
                new Product
                {
                    ProductName="Alonso",
                    Department = departments[0],
                    DepartList = departList,
                    DepartCollection = departCollect,
                //    DepartEnumer = departEnum
                },
                new Product
                {
                    ProductName="Anand",
                    Department = departments[1],
                    DepartList = departList,
                    DepartCollection = departCollect,
               //     DepartEnumer = departEnum
                },
            };
            foreach (Product p in products)
            {
                context.Products.Add(p);
            }

            //foreach (Department c in departEnum)
            //{
            //    context.Departments.Add(c);
            //}
            context.SaveChanges();

            
            context.SaveChanges();

            context.SaveChanges();
        }
    }
}

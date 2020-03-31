// Copyright (c) Microsoft Corporation.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.

using Microsoft.EntityFrameworkCore;
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

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        //{
        //    optionsBuilder.UseSqlServer(
        //        @"Server=(localdb)\mssqllocaldb;Database=SamProductAndDepartmentTest;Integrated Security=True");
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
                new Department{ Depid = "1", DepName="Chemistry"},
                new Department{  Depid = "2",DepName="Microeconomics"}
            };
            foreach (Department c in departments)
            {
                context.Departments.Add(c);
            }

            var products = new Product[]
            {
                new Product{ProductName="Alexander", Depid="1", Department = departments[0] },
                new Product{ProductName="Alonso", Depid="1", Department = departments[0]},
                new Product{ProductName="Anand", Depid="2", Department = departments[1]},
            };
            foreach (Product p in products)
            {
                context.Products.Add(p);
            }
            context.SaveChanges();

            
            context.SaveChanges();

            context.SaveChanges();
        }
    }
}

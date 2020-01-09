using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreOData3xSqlServerSample.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Person> People { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Person>().OwnsOne(p => p.Car);
            modelBuilder.Entity<Person>().OwnsMany(p => p.Cars);
        }
    }


    public class Person
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Car Car { get; set; }
        public List<Car> Cars { get; set; }
    }

    [Owned, ComplexType]
    public class Car
    {
        public string Name { get; set; }
    }
}

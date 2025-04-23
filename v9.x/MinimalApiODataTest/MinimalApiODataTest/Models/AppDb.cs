using Microsoft.EntityFrameworkCore;
using System.Net;

namespace MinimalApiODataTest.Models
{
    public class AppDb : DbContext
    {
        public AppDb(DbContextOptions<AppDb> options)
        : base(options) { }

        public DbSet<Customer> Customers => Set<Customer>();

        public DbSet<Order> Orders => Set<Order>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Customer>().HasKey(x => x.Id);
            modelBuilder.Entity<Order>().HasKey(x => x.Id);
            modelBuilder.Entity<Customer>().OwnsOne(x => x.HomeAddress);
        }
    }
}

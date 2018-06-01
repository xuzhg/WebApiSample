using Microsoft.EntityFrameworkCore;

namespace BasicWebApiSample.Models
{
    public class CustomerOrderContext : DbContext
    {
        public CustomerOrderContext(DbContextOptions<CustomerOrderContext> options)
            : base(options)
        {

        }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<Order> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>().OwnsOne(c => c.HomeAddress);
        }
    }
}

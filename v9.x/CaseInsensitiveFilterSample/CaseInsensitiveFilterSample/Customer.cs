using Microsoft.EntityFrameworkCore;

namespace CaseInsensitiveFilterSample
{
    public class Customer
    {
        public int CustomerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
      //  public virtual List<Invoice> Invoices { get; set; }
    }

    public class MyContext : DbContext
    {
        public MyContext(DbContextOptions options) : base(options) { }
        public DbSet<Customer> Customers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>().ToTable("Customers");

            modelBuilder.Entity<Customer>()
           .HasData(
               new { CustomerId = 1, FirstName = "Mercury", LastName ="Dog" },
               new { CustomerId = 2, FirstName = "Venus", LastName = "John" },
               new { CustomerId = 3, FirstName = "Earth", LastName = "Cat" },
               new { CustomerId = 4, FirstName = "Mars", LastName = "dog" },
               new { CustomerId = 5, FirstName = "Jupiter", LastName = "doG" },
               new { CustomerId = 6, FirstName = "Saturn", LastName = "pet" },
               new { CustomerId = 7, FirstName = "Uranus", LastName = "petty" },
               new { CustomerId = 8, FirstName = "Neptune" , LastName ="john" },
               new { CustomerId = 9, FirstName = "Pluto", LastName = "Pett" }
           );
        }
    }
}

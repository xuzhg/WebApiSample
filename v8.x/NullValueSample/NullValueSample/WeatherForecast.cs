using Microsoft.EntityFrameworkCore;

namespace NullValueSample
{

    public static class DbExtensions
    {

        public static void MakeSureDbCreated(this WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                var context = services.GetRequiredService<ApplicationDbContext>();

                // uncomment the following to delete it
               // context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
            }
        }
    }


    public class WeatherForecast
    {
        public DateOnly Date { get; set; }

        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public string Summary { get; set; }
    }

    // Complex
    public class VatNumber
    {
        public string CountryCode { get; set; }

        public string Number { get; set; }
    }

    // Entity
    public class Customer
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public VatNumber? VatNumber { get; set; } // Nullable/optional
    }

    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Customer> Customers { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Customer>()
            .HasData(
                new { Id = 1, Name = "Mercury Middle School" },
                new { Id = 2, Name = "Venus High School" },
                new { Id = 3, Name = "Earth Univerity" },
                new { Id = 4, Name = "Mars Elementary School ", },
                new { Id = 5, Name = "Jupiter College"},
                new { Id = 6, Name = "Saturn Middle School" },
                new { Id = 7, Name = "Uranus High School" },
                new { Id = 8, Name = "Neptune Elementary School" },
                new { Id = 9, Name = "Pluto University" }
            );

            // config the complex value for MailAddress of each School
            modelBuilder.Entity<Customer>().OwnsOne(x => x.VatNumber)
                .HasData(
                    new { CustomerId = 1, CountryCode = "1A", Number = "98051" },
                   // null,
                    new { CustomerId = 3, CountryCode = "3B", Number = "98043" },
                    new { CustomerId = 5, CountryCode = "4C", Number = "98023" },
                    new { CustomerId = 8, CountryCode = "5D", Number = "78123" }
                   // null
                );
        }
    }
}

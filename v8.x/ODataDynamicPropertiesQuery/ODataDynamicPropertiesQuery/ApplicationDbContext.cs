using Microsoft.EntityFrameworkCore;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions options)
        : base(options)
    {
    }

    public DbSet<InactiveIdentitiesAzureApp> Identities { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<InactiveIdentitiesAzureApp>()
            .HasData(
                new { FindingId = 1, Name = "Mercury Middle School", Pci = 7, TasksGranted = 17 },
                new { FindingId = 2, Name = "Venus High School", Pci = 8, TasksGranted = 28 },
                new { FindingId = 3, Name = "Earth Univerity", Pci = 9, TasksGranted = 39 },
                new { FindingId = 4, Name = "Mars Elementary School", Pci = 8, TasksGranted = 48 },
                new { FindingId = 5, Name = "Jupiter College", Pci = 8, TasksGranted = 58 },
                new { FindingId = 6, Name = "Saturn Middle School", Pci = 8, TasksGranted = 68 },
                new { FindingId = 7, Name = "Uranus High School", Pci = 8, TasksGranted = 78 },
                new { FindingId = 8, Name = "Neptune Elementary Scho", Pci = 8, TasksGranted = 88 },
                new { FindingId = 9, Name = "Pluto University", Pci = 8, TasksGranted = 98 }
            );

    }
}


public static class DbExtensions
{
    public static void MakeSureDbCreated(this WebApplication app)
    {
        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;

            var context = services.GetRequiredService<ApplicationDbContext>();

            // uncomment the following to delete it
            //context.Database.EnsureDeleted();

            context.Database.EnsureCreated();
        }
    }
}

public static class EdmModel
{
    public static IEdmModel GetEdmModel()
    {
        var builder = new ODataConventionModelBuilder();
        builder.EntitySet<Finding>("Findings");
        return builder.GetEdmModel();
    }
}

public class InactiveIdentitiesAzureApp
{
    [Key] public int FindingId { get; set; }
    public string Name { get; set; }
    public int? Pci { get; set; }

    public int TasksGranted { get; set; }
}

public class Finding
{
    public Finding()
    {
        DynamicProperties = new Dictionary<string, object>();
        DynamicProperties.Add("FindingType", "Base");
    }

    [Key]
    public int Id { get; set; }

    public string Name { get; set; } = "";

    public IDictionary<string, object> DynamicProperties { get; set; }

}
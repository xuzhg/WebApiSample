using CalculatedPropertyTest.Models;

namespace CalculatedPropertyTest.Extensions;

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


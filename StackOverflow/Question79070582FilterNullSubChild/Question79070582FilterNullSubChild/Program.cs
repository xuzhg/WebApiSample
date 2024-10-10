using Microsoft.AspNetCore.OData;

namespace Question79070582FilterNullSubChild;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers()
            .AddOData(opt => opt.EnableQueryFeatures().AddRouteComponents("odata", EdmModelBuilder.GetEdmModel()));

        var app = builder.Build();

        // Configure the HTTP request pipeline.

        app.UseAuthorization();

        app.UseODataRouteDebug();

        app.MapControllers();

        app.Run();
    }
}

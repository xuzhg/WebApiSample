using Microsoft.AspNetCore.OData;
using Microsoft.AspNetCore.OData.Formatter.Deserialization;
using UpdateNestedNavigationPropertySample.Extensions;
using UpdateNestedNavigationPropertySample.Models;

namespace UpdateNestedNavigationPropertySample;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers()
            .AddOData(opt =>
            {
                opt.EnableQueryFeatures();
                opt.AddRouteComponents("odata/v{version}", EdmModelBuilder.GetEdmModel(),
                    services => services.AddSingleton<ODataResourceDeserializer, MyResourceDeserializer>());
            });

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        app.UseODataRouteDebug();

        app.UseAuthorization();
        app.MapControllers();

        app.Run();
    }
}
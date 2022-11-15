using GenericControllerSample.Extensions;
using GenericControllerSample.Models;
using Microsoft.AspNetCore.OData;

namespace GenericControllerSample
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllersWithViews()
                .AddOData(opt =>
                {
                    opt.EnableQueryFeatures();
                    opt.AddRouteComponents("api/odata", ODataBuilder.GetEdmModel());
                })
                .ConfigureApplicationPartManager(pm =>
                {
                    pm.FeatureProviders.Add(new TestProvider());
                });

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseAuthorization();

            app.UseODataRouteDebug();

            app.MapControllers();

            app.Run();
        }
    }
}
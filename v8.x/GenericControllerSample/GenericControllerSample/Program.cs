using GenericControllerSample.Extensions;
using GenericControllerSample.Models;
using Microsoft.AspNetCore.OData;
using Microsoft.AspNetCore.OData.Batch;
using Microsoft.AspNetCore.OData.Formatter.Deserialization;

namespace GenericControllerSample
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var model = ODataBuilder.GetEdmModel();

            builder.Services.AddControllersWithViews()
                .AddOData(opt =>
                {
                    opt.EnableQueryFeatures();
                opt.AddRouteComponents("api/odata", model,
                    services => services.AddSingleton<ODataEntityReferenceLinkDeserializer, MyEntityReferenceLinkDeserializer>()
                    .AddSingleton(sp => new DefaultODataBatchHandler()));
                })
                .ConfigureApplicationPartManager(pm =>
                {
                    pm.FeatureProviders.Add(new TestProvider());
                });

            builder.Services.AddMvc(options =>
            {
                options.Conventions.Add(new GenericEdmOperationRouteConvention(model));
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseODataBatching();

            app.UseAuthorization();

            app.UseODataRouteDebug();

            app.MapControllers();

            app.Run();
        }
    }
}
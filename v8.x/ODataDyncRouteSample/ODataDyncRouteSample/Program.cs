using Microsoft.AspNetCore.OData;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;

using ODataDyncRouteSample.Models;
using ODataDyncRouteSample.Routing;

namespace ODataDyncRouteSample
{
    public class Program
    {
        public static void Main(string[] args)
        {
            ODataConventionModelBuilder modelBuilder = new ODataConventionModelBuilder();
            modelBuilder.EntitySet<AzureUpdate>("Azure");
            modelBuilder.EntitySet<M365Roadmap>("M365");
            IEdmModel edmModel = modelBuilder.GetEdmModel();

            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services
                .AddMvc(o => o.Conventions.Add(new CommsEntityControllerModelConvention(edmModel)))
                .ConfigureApplicationPartManager(manager =>
                {
                    manager.FeatureProviders.Add(new CommsEntityControllerFeatureProvider());
                })
                .AddOData(options =>
                {
                    options.Filter().Count().Select().OrderBy()
                           .AddRouteComponents(
                               "api/v2",
                               edmModel);
                });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            app.UseODataRouteDebug();

            app.MapControllers();

            app.Run();
        }
    }
}

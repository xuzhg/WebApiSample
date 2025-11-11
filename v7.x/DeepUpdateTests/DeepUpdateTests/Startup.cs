using DeepUpdateTests.Models;
using Microsoft.AspNet.OData.Batch;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNet.OData.Formatter.Deserialization;
using Microsoft.AspNet.OData.Routing.Conventions;
using Microsoft.OData;
using Microsoft.OData.Edm;

namespace DeepUpdateTests;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
      //  services.AddDbContext<CustomerOrderContext>(opt => opt.UseLazyLoadingProxies().UseInMemoryDatabase("CustomerOrderList"));
        services.AddOData();
        services.AddRouting();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        IEdmModel model = EdmModelBuilder.GetEdmModel();

        // Please add "UseODataBatching()" before "UseRouting()" to support OData $batch.
        app.UseODataBatching();

        app.UseRouting();

        app.UseEndpoints(endpoints =>
        {
            //endpoints.MapODataRoute("odataPrefix", "odata", builder =>
            //{
            //    builder.AddService(Microsoft.OData.ServiceLifetime.Singleton, sp => model);

            //    builder.AddService(Microsoft.OData.ServiceLifetime.Singleton, sp => new ODataMessageWriterSettings
            //    {
            //        EnableMessageStreamDisposal = false,
            //        MessageQuotas = new ODataMessageQuotas { MaxReceivedMessageSize = Int64.MaxValue },
            //        Version = ODataVersion.V401
            //    });

            //    builder.AddService<IEnumerable<IODataRoutingConvention>>(Microsoft.OData.ServiceLifetime.Singleton,
            //                sp => ODataRoutingConventions.CreateDefaultWithAttributeRouting("odataPrefix", endpoints.ServiceProvider));
            //});

            endpoints.MapODataRoute("odataPrefix", "odata", model);
        });
    }
}

using DeepUpdateTests.Models;
using Microsoft.AspNet.OData.Batch;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNet.OData.Formatter.Deserialization;
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
            endpoints.MapODataRoute("odataPrefix", "odata", model);
        });
    }
}

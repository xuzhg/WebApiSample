using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.OData.Edm;
using Microsoft.AspNet.OData.Builder;
using System;
using BasicWebApiSample.Models;
using Microsoft.EntityFrameworkCore;

namespace BasicWebApiSample
{
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
           // services.AddODataQueryFilter();
            services.AddDbContext<CustomerOrderContext>(opt => opt.UseInMemoryDatabase("CustomerOrdersList"));
            services.AddOData();
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            var model = GetEdmModel(app.ApplicationServices);

            app.UseMvc(builder =>
            {
                builder.Filter().Expand().Select().Count().OrderBy().MaxTop(null);

                builder.MapODataServiceRoute("odata", "odata", model);

                builder.MapODataServiceRoute("odata1", "inmem", model);
            });
        }

        private static IEdmModel GetEdmModel(IServiceProvider servicePrivider)
        {
            var b = new ODataConventionModelBuilder(servicePrivider);
            b.EntitySet<Customer>("Customers");
            b.EntitySet<Order>("Orders");
            return b.GetEdmModel();
        }
    }
}

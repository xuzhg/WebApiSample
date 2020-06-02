using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNet.OData.Routing.Conventions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OData;
using Microsoft.OData.Edm;

namespace Issue2187
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
            services.AddControllers();
            services.AddOData();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapODataRoute("odata", "odata",
                    b =>
                    {
                        b.AddService(Microsoft.OData.ServiceLifetime.Singleton,
                          sp => GetEdmModel());

                        b.AddService<IEnumerable<IODataRoutingConvention>>(Microsoft.OData.ServiceLifetime.Singleton,
                          sp => ODataRoutingConventions.CreateDefaultWithAttributeRouting("odata", endpoints.ServiceProvider));
                    });
            });
        }

        private static IEdmModel GetEdmModel()
        {
            ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
            // Add a global function
            var funcSayAnything = builder.Function("SayAnything");
            funcSayAnything.Namespace = "Bar";
            funcSayAnything.Parameter<string>("value");
            funcSayAnything.Parameter<int>("repeat");

            // funcSayAnything.Returns<IActionResult>();
            funcSayAnything.Returns<string>();
            return builder.GetEdmModel();
        }
    }
}

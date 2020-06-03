using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using issue2045.Models;
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

namespace issue2045
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
                endpoints.MapControllers();
                endpoints.MapODataRoute("odata", "odata", GetEdmModel());
            });
        }

        private static IEdmModel GetEdmModel()
        {
            var builder = new ODataConventionModelBuilder();
            builder.EntitySet<InspectionDuty>("InspectionDuty");

            // Defined a bound action bounded to the collection
            var action = builder.EntityType<InspectionDuty>().Collection.Action("SingleChange");
            action.Parameter<string>("Comment");
            action.Parameter<InspectionDutyChange>("Change");

            // Defined an unbound action
            // unbound action should define from builder
            var unboundAction = builder.Action("UnboundSingleChange");
            unboundAction.Parameter<string>("Comment");
            unboundAction.Parameter<InspectionDutyChange>("Change");

            return builder.GetEdmModel();
        }
    }
}

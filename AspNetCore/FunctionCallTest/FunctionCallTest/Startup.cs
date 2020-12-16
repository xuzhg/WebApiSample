using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.OData.Batch;
using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNet.OData.Routing.Conventions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OData.Edm;

namespace FunctionCallTest
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
            services.AddOData();
            services.AddMvc(opt =>
            {
                //opt.Conventions.Add(new EntityControllerRouteConvention());
                //opt.Conventions.Add(new AADAuthorizeAttributeConvention(ApplicationRoleType.OData));
                opt.EnableEndpointRouting = false;
            }).AddControllersAsServices()
            .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var modelBuilder = GetEdmModel();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Create the default collection of built-in conventions.
            var conventions = ODataRoutingConventions.CreateDefault();

            // Insert the custom convention at the start of the collection.
            //conventions.Insert(0, new PropertyValueRoutingConvention());
            //conventions.Insert(1, new CountODataRoutingConvention());
            app.UseODataBatching();

            // var batchHandler = app.ApplicationServices.GetRequiredService<AtomicODataBatchHandler<EngagementEntities>>();
            var batchHandler = new DefaultODataBatchHandler();
            var route = env.IsDevelopment() ? "odata/v2" : string.Empty;
            app.UseMvc(builder =>
            {
                TimeZoneInfo timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("UTC");
                builder.Select().Expand().Filter().OrderBy().MaxTop(100).Count().SetTimeZoneInfo(timeZoneInfo);
                builder.MapODataServiceRoute("odata", route, modelBuilder, new DefaultODataPathHandler(), conventions, batchHandler);
            });
        }

        private static IEdmModel GetEdmModel()
        {
            var builder = new ODataConventionModelBuilder();
            builder.EntitySet<Engagement>("Engagement");

            var engagementType = builder.EntityType<Engagement>();
            var function = engagementType.Function("GetEngagementAlertsMetrics").ReturnsCollection<EngagementMetricsResult>();
            function.Parameter<int>("engagementId");
            function.Parameter<Guid>("userId");
            function.Parameter<int>("days");

            return builder.GetEdmModel();
        }
    }

    public class Engagement
    {
        public int EngagementID { get; set; }
    }

    public class EngagementMetricsResult
    {
        public int EngagementId { get; set; }
    }
}

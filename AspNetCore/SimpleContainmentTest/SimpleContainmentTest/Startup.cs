using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OData.Edm;

namespace SimpleContainmentTest
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseOData(GetEdmModel());
        }

        private static IEdmModel GetEdmModel()
        {
            var b = new ODataConventionModelBuilder();
            // var eventType = b.EntitySet<Event>("Events").EntityType;
            var eventType = b.EntityType<Event>();
            eventType.ContainsOptional(e => e.Response);
         //   eventType.ContainsRequired(e => e.Response2);
            return b.GetEdmModel();
        }
    }

    public class Event
    {
        public int Id { get; set; }

        public EventResponse Response { get; set; }

        public EventResponse2 Response2 { get; set; }

    }

    public class EventResponse
    {
        [Key]
        public int MyKey { get; set; }
    }

    public class EventResponse2
    {
        public int MyKey { get; set; }
    }
}

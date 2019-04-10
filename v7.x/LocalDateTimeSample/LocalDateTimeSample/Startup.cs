using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using LocalDateTimeSample.Models;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OData.Edm;

namespace LocalDateTimeSample
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
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            var model = GetEdmModel();
            app.UseMvc(builder =>
            {
                builder.Select().Expand().Filter().OrderBy().MaxTop(100).Count();

                builder.MapODataServiceRoute("odata", "odata", model);
            });
        }

        public static IEdmModel GetEdmModel()
        {
            var builder = new ODataConventionModelBuilder();
            var customer = builder.EntitySet<Customer>("Customers").EntityType;

            var type = builder.AddEntityType(typeof(Customer));

            IList<PropertyInfo> removedProperties = new List<PropertyInfo>();
            foreach (var propertyInfo in typeof(Customer).GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance))
            {
                if (propertyInfo.PropertyType == typeof(DateTime) ||
                    propertyInfo.PropertyType == typeof(DateTime?)) // add more codes to process collection
                {
                    type.RemoveProperty(propertyInfo);
                    removedProperties.Add(propertyInfo);
                }
            }
            // customer.Ignore(c => c.ReleaseDate);

            EdmModel model = builder.GetEdmModel() as EdmModel;

            EdmEntityType customerType = model.SchemaElements.OfType<IEdmEntityType>().First(c => c.Name == "Customer") as EdmEntityType;
            var localDateTime = model.FindType("Org.OData.Core.V1.LocalDateTime") as IEdmTypeDefinition;

            var localDateTimeNullableRef = new EdmTypeDefinitionReference(localDateTime, true);
            var localDateTimenonNullableRef = new EdmTypeDefinitionReference(localDateTime, false);
            foreach (var propertyInfo in removedProperties)
            {
                EdmProperty edmProperty = null;
                if (propertyInfo.PropertyType == typeof(DateTime))
                {
                    edmProperty = customerType.AddStructuralProperty(propertyInfo.Name, localDateTimenonNullableRef);
                }
                else if (propertyInfo.PropertyType == typeof(DateTime?))
                {
                    edmProperty = customerType.AddStructuralProperty(propertyInfo.Name, localDateTimeNullableRef);
                }

                if (edmProperty != null)
                {
                    model.SetAnnotationValue(edmProperty, new ClrPropertyInfoAnnotation(propertyInfo));
                }
            }

            LocalDateTimeConverter converter = new LocalDateTimeConverter();
            model.SetPrimitiveValueConverter(localDateTimeNullableRef, converter);
            model.SetPrimitiveValueConverter(localDateTimenonNullableRef, converter);

            return model;
        }
    }
}

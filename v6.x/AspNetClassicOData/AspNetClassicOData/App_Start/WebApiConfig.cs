using AspNetClassicOData.Extensions;
using AspNetClassicOData.Models;
using Microsoft.OData;
using Microsoft.OData.Edm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.OData.Extensions;
using System.Web.OData.Formatter.Serialization;
using System.Web.OData.Routing.Conventions;

namespace AspNetClassicOData
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            //// Web API routes
            //config.MapHttpAttributeRoutes();

            //config.Routes.MapHttpRoute(
            //    name: "DefaultApi",
            //    routeTemplate: "api/{controller}/{id}",
            //    defaults: new { id = RouteParameter.Optional }
            //);


            IEdmModel model = ModelBuilder.GetEdmModel();

            config.MapODataServiceRoute("odata", "odata", builder =>
                builder.AddService(ServiceLifetime.Singleton, sp => model)
                    .AddService<ODataSerializerProvider, ShowHiddenSerializerProvider>(ServiceLifetime.Singleton)
                       .AddService<IEnumerable<IODataRoutingConvention>>(ServiceLifetime.Singleton, sp =>
                           ODataRoutingConventions.CreateDefaultWithAttributeRouting("odata", config)));
        }
    }
}

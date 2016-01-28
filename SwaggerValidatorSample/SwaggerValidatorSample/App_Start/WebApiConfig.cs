using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.OData.Builder;
using System.Web.OData.Extensions;
using System.Web.OData.Routing;
using System.Web.OData.Routing.Conventions;
using Microsoft.OData.Edm;
using SwaggerValidatorSample.Swagger;

namespace SwaggerValidatorSample
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            IODataPathHandler handler = new SwaggerPathHandler();
            IList<IODataRoutingConvention> conventions = ODataRoutingConventions.CreateDefault();
            conventions.Insert(0, new SwaggerRoutingConvention());
            config.MapODataServiceRoute("odata", "odata", GetEdmModel(), handler, conventions);
        }

        private static IEdmModel GetEdmModel()
        {
            var builder = new ODataConventionModelBuilder();
            builder.EntitySet<Customer>("Customers");
            builder.EntitySet<Order>("Orders");
            builder.Function("UnboundFunction").Returns<string>().Parameter<int>("param");
            builder.Action("UnboundAction").Parameter<double>("param");
            builder.EntityType<Customer>().Function("BoundFunction").Returns<double>().Parameter<string>("name");
            return builder.GetEdmModel();
        }
    }

    public class Customer
    {
        public int CustomerId { get; set; }

        public Order Order { get; set; }
    }

    public class Order
    {
        public int OrderId { get; set; }
    }
}

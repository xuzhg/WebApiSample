using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.OData.Builder;
using System.Web.OData.Extensions;
using Microsoft.OData.Edm;

namespace ODataActionSample
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapODataServiceRoute("odata", "odata", GetEdmModel());
        }

        private static IEdmModel GetEdmModel()
        {
            var builder = new ODataConventionModelBuilder();
            builder.EntitySet<Customer>("Customers");

            // add actions
            builder.EntitySet<Customer>("Customers")
                .EntityType.Collection.Action("IsEmailAvailable")
                .Returns<bool>()
                .Parameter<string>("email");

            builder.EntitySet<Customer>("Customers").EntityType.Action("IsEmailAvailable").Returns<bool>()
                .Parameter<string>("email");

            // add the complex type
            builder.ComplexType<Address>();

            builder.Namespace = "Extra";
            return builder.GetEdmModel();
        }
    }
}

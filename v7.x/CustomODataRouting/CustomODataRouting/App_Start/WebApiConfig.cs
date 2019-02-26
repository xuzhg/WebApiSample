using CustomODataRouting.Extensions;
using CustomODataRouting.Models;
using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNet.OData.Routing.Conventions;
using Microsoft.OData.Edm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace CustomODataRouting
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Expand();
            IEdmModel model = GetEdmModel();
            IList<IODataRoutingConvention> conventions = ODataRoutingConventions.CreateDefaultWithAttributeRouting("odata", config);
            conventions.Insert(0, new MyNavigationPropertyRoutingConvention());

            config.MapODataServiceRoute("odata", "odata", model, new DefaultODataPathHandler(), conventions);
        }

        private static IEdmModel GetEdmModel()
        {
            var builder = new ODataConventionModelBuilder();
            builder.EntitySet<Message>("Messages");
            return builder.GetEdmModel();
        }
    }
}

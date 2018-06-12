using BasicWebApiFxSample.Models;
using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNet.OData.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace BasicWebApiFxSample
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            var builder = new ODataConventionModelBuilder();
            builder.EntitySet<Operation>("Operations");
            config.Filter();
            config.MapODataServiceRoute("odata", null, builder.GetEdmModel());
        }
    }
}

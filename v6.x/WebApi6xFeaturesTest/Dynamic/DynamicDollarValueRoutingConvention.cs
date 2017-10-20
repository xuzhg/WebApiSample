using Microsoft.OData.UriParser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.OData.Routing;
using System.Web.OData.Routing.Conventions;
using ODataPath = System.Web.OData.Routing.ODataPath;

namespace WebApi6xFeaturesTest.Dynamic
{
    public class DynamicDollarValueRoutingConvention : DynamicPropertyRoutingConvention
    {
        public override string SelectAction(ODataPath odataPath, HttpControllerContext controllerContext,
            ILookup<string, HttpActionDescriptor> actionMap)
        {
            string pathTemplate = odataPath.PathTemplate;

            if (controllerContext.Request.Method == HttpMethod.Get &&
                pathTemplate.EndsWith("/dynamicproperty/$value"))
            {
                ODataPath newPath = new ODataPath(odataPath.Segments.TakeWhile(e => !(e is ValueSegment)));
                return base.SelectAction(newPath, controllerContext, actionMap);
            }

            return null;
        }
    }
}

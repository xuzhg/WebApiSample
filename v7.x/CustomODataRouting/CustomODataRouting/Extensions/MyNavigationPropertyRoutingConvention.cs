using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNet.OData.Routing.Conventions;
using Microsoft.OData.UriParser;
using System.Linq;
using System.Net.Http;
using System.Web.Http.Controllers;
using ODataPath = Microsoft.AspNet.OData.Routing.ODataPath;

namespace CustomODataRouting.Extensions
{
    public class MyNavigationPropertyRoutingConvention : NavigationRoutingConvention
    {
        public override string SelectAction(ODataPath odataPath, HttpControllerContext controllerContext, ILookup<string, HttpActionDescriptor> actionMap)
        {
            if (odataPath == null || controllerContext == null || actionMap == null)
            {
                return null;
            }

            if (controllerContext.Request.Method != HttpMethod.Put)
            {
                return null;
            }

            if (odataPath.PathTemplate != "~/entityset/key/navigation")
            {
                return null;
            }

            KeySegment keySegment = odataPath.Segments[1] as KeySegment;
            controllerContext.RouteData.Values[ODataRouteConstants.Key] = keySegment.Keys.First().Value;
           
            NavigationPropertySegment navSegment = odataPath.Segments[2] as NavigationPropertySegment;

            return "PutTo" + navSegment.NavigationProperty.Name;
        }
    }
}
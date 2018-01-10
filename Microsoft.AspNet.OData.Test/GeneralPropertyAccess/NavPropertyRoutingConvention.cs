using System.Linq;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.OData.Routing;
using System.Web.OData.Routing.Conventions;
using Microsoft.OData.Core;
using Microsoft.OData.Core.UriParser;
using Microsoft.OData.Edm;

namespace Microsoft.AspNet.OData.Test.GeneralPropertyAccess
{
    public class NavPropertyRoutingConvention : NavigationRoutingConvention
    {
        public override string SelectAction(ODataPath odataPath, HttpControllerContext controllerContext, ILookup<string, HttpActionDescriptor> actionMap)
        {
            HttpMethod httpMethod = controllerContext.Request.Method;
            if (httpMethod != HttpMethod.Get)
            {
                return null;
            }

            if (odataPath.PathTemplate == "~/entityset/key/navigation/key")
            {
                KeyValuePathSegment segment = (KeyValuePathSegment)odataPath.Segments[1];
                object value = ODataUriUtils.ConvertFromUriLiteral(segment.Value, ODataVersion.V4);
                controllerContext.RouteData.Values.Add("key", value);

                segment = (KeyValuePathSegment)odataPath.Segments[3];
                value = ODataUriUtils.ConvertFromUriLiteral(segment.Value, ODataVersion.V4);
                controllerContext.RouteData.Values.Add("relatedKey", value);

                NavigationPathSegment property = odataPath.Segments[2] as NavigationPathSegment;
                // controllerContext.RouteData.Values.Add("propertyName", property.NavigationProperty.Name);

                return "Get" + property.NavigationProperty.Name;
            }

            return null;
        }
    }
}

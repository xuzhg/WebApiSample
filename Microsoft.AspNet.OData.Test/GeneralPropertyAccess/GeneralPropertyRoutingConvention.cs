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
    public class GeneralPropertyRoutingConvention : PropertyRoutingConvention
    {
        public override string SelectAction(ODataPath odataPath, HttpControllerContext controllerContext, ILookup<string, HttpActionDescriptor> actionMap)
        {
            HttpMethod httpMethod = controllerContext.Request.Method;
            if (httpMethod != HttpMethod.Get)
            {
                return null;
            }

            if (odataPath.PathTemplate == "~/entityset/key/property")
            {
                KeyValuePathSegment segment = (KeyValuePathSegment)odataPath.Segments[1];
                object value = ODataUriUtils.ConvertFromUriLiteral(segment.Value, ODataVersion.V4);
                controllerContext.RouteData.Values.Add("key", value);

                PropertyAccessPathSegment property = odataPath.Segments.Last() as PropertyAccessPathSegment;
                controllerContext.RouteData.Values.Add("propertyName", property.PropertyName);

                return "ReturnPropertyValue";
            }

            return null;
        }
    }
}

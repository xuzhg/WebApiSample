using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.OData.Routing;
using System.Web.OData.Routing.Conventions;

namespace UnboundFunctionConventionRouting.Extensions
{
    public class UnboundConventionRoutingConvention : IODataRoutingConvention
    {
        public string SelectController(ODataPath odataPath, HttpRequestMessage request)
        {
            if (odataPath == null || request == null)
            {
                return null;
            }

            if (request.Method == HttpMethod.Get)
            {
                if (odataPath.PathTemplate == "~/unboundfunction")
                {
                    return "Any";
                }
            }

            return null;
        }

        public string SelectAction(ODataPath odataPath, HttpControllerContext controllerContext, ILookup<string, HttpActionDescriptor> actionMap)
        {
            if (odataPath == null || controllerContext == null || actionMap == null)
            {
                return null;
            }

            if (controllerContext.Request.Method != HttpMethod.Get)
            {
                return null;
            }

            if (odataPath.PathTemplate != "~/unboundfunction")
            {
                return null;
            }

            string actionName = null;
            UnboundFunctionPathSegment function = null;
            function = odataPath.Segments.First() as UnboundFunctionPathSegment;
            if (function == null)
            {
                return null;
            }

            actionName = function.FunctionName;
            AddKeyValueToRouteData(controllerContext, odataPath);

            // how to add the parameter values;

            return actionName;
        }

        public static void AddKeyValueToRouteData(HttpControllerContext controllerContext, ODataPath odataPath)
        {
            KeyValuePathSegment keyValueSegment = odataPath.Segments[1] as KeyValuePathSegment;
            if (keyValueSegment != null)
            {
                controllerContext.RouteData.Values[ODataRouteConstants.Key] = keyValueSegment.Value;
            }
        }
    }
}

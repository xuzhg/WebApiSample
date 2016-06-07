using System;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.OData.Builder;
using System.Web.OData.Extensions;
using System.Web.OData.Routing;
using System.Web.OData.Routing.Conventions;
using Microsoft.OData.Core;
using Microsoft.OData.Edm;

namespace Microsoft.AspNet.OData.Test.Function.CustomFunctionResolve
{
    public class CustomControllerRoutingConvention : NavigationSourceRoutingConvention
    {
        public override string SelectAction(ODataPath odataPath, HttpControllerContext context,
            ILookup<string, HttpActionDescriptor> actionMap)
        {
            string action = null;

            if (odataPath.PathTemplate == "~/entityset/function")
            {
                var segment = odataPath.Segments[odataPath.Segments.Count - 1] as BoundFunctionPathSegment;
                action = segment.Function.Name;
                AddFunctionParametersToRouteData(context, segment);
            }

            return action;
        }

        private static void AddFunctionParametersToRouteData(HttpControllerContext controllerContext,
            BoundFunctionPathSegment functionSegment)
        {
            // skip the first binding parameter
            for (int i = 1; i < functionSegment.Function.Parameters.Count(); i++)
            {
                var param = functionSegment.Function.Parameters.ElementAt(i);
                string name = param.Name;

                object value = functionSegment.GetParameterValue(name);
                ODataEnumValue enumValue = value as ODataEnumValue;
                if (enumValue != null)
                {
                    // Remove the type name of the ODataEnumValue and keep the value.
                    value = enumValue.Value;
                }

                controllerContext.RouteData.Values.Add(name, value);

                Type type = typeof(ODataConventionModelBuilder).Assembly.GetTypes().First(c => c.Name == "ODataParameterValue");
                object instance = Activator.CreateInstance(type, value, param.Type);
                controllerContext.Request.ODataProperties().RoutingConventionsStore.Add("DF908045-6922-46A0-82F2-2F6E7F43D1B1_" + name, instance);
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.OData.Extensions;
using System.Web.OData.Routing;
using System.Web.OData.Routing.Conventions;
using Microsoft.OData.Core;
using Microsoft.OData.Core.UriParser;
using Microsoft.OData.Edm;
using Sematic = Microsoft.OData.Core.UriParser.Semantic;

namespace Microsoft.AspNet.OData.Test.CompositeKey
{
    public class CompositeKeyRoutingConvention : NavigationSourceRoutingConvention
    {
        public override string SelectAction(ODataPath odataPath, HttpControllerContext controllerContext, ILookup<string, HttpActionDescriptor> actionMap)
        {
            if (odataPath.PathTemplate == "~/entityset/key")
            {
                KeyValuePathSegment segment = (KeyValuePathSegment) odataPath.Segments.Last();
                var keyValues = segment.Value.Split(',');
                if (keyValues.Length <= 1)
                {
                    return null; // let other conventions to process
                }

                foreach (var item in keyValues)
                {
                    var keyValue = item.Split('=');
                    if (keyValue.Length != 2)
                    {
                        continue;
                    }
                    string key = keyValue[0].Trim();
                    string valueString = keyValue[1].Trim();

                    object value = ODataUriUtils.ConvertFromUriLiteral(valueString, ODataVersion.V4);
                    controllerContext.RouteData.Values.Add("key" + key, value); // use a prefix
                }

                /*
                IEdmModel model = controllerContext.Request.ODataProperties().Model;

                string a = controllerContext.Request.GetUrlHelper().CreateODataLink(odataPath.Segments);
                //Uri b = new Uri();

                ODataUriParser parser = new ODataUriParser(model, new Uri(a, UriKind.Absolute));
                Sematic.ODataPath path = parser.ParsePath();
                Sematic.KeySegment keySegment = path.LastSegment as Sematic.KeySegment;
                foreach (var key in keySegment.Keys)
                {
                    controllerContext.RouteData.Values.Add("key" + key.Key, key.Value);
                }*/

                HttpMethod httpMethod = controllerContext.Request.Method;
                string httpMethodName;

                switch (httpMethod.ToString().ToUpperInvariant())
                {
                    case "GET":
                        httpMethodName = "Get";
                        break;
                    case "PUT":
                        httpMethodName = "Put";
                        break;
                    case "PATCH":
                    case "MERGE":
                        httpMethodName = "Patch";
                        break;
                    case "DELETE":
                        httpMethodName = "Delete";
                        break;
                    default:
                        return null;
                }

                IEdmEntityType entityType = odataPath.EdmType as IEdmEntityType;
                string actionName = FindMatchingAction(actionMap,
                    httpMethodName + entityType.Name,
                    httpMethodName);

                return actionName;
            }

            return null;
        }

        public static string FindMatchingAction(ILookup<string, HttpActionDescriptor> actionMap, params string[] targetActionNames) 
         { 
             foreach (string targetActionName in targetActionNames) 
             { 
                 if (actionMap.Contains(targetActionName)) 
                 { 
                     return targetActionName; 
                 } 
             } 
             return null; 
         } 

    }
}

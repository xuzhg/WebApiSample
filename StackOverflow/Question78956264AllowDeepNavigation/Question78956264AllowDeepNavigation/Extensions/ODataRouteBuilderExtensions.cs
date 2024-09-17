using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.OData;
using Microsoft.AspNetCore.OData.Abstracts;
using Microsoft.AspNetCore.OData.Extensions;
using Microsoft.AspNetCore.OData.Routing.Parser;
using Microsoft.AspNetCore.OData.Routing.Template;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;
using Microsoft.OData.Edm;
using Microsoft.OData.UriParser;
using Question78956264AllowDeepNavigation.Controllers;

namespace Question78956264AllowDeepNavigation.Extensions;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class BrowserPropertyAttribute : Attribute { }

public class MyActionRoutingConvention : Attribute, IActionModelConvention
{
    public MyActionRoutingConvention(string prefix, IEdmModel model)
    {
        Prefix = prefix;
        Model = model;
    }

    public string Prefix { get; }
    public IEdmModel Model { get; }

    public void Apply(ActionModel action)
    {
        var hasAttributeRouting = action.Selectors.Any(s => s.AttributeRouteModel != null);
        if (hasAttributeRouting)
        {
            return;
        }

        if (!action.Attributes.Any(a => a is BrowserPropertyAttribute))
        {
            return;
        }

        ODataPathTemplate template = new ODataPathTemplate
        {
            new BrowserPropertySegment(Prefix)
        };

        action.AddSelector("get", Prefix, Model, template);
    }
}


public class MyODataFeature
{
    public string RouteName { get; set; }

    public IEdmModel Model { get; set; }
}

public static class ODataRouteBuilderExtensions
{
    public static readonly string ODataPathTemplate = "{**odataPath:odataconstraint}";
   // public static readonly string ODataPathTemplate = "{odataPath:odataconstraint}";

    public static IEndpointRouteBuilder MapODataServiceRoute(this IEndpointRouteBuilder endpoints,
        string routeName,
        string routePrefix,
        IEdmModel model)
    {
        routePrefix = routePrefix ?? string.Empty;

        endpoints.ServiceProvider.GetService<IOptions<ODataOptions>>()?.Value.AddRouteComponents(routePrefix, model);

        //endpoints.MapControllerRoute(routeName, GetRouteTemplate(routePrefix),
        //    constraints: new { odataconstraint = new ODataPathConstraint(routeName, model) });

        string pattern = GetRouteTemplate(routePrefix);

        endpoints.MapControllerRoute(routeName,
            pattern,
            defaults: new { controller = "People", action = "List", ODataRoutePrefix = routePrefix, ODataRouteName = routeName, ODataModel = model });

        return endpoints;
    }

    public static MyODataFeature MyODataFeature(this HttpContext httpContext)
    {
        MyODataFeature? odataFeature = httpContext.Features.Get<MyODataFeature>();
        if (odataFeature == null)
        {
            odataFeature = new MyODataFeature();
            httpContext.Features.Set(odataFeature);
        }

        return odataFeature;
    }

    private static string GetRouteTemplate(string prefix)
    {
        return string.IsNullOrEmpty(prefix) ?
            ODataPathTemplate :
            prefix + '/' + ODataPathTemplate;
    }
}

public class ODataPathConstraint : IRouteConstraint
{
    public ODataPathConstraint()
    {
        int a = 0;
    }
    public ODataPathConstraint(string routeName, IEdmModel model)
    {
        RouteName = routeName;
        Model = model;
    }

    public string RouteName { get; }
    public IEdmModel Model { get; }


    public bool Match(HttpContext? httpContext, IRouter? route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
    {
        if (routeDirection == RouteDirection.IncomingRequest)
        {
            object oDataPathValue;
            if (!values.TryGetValue("odataPath", out oDataPathValue))
            {
                return false;
            }

            string routePrefix = values["ODataRoutePrefix"] as string;
            string routeName = values["ODataRouteName"] as string;
            IEdmModel oDataModel = values["ODataModel"] as IEdmModel;

            httpContext.ODataFeature().RoutePrefix = routePrefix ?? string.Empty;
            IServiceProvider sp = httpContext.Request.GetRouteServices();

            string odataPath = oDataPathValue as string;
            ODataUriParser parser = new ODataUriParser(oDataModel, new Uri(odataPath, UriKind.Relative), sp);
            try
            {
                ODataPath path = parser.ParsePath();

                httpContext.ODataFeature().Model = oDataModel;
                httpContext.ODataFeature().Path = path;

                DefaultRoutingConvention convention = new DefaultRoutingConvention();
                if (convention.SelectAction(httpContext, values, out string controllerName, out string actionName))
                {
                    values["controller"] = controllerName;
                    values["action"] = actionName;
                    return true;
                }

                return false;

            }
            catch
            {
                return false;
            }
        }

        return false;
    }
}

public interface IODataRoutingConvention
{
    bool SelectAction(HttpContext context, RouteValueDictionary values, out string controller, out string action);
}

public class DefaultRoutingConvention : IODataRoutingConvention
{
    public bool SelectAction(HttpContext context, RouteValueDictionary values, out string controller, out string action)
    {
        controller = null;
        action = null;

        //context.Request.Method == "GET"

        ODataPath path = context.ODataFeature().Path;

        ODataPathSegment fistSegment = path.FirstOrDefault();
        if (fistSegment == null) { return false; }

        if (fistSegment is EntitySetSegment entitySet)
        {
            controller = entitySet.EntitySet.Name;
        }

        action = RetrieveSegments(path, values);

        values["controller"] = controller;
        values["action"] = action;

        return controller != null && action != null;
    }

    private string RetrieveSegments(ODataPath path, RouteValueDictionary values)
    {
        IList<SegmentInfo> propertyLists = new List<SegmentInfo>();

        foreach (var segment in path.Skip(1)) // skip the first one because it's processed
        {
            if (segment is KeySegment key)
            {
                var keyValue = key.Keys.First(); // for simplicity, only take care of one key scenario
                IntKeySegmentInfo keySeg = new IntKeySegmentInfo
                {
                    KeyName = keyValue.Key,
                    Value = (int)keyValue.Value
                };
                propertyLists.Add(keySeg);
            }
            else if (segment is PropertySegment prop)
            {
                PropertySegmentInfo seg = new PropertySegmentInfo
                {
                    PropertyName = prop.Property.Name
                };
            }
            else if (segment is NavigationPropertySegment navProp)
            {
                PropertySegmentInfo seg = new PropertySegmentInfo
                {
                    PropertyName = navProp.NavigationProperty.Name
                };
            }
            else
            {
                return null;
            }
        }

        values["propertyLists"] = propertyLists;
        return "GetProperties";
    }
}

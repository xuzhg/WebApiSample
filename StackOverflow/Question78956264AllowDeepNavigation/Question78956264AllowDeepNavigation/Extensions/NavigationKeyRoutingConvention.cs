using Microsoft.AspNetCore.OData.Extensions;
using Microsoft.AspNetCore.OData.Routing;
using Microsoft.AspNetCore.OData.Routing.Conventions;
using Microsoft.AspNetCore.OData.Routing.Template;
using Microsoft.OData.Edm;
using Question78956264AllowDeepNavigation.Controllers;

namespace Question78956264AllowDeepNavigation.Extensions;

// There are lots of ways to create the convention.
// For example: Same as here to implement 'IODataControllerActionConvention' and register it into 'Conventions' of ODataOptions
// or, implement 'IControllerModelConvention' or 'IActionModelConvention' and decorate it on the controller or action
public class NavigationKeyRoutingConvention : IODataControllerActionConvention
{
    public virtual int Order => 1;

    public bool AppliesToController(ODataControllerActionContext context)
    {
        return context.Controller.ControllerType == typeof(ServicesController);
    }

    public bool AppliesToAction(ODataControllerActionContext context)
    {
        if (context.Action.ActionName != "GetServiceArticle")
        {
            return false;
        }

        IEdmModel model = context.Model;
        IEdmEntitySet services = model.EntityContainer.FindEntitySet("Services");
        IEdmNavigationProperty navProperty = services.EntityType.FindProperty("ServiceArticle") as IEdmNavigationProperty;
        IEdmEntitySet articles = model.EntityContainer.FindEntitySet("Articles");
        ODataPathTemplate template = new ODataPathTemplate(
            new EntitySetSegmentTemplate(services),
            new KeySegmentTemplate(new Dictionary<string, string>
            {
                { "ServiceId", "{keyServiceId}" }
            },
            services.EntityType,
            services),
            new NavigationSegmentTemplate(navProperty, articles),
            new KeySegmentTemplate(new Dictionary<string, string>
            {
                { "ServiceArticleId", "{keyServiceArticleId}" }
            },
            articles.EntityType,
            articles)
        );

        ODataRouteOptions routeOptions = new ODataRouteOptions
        {
            EnableKeyAsSegment = false
        };
        context.Action.AddSelector("GET", context.Prefix, context.Model, template, routeOptions/*context.Options?.RouteOptions*/);
        return true;
    }
}

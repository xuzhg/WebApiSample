using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.OData.Extensions;
using Microsoft.AspNetCore.OData.Routing.Template;
using Microsoft.OData.Edm;

namespace Question78956264AllowDeepNavigation.Extensions;

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

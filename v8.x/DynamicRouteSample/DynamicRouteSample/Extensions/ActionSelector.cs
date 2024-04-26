using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.OData.UriParser;

namespace DynamicRouteSample.Extensions;

public static class ActionSelector
{
    // Perf? next time or do me a favor.
    public static ControllerActionDescriptor FindBestAction(this IActionDescriptorCollectionProvider actionDescriptors,
        string prefix, ODataPath path, RouteValueDictionary values)
    {
        foreach (var item in actionDescriptors.ActionDescriptors.Items)
        {
            if (item is ControllerActionDescriptor actionDescriptor)
            {
                var matchers = actionDescriptor.EndpointMetadata.OfType<IODataRouteMatcher>();
                foreach (var matcher in matchers)
                {
                    if (matcher.Match(prefix, path, values))
                    {
                        return actionDescriptor;
                    }
                }
            }
        }

        return null;
    }
}

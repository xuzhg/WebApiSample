using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.OData.Abstracts;
using Microsoft.AspNetCore.OData.Extensions;
using Microsoft.OData.Edm;
using Microsoft.OData.UriParser;

namespace DynamicRouteSample.Extensions;

public class ODataRouteTransformer : DynamicRouteValueTransformer
{
    public static readonly string ODataEndpointPath = "ODataEndpointPath";

    private readonly IODataModelProvider _modelProvider;
    private readonly IActionDescriptorCollectionProvider _controllerProvider;

    public ODataRouteTransformer(IODataModelProvider modelProvider, IActionDescriptorCollectionProvider provider)
    {
        _modelProvider = modelProvider;
        _controllerProvider = provider;
    }

    public override async ValueTask<RouteValueDictionary> TransformAsync(HttpContext httpContext, RouteValueDictionary values)
    {
        if (values == null ||
            !values.ContainsKey(ODataEndpointPath) ||
            httpContext.ODataFeature().Path != null)
        {
            return values;
        }

        string odataPath = values[ODataEndpointPath] as string;
        Uri odataUri = new Uri(odataPath, UriKind.Relative);

        ODataState state = (ODataState)(State);
        State = null; // this line seems necessary but I don't know the root cause.

        string prefix = state.Prefix;
        IServiceProvider sp = state.ServiceProvider;
        IEdmModel model = _modelProvider.GetEdmModel(prefix);

        HttpRequest request = httpContext.Request;
        IODataFeature feature = request.ODataFeature();

        ODataUriParser parser = new ODataUriParser(model, odataUri, sp);
        ODataPath path = parser.ParsePath();

        ControllerActionDescriptor actionDescriptor = _controllerProvider.FindBestAction(prefix, path, values);

        if (actionDescriptor != null)
        {
            values["controller"] = actionDescriptor.ControllerName;
            values["action"] = actionDescriptor.ActionName;

            feature.Path = path;
            feature.Services = sp;
            feature.RoutePrefix = prefix;
            feature.Model = model;
            feature.RoutingConventionsStore["odata_action"] = actionDescriptor; // for endpoint policy
            feature.RoutingConventionsStore["odata_prefix"] = prefix; // for controller action to use
        }

        return await ValueTask.FromResult(values);
    }
}

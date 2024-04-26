using Microsoft.AspNetCore.OData;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.OData.Edm;

namespace DynamicRouteSample.Extensions;

public static class DynamicRouteExtensions
{
    public static IServiceCollection AddDynamicOData(this IServiceCollection services)
    {
        services.AddControllers(opt =>
        {
            // Read formatters
            foreach (ODataInputFormatter inputFormatter in ODataInputFormatterFactory.Create().Reverse())
            {
                opt.InputFormatters.Insert(0, inputFormatter);
            }

            // Write formatters
            foreach (ODataOutputFormatter outputFormatter in ODataOutputFormatterFactory.Create().Reverse())
            {
                opt.OutputFormatters.Insert(0, outputFormatter);
            }
        });

        services.AddSingleton<IODataModelProvider, ODataModelProvider>();
        services.AddSingleton<ODataRouteTransformer>();
        services.AddSingleton<MatcherPolicy, ODataEndpointSelectorPolicy>();

        return services;
    }

    public static IEndpointRouteBuilder MapODataRoute(this IEndpointRouteBuilder endpoints,
        string prefix,
        Action<DefaultQueryConfigurations> queryConfigure = null,
        Action<IServiceCollection> configureServices = null)
    {
        string pattern;
        if (string.IsNullOrEmpty(prefix))
        {
            pattern = $"{{**{ODataRouteTransformer.ODataEndpointPath}}}";
        }
        else
        {
            pattern = $"{prefix}/{{**{ODataRouteTransformer.ODataEndpointPath}}}";
        }

        ODataOptions options = new ODataOptions();
        if (queryConfigure != null)
        {
            queryConfigure(options.QueryConfigurations);
        }

        IODataModelProvider modelProvider = endpoints.ServiceProvider.GetRequiredService<IODataModelProvider>();
        IEdmModel model = modelProvider.GetEdmModel(prefix);

        // Create a dummy component to get the route service
        options.AddRouteComponents(prefix, model, configureServices);
        IServiceProvider provider = options.GetRouteServices(prefix);
        ODataState state = new ODataState(prefix, provider);

        endpoints.MapDynamicControllerRoute<ODataRouteTransformer>(pattern, state);
        return endpoints;
    }
}

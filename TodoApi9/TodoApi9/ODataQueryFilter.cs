using Microsoft.Extensions.Logging;
using System.Reflection;

public class ODataQueryFilter : IODataQueryFilter
{
    protected readonly ILogger Logger;
    private readonly string _methodName;

    public ODataQueryFilter()
    { }

    public ODataQueryFilter(ILoggerFactory loggerFactory)
    {
        Logger = loggerFactory.CreateLogger<ODataQueryFilter>();
        _methodName = GetType().Name;
    }

    public virtual async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context,
        EndpointFilterDelegate next)
    {
        var endPoint = context.HttpContext.GetEndpoint();
        if (endPoint is null)
        {
            return await next(context);
        }

        ILoggerFactory loggerFactory = context.HttpContext.RequestServices.GetRequiredService<ILoggerFactory>();
        ILogger logger = loggerFactory.CreateLogger("ODataQuery");
        logger.LogInformation("Begin InvokeAsync...");

        // https://github.com/dotnet/aspnetcore/blob/main/src/Http/Routing/src/RouteEndpointDataSource.cs#L171
        // Add MethodInfo and HttpMethodMetadata(if any) as first metadata items as they are intrinsic to the route much like
        // the pattern or default display name. This gives visibility to conventions like WithOpenApi() to intrinsic route details
        // (namely the MethodInfo) even when applied early as group conventions.
        MethodInfo? methodInfo = endPoint.Metadata.OfType<MethodInfo>().FirstOrDefault();

        methodInfo = methodInfo ?? endPoint.RequestDelegate.Method;

        if (methodInfo is null)
        {
            // So, if you put the OData query filter on RequestDelegate, there's no methodInfo and we will skip the query functionalities
            return await next(context);
        }

        this.Logger.LogInformation($"methodInfo: {methodInfo.Name}");

        logger.LogInformation("Starting InvokeAsync...");

        var odataFilterContext = new ODataQueryFilterInvocationContext { MethodInfo = methodInfo, InvocationContext = context };

        await OnFilterExecutingAsync(odataFilterContext);

        var result = await next(context);

        logger.LogInformation("Ending InvokeAsync...");
        var finalResult = await OnFilterExecutedAsync(result, odataFilterContext);

        return result;
    }

    public virtual async ValueTask OnFilterExecutingAsync(ODataQueryFilterInvocationContext context)
    {
        HttpContext httpContext = context.HttpContext;

        var endpoint = httpContext.GetEndpoint();

        ODataQueryOptions queryOptions = context.InvocationContext.Arguments.FirstOrDefault(a => typeof(ODataQueryOptions).IsAssignableFrom(a.GetType())) as ODataQueryOptions;
        if (queryOptions != null)
        { }
    }

    public virtual async ValueTask<object?> OnFilterExecutedAsync(object? responseValue, ODataQueryFilterInvocationContext context)
    {
        return responseValue;
    }

    protected virtual IEdmModel GetEdmModel()
    {
        // 1 Retrieve the model from ODataFeature if have
        // 2 build the model from the return type.
        return null;
    }
}

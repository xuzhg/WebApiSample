﻿using Microsoft.AspNetCore.Http;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

public static class ODataEndpointFilterExtensions
{
    // It's better to add ODataQueryFilter as early as possible,
    // So, we can do the validation as early as possible,
    // but will do the applyTo as later as possbile.
    public static TBuilder AddODataQueryFilter<TBuilder>(this TBuilder builder, IODataQueryFilter queryFilter) where TBuilder : IEndpointConventionBuilder =>
        builder.AddEndpointFilter(queryFilter);

    public static TBuilder AddODataQueryFilter<TBuilder, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TFilterType>(this TBuilder builder)
        where TBuilder : IEndpointConventionBuilder
        where TFilterType : IODataQueryFilter =>
        builder.AddEndpointFilter<TBuilder, TFilterType>();

    public static RouteHandlerBuilder AddODataQueryFilter<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TFilterType>(this RouteHandlerBuilder builder)
        where TFilterType : IODataQueryFilter =>
        builder.AddEndpointFilter<TFilterType>();

    public static RouteGroupBuilder AddODataQueryFilter<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TFilterType>(this RouteGroupBuilder builder)
        where TFilterType : IODataQueryFilter =>
        builder.AddEndpointFilter<TFilterType>();


    public static TBuilder AddODataQueryFilterFactory<TBuilder>(this TBuilder builder) where TBuilder : IEndpointConventionBuilder
    {
        return builder.AddODataQueryFilterFactory(new ODataQueryFilter());
    }

    public static TBuilder AddODataQueryFilterFactory<TBuilder>(this TBuilder builder, IODataQueryFilter queryFilter) where TBuilder : IEndpointConventionBuilder
    {
        return builder.AddEndpointFilterFactory((filterFactoryContext, next) =>
        {
            MethodInfo methodInfo = filterFactoryContext.MethodInfo;

            return async invocationContext =>
            {
                ILoggerFactory loggerFactory = invocationContext.HttpContext.RequestServices.GetRequiredService<ILoggerFactory>();
                ILogger logger = loggerFactory.CreateLogger("ODataQuery");
                logger.LogInformation("Starting OdataQuery. ..");

                var odataFilterContext = new ODataQueryFilterInvocationContext { MethodInfo = methodInfo, InvocationContext = invocationContext };

                await queryFilter.OnFilterExecutingAsync(odataFilterContext);

                var result = await next(invocationContext);

                logger.LogInformation("Ending OdataQuery. ..");

                return await queryFilter.OnFilterExecutedAsync(result, odataFilterContext);
            };
         });
    }

    //public static TBuilder AddODataQueryFilter<TBuilder>(this TBuilder builder, IEndpointFilter endpointFilter) where TBuilder : IEndpointConventionBuilder
    //{

    //}

    public static TBuilder AddODataQueryFilter<TBuilder>(this TBuilder builder, Func<EndpointFilterInvocationContext, EndpointFilterDelegate, ValueTask<object?>> routeHandlerFilter)
    where TBuilder : IEndpointConventionBuilder
    {
        builder.WithName("ok");
        // builder.Add(b => b.)
        // builder.Finally(null);
        return builder.AddEndpointFilterFactory((routeHandlerContext, next) => (context) => routeHandlerFilter(context, next));
    }

    public static TBuilder UseModel<TBuilder>(this TBuilder builder, IEdmModel model) where TBuilder : IEndpointConventionBuilder
    {
        builder.Add(b =>
        {
            if (b.Metadata.Any(c => c is IEdmModelMetadata))
            {
                foreach (var item in b.Metadata.OfType<IEdmModelMetadata>().ToList())
                {
                    b.Metadata.Remove(item);
                }
            }

            b.Metadata.Add(new EdmModelMetadata(model));
        });

        return builder;
    }
}

public class EdmModel : IEdmModel
{
    public EdmModel(string name)
    {
        Name = name;
    }
    public string Name { get; }
}

public interface IEdmModelMetadata
{
    IEdmModel Model { get; }
}

public sealed class EdmModelMetadata : IEdmModelMetadata
{
    public EdmModelMetadata(IEdmModel model)
    {
        ArgumentNullException.ThrowIfNull(model);

        Model = model;
    }

    public IEdmModel Model { get; }
}
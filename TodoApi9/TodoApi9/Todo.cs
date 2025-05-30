﻿using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.AspNetCore.Routing;
using System.Collections.Generic;
using System.Reflection;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TodoApi9
{
    public class Todo
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public bool IsComplete { get; set; }
    }
}

public interface IEdmModel { string Name { get; } }

public interface IODataQueryFilter : IEndpointFilter
{
    ValueTask OnFilterExecutingAsync(ODataQueryFilterInvocationContext context);

    ValueTask<object?> OnFilterExecutedAsync(object? responseValue, ODataQueryFilterInvocationContext context);
    // ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next);
}


public class ODataQueryOptions
{ }

public class ODataQueryOptions<T> : ODataQueryOptions, IEndpointParameterMetadataProvider, IEndpointMetadataProvider
{
    public static void PopulateMetadata(ParameterInfo parameter, EndpointBuilder builder)
    {
        builder.Metadata.Add(new EdmModelMetadata(new EdmModel("abc")));
    }

    public static void PopulateMetadata(MethodInfo method, EndpointBuilder builder)
    {
        builder.Metadata.Add(new EdmModelMetadata(new EdmModel("abc")));
    }

    public static async ValueTask<ODataQueryOptions<T>> BindAsync(HttpContext context, ParameterInfo parameter)
    {
        return new ODataQueryOptions<T>();
    }
}


public class ODataQueryFilterInvocationContext
{
    /// <summary>
    /// The <see cref="MethodInfo"/> associated with the current route handler, <see cref="RequestDelegate"/> or MVC action.
    /// </summary>
    public required MethodInfo MethodInfo { get; init; }

    /// <summary>
    /// The <see cref="EndpointFilterInvocationContext"/> associated with the current route filter.
    /// </summary>
    public required EndpointFilterInvocationContext InvocationContext { get; set; }

    /// <summary>
    /// Gets the <see cref="HttpContext"/>
    /// </summary>
    public HttpContext HttpContext => InvocationContext.HttpContext;
}

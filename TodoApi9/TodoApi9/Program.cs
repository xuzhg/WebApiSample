using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using TodoApi9;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<TodoDb>(opt => opt.UseInMemoryDatabase("TodoList"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddOpenApi();
WebApplication app = builder.Build();

MethodInfo PopulateMetadataForParameterMethod = typeof(IODataResult2).GetMethod(nameof(IODataResult2.PopulateMetadataForEndpoint), BindingFlags.Public | BindingFlags.Static);

object[] invokeArgs = new object[2];
invokeArgs[0] = null;
invokeArgs[1] = null;
// PopulateMetadataForParameterMethod.MakeGenericMethod(typeof(IODataResult2)).Invoke(null, invokeArgs);

//app.MapGet("null", () => (Todo)null);

app.MapGet("odata", (ODataQueryOptions<Todo> queryOptions) => Results.Extensions.OData(new Todo()));

//app.MapGet("odata2", (ODataQueryOptions<Todo> queryOptions) => Results.Extensions.OData2(new Todo()));

app.MapGet("odata1", () => new ODataResult(new Todo()));

app.MapGet("odata2", () => Results.Json(new Todo()));

app.MapGet("/todos", async (TodoDb dbContext) =>
{
    return TypedResults.Ok(await dbContext.Todos.ToListAsync());
});

var group = app.MapGroup(string.Empty);
group.UseModel(new EdmModel("aa"))
    .AddEndpointFilter(async (efiContext, next) =>
    {
        var endpoint = efiContext.HttpContext.GetEndpoint();
        app.Logger.LogInformation("----Before calling");
        var result = await next(efiContext);
        app.Logger.LogInformation($"----After calling, {result?.GetType().Name}");
        return result;
    }
    );

group.MapGet("v1", () => "hello v1").Finally(v =>
{
    v.Metadata.Add(new Todo());
});
group.MapGet("v2", () => "hello v2").UseModel(new EdmModel("bb"));

group.UseModel(new EdmModel("cc"));

app.MapGet("/request", async (context) =>
{
    var endpoint = context.GetEndpoint();
    await context.Response.WriteAsync("In httpRequest delegate");
    await Task.CompletedTask;
})
    .AddODataQueryFilterFactory()
    .AddODataQueryFilter(new ODataQueryFilter(app.Services.GetRequiredService<ILoggerFactory>()));


var routeHandler = app.MapGet("/", () => "Hello World!")
    .WithSummary("This is a summary.")
    .WithDescription("This is a description.");

routeHandler.WithTags("MyTags");

app.MapOpenApi();

app.MapGet("/todoitems", async (TodoDb db/*, ODataQueryOptions<Todo> odataQuery*/) =>
    {
        app.Logger.LogInformation("----In Handler");
        return await db.Todos.ToListAsync();
    })
    .AddODataQueryFilter(new ODataQueryFilter(app.Services.GetRequiredService<ILoggerFactory>()))
    .AddODataQueryFilterFactory()
    .AddEndpointFilter(async (efiContext, next) =>
    {
        app.Logger.LogInformation("----Before calling");
        var result = await next(efiContext);
        app.Logger.LogInformation($"----After calling, {result?.GetType().Name}");
        return result;
    }
    )
    .AddEndpointFilterFactory((filterFactoryContext, next) =>
    {
        if (filterFactoryContext.MethodInfo != null)
        {
            return async invocationContext =>
            {

                var result = await next(invocationContext);


                return result;
            };
        }
        return invocationContext => next(invocationContext);
    }); ;

app.MapGet("todoitems/complete", async (TodoDb db) =>
    await db.Todos.Where(t => t.IsComplete).ToListAsync())
    .WithOpenApi(b => b);

app.MapGet("/todoitems/{id}", async (int id, TodoDb db) =>
    await db.Todos.FindAsync(id)
        is Todo todo
            ? Results.Ok(todo)
            : Results.NotFound());

app.MapPost("/todoitems", async (Todo todo, TodoDb db) =>
{
    db.Todos.Add(todo);
    await db.SaveChangesAsync();

    return Results.Created($"/todoitems/{todo.Id}", todo);
});

app.MapPut("/todoitems/{id}", async (int id, Todo inputTodo, TodoDb db) =>
{
    var todo = await db.Todos.FindAsync(id);

    if (todo is null) return Results.NotFound();

    todo.Name = inputTodo.Name;
    todo.IsComplete = inputTodo.IsComplete;

    await db.SaveChangesAsync();

    return Results.NoContent();
});

app.MapDelete("/todoitems/{id}", async (int id, TodoDb db) =>
{
    if (await db.Todos.FindAsync(id) is Todo todo)
    {
        db.Todos.Remove(todo);
        await db.SaveChangesAsync();
        return Results.NoContent();
    }

    return Results.NotFound();
}).Finally(c => { });

app.Run();

using DynamicRouteSample.Extensions;
using Microsoft.AspNetCore.OData;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDynamicOData();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseODataRouteDebug();

app.UseAuthorization();

app.MapODataRoute("v1", q => q.EnableSelect = true);
app.MapODataRoute("v2");

app.Run();

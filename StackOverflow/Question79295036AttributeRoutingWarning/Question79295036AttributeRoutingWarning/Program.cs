using Microsoft.AspNetCore.OData;
using Question79295036AttributeRoutingWarning.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddOData(opt => opt.AddRouteComponents("odata", EdmModelBuilder.GetEdmModel()));

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.UseODataRouteDebug();

app.MapControllers();

app.Run();

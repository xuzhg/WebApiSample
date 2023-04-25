using Microsoft.AspNetCore.OData;
using PermissionsAnalytics.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().
    AddOData(opt => opt.EnableQueryFeatures().AddRouteComponents("odata", EdmModelBuilder.GetEdmModel()));

builder.Services.AddSingleton<IMappingService, DefaultMappingService>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseODataRouteDebug();

app.UseAuthorization();

app.MapControllers();

app.Run();

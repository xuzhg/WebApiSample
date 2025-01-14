using CollectionOfCollectionUsingEdmUntyped.Models;
using Microsoft.AspNetCore.OData;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddOData(opt => opt.EnableQueryFeatures().AddRouteComponents("odata", ModelBuilder.GetEdmModel()));

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseODataRouteDebug();

app.UseAuthorization();


app.MapControllers();

app.Run();

using Microsoft.AspNetCore.OData;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using JustRoutingSamples.Models;
using System.Runtime.CompilerServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddOData(opt => opt.AddRouteComponents("odata", GetEdmModel()));

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseODataRouteDebug();

app.UseAuthorization();

app.MapControllers();

app.Run();


static IEdmModel GetEdmModel()
{
    var odataBuilder = new ODataConventionModelBuilder();
    odataBuilder.EntitySet<User>("Users");
    odataBuilder.Singleton<User>("Me").HasManyBinding(u => u.Orders, "Orders"); // this is requird for /me/orders navigation
    odataBuilder.EntitySet<Order>("Orders");


    return odataBuilder.GetEdmModel();
}
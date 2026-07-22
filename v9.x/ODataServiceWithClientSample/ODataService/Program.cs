using Microsoft.AspNetCore.OData;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using ODataService.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddOData(options => options
        .EnableQueryFeatures()
        .AddRouteComponents("odata", GetEdmModel()));

var app = builder.Build();

app.UseRouting();
app.MapControllers();

app.Run();

static IEdmModel GetEdmModel()
{
    var modelBuilder = new ODataConventionModelBuilder();
    modelBuilder.EntitySet<Customer>("Customers");
    modelBuilder.EntitySet<Order>("Orders");
    return modelBuilder.GetEdmModel();
}

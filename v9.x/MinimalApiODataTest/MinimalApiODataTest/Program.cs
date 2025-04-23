using Microsoft.AspNetCore.OData;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.EntityFrameworkCore;
using Microsoft.OData.Edm;
using Microsoft.OData.UriParser;
using MinimalApiODataTest.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<AppDb>(options => options.UseInMemoryDatabase("CustomerOrderLists"));
builder.Services.AddOData(opt => opt.EnableAll());

var app = builder.Build();

app.MakeSureDbCreated();

app.MapGet("/", () => "Welcome to OData Minimal API samples. It's not finished yet. Please share your feedbacks.");

IEdmModel model = EdmModelBuilder.GetEdmModel();

var odataApp = app.MapGroup("odata")
    .WithODataResult()
    .WithODataModel(model)
    .WithODataBaseAddressFactory(h => new Uri("http://localhost:5202/odata"));

odataApp.MapGet("customers", (AppDb db, ODataQueryOptions<Customer> queryOptions)
    => queryOptions.ApplyTo(db.Customers.Include(c => c.Orders)));

// odataApp.MapODataMetadata("$odata", model); // this line has a bug, since WithODataResult() on the group, I will fix it in the next nightly
app.MapODataMetadata("$odata", model);

app.Run();

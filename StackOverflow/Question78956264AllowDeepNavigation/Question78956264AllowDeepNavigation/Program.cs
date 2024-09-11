using Microsoft.AspNetCore.OData;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using Question78956264AllowDeepNavigation;
using Question78956264AllowDeepNavigation.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
IEdmModel model = GetEdmModel();

builder.Services.AddControllers()
    .AddOData(
        opt =>
        {
            opt.AddRouteComponents("convention", model);
            opt.AddRouteComponents("attribute", model);
            opt.Conventions.Add(new NavigationKeyRoutingConvention());
        }
    );

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseODataRouteDebug();

app.MapControllers();

app.Run();


static IEdmModel GetEdmModel()
{
    var builder = new ODataConventionModelBuilder();
    builder.EntitySet<Service>("Services");
    builder.EntitySet<ServiceArticle>("Articles");
    return builder.GetEdmModel();
}
using Microsoft.AspNetCore.OData;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using Question78956264AllowDeepNavigation;
using Question78956264AllowDeepNavigation.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
IEdmModel model = GetEdmModel();

//builder.Services.AddRouting(
//    opt => opt.ConstraintMap.Add("odataconstraint", typeof(ODataPathConstraint)));

builder.Services.AddControllers
    (
        opt => opt.Conventions.Add(new MyActionRoutingConvention("v1.0", model))
    )
    .AddOData
    (
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

//app.MapControllerRoute(
//    name: "people",
//    pattern: "people/{ssn:odataconstraint}",
//  //  constraints: new { ssn = "^\\d{3}-\\d{2}-\\d{4}$", },
//    defaults: new { controller = "People", action = "List" });

//app.MapControllerRoute(
//    name: "odata1",
//    pattern: "v2.0/{**odataPath:odataconstraint}",
//    // constraints: new { routeName = "odata", },
//    defaults: new { controller = "People", action = "List", ODataRoutePrefix = "v2.0", ODataRouteName = "odata", ODataModel = model }
//    // defaults: new { ODataRouteName="odata" }
//    );

//app.MapODataServiceRoute("odata", "v1.0", model);

app.Run();


static IEdmModel GetEdmModel()
{
    var builder = new ODataConventionModelBuilder();
    builder.EntitySet<Service>("Services");
    builder.EntitySet<ServiceArticle>("ServiceArticles");
    builder.EntitySet<Article>("Articles");

    builder.EntityType<Article>().Function("ArticleBoundFunction").Returns<string>();
    return builder.GetEdmModel();
}


using CalculatedPropertyTest.Extensions;
using CalculatedPropertyTest.Models;
using Microsoft.AspNetCore.OData;
using Microsoft.AspNetCore.OData.Query.Expressions;
using Microsoft.EntityFrameworkCore;
using OData.WebApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(
    options => options.UseSqlite("Data Source=app.db"));

builder.Services.AddControllers().AddOData(opt =>
    opt.EnableQueryFeatures()
    .AddRouteComponents(
        "odata",
        EdmModelBuilder.GetEdmModel()
        ,
        services => services.//AddSingleton<ISearchBinder, StudentSearchBinder>().
            AddSingleton<ISelectExpandBinder, SchoolStudentSelectExpandBinder>()
        //    AddSingleton<ODataResourceSerializer, MyResourceSerializer>().
        //    AddSingleton<IFilterBinder, SchoolStudentFilterBinder>()
    ) // End of AddRouteComponents
); // End of AddOData

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MakeSureDbCreated();

app.UseAuthorization();

app.MapControllers();

app.Run();

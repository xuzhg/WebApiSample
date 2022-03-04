using Microsoft.AspNetCore.OData;
using Microsoft.OData.Edm;
using TimeZoneSamples.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var timeZone = TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");

builder.Services.AddControllers()
    .AddOData(opt => opt.EnableQueryFeatures().AddRouteComponents("odata", EdmModelBuilder.GetEdmModel())
        .TimeZone = timeZone);

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseODataRouteDebug();

app.UseAuthorization();

app.MapControllers();

app.Run();

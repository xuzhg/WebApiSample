using Microsoft.AspNetCore.OData;
using SpeakerWebApi.Controllers;
using SpeakerWebApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<ISpeakerSubmissionsRepository, SpeakerSubmissionsRepository>();
builder.Services.AddControllers()
    .AddOData(opt => opt.EnableQueryFeatures().AddRouteComponents("odata", EdmModelBuilder.GetEdmModel()));

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseODataRouteDebug();

app.UseAuthorization();

app.MapControllers();

app.Run();

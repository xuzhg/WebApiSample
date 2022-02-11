using AddTypeAnnotationExtensions.Extensions;
using AddTypeAnnotationExtensions.Models;
using Microsoft.AspNetCore.OData;
using Microsoft.AspNetCore.OData.Formatter.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//builder.Services.AddControllers()
//    .AddOData(opt => opt.AddRouteComponents("odata", ModelBuilder.GetEdmModel(),
//        builder => builder.AddSingleton<IODataSerializerProvider, AddTypeAnnotationSerializerProvider>()));

builder.Services.AddControllers()
    .AddOData(opt => opt.AddRouteComponents("odata", ModelBuilder.GetEdmModel()));

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

app.Run();

using Microsoft.AspNetCore.OData;
using Microsoft.AspNetCore.OData.Formatter.Serialization;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddOData(opt =>
    opt.AddRouteComponents("odata", GetEdmModel(), builder =>
    {
        builder.AddSingleton<IODataSerializerProvider, ODataCustomODataSerializerProvider>();
    })
    );

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseODataRouteDebug();

app.UseAuthorization();

app.MapControllers();

app.Run();


static IEdmModel GetEdmModel()
{
    var builder = new ODataConventionModelBuilder();
    builder.AddComplexType(typeof(TestInfoDto));

    builder.Function("GetTest").ReturnsCollection<TestInfoDto>();
    return builder.GetEdmModel();
}

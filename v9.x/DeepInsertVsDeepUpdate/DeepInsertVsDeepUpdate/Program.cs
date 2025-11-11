using DeepInsertVsDeepUpdate;
using Microsoft.AspNetCore.OData;
using Microsoft.AspNetCore.OData.Formatter.Deserialization;
using Microsoft.OData;
using Microsoft.OData.Edm;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

IEdmModel model = EdmModelBuilder.BuildEdmModel();

InMemoryDataSource dataSource = new InMemoryDataSource();
dataSource.Init();

builder.Services.AddSingleton<IDataSource>(dataSource);

builder.Services.AddControllers()
    .AddOData(options => options
        .AddRouteComponents("odata", model, ODataVersion.V401, services => services.AddSingleton<ODataResourceDeserializer, MyODataResourceDeserializer>())
        .Select()
        .Filter()
        .Expand()
        .OrderBy()
        .SetMaxTop(100)
        .Count());

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseODataRouteDebug();

app.UseAuthorization();

app.MapControllers();

app.Run();

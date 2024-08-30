using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlite("Data Source=app.db"));

builder.Services.AddControllers().AddOData(opts =>
{
    opts.AddRouteComponents(
        "odata",
        EdmModel.GetEdmModel());
    opts.EnableQueryFeatures();
});

var app = builder.Build();

// Configure the HTTP request pipeline.

app.MakeSureDbCreated();

app.UseAuthorization();

app.MapControllers();

app.Run();

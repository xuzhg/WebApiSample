using CaseInsensitiveFilterSample;
using Microsoft.AspNetCore.OData;
using Microsoft.AspNetCore.OData.Query.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.OData.Edm;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
IEdmModel model = GetEdmModel();

builder.Services.AddDbContext<MyContext>(options => options.UseSqlite("Data Source=app.db"));
builder.Services.AddControllers()
    .AddOData(opt => opt.EnableQueryFeatures().AddRouteComponents("odata", model, services => services.AddSingleton<IFilterBinder, MyFilterBinder>()));

var app = builder.Build();

// Configure the HTTP request pipeline.

MakeSureDbCreated(app);

app.UseAuthorization();

app.MapControllers();

app.Run();

static IEdmModel GetEdmModel()
{
    var odataBuilder = new Microsoft.OData.ModelBuilder.ODataConventionModelBuilder();
    odataBuilder.EntitySet<Customer>("Customers");
    return odataBuilder.GetEdmModel();
}

static void MakeSureDbCreated(WebApplication app)
{
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;

        var context = services.GetRequiredService<MyContext>();

        // uncomment the following to delete it
        //context.Database.EnsureDeleted();

        context.Database.EnsureCreated();
    }
}
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;

namespace NullValueSample
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlite("Data Source=app.db"));

            IEdmModel model = GetEdmModel();
            builder.Services.AddControllers()
                .AddOData(opt => opt.EnableQueryFeatures().AddRouteComponents("null", model).AddRouteComponents("empty", model));

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.MakeSureDbCreated();

            app.MapControllers();

            app.Run();
        }

        private static IEdmModel GetEdmModel()
        {
            var builder = new ODataConventionModelBuilder();
            builder.EntitySet<Customer>("Customers");
            return builder.GetEdmModel();
        }
    }
}

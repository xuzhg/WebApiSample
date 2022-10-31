using CreateNewTypeSample.Extensions;
using Microsoft.AspNetCore.OData;
using Microsoft.AspNetCore.OData.Formatter.Serialization;
using Microsoft.OData;

namespace CreateNewTypeSample
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers()
                .AddOData(opt => 
                opt.AddRouteComponents("odata", EdmModelBuilder.GetEdmModel(),
                services => services.AddSingleton<ODataPrimitiveSerializer, MyPrimitiveReserializer>()
                .AddSingleton<ODataPayloadValueConverter, MyPayloadValueConverter>()
                .AddSingleton<ODataResourceSerializer, MyResourceReserializer>()).EnableQueryFeatures());

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
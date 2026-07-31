using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OData.ModelBuilder;
using ODataTestService.Models;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.OData.UriParser;
using Microsoft.OData.Edm;

namespace ODataTestService;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddDbContextFactory<MainContext>(options =>
            options.UseSqlite("data source=data.sqlite")
                   .EnableSensitiveDataLogging());

        // 1. 动态构建 OData EDM 模型
        var modelBuilder = new ODataConventionModelBuilder();
        var dbSetProperties = typeof(MainContext)
                             .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                             .Where(p => p.PropertyType.IsGenericType && p.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>))
                             .ToList();

        foreach (var prop in dbSetProperties)
        {
            modelBuilder.AddEntitySet(prop.Name, modelBuilder.AddEntityType(prop.PropertyType.GetGenericArguments()[0]));
        }

        var edmModel = modelBuilder.GetEdmModel();

        // 3. 启用 OData 支持
        //builder.Services.AddControllers()
        //       .AddOData(options =>
        //            options.Select().Filter().OrderBy().Expand().Count().SetMaxTop(100)
        //                   .AddRouteComponents("main", edmModel));

        //builder.Services.AddOData(options =>
        //            options.Select().Filter().OrderBy().Expand().Count().SetMaxTop(100));
        // builder.Services.AddOData(options => options.EnableAll());

        builder.Services.AddControllers()
           .AddOData(options =>
               options.Select().Filter().OrderBy().Expand().Count().SetMaxTop(100)
                      .AddRouteComponents("main", edmModel));

        var app = builder.Build();

        using (var scope = app.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetService<MainContext>();
            await db.Database.EnsureCreatedAsync();

            if (!db.Orders.Any())
            {
                db.Orders.Add(new()
                {
                    Id        = 1,
                    Amount    = 3,
                    CreatedAt = new(2026, 7, 24, 19, 8, 0, DateTimeKind.Unspecified),
                });
                db.SaveChanges();
                db.Orders.Add(new()
                {
                    Id        = 2,
                    Amount    = 2,
                    CreatedAt = new(2026, 7, 23, 18, 7, 0, DateTimeKind.Unspecified),
                });
                db.SaveChanges();
            }
        }

        //app.MapODataMetadata("/main/$metadata", edmModel);
        //app.MapControllers();

        //app.MapGet("/main/orders", (MainContext db) => db.Orders.AsQueryable())
        //   .AddODataQueryEndpointFilter() // Automatically applies $filter, $select, etc.
        //   .WithODataModel(edmModel)      // Binds the schema layout
        //   .WithODataResult();            // Formats payload cleanly into OData JSON

        // Configure the HTTP request pipeline.

        // 3. Map the Controllerless Endpoint
        /*

        // 2. 动态调用我们自定义的泛型注册方法
        foreach (var prop in dbSetProperties)
        {
            var entityType = prop.PropertyType.GetGenericArguments()[0];
            var entityName = prop.Name.ToLower();

            // 反射找到下方的通用注册方法并填入泛型参数 TEntity
            var genericRegister = typeof(ODataEndpointBuilder)
                                 .GetMethod(nameof(ODataEndpointBuilder.Register), BindingFlags.Public | BindingFlags.Static)?
                                 .MakeGenericMethod(entityType);

            // 传入参数执行
            genericRegister?.Invoke(null, [app, entityName, edmModel]);
        }
        
        */


        app.UseODataRouteDebug();

        app.MapControllers();

        app.Run();
    }

    // 5. 泛型端点注册器（供反射调用）
    //static void RegisterODataEndpoint<TEntity>(WebApplication app, string entityName, Microsoft.OData.Edm.IEdmModel edmModel)
    //    where TEntity : class
    //{
    //    app.MapGet($"/main/{entityName}", (MainContext db) => db.Set<TEntity>().AsQueryable())
    //       .AddODataQueryEndpointFilter()
    //       .WithODataModel(edmModel)
    //       .WithODataResult();
    //}
}

// 提取出来的泛型建造器：没有任何废代码，逻辑更清晰
public static class ODataEndpointBuilder
{
    public static void Register<TEntity>(WebApplication app, string entityName, Microsoft.OData.Edm.IEdmModel edmModel)
        where TEntity : class
    {
        // 这里是强类型的 Minimal API，书写体验最好
        app.MapGet($"/main/{entityName}", (MainContext db) => db.Set<TEntity>().AsQueryable())
           .AddODataQueryEndpointFilter()
           .WithODataModel(edmModel)
           .WithODataResult()
           .WithODataPathFactory((c, t) =>
           {
                IEdmEntitySet entitySet = edmModel.EntityContainer.EntitySets().First(e => string.Equals(e.Name, entityName, StringComparison.OrdinalIgnoreCase));
                return new ODataPath(new EntitySetSegment(entitySet));
           });
    }
}
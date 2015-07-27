using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.SelfHost;
using System.Web.OData.Builder;
using System.Web.OData.Extensions;
using Microsoft.OData.Edm;
using SelfHostServer.Models;

namespace SelfHostServer
{
    class Program
    {
        static void Main(string[] args)
        {
            var config = new HttpSelfHostConfiguration("http://localhost:12345");

            config.MapODataServiceRoute("odata", "odata", GetEdmModel());

            using (HttpSelfHostServer server = new HttpSelfHostServer(config))
            {
                server.OpenAsync().Wait();
                Console.WriteLine("Service is running. Press any key to quit....");
                Console.ReadKey();
            }
        }

        private static IEdmModel GetEdmModel()
        {
            var builder = new ODataConventionModelBuilder();
            builder.EntitySet<Customer>("Customers");
            builder.EntitySet<Category>("Categories");
            builder.EntitySet<Order>("Orders");

            builder.EntityType<Customer>()
                .Function("RefreshCustomer")
                .ReturnsFromEntitySet<Customer>("Customers")
                .IsComposable = true;

            builder.EntityType<Customer>()
                .Function("RefreshCustomer2")
                .ReturnsFromEntitySet<Customer>("Customers")
                .IsComposable = true;

            return builder.GetEdmModel();
        }
    }
}

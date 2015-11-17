using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.OData.Builder;
using System.Web.OData.Extensions;
using System.Web.OData.Routing;
using Microsoft.OData.Edm;

namespace ODataByteSample
{
    class Program
    {
        static void Main(string[] args)
        {
            HttpClient client = GetClient();

            Query(client, "$metadata");

            Query(client, "Customers");

            Query(client, "Customers?$expand=Bytes");
        }

        private static void Query(HttpClient client, string uri)
        {
            string requestUri = "http://localhost/odata/" + uri;
            Console.WriteLine();
            Console.WriteLine(requestUri);

            var response = client.GetAsync(requestUri).Result;

            Console.WriteLine(response.StatusCode);

            if (response.Content != null)
            {
                Console.WriteLine(response.Content.ReadAsStringAsync().Result);
            }
        }

        private static HttpClient GetClient()
        {
            var config = new HttpConfiguration();
            config.MapODataServiceRoute("odata", "odata", GetEdmModel());
            return new HttpClient(new HttpServer(config));
        }

        private static IEdmModel GetEdmModel()
        {
            var builder = new ODataConventionModelBuilder();
            builder.EntitySet<Customer>("Customers");
            return builder.GetEdmModel();
        }
    }
}

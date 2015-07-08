using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.OData.Builder;
using System.Web.OData.Extensions;
using Microsoft.OData.Edm;

namespace ODataAttributeRoutingTest
{
    class Program
    {
        static void Main(string[] args)
        {
            HttpClient client = GetClient();

            //QueryMetadata(client);

            Delete(client);

            DeleteWithId(client);

            Console.ReadKey();
        }

        private static async void QueryMetadata(HttpClient client)
        {
            string req = "http://localhost/odata/$metadata";

            HttpResponseMessage resp = await client.GetAsync(req);

            Console.WriteLine(await resp.Content.ReadAsStringAsync());
        }

        private static async void Delete(HttpClient client)
        {
            string req = "http://localhost/odata/Restaurants(1)/ResDishes/$ref";

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Delete, req);

            HttpResponseMessage resp = await client.SendAsync(request);

            Console.WriteLine(await resp.Content.ReadAsStringAsync());
        }

        private static async void DeleteWithId(HttpClient client)
        {
            string req = "http://localhost/odata/Restaurants(1)/ResDishes/$ref?$id=http://localhost/odata/Dishes(5)";

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Delete, req);

            HttpResponseMessage resp = await client.SendAsync(request);

            Console.WriteLine(await resp.Content.ReadAsStringAsync());
        }

        private static HttpClient GetClient()
        {
            var configuration = new HttpConfiguration();

            configuration.MapODataServiceRoute("odata", "odata", GetEdmModel());

            return new HttpClient(new HttpServer(configuration));
        }

        private static IEdmModel GetEdmModel()
        {
            var builder = new ODataConventionModelBuilder();
            builder.EntitySet<Restaurant>("Restaurants");
            builder.EntitySet<Dish>("Dishes");
            return builder.GetEdmModel();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.OData.Builder;
using System.Web.Http.OData.Extensions;
using HostingDb.Pandora;
using Microsoft.Data.Edm;

namespace ExpandTestInV3
{
    class Program
    {
        static void Main(string[] args)
        {
            HttpClient client = GetClient();

            Query(client, "$metadata");

            Query(client, "Components"); // will call the Get() function

            Query(client, "Components?$expand=Childs"); // still call the Get() function

            Query(client, "Components('XYZ2')?$expand=Childs"); // will call Get(string) function

            Query(client, "Components('XYZ4')/Childs"); // will call GetChilds(string) function
        }

        private static void Query(HttpClient client, string uri)
        {
            string requestUri = "http://localhost/odata/" + uri;
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
            HttpConfiguration config = new HttpConfiguration();
            config.Routes.MapODataServiceRoute("odata", "odata", GetEdmModel());
            return new HttpClient(new HttpServer(config));
        }

        private static IEdmModel GetEdmModel()
        {
            var builder = new ODataConventionModelBuilder();
            var components = builder.EntitySet<Component>("Components");
            builder.EntitySet<ComponentUser>("ComponentUsers");
            return builder.GetEdmModel();
        }
    }
}

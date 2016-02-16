using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.OData.Builder;
using System.Web.OData.Extensions;
using Microsoft.OData.Edm;
using Newtonsoft.Json.Linq;

namespace NestedFilterSample
{
    public class RegionTest
    {
        private const string BaseUri = "http://localhost/odata1/";

        public static void RunTest()
        {
            Console.WriteLine("===============Region test ==============");
            HttpClient client = GetClient();

            Query(client, "Regions?$expand=Facilities($expand=Departments)");

            Query(client, "Regions?$expand=Facilities($filter=Active eq true;$expand=Departments)");

            Query(client, "Regions?$expand=Facilities($filter=Active eq true;$expand=Departments($filter=Active eq true))");
        }

        private static void Query(HttpClient client, string uri)
        {
            string requestUri = BaseUri + uri;
            Console.WriteLine("\n[Query]: " + requestUri);
            HttpResponseMessage response = client.GetAsync(requestUri).Result;

            if (response.StatusCode != HttpStatusCode.OK)
            {
                Console.WriteLine("\n==> Failed.");
            }
            else
            {
                Console.WriteLine("\n==> Succeed.");
            }

            if (response.Content != null)
            {
                Console.WriteLine(JObject.Parse(response.Content.ReadAsStringAsync().Result));
            }
        }

        private static HttpClient GetClient()
        {
            var config = new HttpConfiguration();
            config.MapODataServiceRoute("odata1", "odata1", GetEdmModel());
            return new HttpClient(new HttpServer(config));
        }

        private static IEdmModel GetEdmModel()
        {
            var builder = new ODataConventionModelBuilder();
            builder.EntitySet<Region>("Regions");
            builder.EntitySet<Facility>("Facilities");
            builder.EntitySet<Department>("Departments");
            return builder.GetEdmModel();
        }
    }
}

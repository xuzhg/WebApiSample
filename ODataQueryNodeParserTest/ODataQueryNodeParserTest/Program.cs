using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.OData.Extensions;
using ODataQueryNodeParserTest.Models;

namespace ODataQueryNodeParserTest
{
    class Program
    {
        static void Main(string[] args)
        {
            HttpClient client = GetClient();

            Query(client, "$metadata");

            Query(client, "Customers");

            Query(client, "Customers?$filter=CreatedOn gt 2015-10-03T01:02:03.004Z");

            Query(client, "Customers?$filter=CreatedOn gt 2015-10-03T01:02:03.004Z and CreatedOn lt 2015-10-05T01:02:03.004Z");
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
            config.MapODataServiceRoute("odata", "odata", EdmModelBuilder.EdmModel);
            return new HttpClient(new HttpServer(config));
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.OData.Builder;
using System.Web.OData.Extensions;
using Microsoft.OData.Edm;
using Newtonsoft.Json.Linq;
using ModelsNS;

namespace ODataInheritanceSample
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = GetClient();

            Query(client, "$metadata");
            Query(client, "Employees");

            Post(client);
            PostFromManager(client);
            PostFromSeller(client);
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
                if (response.Content.Headers.ContentType.MediaType == "application/xml")
                {
                    Console.WriteLine(response.Content.ReadAsStringAsync().Result);
                }
                else
                {
                    Console.WriteLine(JObject.Parse(response.Content.ReadAsStringAsync().Result));
                }
            }
        }

        private static void Post(HttpClient client)
        {
            string requestUri = "http://localhost/odata/Employees";

            string payload = "{" +
                "\"Name\":\"Jimmy\"," + 
                "\"Address\":{\"City\":\"London\",\"Street\":\"London Rd\"}" +
                "}";

            Post(client, requestUri, payload);
        }

        private static void PostFromManager(HttpClient client)
        {
            // # 1
            string requestUri = "http://localhost/odata/Employees";

            string payload = "{" +
                "\"@odata.type\":\"#ModelsNS.Manager\"," +
                "\"Name\":\"Peter\"," +
                "\"Salary\":101.01," +
                "\"Address\":{\"@odata.type\":\"#ModelsNS.UsAddress\",\"City\":\"NewYork\",\"Street\":\"NewYork City\",\"ZipCode\":\"9001\"}" +
                "}";

            Post(client, requestUri, payload);


            // #2
            requestUri = "http://localhost/odata/Employees/ModelsNS.Manager";

            payload = "{" +
                "\"Name\":\"Peter\"," +
                "\"Salary\":101.01," +
                "\"Address\":{\"@odata.type\":\"#ModelsNS.UsAddress\",\"City\":\"NewYork\",\"Street\":\"NewYork City\",\"ZipCode\":\"9001\"}" +
                "}";

            Post(client, requestUri, payload);
        }


        private static void PostFromSeller(HttpClient client)
        {
            // # 1
            string requestUri = "http://localhost/odata/Employees";

            string payload = "{" +
                "\"@odata.type\":\"#ModelsNS.Seller\"," +
                "\"Name\":\"John\"," +
                "\"Bonus\":1.012345," +
                "\"Address\":{\"@odata.type\":\"#ModelsNS.CnAddress\",\"City\":\"Shanghai\",\"Street\":\"Shanghai Rd\",\"PostCode\":\"201115\"}" +
                "}";

            Post(client, requestUri, payload);


            // #2
            requestUri = "http://localhost/odata/Employees/ModelsNS.Seller";

            payload = "{" +
                "\"Name\":\"John\"," +
                "\"Bonus\":1.012345," +
                "\"Address\":{\"@odata.type\":\"#ModelsNS.CnAddress\",\"City\":\"Shanghai\",\"Street\":\"Shanghai Rd\",\"PostCode\":\"201115\"}" +
                "}";

            Post(client, requestUri, payload);
        }

        private static void Post(HttpClient client, string requestUri, string payload)
        {
            Console.WriteLine();
            Console.WriteLine(requestUri);

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, requestUri);
            request.Content = new StringContent(payload);
            request.Content.Headers.ContentType = MediaTypeWithQualityHeaderValue.Parse("application/json");

            HttpResponseMessage response = client.SendAsync(request).Result;
            Console.WriteLine(response.StatusCode);

            if (response.Content != null)
            {
                Console.WriteLine(JObject.Parse(response.Content.ReadAsStringAsync().Result));
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
            builder.EntitySet<Employee>("Employees");
            return builder.GetEdmModel();
        }
    }
}

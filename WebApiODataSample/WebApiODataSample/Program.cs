using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.OData.Extensions;
using System.Web.OData.Formatter.Deserialization;
using EdmModelLib;
using Newtonsoft.Json.Linq;

namespace WebApiODataSample
{
    class Program
    {
        private static string baseUri = "http://localhost/odata/";
        static void Main(string[] args)
        {
            HttpClient client = GetClient();

            //Query(client, "$metadata");

            PatchTest(client, "Customers(2)");

            DollarRefTest(client);
        }

        private static void Query(HttpClient client, string uri)
        {
            string requestUri = baseUri + uri;
            Console.WriteLine("\n######");
            Console.WriteLine("GET: " + requestUri);

            var response = client.GetAsync(requestUri).Result;

            Console.WriteLine("-- " + response.StatusCode);
            if (response.Content != null)
            {
                JObject content = response.Content.ReadAsAsync<JObject>().Result;
                Console.WriteLine(content);
            }
        }

        private static void PatchTest(HttpClient client, string uri)
        {
            // Simple
            string payload = "{\"Name\":\"Sam\"}";

            Patch(client, uri, payload);

            Query(client, uri + "?$expand=Orders");

            // deep insert is not supported:
            // see the stack overflow: http://stackoverflow.com/questions/33441307/odata-v4-webapi-patch-with-navigationproperty-collection-breaks-deserailizatio
            payload = "{\"Name\":\"Sam\", \"Orders\":[]}";

            Patch(client, uri, payload);

            // Query(client, uri + "?$expand=Orders");
        }

        private static void DollarRefTest(HttpClient client)
        {
            Console.WriteLine("=============test $ref=============");

            string payload = "{\"Name\":\"Sam\"}";

            // Post a customer
            int id = Post(client, "Customers", payload);

            Query(client, "Customers(" + id + ")?$expand=Orders");

            // Post two orders
            int order1 = Post(client, "Orders", "{\"Title\":\"Book\"}");
            int order2 = Post(client, "Orders", "{\"Title\":\"Magazine\"}");

            // Post the $ref

            string dolladId = "{ \"@odata.id\" : \"" + baseUri + "Orders(" + order1 + ")\"}";
            Post(client, "Customers(" + id + ")/Orders/$ref", dolladId);

            dolladId = "{ \"@odata.id\" : \"" + baseUri + "Orders(" + order2 + ")\"}";
            Post(client, "Customers(" + id + ")/Orders/$ref", dolladId);

            Query(client, "Customers(" + id + ")?$expand=Orders");
        }

        private static int Post(HttpClient client, string uri, string payload)
        {
            string requestUri = baseUri + uri;
            Console.WriteLine("\n######");
            Console.WriteLine("POST: " + requestUri);
            Console.WriteLine("Body: ");
            Console.WriteLine(payload);
            Console.WriteLine();
            ODataEntityDeserializer a = new ODataEntityDeserializer(new DefaultODataDeserializerProvider());
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, requestUri);
            request.Content = new StringContent(payload);
            request.Content.Headers.ContentType = MediaTypeWithQualityHeaderValue.Parse("application/json");
            request.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json;odata.metadata=full"));
            HttpResponseMessage response = client.SendAsync(request).Result;

            Console.WriteLine("-- " + response.StatusCode);
            if (response.Content == null)
            {
                return -1;
            }

            JObject content = response.Content.ReadAsAsync<JObject>().Result;
            Console.WriteLine(content);

            return content["Id"].Value<int>();
        }

        private static void Patch(HttpClient client, string uri, string payload)
        {
            string requestUri = baseUri + uri;
            Console.WriteLine("\n######");
            Console.WriteLine("PATCH: " + requestUri);
            Console.WriteLine("Body: ");
            Console.WriteLine(payload);
            Console.WriteLine();
            ODataEntityDeserializer a = new ODataEntityDeserializer(new DefaultODataDeserializerProvider());
            HttpRequestMessage request = new HttpRequestMessage(new HttpMethod("PATCH"), requestUri);
            request.Content = new StringContent(payload);
            request.Content.Headers.ContentType = MediaTypeWithQualityHeaderValue.Parse("application/json");
            request.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json;odata.metadata=full"));
            HttpResponseMessage response = client.SendAsync(request).Result;

            Console.WriteLine("-- " + response.StatusCode);
            if (response.Content != null)
            {
                JObject content = response.Content.ReadAsAsync<JObject>().Result;
                Console.WriteLine(content);
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

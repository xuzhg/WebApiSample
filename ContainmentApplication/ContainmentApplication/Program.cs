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

namespace ContainmentApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            HttpClient client = GetClient();

            Query(client, "$metadata");

            Query(client, "Customers(1)/Infos");

            Query(client, "Customers(1)/Infos(2)");

            PutRequest(client, "Customers(1)/Infos(2)");

            DeleteRequest(client, "Customers(1)/Infos(2)");
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

        private static void PutRequest(HttpClient client, string uri)
        {
            string requestUri = "http://localhost/odata/" + uri;
            Console.WriteLine();
            Console.WriteLine(requestUri);

            var request = new HttpRequestMessage(HttpMethod.Put, requestUri);
            string payload = @"{'Id':1,'Email':'saxu@microsoft.com'}";
            request.Content = new StringContent(payload);
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var response = client.SendAsync(request).Result;

            Console.WriteLine(response.StatusCode);

            if (response.Content != null)
            {
                Console.WriteLine(response.Content.ReadAsStringAsync().Result);
            }
        }

        private static void DeleteRequest(HttpClient client, string uri)
        {
            string requestUri = "http://localhost/odata/" + uri;
            Console.WriteLine();
            Console.WriteLine(requestUri);

            var request = new HttpRequestMessage(HttpMethod.Delete, requestUri);

            var response = client.SendAsync(request).Result;

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

    public class Customer
    {
        public int Id { get; set; }

        public string Content { get; set; }

        [Contained]
        public IList<CustomerInfo> Infos { get; set; }
    }

    public class CustomerInfo
    {
        public int Id { get; set; }

        public string Email { get; set; }
    }
}

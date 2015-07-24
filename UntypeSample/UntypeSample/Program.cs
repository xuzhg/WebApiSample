using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.OData.Extensions;
using System.Web.OData.Formatter;
using Microsoft.OData.Edm;
using UntypeSample.Models;

namespace UntypeSample
{
    class Program
    {
        static void Main(string[] args)
        {
            HttpClient client = GetClient();

            RequestQuery(client, "$metadata");

            RequestQuery(client, "Customers");

            RequestQuery(client, "Customers(1)/Name");

            RequestQuery(client, "Customers(1)/Name/$value");

            RequestQuery(client, "Customers(1)/Color"); // doesn't work

            UpdateCustomer(client, 1);
        }

        private async static void RequestQuery(HttpClient client, string requestUri)
        {
            string uri = "http://localohost/odata/" + requestUri;
            Console.WriteLine("\n\n==>: " + uri);

            HttpResponseMessage response;

            try
            {
                response = await client.GetAsync(uri);
                string content = "";
                if (response.Content != null)
                {
                    content = await response.Content.ReadAsStringAsync();
                }

                Console.WriteLine("RESPONSE:\n | Status code: [ " + response.StatusCode + " ] \n | Content: \n" + content);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private async static void UpdateCustomer(HttpClient client, int customerId)
        {
            string Uri = "http://localhost/odata/Customers(" + customerId + ")";
            Console.WriteLine("\n\n==>: " + Uri);

            HttpResponseMessage response;
            HttpRequestMessage request = new HttpRequestMessage(new HttpMethod("PATCH"), Uri);
            request.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));
            string payload = @"{ 
                ""Name"": ""Sam"" 
            }";
            request.Content = new StringContent(payload);
            request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

            try
            {
                response = client.SendAsync(request).Result;

                string content = "";
                if (response.Content != null)
                {
                    content = await response.Content.ReadAsStringAsync();
                }

                Console.WriteLine("RESPONSE:\n | Status code: [ " + response.StatusCode + " ] \n | Content: \n" + content);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private static HttpClient GetClient()
        {
            HttpConfiguration config = new HttpConfiguration();

            config.MapODataServiceRoute("odata", "odata", EdmModelBuilder.EdmModel);

            return new HttpClient(new HttpServer(config));
        }
    }
}

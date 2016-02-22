using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.OData.Extensions;
using LowerCamelCaseSample.Model;
using Newtonsoft.Json.Linq;

namespace LowerCamelCaseSample
{
    class Program
    {
        private static HttpClient _client = GetClient();
        private static HttpClient _caseInsensitiveClient = GetCaseInsensitiveClient();

        static void Main(string[] args)
        {
            Query("$metadata");

            Query("Customers");

            Query("Customers?$filter=Status eq LowerCamelCaseSample.ServiceStatus'Active'"); // V1 ok, V2 no

            Query("Customers?$filter=Status eq LowerCamelCaseSample.serviceStatus'Active'"); // V1, V2 no

            Query("Customers?$filter=status eq LowerCamelCaseSample.ServiceStatus'Active'"); // V1 no, V2 ok

            Query("Customers?$filter=status eq LowerCamelCaseSample.serviceStatus'Active'"); // V1 V2 no

            QueryCaseInsensitive("Customers?$filter=status eq LowerCamelCaseSample.serviceStatus'Active'"); // not work for type name case insensitive
            QueryCaseInsensitive("Customers?$filter=status eq 'Active'");
        }

        private static void Query(string uri)
        {
            string requestUri = "http://localhost/v1/" + uri;
            Console.WriteLine(requestUri);
            var response = _client.GetAsync(requestUri).Result;
            OutputContent(response);

            requestUri = "http://localhost/v2/" + uri;
            Console.WriteLine(requestUri);
            response = _client.GetAsync(requestUri).Result;
            OutputContent(response);
        }

        private static void QueryCaseInsensitive(string uri)
        {
            string requestUri = "http://localhost/v3/" + uri;
            Console.WriteLine("CaseInsensitive: " + requestUri);
            var response = _caseInsensitiveClient.GetAsync(requestUri).Result;
            OutputContent(response);
        }

        private static void OutputContent(HttpResponseMessage response)
        {
            Console.WriteLine(response.StatusCode);
            if (response.Content != null)
            {
                var s = response.Content.ReadAsStringAsync().Result;
                if (response.Content.Headers.ContentType.MediaType.Contains("xml"))
                {
                    Console.WriteLine(s);
                }
                else
                {
                    Console.WriteLine(JObject.Parse(s));
                }
            }
        }

        private static HttpClient GetClient()
        {
            var config = new HttpConfiguration();
            config.MapODataServiceRoute("odata1", "v1", EdmModelBuilder.GetEdmModel());
            config.MapODataServiceRoute("odata2", "v2", EdmModelBuilder.GetEdmModelLowerCamelCase());
            return new HttpClient(new HttpServer(config));
        }

        private static HttpClient GetCaseInsensitiveClient()
        {
            var config = new HttpConfiguration();
            config.EnableCaseInsensitive(true);
            config.EnableEnumPrefixFree(true);
            config.MapODataServiceRoute("odata3", "v3", EdmModelBuilder.GetEdmModel());
            return new HttpClient(new HttpServer(config));
        }
    }
}

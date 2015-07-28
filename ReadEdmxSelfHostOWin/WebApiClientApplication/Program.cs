using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace WebApiClientApplication
{
    class Program
    {
        private static string _baseUri = "http://localhost:12345/";
        private static HttpClient _client = new HttpClient();


        static void Main(string[] args)
        {
           // Query("odata1/$metadata");

            // Query("odata2/$metadata");

           // Query("odata1/Protocols");

            Patch();

            Console.ReadKey();
        }

        private static void Query(string queryString)
        {
            string requestUri = _baseUri + queryString;
            Console.WriteLine();
            Console.WriteLine(requestUri);

            HttpResponseMessage response = _client.GetAsync(requestUri).Result;

            response.EnsureSuccessStatusCode();

            Console.WriteLine(response.Content.ReadAsStringAsync().Result);
        }

        static void Patch()
        {
            Console.WriteLine();

            string requestUri = "http://localhost:12345/odata1/Protocols(0f092243-92c0-4350-a27e-000168aac402)/Fields(d08cec51-a6b1-4113-bfdd-823ac3953cad)";

            HttpRequestMessage request = new HttpRequestMessage(new HttpMethod("PATCH"), requestUri);
            request.Headers.Add("accept", "application/json");
            string payload = @"{ 
                ""Name"": ""Sam""
            }";

            request.Content = new StringContent(payload);
            request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

            HttpResponseMessage resp = _client.SendAsync(request).Result;

            resp.EnsureSuccessStatusCode();

            string result = resp.Content.ReadAsStringAsync().Result;
            Console.WriteLine(result);
        }
    }
}

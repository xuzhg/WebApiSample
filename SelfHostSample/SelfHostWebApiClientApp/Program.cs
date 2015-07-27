using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace SelfHostWebApiClientApp
{
    class Program
    {
        private static string _baseUri = "http://localhost:12345/odata/";
        private static HttpClient _client = new HttpClient();

        static void Main(string[] args)
        {
            //_client.BaseAddress = _baseUri;
            /*
            QueryCustomers();

            QuerySingleCustomer();*/

            PostCustomer();
        }

        static void QueryCustomers()
        {
            Console.WriteLine();

            HttpResponseMessage resp = _client.GetAsync(_baseUri + "Customers").Result;

            resp.EnsureSuccessStatusCode();

            string result = resp.Content.ReadAsStringAsync().Result;

            Console.WriteLine(result);
        }

        static void QuerySingleCustomer()
        {
            Console.WriteLine();

            HttpResponseMessage resp = _client.GetAsync(_baseUri + "Customers(3)").Result;

            resp.EnsureSuccessStatusCode();

            string result = resp.Content.ReadAsStringAsync().Result;
            Console.WriteLine(result);
        }

        static void PostCustomer()
        {
            Console.WriteLine();

            string requestUri = "http://localhost:12345/odata/Customers?$expand=Category";

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, requestUri);
            request.Headers.Add("accept", "application/json");
            string payload = @"{ 
                ""CustomerId"": 1,
                ""CustomerName"": ""Peter"",
                ""Location"": { ""Country"": ""Russia"", ""City"": ""Mosco""},
                ""Category"": { ""CategoryId"":9, ""CategoryType"": ""Vip""}
            }";

        //""FavoriteColor"": SelfHostServer.Models.Color'Red',

            request.Content = new StringContent(payload);
            request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

            HttpResponseMessage resp = _client.SendAsync(request).Result;

            resp.EnsureSuccessStatusCode();

            string result = resp.Content.ReadAsStringAsync().Result;
            Console.WriteLine(result);
        }
    }
}

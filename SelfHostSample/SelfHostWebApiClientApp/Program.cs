using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
            QueryCustomers();

            QuerySingleCustomer();
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
    }
}

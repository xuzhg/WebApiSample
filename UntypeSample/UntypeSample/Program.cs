using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.OData.Extensions;
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

        private static HttpClient GetClient()
        {
            HttpConfiguration config = new HttpConfiguration();

            config.MapODataServiceRoute("odata", "odata", EdmModelBuilder.EdmModel);

            return new HttpClient(new HttpServer(config));
        }
    }
}

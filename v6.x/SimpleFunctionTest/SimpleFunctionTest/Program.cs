using Microsoft.OData.Edm;
using SimpleFunctionTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.OData.Builder;
using System.Web.OData.Extensions;

namespace SimpleFunctionTest
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var client = GetClient();

            var requestUri = @"http://localhost/odata/$metadata";
            var response = client.GetAsync(requestUri).Result;
            Console.WriteLine(response.Content.ReadAsStringAsync().Result);


            requestUri = @"http://localhost/odata/Apps(1)";
            Console.WriteLine();

            Console.WriteLine(requestUri);
            response = client.GetAsync(requestUri).Result;

            Console.WriteLine(response.Content.ReadAsStringAsync().Result);

            //requestUri = @"http://localhost/odata/Apps(1)/Default.Download(p='txt')";
              requestUri = @"http://localhost/odata/Apps(1)/Default.Download(p='Foo%2FBar%2Ftest.txt')";
            Console.WriteLine();

            Console.WriteLine(requestUri);
            response = client.GetAsync(requestUri).Result;

            Console.WriteLine(response.Content.ReadAsStringAsync().Result);

        }

        private static HttpClient GetClient()
        {
            var c = new HttpConfiguration();
            c.MapODataServiceRoute("odata", "odata", GetEdmModel());
            return new HttpClient(new HttpServer(c));
        }

        private static IEdmModel GetEdmModel()
        {
            var b = new ODataConventionModelBuilder();
            var f = b.EntitySet<App>("Apps").EntityType.Function("Download").Returns<int>();
            f.Parameter<string>("p");
            return b.GetEdmModel();
        }
    }
}

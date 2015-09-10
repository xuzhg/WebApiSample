using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.OData.Builder;
using System.Web.OData.Extensions;
using Microsoft.OData.Edm;

namespace EnumParamterTest
{
    class Program
    {
        static void Main(string[] args)
        {
            HttpClient client = GetClient();

            QueryMetdata(client);

            CallFunction(client);
        }

        private static void QueryMetdata(HttpClient client)
        {
            var response = client.GetAsync("http://localhost/odata/$metadata").Result;
            Console.WriteLine(response.Content.ReadAsStringAsync().Result);
        }

        private static void CallFunction(HttpClient client)
        {
            Console.WriteLine("\n1): category=EnumParamterTest.Category'Vip'");
            var response = client.GetAsync("http://localhost/odata/GetCountByCategory(category=EnumParamterTest.Category'Vip')").Result;
            Console.WriteLine(response.Content.ReadAsStringAsync().Result);

            // doesn't work only if you enable enum prefix free
            Console.WriteLine("\n2): category='Vip'");
            response = client.GetAsync("http://localhost/odata/GetCountByCategory(category='Vip')").Result;
            Console.WriteLine(response.Content.ReadAsStringAsync().Result);

            Console.WriteLine("\n3): (category=@p)?@p=EnumParamterTest.Category'General'");
            response = client.GetAsync("http://localhost/odata/GetCountByCategory(category=@p)?@p=EnumParamterTest.Category'General'").Result;
            Console.WriteLine(response.Content.ReadAsStringAsync().Result);

            // doesn't work only if you enable enum prefix free
            Console.WriteLine("\n4): (category=@p)?@p='General'");
            response = client.GetAsync("http://localhost/odata/GetCountByCategory(category=@p)?@p='General'").Result;
            Console.WriteLine(response.Content.ReadAsStringAsync().Result);
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

            builder.Function("GetCountByCategory").Returns<int>().Parameter<Category>("category");

            return builder.GetEdmModel();
        }
    }
}

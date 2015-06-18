using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.OData.Builder;
using System.Web.OData.Extensions;
using System.Web.OData.Formatter;
using Microsoft.OData.Edm;
using Ga;

namespace UnqualifiedNameCall
{
    class Program
    {
        static void Main(string[] args)
        {
            HttpClient client = GetClient();

            QueryMetadata(client);

            QueryPerson(client);

            CallFunction(client);

            Console.WriteLine("---OK---");
        }

        private static void QueryMetadata(HttpClient client)
        {
            string requestUri = "http://localhost/odata/$metadata";

            HttpResponseMessage response = client.GetAsync(requestUri).Result;

            Console.WriteLine(response.Content.ReadAsStringAsync().Result);
        }

        private static void QueryPerson(HttpClient client)
        {
            string requestUri = "http://localhost/odata/People";

            HttpResponseMessage response = client.GetAsync(requestUri).Result;

            Console.WriteLine(response.Content.ReadAsStringAsync().Result);

            requestUri = "http://localhost/odata/People?$select=Name";

            response = client.GetAsync(requestUri).Result;

            Console.WriteLine(response.Content.ReadAsStringAsync().Result);
        }

        private static void CallFunction(HttpClient client)
        {
            string requestUri = "http://localhost/odata/People/Ga.DoIt()";
            Console.WriteLine(requestUri);
            HttpResponseMessage response = client.GetAsync(requestUri).Result;

            Console.WriteLine(response.Content.ReadAsStringAsync().Result);

            // 
            requestUri = "http://localhost/odata/People/DoIt()";
            Console.WriteLine(requestUri);
            response = client.GetAsync(requestUri).Result;

            Console.WriteLine(response.Content.ReadAsStringAsync().Result);
        }

        private static HttpClient GetClient()
        {
            HttpConfiguration config = new HttpConfiguration();
            config.EnableUnqualifiedNameCall(true);
            config.EnableEnumPrefixFree(true);

            config.MapODataServiceRoute("odata", "odata", GetEdmModel());
            return new HttpClient(new HttpServer(config));
        }

        private static IEdmModel GetEdmModel()
        {
            ODataModelBuilder builder = new ODataConventionModelBuilder();

            builder.Namespace = "Ga";
            builder.ContainerName = "Default";

            var entity = builder.EntitySet<Person>("People").EntityType;
            var func = entity.Collection.Function("DoIt").ReturnsCollectionFromEntitySet<Person>("People");

            return builder.GetEdmModel();
        }
    }
}

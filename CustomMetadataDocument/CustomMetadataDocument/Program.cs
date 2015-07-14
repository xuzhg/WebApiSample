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
using System.Web.OData.Formatter.Deserialization;
using CustomMetadataDocument.Extensions;
using CustomMetadataDocument.Models;
using Microsoft.OData.Edm;
using Microsoft.OData.Edm.Csdl;

namespace CustomMetadataDocument
{
    class Program
    {
        static void Main(string[] args)
        {
            HttpClient client = GetClient();

            string requestUri = "http://localhost/odata/$metadata";

            HttpResponseMessage response = client.GetAsync(requestUri).Result;

            Console.WriteLine(response.Content.ReadAsStringAsync().Result);

            Console.ReadKey();
        }

        private static HttpClient GetClient()
        {
            var config = new HttpConfiguration();

            config.Formatters.InsertRange(
                    0,
                    ODataMediaTypeFormatters.Create(new CustomSerializerProvider(), new DefaultODataDeserializerProvider()));

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
}

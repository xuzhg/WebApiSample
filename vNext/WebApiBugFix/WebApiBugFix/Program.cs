using Microsoft.OData.Edm;
using Microsoft.OData.Edm.Csdl;
using Microsoft.OData.Edm.Validation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Xml;
using System.Web.OData.Extensions;
using System.Web.OData.Builder;
using Garnet.Data.Entities;
using Garnet.Data.Entities.Values;
using Newtonsoft.Json.Linq;

namespace WebApiBugFix
{
    class Program
    {
        private static string baseUri = "http://localhost/odata/";

        static void Main(string[] args)
        {
            HttpClient client = GetClient();

            Query(client, "$metadata");

            Query(client, "Containers");

            Query(client, "Containers?$filter=Name eq 'Products'");

            Query(client, "Containers?$filter=Name eq 'NotFound'");

            Query(client, "Containers?$filter=Name eq 'Products'&$expand=Items");

            Query(client, "Containers?$filter=Name eq 'Products'&$expand=Items($expand=Values)");

            Query(client, "Containers?$filter=Name eq 'Products'&$expand=Items($expand=Values($expand=IntValue))");

            Query(client, "Containers?$filter=Name eq 'Products'&$expand=Items($expand=Values($expand=IntValue,DoubleValue))");

            // doesnot work
            Query(client, "Containers?$filter=Name eq 'Products'&$expand=Items($expand=Values($expand=IntValue,DoubleValue,BoolValue))");

            // doesnot work
            Query(client, "Containers?$filter=Name eq 'Products'&$expand=Items($expand=Values($expand=IntValue,StringValue,BoolValue))");
        }

        private static void Query(HttpClient client, string uri)
        {
            Console.WriteLine();
            string requestUri = baseUri + uri;
            Console.WriteLine("\n######");
            Console.WriteLine("GET: " + requestUri);

            var response = client.GetAsync(requestUri).Result;
            Console.WriteLine("-- " + response.StatusCode);

            if (response.Content != null)
            {
                if (response.Content.Headers.ContentType.MediaType.Contains("xml"))
                {
                    Console.WriteLine(response.Content.ReadAsStringAsync().Result);
                }
                else
                {
                    Console.WriteLine(JObject.Parse(response.Content.ReadAsStringAsync().Result));
                }
            }
        }

        private static HttpClient GetClient()
        {
            HttpConfiguration config = new HttpConfiguration();
            config.MapODataServiceRoute("odata1", "odata1", ReadEdmModel());
            config.MapODataServiceRoute("odata", "odata", ReadEdmModel());
            config.Filter().Expand().Count();
            HttpClient client = new HttpClient(new HttpServer(config));
            return client;
        }

        private static IEdmModel GetEdmModel()
        {
            var builder = new ODataConventionModelBuilder();
            builder.EntitySet<ContainerEntity>("Containers");

            builder.EntitySet<ItemEntity>("Items");

            builder.EntitySet<PropertyEntity>("Properties");
            builder.EntitySet<PropertyValueEntity>("PropertyValues");
            builder.EntitySet<ContainerRelationshipEntity>("ContainerRelationships");
            builder.EntitySet<BoolValueEntity>("BoolBalues");
            builder.EntitySet<DoubleValueEntity>("DoubleValues");
            builder.EntitySet<IntValueEntity>("IntValues");
            builder.EntitySet<StringValueEntity>("StringValues");
            return builder.GetEdmModel();
        }

        private static IEdmModel ReadEdmModel()
        {

            using (Stream stream = GetStream("EdmModel.xml"))
            {
                IEnumerable<EdmError> errors;
                Debug.Assert(stream != null, "EdmModel.xml: stream!=null");
                IEdmModel model;

                if (CsdlReader.TryParse(XmlReader.Create(stream), out model, out errors))
                {
                    return model;
                }
            }

            return null;
        }

        private static Stream GetStream(string fileName)
        {
            const string projectDefaultNamespace = "WebApiBugFix";
            const string pathSeparator = ".";
            string path = projectDefaultNamespace + pathSeparator + fileName;

            Stream stream = typeof(Program).Assembly.GetManifestResourceStream(path);

            if (stream == null)
            {
                string message = String.Format("The embedded resource '{0}' was not found.", path);
                throw new FileNotFoundException(message, path);
            }

            return stream;
        }
    }
}

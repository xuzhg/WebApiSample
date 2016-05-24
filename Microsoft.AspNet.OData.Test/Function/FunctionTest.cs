using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.OData;
using System.Web.OData.Builder;
using System.Web.OData.Extensions;
using System.Web.OData.Routing;
using Microsoft.OData.Edm;
using Newtonsoft.Json.Linq;
using Xunit;

namespace Microsoft.AspNet.OData.Test.Function
{
    public class FunctionTest
    {
        [Fact]
        public void StringParameterWithDot()
        {
            var builder = new ODataConventionModelBuilder();
            builder.EntitySet<Customer>("Customers");
            var f = builder.EntityType<Customer>().Collection.Function("GetResource");
            f.Parameter<string>("path");
            f.Parameter<string>("culture");
            f.Returns<string>();

            IEdmModel model = builder.GetEdmModel();

            var configuration = new[] { typeof(MetadataController), typeof(CustomersController) }.GetHttpConfiguration();
            configuration.MapODataServiceRoute("odata", "odata", model);
            HttpServer server = new HttpServer(configuration);
            var client = new HttpClient(server);

            var response = client.GetAsync("http://localhost/odata/Customers/Default.GetResource(path='foo.bar', culture='en-gb')").Result;

            string payload = response.Content.ReadAsStringAsync().Result;
            Console.WriteLine(payload);

            JObject result = JObject.Parse(payload);
            Assert.Equal("foo.baren-gb", result["value"]);
        }

        public class Customer
        {
            public int Id { get; set; }
        }

        public class CustomersController : ODataController
        {
            [HttpGet]
           // [ODataRoute("Customers/Default.GetResource(path={path},culture={culture})")]
            public string GetResource([FromODataUri]string path, [FromODataUri]string culture)
            {
                return path + culture;
            }
        }
    }
}

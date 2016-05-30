using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.OData;
using System.Web.OData.Builder;
using System.Web.OData.Extensions;
using System.Web.OData.Formatter;
using Microsoft.OData.Edm;
using Newtonsoft.Json.Linq;
using Xunit;

namespace Microsoft.AspNet.OData.Test.ETag
{
    public class ETagTest
    {
        [Fact]
        public void CanQueryEntityWithDefaultETagHandler()
        {
            IEdmModel model = GetEdmModel();
            HttpConfiguration configuration = new[] { typeof(CustomersController) }.GetHttpConfiguration();
            configuration.MapODataServiceRoute("odata", "odata", model);
            HttpServer server = new HttpServer(configuration);
            HttpClient client = new HttpClient(server);

            string requestUri = "http://localhost/odata/Customers(1)";

            HttpRequestMessage request = new HttpRequestMessage(new HttpMethod("Get"), requestUri);
            HttpResponseMessage response = client.SendAsync(request).Result;

            response.EnsureSuccessStatusCode();

            JObject result = JObject.Parse(response.Content.ReadAsStringAsync().Result);
            Console.WriteLine(result);
            Assert.Equal("W/\"dHJ1ZQ==,OTk=\"", result["@odata.etag"]);
        }

        [Fact]
        public void CanQueryEntityWithCustomETagHandler()
        {
            IEdmModel model = GetEdmModel();

            IETagHandler handler = new MyETagHandler(); // MyETag
            HttpConfiguration configuration = new[] { typeof(CustomersController) }.GetHttpConfiguration();
            configuration.SetETagHandler(handler);
            configuration.MapODataServiceRoute("odata", "odata", model);
            HttpServer server = new HttpServer(configuration);
            HttpClient client = new HttpClient(server);

            string requestUri = "http://localhost/odata/Customers(1)";

            HttpRequestMessage request = new HttpRequestMessage(new HttpMethod("Get"), requestUri);
            HttpResponseMessage response = client.SendAsync(request).Result;

            response.EnsureSuccessStatusCode();

            JObject result = JObject.Parse(response.Content.ReadAsStringAsync().Result);
            Console.WriteLine(result);
            Assert.Equal("W/\"IsOk,true/Salary,99\"", result["@odata.etag"]);
        }

        [Fact]
        public void CustomETagHandlerRoundTrip()
        {
            Dictionary<string, object> properties = new Dictionary<string, object>
            {
                { "BooleanProperty", (bool)true },
                { "IntProperty", (int)123},
                { "GuidProperty", Guid.Empty}
            };

            IETagHandler handler = new MyETagHandler(); // MyETag
            EntityTagHeaderValue etagHeaderValue = handler.CreateETag(properties);
            IDictionary<string, object> values = handler.ParseETag(etagHeaderValue);

            Assert.Equal(properties, values);
        }

        private static IEdmModel GetEdmModel()
        {
            var builder = new ODataConventionModelBuilder();
            builder.EntitySet<Customer>("Customers");
            return builder.GetEdmModel();
        }

        public class CustomersController : ODataController
        {
            [EnableQuery]
            public IHttpActionResult Get(int key)
            {
                return Ok(_customers.Single(c => c.Id == key));
            }

            private static IList<Customer> _customers;

            static CustomersController()
            {
                _customers = new List<Customer>
                {
                    new Customer { Id = 1, Salary = 99, IsOk = true},
                    new Customer { Id = 2, Salary = 12, IsOk = false}
                };
            }
        }

        public class Customer
        {
            public int Id { get; set; }

            [ConcurrencyCheck]
            public int Salary { get; set; }

            [ConcurrencyCheck]
            public bool IsOk { get; set; }
        }
    }
}

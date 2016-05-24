using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.OData;
using System.Web.OData.Builder;
using System.Web.OData.Extensions;
using System.Web.OData.Routing;
using Microsoft.OData.Edm;
using Xunit;

namespace Microsoft.AspNet.OData.Test.CRUD
{
    public class UpdatePropertyTest
    {
        private readonly HttpConfiguration _configuration;
        private HttpClient _client;

        public UpdatePropertyTest()
        {
            _configuration = new[] { typeof(MetadataController), typeof(CustomersController) }.GetHttpConfiguration();
            _configuration.MapODataServiceRoute("odata", "odata", GetEdmModel());
            HttpServer server = new HttpServer(_configuration);
            _client = new HttpClient(server);
        }

        [Fact]
        public void CanUpdatePrimitiveProperty()
        {
            string requestUri = "http://localhost/odata/Customers(1)/Name";
            string payload = @"{""value"":""ChangedName""}";

            HttpRequestMessage request = new HttpRequestMessage(new HttpMethod("Patch"), requestUri);
            request.Content = new StringContent(payload);
            request.Content.Headers.ContentType = MediaTypeWithQualityHeaderValue.Parse("application/json");
            HttpResponseMessage response = _client.SendAsync(request).Result;

            response.EnsureSuccessStatusCode();
        }

        [Fact]
        [Category("Since v5.9.1")]
        public void CanUpdateEnumProperty()
        {
            string requestUri = "http://localhost/odata/Customers(1)/FavoriteColor";

            //string payload = @"{""value"":""Green""}"; // don't work
            //string payload = @"{""value"":Microsoft.AspNet.OData.Test.CRUD.Color'Green'}"; // doesn't work
            string payload = @"{""@odata.type"":""#Microsoft.AspNet.OData.Test.CRUD.Color"",""value"":""Blue""}"; // work

            HttpRequestMessage request = new HttpRequestMessage(new HttpMethod("Patch"), requestUri);
            request.Content = new StringContent(payload);
            request.Content.Headers.ContentType = MediaTypeWithQualityHeaderValue.Parse("application/json");
            HttpResponseMessage response = _client.SendAsync(request).Result;

            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public void CanUpdateComplexProperty()
        {
            string requestUri = "http://localhost/odata/Customers(1)/Location";
            const string payload = "{\"value\":{" +
              "\"Street\":\"UpdatedStreet\"," +
              "\"City\":\"UpdatedCity\"" +
            "}}";

            HttpRequestMessage request = new HttpRequestMessage(new HttpMethod("Patch"), requestUri);
            request.Content = new StringContent(payload);
            request.Content.Headers.ContentType = MediaTypeWithQualityHeaderValue.Parse("application/json");
            HttpResponseMessage response = _client.SendAsync(request).Result;

            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public void CanUpdateCollectionProperty()
        {

        }

        private static IEdmModel GetEdmModel()
        {
            var builder = new ODataConventionModelBuilder();
            builder.EntitySet<Customer>("Customers");
            return builder.GetEdmModel();
        }

        public class CustomersController : ODataController
        {
            [HttpPatch]
            public IHttpActionResult PatchToName(int key, [FromBody] string name)
            {
                Assert.Equal(1, key);
                Assert.Equal("ChangedName", name);
                return Ok();
            }

            public IHttpActionResult PatchToLocation(int key, Delta<Address> patch)
            {
                Assert.Equal(new[] {"Street", "City"}, patch.GetChangedPropertyNames());

                // Verify the origin address
                Address address = new Address();
                patch.Patch(address);

                Assert.Equal("UpdatedStreet", address.Street);
                Assert.Equal("UpdatedCity", address.City);

                return Ok();
            }

            [HttpPatch]
            [ODataRoute("Customers({customerId})/FavoriteColor")]
            public IHttpActionResult AnyActionNameHere(int customerId, [FromBody] Color color)
            {
                // It'll be failed. See https://github.com/OData/WebApi/issues/742
                Assert.Equal(Color.Blue, color);

                return Ok();
            }

            public IHttpActionResult GetFavoriteColor(int key)
            {
                return Ok(Color.Blue);
            }

            public IHttpActionResult Get(int key)
            {
                return Ok(new Customer
                {
                    Id = 1,
                    Name = "name",
                    FavoriteColor = Color.Blue
                });
            }
        }

        public class Customer
        {
            public int Id { get; set; }
            public string Name { get; set; }

            public Color FavoriteColor { get; set; }

            public Address Location { get; set; }
        }

        public class Address
        {
            public string City { get; set; }
            public string Street { get; set; }
        }

        public enum Color
        {
            Red,
            Green,
            Blue
        }
    }
}

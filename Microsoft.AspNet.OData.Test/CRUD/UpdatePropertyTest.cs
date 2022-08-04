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
using Microsoft.OData.Edm.Library;
using Xunit;
using Xunit.Extensions;

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
        public void CanQueryEntity()
        {
            string requestUri = "http://localhost/odata/Customers(1)";

            HttpRequestMessage request = new HttpRequestMessage(new HttpMethod("Get"), requestUri);
            HttpResponseMessage response = _client.SendAsync(request).Result;

            response.EnsureSuccessStatusCode();
            Console.WriteLine(response.Content.ReadAsStringAsync().Result);
        }

        [Theory]
        [InlineData("Patch")]
        [InlineData("Put")]
        public void CanUpdatePrimitiveProperty(string httpMethod)
        {
            string requestUri = "http://localhost/odata/Customers(1)/Name";
            string payload = @"{""value"":""ChangedName""}";

            HttpRequestMessage request = new HttpRequestMessage(new HttpMethod(httpMethod), requestUri);
            request.Content = new StringContent(payload);
            request.Content.Headers.ContentType = MediaTypeWithQualityHeaderValue.Parse("application/json");
            HttpResponseMessage response = _client.SendAsync(request).Result;

            response.EnsureSuccessStatusCode();
        }

        [Theory]
        [InlineData("Patch")]
        [InlineData("Put")]
        [Category("Since v5.9.1")]
        public void CanUpdateEnumProperty(string httpMethod)
        {
            string requestUri = "http://localhost/odata/Customers(1)/FavoriteColor";

            //string payload = @"{""value"":""Green""}"; // don't work
            //string payload = @"{""value"":Microsoft.AspNet.OData.Test.CRUD.Color'Green'}"; // doesn't work
            string payload = @"{""@odata.type"":""#Microsoft.AspNet.OData.Test.CRUD.Color"",""value"":""Blue""}"; // work

            HttpRequestMessage request = new HttpRequestMessage(new HttpMethod(httpMethod), requestUri);
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
        [Category("This test case will fail, leave here just for reference.")]
        public void FailedToUpdateComplexPropertyUsingPayloadWithoutTopValue()
        {
            string requestUri = "http://localhost/odata/Customers(1)/Location";
            const string payload = "{" +
              "\"Street\":\"UpdatedStreet\"," +
              "\"City\":\"UpdatedCity\"" +
            "}";

            HttpRequestMessage request = new HttpRequestMessage(new HttpMethod("Patch"), requestUri);
            request.Content = new StringContent(payload);
            request.Content.Headers.ContentType = MediaTypeWithQualityHeaderValue.Parse("application/json");
            HttpResponseMessage response = _client.SendAsync(request).Result;

            string payload3 = response.Content.ReadAsStringAsync().Result;

            // Check the model binding:
            // 'A top-level property with name 'Street' was found in the payload; however, property and collection payloads must always have a top-level property with name 'value'."

            response.EnsureSuccessStatusCode(); // here will make the test failing
        }

        // To patch a collection property is not reasonable.
        [Fact]
        public void CanUpdateCollectionOfPrimitiveProperty()
        {
            string requestUri = "http://localhost/odata/Customers(1)/Dates";
            const string payload = "{\"value\":[" +
              "\"2011-08-11\"," +
              "\"2016-03-12\"," +
              "\"2019-09-01\"" +
            "]}";

            HttpRequestMessage request = new HttpRequestMessage(new HttpMethod("Put"), requestUri);
            request.Content = new StringContent(payload);
            request.Content.Headers.ContentType = MediaTypeWithQualityHeaderValue.Parse("application/json");
            HttpResponseMessage response = _client.SendAsync(request).Result;

            response.EnsureSuccessStatusCode();
        }

        [Fact]
        [Category("Bug: https://github.com/OData/odata.net/issues/580")]
        public void CanUpdateCollectionOfEnumProperty()
        {
            string requestUri = "http://localhost/odata/Customers(1)/Colors";
            const string payload = "{\"@odata.type\":\"Collection(Microsoft.AspNet.OData.Test.CRUD.Color)\",\"value\":[" +
              "\"Blue\"," +
              "\"Red\"," +
              "\"Green\"" +
            "]}";

            HttpRequestMessage request = new HttpRequestMessage(new HttpMethod("Put"), requestUri);
            request.Content = new StringContent(payload);
            request.Content.Headers.ContentType = MediaTypeWithQualityHeaderValue.Parse("application/json");
            HttpResponseMessage response = _client.SendAsync(request).Result;

            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public void CanUpdateCollectionOfComplexProperty()
        {
            string requestUri = "http://localhost/odata/Customers(1)/Addresses";

            string ComplexValue1 = "{\"Street\":\"NE 24th St.\",\"City\":\"Redmond\"}";
            string ComplexValue2 = "{\"Street\":\"LianHua Rd.\",\"City\":\"Shanghai\"}";
            string payload = "{\"value\":[" + ComplexValue1 + "," + ComplexValue2 + "]}";

            HttpRequestMessage request = new HttpRequestMessage(new HttpMethod("Put"), requestUri);
            request.Content = new StringContent(payload);
            request.Content.Headers.ContentType = MediaTypeWithQualityHeaderValue.Parse("application/json");
            HttpResponseMessage response = _client.SendAsync(request).Result;

            response.EnsureSuccessStatusCode();
        }

        private static IEdmModel GetEdmModel()
        {
            var builder = new ODataConventionModelBuilder();
            builder.EntitySet<Customer>("Customers");
            return builder.GetEdmModel();
        }

        public class CustomersController : ODataController
        {
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
                    Dates = new List<Date> { new Date(1999, 9, 1), new Date(2016, 5, 24) },
                    FavoriteColor = Color.Blue
                });
            }

            [HttpPatch]
            public IHttpActionResult PatchToName(int key, [FromBody] string name)
            {
                Assert.Equal(1, key);
                Assert.Equal("ChangedName", name);
                return Ok();
            }

            [HttpPut]
            public IHttpActionResult PutToName(int key, [FromBody] string name)
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
            [HttpPut]
            [ODataRoute("Customers({customerId})/FavoriteColor")]
            public IHttpActionResult AnyActionNameHere(int customerId, [FromBody] Color color)
            {
                // It'll be failed. See https://github.com/OData/WebApi/issues/742
                Assert.Equal(Color.Blue, color);

                return Ok();
            }

            // It's not reasonable to patch a collection. So, only "Put" is supported to update the collection.
            public IHttpActionResult PutToDates(int key, [FromBody]IEnumerable<Date> dates)
            {
                Assert.Equal(
                    new[]
                    {
                        new Date(2011, 8, 11),
                        new Date(2016, 3, 12),
                        new Date(2019, 9, 1)
                    }, dates);

                return Ok();
            }

            public IHttpActionResult PutToColors(int key, [FromBody]IEnumerable<Color> colors)
            {
                Assert.Equal(
                    new[]
                    {
                        Color.Blue,
                        Color.Red,
                        Color.Green
                    }, colors);

                return Ok();
            }

            public IHttpActionResult PutToAddresses(int key, [FromBody]IEnumerable<Address> addresses)
            {
                var enumerable = addresses as IList<Address> ?? addresses.ToList();
                Assert.Equal(2, enumerable.Count);

                Address address = enumerable.First();
                Assert.Equal("NE 24th St.", address.Street);
                Assert.Equal("Redmond", address.City);

                address = enumerable.Last();
                Assert.Equal("LianHua Rd.", address.Street);
                Assert.Equal("Shanghai", address.City);

                return Ok();
            }
        }

        public class Customer
        {
            public int Id { get; set; }
            public string Name { get; set; }

            public Color FavoriteColor { get; set; }

            public Address Location { get; set; }

            public IList<Date> Dates { get; set; }

            public IList<Color> Colors { get; set; }

            public IList<Address> Addresses { get; set; }
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

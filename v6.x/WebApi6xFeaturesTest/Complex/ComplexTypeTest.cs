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
using Microsoft.AspNet.OData.Test;
using Microsoft.OData.Edm;
using ModelLibrary;
using Xunit;
using Xunit.Abstractions;
using System.Net.Http.Headers;
using System.Net;

namespace WebApi6xFeaturesTest.QueryComplexTypeTest
{
    public class ComplexTypeTest
    {
        private readonly ITestOutputHelper _output;

        public ComplexTypeTest(ITestOutputHelper output)
        {
            this._output = output;
        }

        [Fact]
        public void PatchEntityWithCollectionOfComplexTypeProperty()
        {
            // Arrange
            const string payload = "{" +
              "\"Id\":99," +
              "\"Locations\":[{\"Street\":\"UpdatedStreet\",\"CityInfo\": {\"Id\": 100}}]" +
              "}";

            const string requestUri = "http://localhost/odata/OpenCustomers(1)";

            HttpClient client = GetClient();

            // Act
            HttpRequestMessage request = new HttpRequestMessage(new HttpMethod("Patch"), requestUri);
            request.Content = new StringContent(payload);
            request.Content.Headers.ContentType = MediaTypeWithQualityHeaderValue.Parse("application/json");
            HttpResponseMessage response = client.SendAsync(request).Result;

            // Assert
            Assert.True(response.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        private static HttpClient GetClient()
        {
            var config = new[] { typeof(MetadataController), typeof(OpenCustomersController) }.GetHttpConfiguration();
            config.Count().Filter().OrderBy().Expand().Select().MaxTop(null);

            IEdmModel model = GetEdmModel();
            config.MapODataServiceRoute("odata", "odata", model);
            HttpServer server = new HttpServer(config);
            return new HttpClient(server);
        }

        private static IEdmModel GetEdmModel()
        {
            ODataModelBuilder builder = new ODataConventionModelBuilder();
            builder.EntitySet<Customer>("OpenCustomers");
            return builder.GetEdmModel();
        }
    }

    public class OpenCustomersController : ODataController
    {
        public IHttpActionResult Patch(int key, Delta<Customer> patch)
        {
            Customer customer = new Customer();

            // Guard
            Assert.Equal(0, customer.Id);
            Assert.Null(customer.Locations);

            patch.Patch(customer);

            // Assert
            Assert.Equal(99, customer.Id);
            Assert.NotNull(customer.Locations);
            var location = Assert.Single(customer.Locations);
            Assert.Equal("UpdatedStreet", location.Street);
            Assert.NotNull(location.CityInfo);
            Assert.Equal(100, location.CityInfo.Id);

            return Updated(customer);
        }
    }
}

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

namespace WebApi6xFeaturesTest.QueryComplexTypeTest
{
    public class QueryComplexTypeTest
    {
        private readonly ITestOutputHelper _output;

        public QueryComplexTypeTest(ITestOutputHelper output)
        {
            this._output = output;
        }

        [Fact]
        public void CanReturnTheArrayOfComplexType()
        {
            ODataModelBuilder builder = new ODataConventionModelBuilder();
            builder.EntitySet<Customer>("Customers");
            HttpClient client = GetClient(builder.GetEdmModel());

            string requestUri = "http://localhost/odata/Customers(1)/Locations";

            HttpRequestMessage request = new HttpRequestMessage(new HttpMethod("Get"), requestUri);
            HttpResponseMessage response = client.SendAsync(request).Result;

            string result =
                @"{""@odata.context"":""http://localhost/odata/$metadata#Customers(1)/Locations"",""value"":[{""Street"":""Street #1""},{""Street"":""Street #2""},{""Street"":""Street #3""}]}";
            
            response.EnsureSuccessStatusCode();
            _output.WriteLine(response.Content.ReadAsStringAsync().Result);
            Assert.Equal(result, response.Content.ReadAsStringAsync().Result);
        }

        [Fact(Skip = "Can't work, need to continue to test")]
        public void CanExpandTheNavigationPropertyOnComplexType()
        {
            ODataModelBuilder builder = new ODataConventionModelBuilder();
            builder.EntitySet<Customer>("Customers");
            HttpClient client = GetClient(builder.GetEdmModel());

            string requestUri = "http://localhost/odata/Customers(1)/Locations?$expand=CityInfo";

            HttpRequestMessage request = new HttpRequestMessage(new HttpMethod("Get"), requestUri);
            HttpResponseMessage response = client.SendAsync(request).Result;

            string result =
                @"{""@odata.context"":""http://localhost/odata/$metadata#Customers(1)/Locations"",""value"":[{""Street"":""Street #1""},{""Street"":""Street #2""},{""Street"":""Street #3""}]}";

            response.EnsureSuccessStatusCode();
            _output.WriteLine(response.Content.ReadAsStringAsync().Result);
            Assert.Equal(result, response.Content.ReadAsStringAsync().Result);
        }

        private static HttpClient GetClient(IEdmModel model)
        {
            var config = new[] { typeof(MetadataController), typeof(CustomersController) }.GetHttpConfiguration();
            config.Count().Filter().OrderBy().Expand().Select().MaxTop(null);

            config.MapODataServiceRoute("odata", "odata", model);
            HttpServer server = new HttpServer(config);
            return new HttpClient(server);
        }
    }

    public class CustomersController : ODataController
    {
        [EnableQuery]
        public IHttpActionResult Get(int key)
        {
            Customer c = new Customer
            {
                Id = key,
                Location = new Address
                {
                    Street = "Shanghai Rd",
                    CityInfo = new City { Id = 99},
                    Cities = new List<City>()
                },
                Locations = Enumerable.Range(1, 3).Select(e => new Address
                {
                    Street = "Street #" + e,
                    CityInfo = new City { Id = e },
                    Cities = new List<City>()
                }).ToArray()
            };

            return Ok(c);
        }

        [EnableQuery]
        public IHttpActionResult GetLocations(int key)
        {
            Customer c = new Customer
            {
                Id = key,
                Locations = Enumerable.Range(1, 3).Select(e => new Address
                {
                    Street = "Street #" + e,
                    CityInfo = new City {Id = e},
                    Cities = new List<City>()
                }).ToArray()
            };

            return Ok(c.Locations);
        }
    }
}

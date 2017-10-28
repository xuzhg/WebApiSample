using Microsoft.AspNet.OData.Test;
using Microsoft.OData;
using Microsoft.OData.Edm;
using ModelLibrary;
using System.Net.Http;
using System.Web.Http;
using System.Web.OData;
using System.Web.OData.Builder;
using System.Web.OData.Extensions;
using Xunit;
using Xunit.Abstractions;
using Newtonsoft.Json.Linq;

namespace WebApi6xFeaturesTest.Dynamic
{
    public class DynamicPropertyQueryOptionTest
    {
        private readonly ITestOutputHelper _output;

        public DynamicPropertyQueryOptionTest(ITestOutputHelper output)
        {
            this._output = output;
        }

        [Fact]
        public void QueryEntitySet()
        {
            HttpClient client = GetClient(GetEdmModel());

            string requestUri = "http://localhost/odata/People";

            HttpRequestMessage request = new HttpRequestMessage(new HttpMethod("Get"), requestUri);
            HttpResponseMessage response = client.SendAsync(request).Result;

            response.EnsureSuccessStatusCode();
            string payload = response.Content.ReadAsStringAsync().Result;
            _output.WriteLine(payload);

            JObject obj = JObject.Parse(payload);
            Assert.Equal(2, ((JArray)obj["value"]).Count);
        }

        [Fact]
        public void FilterDynamicProperties()
        {
            HttpClient client = GetClient(GetEdmModel());

            string requestUri = @"http://localhost/odata/People?$filter=foo eq 'foo2'";

            HttpRequestMessage request = new HttpRequestMessage(new HttpMethod("Get"), requestUri);
            HttpResponseMessage response = client.SendAsync(request).Result;

            response.EnsureSuccessStatusCode();
            string payload = response.Content.ReadAsStringAsync().Result;
            _output.WriteLine(payload);

            JObject obj = JObject.Parse(payload);
            JToken value = Assert.Single(((JArray)obj["value"]));
            Assert.Equal("2", obj["value"][0]["Id"]);
        }

        [Fact]
        public void SelectDynamicProperties()
        {
            HttpClient client = GetClient(GetEdmModel());

            string requestUri = @"http://localhost/odata/People?$select=foo";

            HttpRequestMessage request = new HttpRequestMessage(new HttpMethod("Get"), requestUri);
            HttpResponseMessage response = client.SendAsync(request).Result;

            response.EnsureSuccessStatusCode();
            string payload = response.Content.ReadAsStringAsync().Result;
            _output.WriteLine(payload);

            Assert.Equal("{\"@odata.context\":\"http://localhost/odata/$metadata#People(foo)\",\"value\":[{\"foo\":\"foo1\"},{\"foo\":\"foo2\"}]}", payload);
        }

        private static HttpClient GetClient(IEdmModel model)
        {
            var config = new[] { typeof(MetadataController), typeof(PeopleController) }.GetHttpConfiguration();
            config.Count().Filter().OrderBy().Select();
            config.MapODataServiceRoute("odata", "odata", model);
            HttpServer server = new HttpServer(config);
            return new HttpClient(server);
        }

        private static IEdmModel GetEdmModel()
        {
            ODataModelBuilder builder = new ODataConventionModelBuilder();
            builder.EntitySet<Person>("People");
            return builder.GetEdmModel();
        }
    }
}

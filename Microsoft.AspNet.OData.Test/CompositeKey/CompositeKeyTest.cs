using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.OData;
using System.Web.OData.Extensions;
using System.Web.OData.Routing;
using System.Web.OData.Routing.Conventions;
using Microsoft.OData.Edm;
using Xunit;
using Xunit.Extensions;

namespace Microsoft.AspNet.OData.Test.CompositeKey
{
    public class CompositeKeyTest
    {
        private readonly HttpConfiguration _configuration;
        private HttpClient _client;

        public CompositeKeyTest()
        {
            _configuration = new[]
            {
                typeof(MetadataController), typeof(DriverReleaseLifecycleDescriptionsController),
                typeof(AnyController)
            }.GetHttpConfiguration();

            IEdmModel model = CompositeEdmModel.GetEdmModel();

            // only convention routings
            var routingConventions = ODataRoutingConventions.CreateDefault();
            routingConventions.Insert(0, new CompositeKeyRoutingConvention());
            _configuration.MapODataServiceRoute("odata1", "odata", model, new DefaultODataPathHandler(), routingConventions);

            // only attribute routings
            var attrRouting = new AttributeRoutingConvention(model, _configuration);
            IList<IODataRoutingConvention> attributeRoutingConventions = new List<IODataRoutingConvention> {attrRouting};
            _configuration.MapODataServiceRoute("odata2", "attribute", model, new DefaultODataPathHandler(),
                attributeRoutingConventions);

            HttpServer server = new HttpServer(_configuration);
            _client = new HttpClient(server);
        }

        [Theory]
        [InlineData("odata")]
        [InlineData("attribute")]
        public void QueryEntity(string route)
        {
            string requestUri = String.Format("http://localhost/{0}/DriverReleaseLifecycleDescriptions(DriverReleaseLifecycleStateId=3,DriverReleaseLifecycleSubstateId=18)", route);

            string result = @"{
  ""@odata.context"":""http://localhost/XXX/$metadata#Edm.String"",""value"":""(3,18)""
}".Replace("XXX", route);

            HttpResponseMessage response = _client.GetAsync(requestUri).Result;

            response.EnsureSuccessStatusCode();

            Assert.Equal(result, response.Content.ReadAsStringAsync().Result);
        }

        [Theory]
        [InlineData("odata")]
        [InlineData("attribute")]
        public void PatchEntity(string route)
        {
            string requestUri = String.Format("http://localhost/{0}/DriverReleaseLifecycleDescriptions(DriverReleaseLifecycleStateId=4,DriverReleaseLifecycleSubstateId=5)", route);

            string result = "{\"Text\":\"Waiting for launch approval\",\"ColorName\":\"Gray\"}";

            HttpRequestMessage request = new HttpRequestMessage(new HttpMethod("Patch"), requestUri)
            {
                Content = new StringContent(result)
            };
            request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
            HttpResponseMessage response = _client.SendAsync(request).Result;

            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public void MetadataTest()
        {
            string requestUri = "http://localhost/odata/$metadata";

            string result = @"<?xml version=""1.0"" encoding=""utf-8""?>
<edmx:Edmx Version=""4.0"" xmlns:edmx=""http://docs.oasis-open.org/odata/ns/edmx"">
  <edmx:DataServices>
    <Schema Namespace=""Microsoft.AspNet.OData.Test.CompositeKey"" xmlns=""http://docs.oasis-open.org/odata/ns/edm"">
      <EntityType Name=""DriverReleaseLifecycleDescription"">
        <Key>
          <PropertyRef Name=""DriverReleaseLifecycleStateId"" />
          <PropertyRef Name=""DriverReleaseLifecycleSubstateId"" />
        </Key>
        <Property Name=""Text"" Type=""Edm.String"" />
        <Property Name=""ColorName"" Type=""Edm.String"" />
        <Property Name=""DriverReleaseLifecycleStateId"" Type=""Edm.Int32"" Nullable=""false"" />
        <Property Name=""DriverReleaseLifecycleSubstateId"" Type=""Edm.Int32"" Nullable=""false"" />
        <Property Name=""InsertedTime"" Type=""Edm.DateTimeOffset"" Nullable=""false"" />
        <Property Name=""UpdatedTime"" Type=""Edm.DateTimeOffset"" Nullable=""false"" />
        <Property Name=""Version"" Type=""Edm.Binary"" ConcurrencyMode=""Fixed"" />
      </EntityType>
    </Schema>
    <Schema Namespace=""Default"" xmlns=""http://docs.oasis-open.org/odata/ns/edm"">
      <EntityContainer Name=""Container"">
        <EntitySet Name=""DriverReleaseLifecycleDescriptions"" EntityType=""Microsoft.AspNet.OData.Test.CompositeKey.DriverReleaseLifecycleDescription"">
          <Annotation Term=""Org.OData.Core.V1.OptimisticConcurrency"">
            <Collection>
              <PropertyPath>Version</PropertyPath>
            </Collection>
          </Annotation>
        </EntitySet>
      </EntityContainer>
    </Schema>
  </edmx:DataServices>
</edmx:Edmx>";

            HttpRequestMessage request = new HttpRequestMessage(new HttpMethod("Get"), requestUri);
            HttpResponseMessage response = _client.SendAsync(request).Result;

            response.EnsureSuccessStatusCode();

            Assert.Equal(result, response.Content.ReadAsStringAsync().Result);
        } 
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.OData;
using System.Web.OData.Builder;
using System.Web.OData.Extensions;
using Microsoft.OData.Edm;
using Xunit;

namespace Microsoft.AspNet.OData.Test.CRUD
{
    public class UpdateEntityTest
    {
        private readonly HttpConfiguration _configuration;
        private HttpClient _client;

        public UpdateEntityTest()
        {
            _configuration = new[] { typeof(MetadataController), typeof(ConnectorGroupsController) }.GetHttpConfiguration();
            _configuration.MapODataServiceRoute("odata", "odata", GetEdmModel());
            HttpServer server = new HttpServer(_configuration);
            _client = new HttpClient(server);
        }

        [Fact]
        public void CanUpdateEntityWithoutComplexProperty()
        {
            string requestUri = "http://localhost/odata/ConnectorGroups('abc')";
            string payload = @"{""Name"":""blah""}";

            HttpRequestMessage request = new HttpRequestMessage(new HttpMethod("Patch"), requestUri);
            request.Content = new StringContent(payload);
            request.Content.Headers.ContentType = MediaTypeWithQualityHeaderValue.Parse("application/json");
            HttpResponseMessage response = _client.SendAsync(request).Result;

            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public void CanUpdateEntityWithComplexProperty()
        {
            string requestUri = "http://localhost/odata/ConnectorGroups('ijk')";
            string payload = @"{""Name"":""Sam"", ""Member"":{""Title"":""New Title""}}";

            HttpRequestMessage request = new HttpRequestMessage(new HttpMethod("Patch"), requestUri);
            request.Content = new StringContent(payload);
            request.Content.Headers.ContentType = MediaTypeWithQualityHeaderValue.Parse("application/json");
            HttpResponseMessage response = _client.SendAsync(request).Result;

            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public void CanUpdateEntityWithEmptyCollectComplexProperty()
        {
            string requestUri = "http://localhost/odata/ConnectorGroups('rst')";
            string payload = @"{""Name"":""Fan"", ""Members"":[]}";

            HttpRequestMessage request = new HttpRequestMessage(new HttpMethod("Patch"), requestUri);
            request.Content = new StringContent(payload);
            request.Content.Headers.ContentType = MediaTypeWithQualityHeaderValue.Parse("application/json");
            HttpResponseMessage response = _client.SendAsync(request).Result;

            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public void CanUpdateEntityWithNonEmptyCollectComplexProperty()
        {
            string requestUri = "http://localhost/odata/ConnectorGroups('xyz')";
            string payload = @"{""Name"":""Microsoft"", ""Members"":[{""Title"":""Redmond""},{""Title"":""Shanghai""}]}";

            HttpRequestMessage request = new HttpRequestMessage(new HttpMethod("Patch"), requestUri);
            request.Content = new StringContent(payload);
            request.Content.Headers.ContentType = MediaTypeWithQualityHeaderValue.Parse("application/json");
            HttpResponseMessage response = _client.SendAsync(request).Result;

            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public void MetadataTest()
        {
            string requestUri = "http://localhost/odata/$metadata";

            string result = @"<?xml version=""1.0"" encoding=""utf-8""?>
<edmx:Edmx Version=""4.0"" xmlns:edmx=""http://docs.oasis-open.org/odata/ns/edmx"">
  <edmx:DataServices>
    <Schema Namespace=""Microsoft.AspNet.OData.Test.CRUD"" xmlns=""http://docs.oasis-open.org/odata/ns/edm"">
      <EntityType Name=""ConnectorGroup"">
        <Key>
          <PropertyRef Name=""Id"" />
        </Key>
        <Property Name=""Id"" Type=""Edm.String"" Nullable=""false"" />
        <Property Name=""Name"" Type=""Edm.String"" />
        <Property Name=""Member"" Type=""Microsoft.AspNet.OData.Test.CRUD.Connector"" />
        <Property Name=""Members"" Type=""Collection(Microsoft.AspNet.OData.Test.CRUD.Connector)"" />
      </EntityType>
      <ComplexType Name=""Connector"">
        <Property Name=""Title"" Type=""Edm.String"" />
      </ComplexType>
    </Schema>
    <Schema Namespace=""Default"" xmlns=""http://docs.oasis-open.org/odata/ns/edm"">
      <EntityContainer Name=""Container"">
        <EntitySet Name=""ConnectorGroups"" EntityType=""Microsoft.AspNet.OData.Test.CRUD.ConnectorGroup"" />
      </EntityContainer>
    </Schema>
  </edmx:DataServices>
</edmx:Edmx>";

            HttpRequestMessage request = new HttpRequestMessage(new HttpMethod("Get"), requestUri);
            HttpResponseMessage response = _client.SendAsync(request).Result;

            response.EnsureSuccessStatusCode();

            Assert.Equal(result, response.Content.ReadAsStringAsync().Result);
        } 

        private static IEdmModel GetEdmModel()
        {
            var builder = new ODataConventionModelBuilder();
            builder.EntitySet<ConnectorGroup>("ConnectorGroups");
            return builder.GetEdmModel();
        }
    }

    public class ConnectorGroupsController : ODataController
    {
        public IHttpActionResult Patch([FromODataUri] string key, Delta<ConnectorGroup> patch)
        {
            ConnectorGroup a = new ConnectorGroup();

            // before patch
            Assert.Null(a.Name);
            Assert.Null(a.Member);
            Assert.Null(a.Members);

            patch.Patch(a);

            if (key == "abc")
            {
                // patch without complex property
                Assert.Equal("blah", a.Name);
                Assert.Null(a.Member);
                Assert.Null(a.Members);
            }
            else if (key == "ijk")
            {
                // patch with complex property
                Assert.Equal("Sam", a.Name);
                Assert.NotNull(a.Member);
                Assert.Equal("New Title", a.Member.Title);
                Assert.Null(a.Members);
            }
            else if (key == "rst")
            {
                // patch with empty collect of complex property
                Assert.Equal("Fan", a.Name);
                Assert.Null(a.Member);
                Assert.NotNull(a.Members);
                Assert.Empty(a.Members);
            }
            else if (key == "xyz")
            {
                // patch with non-empty collect of complex property
                Assert.Equal("Microsoft", a.Name);
                Assert.Null(a.Member);

                Assert.NotNull(a.Members);
                Assert.NotEmpty(a.Members);

                Assert.Equal(2, a.Members.Count());
                Assert.Equal("Redmond", a.Members.First().Title);
                Assert.Equal("Shanghai", a.Members.Last().Title);
            }

            return Updated(a);
        }
    }

    public class ConnectorGroup
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public Connector Member { get; set; }

        public IEnumerable<Connector> Members { get; set; }
    }

    public class Connector
    {
        public string Title { get; set; }
    }
}

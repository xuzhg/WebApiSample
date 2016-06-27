using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.OData;
using System.Web.OData.Builder;
using System.Web.OData.Extensions;
using System.Web.OData.Routing;
using Microsoft.AspNet.OData.Test.CRUD;
using Microsoft.OData.Edm;
using Xunit;

namespace Microsoft.AspNet.OData.Test.Action
{
    public class ActionTest
    {
        private readonly HttpConfiguration _configuration;
        private HttpClient _client;

        public ActionTest()
        {
            _configuration = new[] { typeof(MetadataController), typeof(SheadsController) }.GetHttpConfiguration();
            _configuration.MapODataServiceRoute("odata", "odata", GetEdmModel());
            HttpServer server = new HttpServer(_configuration);
            _client = new HttpClient(server);
        }

        [Fact]
        public void MetadataTest()
        {
            string requestUri = "http://localhost/odata/$metadata";

            string result = @"<?xml version=""1.0"" encoding=""utf-8""?>
<edmx:Edmx Version=""4.0"" xmlns:edmx=""http://docs.oasis-open.org/odata/ns/edmx"">
  <edmx:DataServices>
    <Schema Namespace=""Microsoft.AspNet.OData.Test.Action"" xmlns=""http://docs.oasis-open.org/odata/ns/edm"">
      <EntityType Name=""Shead"">
        <Key>
          <PropertyRef Name=""Id"" />
        </Key>
        <Property Name=""Id"" Type=""Edm.Int32"" Nullable=""false"" />
      </EntityType>
      <ComplexType Name=""InvoiceReference"">
        <Property Name=""InvoiceNumber"" Type=""Edm.String"" />
        <Property Name=""SupplierId"" Type=""Edm.Int32"" Nullable=""false"" />
      </ComplexType>
    </Schema>
    <Schema Namespace=""Default"" xmlns=""http://docs.oasis-open.org/odata/ns/edm"">
      <Action Name=""ByRefs"" IsBound=""true"">
        <Parameter Name=""bindingParameter"" Type=""Collection(Microsoft.AspNet.OData.Test.Action.Shead)"" />
        <Parameter Name=""refs"" Type=""Collection(Microsoft.AspNet.OData.Test.Action.InvoiceReference)"" />
        <ReturnType Type=""Collection(Microsoft.AspNet.OData.Test.Action.Shead)"" />
      </Action>
      <EntityContainer Name=""Container"">
        <EntitySet Name=""Sheads"" EntityType=""Microsoft.AspNet.OData.Test.Action.Shead"" />
      </EntityContainer>
    </Schema>
  </edmx:DataServices>
</edmx:Edmx>";

            HttpRequestMessage request = new HttpRequestMessage(new HttpMethod("Get"), requestUri);
            HttpResponseMessage response = _client.SendAsync(request).Result;

            response.EnsureSuccessStatusCode();
            Assert.Equal(result, response.Content.ReadAsStringAsync().Result);
        }

        [Fact]
        public void CanWorkForCollectionComplexParameter()
        {
            string requestUri = "http://localhost/odata/Sheads/Default.ByRefs";

            string result = @"{
  ""refs"": [
         {
          ""InvoiceNumber"": ""5100011759"",
          ""SupplierId"": 3
         },
         {
            ""InvoiceNumber"": ""5100012624"",
            ""SupplierId"": 4
        },
        {
            ""InvoiceNumber"": ""5100012625"",
            ""SupplierId"": 5
        }
    ]
}";

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, requestUri)
            {
                Content = new StringContent(result)
            };
            request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

            HttpResponseMessage response = _client.SendAsync(request).Result;

            response.EnsureSuccessStatusCode();

            Assert.Equal(@"{
  ""@odata.context"":""http://localhost/odata/$metadata#Edm.String"",""value"":""5100011759|3,5100012624|4,5100012625|5""
}", response.Content.ReadAsStringAsync().Result);
        }

        private static IEdmModel GetEdmModel()
        {
            var builder = new ODataConventionModelBuilder();
            var getByRefs = builder.EntityType<Shead>().Collection.Action("ByRefs");
            getByRefs.CollectionParameter<InvoiceReference>("refs");
            getByRefs.ReturnsCollectionFromEntitySet<Shead>("Sheads");
            return builder.GetEdmModel();
        }
    }

    public class SheadsController : ODataController
    {
        [HttpPost]
        [EnableQuery]
        [ODataRoute("Sheads/Default.ByRefs")]
        public IHttpActionResult ByRefs(ODataActionParameters p)
        {
            object value;
            bool bOk = p.TryGetValue("refs", out value);
            Assert.True(bOk);

            IEnumerable<InvoiceReference> refs = value as IEnumerable<InvoiceReference>;
            Assert.NotNull(refs);
            Assert.Equal(3, refs.Count());

            return Ok(String.Join(",", refs.Select(e => e.InvoiceNumber + "|" + e.SupplierId)));
        }
    }

    public class Shead
    {
        public int Id { get; set; }
    }

    public class InvoiceReference
    {
        public string InvoiceNumber { get; set; }
        public int SupplierId { get; set; }
    }
}

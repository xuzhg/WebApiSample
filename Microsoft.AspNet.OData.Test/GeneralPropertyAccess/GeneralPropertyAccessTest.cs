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
using System.Web.OData.Builder;

namespace Microsoft.AspNet.OData.Test.GeneralPropertyAccess
{
    public class GeneralPropertyAccessTest
    {
        private readonly HttpConfiguration _configuration;
        private HttpClient _client;

        public GeneralPropertyAccessTest()
        {
            _configuration = new[]
            {
                typeof(MetadataController), typeof(LotOfPropertiesEntitiesController)
            }.GetHttpConfiguration();

            IEdmModel model = GetEdmModel();

            // only convention routings
            var routingConventions = ODataRoutingConventions.CreateDefault();
            routingConventions.Insert(0, new GeneralPropertyRoutingConvention());
            _configuration.MapODataServiceRoute("odata", "odata", model, new DefaultODataPathHandler(), routingConventions);

            HttpServer server = new HttpServer(_configuration);
            _client = new HttpClient(server);
        }

        [Theory]
        [InlineData("Property1")]
        [InlineData("Property2")]
        [InlineData("Property3")]
        [InlineData("Property4")]
        [InlineData("Property5")]
        [InlineData("Property6")]
        [InlineData("Property7")]
        public void QueryEntityProperty(string propertyName)
        {
            string requestUri = "http://localhost/odata/LotOfPropertiesEntities(1)/" + propertyName;

            string result = @"{
  ""@odata.context"":""http://localhost/odata/$metadata#LotOfPropertiesEntities(1)/XXX"",""value"":""XXX""
}".Replace("XXX", propertyName);

            HttpResponseMessage response = _client.GetAsync(requestUri).Result;

            response.EnsureSuccessStatusCode();

            Assert.Equal(result, response.Content.ReadAsStringAsync().Result);
        }

        private static IEdmModel GetEdmModel()
        {
            var builder = new ODataConventionModelBuilder();
            builder.EntitySet<LotOfPropertiesEntity>("LotOfPropertiesEntities");
            return builder.GetEdmModel();
        }
    }

    public class LotOfPropertiesEntity
    {
        public int Id { get; set; }

        public string Property1 { get; set; }
        public bool Property2 { get; set; }
        public byte Property3 { get; set; }
        public short Property4 { get; set; }
        public DateTimeOffset Property5 { get; set; }
        public Guid Property6 { get; set; }
        public decimal Property7 { get; set; }
        public float Property8 { get; set; }
        public int Property9 { get; set; }
        public double Property10 { get; set; }
    }
}

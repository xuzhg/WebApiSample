using Microsoft.AspNet.OData.Test;
using Microsoft.OData;
using Microsoft.OData.Edm;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.OData;
using System.Web.OData.Builder;
using System.Web.OData.Extensions;
using Xunit;
using Xunit.Abstractions;

namespace WebApi6xFeaturesTest.KeyAsSegment
{
    public class KeyAsSegmentTest
    {
        private readonly ITestOutputHelper _output;

        public KeyAsSegmentTest(ITestOutputHelper output)
        {
            this._output = output;
        }

        [Fact]
        public void KeyAsSegmentTestNormal()
        {
            ODataModelBuilder builder = new ODataConventionModelBuilder();
            builder.EntitySet<Entity>("Entities");
            HttpClient client = GetClient(builder.GetEdmModel());

            string requestUri = "http://localhost/odata/Entities?$format=application/json;odata.metadata=full";

            HttpRequestMessage request = new HttpRequestMessage(new HttpMethod("Get"), requestUri);
            HttpResponseMessage response = client.SendAsync(request).Result;

            response.EnsureSuccessStatusCode();
            _output.WriteLine(response.Content.ReadAsStringAsync().Result);
        }

        [Fact]
        public void KeyAsSegmentTestNormalQueryEntity()
        {
            ODataModelBuilder builder = new ODataConventionModelBuilder();
            builder.EntitySet<Entity>("Entities");
            HttpClient client = GetClient(builder.GetEdmModel());

           // string requestUri = "http://localhost/odata/Entities/1?$format=application/json;odata.metadata=full";
            string requestUri = "http://localhost/odata/Entities(IntProp=42,StringProp='xyz123')?$format=application/json;odata.metadata=full";
           // string requestUri = "http://localhost/odata/Entities/(IntProp=42,StringProp='xyz123')?$format=application/json;odata.metadata=full";

            HttpRequestMessage request = new HttpRequestMessage(new HttpMethod("Get"), requestUri);
            HttpResponseMessage response = client.SendAsync(request).Result;

            response.EnsureSuccessStatusCode();
            _output.WriteLine(response.Content.ReadAsStringAsync().Result);
        }

        private static HttpClient GetClient(IEdmModel model)
        {
            var config = new[] { typeof(MetadataController), typeof(EntitiesController) }.GetHttpConfiguration();
            config.Count().Filter().OrderBy().Expand().Select().MaxTop(null);
            config.SetUrlKeyDelimiter(ODataUrlKeyDelimiter.Slash);
            config.MapODataServiceRoute("odata", "odata", model);
            HttpServer server = new HttpServer(config);
            return new HttpClient(server);
        }
    }

    public class EntitiesController : ODataController
    {
        [EnableQuery]
        public IHttpActionResult Get()
        {
            Entity entity = new Entity
            {
                IntProp = 2,
                StringProp = "abc"
            };

            return Ok(new Entity[] { entity } );
        }

        [EnableQuery]
        public IHttpActionResult Get(int keyIntProp, [FromODataUri]string keyStringProp)
        {
            Entity entity = new Entity
            {
                IntProp = keyIntProp,
                // StringProp = "xyz"
                StringProp = keyStringProp
            };

            return Ok(new Entity[] { entity });
        }
    }

    public class Entity
    {
        [Key]
        public int IntProp { get; set; }

        [Key]
        public string StringProp { get; set; }
    }
}

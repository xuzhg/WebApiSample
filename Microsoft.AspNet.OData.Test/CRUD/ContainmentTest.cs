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
using System.Web.OData.Routing;
using Microsoft.OData.Edm;
using Xunit;

namespace Microsoft.AspNet.OData.Test.CRUD
{
    public class ContainmentTest
    {
        private readonly HttpConfiguration _configuration;
        private HttpClient _client;

        public ContainmentTest()
        {
            _configuration = new[] { typeof(MetadataController), typeof(MeController) }.GetHttpConfiguration();
            _configuration.MapODataServiceRoute("odata", "odata", GetEdmModel());
            HttpServer server = new HttpServer(_configuration);
            _client = new HttpClient(server);
        }

        [Fact]
        public void CanQueryMetadata()
        {
            string requestUri = "http://localhost/odata/$metadata";

            HttpRequestMessage request = new HttpRequestMessage(new HttpMethod("Get"), requestUri);
            HttpResponseMessage response = _client.SendAsync(request).Result;

            response.EnsureSuccessStatusCode();
            Console.WriteLine(response.Content.ReadAsStringAsync().Result);
        }

        [Fact]
        public void CanQuerySingleton()
        {
            string requestUri = "http://localhost/odata/Me";

            HttpRequestMessage request = new HttpRequestMessage(new HttpMethod("Get"), requestUri);
            HttpResponseMessage response = _client.SendAsync(request).Result;

            response.EnsureSuccessStatusCode();
            Console.WriteLine(response.Content.ReadAsStringAsync().Result);
        }

        [Fact]
        public void CanQuerySingletonNavigationProperty()
        {
            string requestUri = "http://localhost/odata/Me/Pdp/BLIS/Entries('2')";

            HttpRequestMessage request = new HttpRequestMessage(new HttpMethod("Get"), requestUri);
            HttpResponseMessage response = _client.SendAsync(request).Result;

            response.EnsureSuccessStatusCode();
            Console.WriteLine(response.Content.ReadAsStringAsync().Result);

            Assert.Equal(@"{
  ""@odata.context"":""http://localhost/odata/$metadata#Facets/$entity"",""Id"":""2""
}", response.Content.ReadAsStringAsync().Result);
        }

        private static IEdmModel GetEdmModel()
        {
            var builder = new ODataConventionModelBuilder();
            builder.Singleton<User>("Me").HasRequiredBinding(u => u.Pdp, "Pdps");
            builder.EntitySet<Pdp>("Pdps").HasRequiredBinding(p => p.BLIS, "BLISs");
            builder.EntitySet<BLISFacetDomain>("BLISs").HasManyBinding(b => b.Entries, "Facets");
            return builder.GetEdmModel();
        }

        public class MeController : ODataController
        {
            public IHttpActionResult Get()
            {
                ContainmentTest.User me = new User();
                me.Id = "1";
                me.Pdp = new Pdp();
                me.Pdp.UserId = "2";
                me.Pdp.BLIS = new BLISFacetDomain();
                me.Pdp.BLIS.Id = "3";
                me.Pdp.BLIS.Entries = new BlisFacet[] { new BlisFacet { Id = "4"}, new BlisFacet { Id = "5"}};
                return Ok(me);
            }

            [HttpGet]
            [ODataRoute("Me/Pdp/BLIS/Entries({key})")]
            public IHttpActionResult GetEntries([FromODataUri]string key)
            {
                ContainmentTest.User me = new User();
                me.Id = "1";
                me.Pdp = new Pdp();
                me.Pdp.UserId = "2";
                me.Pdp.BLIS = new BLISFacetDomain();
                me.Pdp.BLIS.Id = "3";
                me.Pdp.BLIS.Entries = new BlisFacet[] { new BlisFacet { Id = "4" }, new BlisFacet { Id = "5" } };

                return Ok(new BlisFacet { Id = key });
            }
        }

        public class User
        {
            public string Id { get; set; }

            [Contained]
            public Pdp Pdp { get; set; }
        }

        public class Pdp
        {
            [Key]
            public string UserId { get; set; }

            public BLISFacetDomain BLIS { get; set; }
        }

        public class BLISFacetDomain
        {
            public string Id { get; set; }

            [Contained]
            public IList<BlisFacet> Entries { get; set; }
        }

        public class BlisFacet
        {
            public string Id { get; set; }
        }
    }
}

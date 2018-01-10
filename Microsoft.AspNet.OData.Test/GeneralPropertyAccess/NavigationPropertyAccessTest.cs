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
    public class NavigationPropertyAccessTest
    {
        private readonly HttpConfiguration _configuration;
        private HttpClient _client;

        public NavigationPropertyAccessTest()
        {
            _configuration = new[]
            {
                typeof(MetadataController), typeof(NavCustomersController)
            }.GetHttpConfiguration();

            IEdmModel model = GetEdmModel();

            // only convention routings
            var routingConventions = ODataRoutingConventions.CreateDefault();
            routingConventions.Insert(0, new NavPropertyRoutingConvention());
            _configuration.MapODataServiceRoute("odata", "odata", model, new DefaultODataPathHandler(), routingConventions);

            HttpServer server = new HttpServer(_configuration);
            _client = new HttpClient(server);
        }

        [Fact]
        public void QueryNavProperty()
        {
            string requestUri = "http://localhost/odata/NavCustomers(1)/Orders(2)";

            string result = @"{
  ""@odata.context"":""http://localhost/odata/$metadata#NavOrders/$entity"",""Id"":2
}";

            HttpResponseMessage response = _client.GetAsync(requestUri).Result;

            response.EnsureSuccessStatusCode();

            Assert.Equal(result, response.Content.ReadAsStringAsync().Result);
        }

        private static IEdmModel GetEdmModel()
        {
            var builder = new ODataConventionModelBuilder();
            builder.EntitySet<NavCustomer>("NavCustomers");
            builder.EntitySet<NavOrder>("NavOrders");
            return builder.GetEdmModel();
        }
    }
    public class NavCustomersController : ODataController
    {
        [HttpGet]
        public IHttpActionResult GetOrders(int key, int relatedKey)
        {
            NavCustomer customer = new NavCustomer
            {
                Id = key,
                Orders = Enumerable.Range(1, 5).Select(c => new NavOrder { Id = c }).ToList()
            };

            return Ok(customer.Orders.FirstOrDefault(o => o.Id == relatedKey));
        }
    }

    public class NavCustomer
    {
        public int Id { get; set; }

        public IList<NavOrder> Orders { get; set; }
    }

    public class NavOrder
    {
        public int Id { get; set; }
    }
}

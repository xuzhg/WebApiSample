using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.OData;
using System.Web.OData.Builder;
using System.Web.OData.Extensions;
using Microsoft.AspNet.OData.Test;
using Microsoft.OData.Edm;
using Xunit;
using Xunit.Abstractions;
using System.Web.OData.Routing.Conventions;
using ODataPath = System.Web.OData.Routing.ODataPath;
using System.Web.Http.Controllers;
using Microsoft.OData.UriParser;
using System.Web.OData.Routing;

namespace WebApi6xFeaturesTest.NavigationPropertyOnComplexType
{
    public class NavigationPropertyTest
    {
        private readonly ITestOutputHelper _output;

        public NavigationPropertyTest(ITestOutputHelper output)
        {
            this._output = output;
        }

        [Fact]
        public void QueryNavProperty()
        {
            IEdmModel model = GetEdmModel();
            HttpClient client = GetClient(model);

            string requestUri = "http://localhost/odata/NavCustomers(1)/Orders(2)";

            string result = @"{""@odata.context"":""http://localhost/odata/$metadata#NavOrders/$entity"",""Id"":2}";

            HttpResponseMessage response = client.GetAsync(requestUri).Result;

            response.EnsureSuccessStatusCode();

            Assert.Equal(result, response.Content.ReadAsStringAsync().Result);
        }

        private static HttpClient GetClient(IEdmModel model)
        {
            var config = new[] { typeof(MetadataController), typeof(NavCustomersController) }.GetHttpConfiguration();

            var routingConventions = ODataRoutingConventions.CreateDefault();
            routingConventions.Insert(0, new NavPropertyRoutingConvention());

            config.MapODataServiceRoute("odata", "odata", model, new DefaultODataPathHandler(), routingConventions);
            HttpServer server = new HttpServer(config);
            return new HttpClient(server);
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

    public class NavPropertyRoutingConvention : NavigationRoutingConvention
    {
        public override string SelectAction(ODataPath odataPath, HttpControllerContext controllerContext, ILookup<string, HttpActionDescriptor> actionMap)
        {
            HttpMethod httpMethod = controllerContext.Request.Method;
            if (httpMethod != HttpMethod.Get)
            {
                return null;
            }

            if (odataPath.PathTemplate == "~/entityset/key/navigation/key")
            {
                KeySegment segment = (KeySegment)odataPath.Segments[1];
                // object value = ODataUriUtils.ConvertFromUriLiteral(segment.Keys, ODataVersion.V4);
                object value = segment.Keys.First().Value; // I don't check the keys number.
                controllerContext.RouteData.Values.Add("key", value);

                segment = (KeySegment)odataPath.Segments[3];
                // value = ODataUriUtils.ConvertFromUriLiteral(segment.Value, ODataVersion.V4);
                value = segment.Keys.First().Value;
                controllerContext.RouteData.Values.Add("relatedKey", value);

                NavigationPropertySegment property = odataPath.Segments[2] as NavigationPropertySegment;
                // controllerContext.RouteData.Values.Add("propertyName", property.NavigationProperty.Name);

                return "Get" + property.NavigationProperty.Name;
            }

            return null;
        }
    }
}

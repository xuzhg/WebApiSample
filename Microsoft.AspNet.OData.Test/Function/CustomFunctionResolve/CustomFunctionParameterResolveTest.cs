using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.OData;
using System.Web.OData.Builder;
using System.Web.OData.Extensions;
using System.Web.OData.Query;
using System.Web.OData.Routing;
using Microsoft.AspNet.OData.Test.Function.CustomFunctionResolve;
using Microsoft.OData.Edm;
using Xunit;

namespace Microsoft.AspNet.OData.Test.Function
{
    public class CustomFunctionParameterResolveTest
    {
        [Fact]
        public void FunctionParameterResolveUsingCustomerRoutingConvention()
        {
            var configuration = new[] { typeof(MetadataController), typeof(CustomersController) }.GetHttpConfiguration();

            var conventions = System.Web.OData.Routing.Conventions.ODataRoutingConventions.CreateDefault();

            conventions.Insert(0, new CustomControllerRoutingConvention());

            configuration.MapODataServiceRoute("odata", "odata", GetEdmModel(), new DefaultODataPathHandler(), conventions);
            HttpServer server = new HttpServer(configuration);
            var client = new HttpClient(server);

            var response = client.GetAsync("http://localhost/odata/Customers/Default.TestFunc(coll=[1,2,3],param='test')").Result;

            response.EnsureSuccessStatusCode();
        }

        private static IEdmModel GetEdmModel()
        {
            var builder = new ODataConventionModelBuilder();
            builder.EntitySet<Customer>("Customers");

            var func = builder.EntityType<Customer>().Collection
                                       .Function("TestFunc")
                                       .Returns<IEnumerable<string>>();
            func.CollectionParameter<int>("coll");
            func.Parameter<string>("param");

            return builder.GetEdmModel();
        }
    }

    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class CustomersController : ODataController
    {
        [HttpGet]
        public IHttpActionResult TestFunc(
            // [FromODataUri] long customerId,
            // [FromODataUri] long accountId,
            [FromODataUri] IEnumerable<int> coll,
            [FromODataUri] string param,
            ODataQueryOptions<Customer> options)
        {
            Assert.True(coll != null);
            Assert.Equal(new [] { 1, 2, 3 }, coll);
            Assert.Equal("test", param);

            return Ok();
        }
    }
}

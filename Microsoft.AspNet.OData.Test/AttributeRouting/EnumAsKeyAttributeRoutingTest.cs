using System;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Web.Http;
using System.Web.OData;
using System.Web.OData.Builder;
using System.Web.OData.Extensions;
using System.Web.OData.Routing;
using Microsoft.OData.Edm;
using Xunit;
using Xunit.Extensions;

namespace Microsoft.AspNet.OData.Test.AttributeRouting
{
    public class EnumAsKeyAttributeRoutingTest
    {
        private readonly HttpConfiguration _configuration;
        private HttpClient _client;

        public EnumAsKeyAttributeRoutingTest()
        {
            _configuration = new[] { typeof(MetadataController), typeof(AnyController) }.GetHttpConfiguration();
            _configuration.MapODataServiceRoute("odata", "odata", GetEdmModel());
            HttpServer server = new HttpServer(_configuration);
            _client = new HttpClient(server);
        }

        private static IEdmModel GetEdmModel()
        {
            var builder = new ODataConventionModelBuilder();
            builder.EntitySet<Customer>("Customers");
            builder.EntitySet<Order>("Orders");
            builder.Namespace = "NS";
            return builder.GetEdmModel();
        }

        [Theory]
        [InlineData("Customers(NS.Color.'Red')")]
        [InlineData("Orders(NS.Color.Black)")]
        public void EnumAsKeyInUrlTemplateWorks(string odataPath)
        {
            // Update this test case: https://github.com/OData/WebApi/issues/722
            Assert.True(false);

            string requestUri = "http://localhost/odata/" + odataPath;

            HttpResponseMessage response = _client.GetAsync(requestUri).Result;

            response.EnsureSuccessStatusCode();

            Console.WriteLine(response.Content.ReadAsStringAsync().Result);
        }
    }

    public class AnyController : ODataController
    {
        [HttpGet]
        [ODataRoute("Customers({myKey})")]
        public IHttpActionResult CustomerAnyFunction([FromODataUri] Color myKey)
        {
            return Ok(new Customer{ ColorId = myKey});
        }

        [HttpGet]
        [ODataRoute("Orders({myKey})")]
        public IHttpActionResult OrderAnyFunction(Color myKey) // without [FromODataUri]
        {
            return Ok(new Order { ColorId = myKey });
        }
    }

    public class Customer
    {
        [Key]
        public Color ColorId { get; set; }
    }

    public class Order
    {
        [Key]
        public Color ColorId { get; set; }
    }

    public enum Color
    {
        Red,
        Green,
        Blue,
        Yellow,
        Black,
        White
    }
}

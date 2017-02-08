using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.OData;
using System.Web.OData.Builder;
using System.Web.OData.Extensions;
using Microsoft.AspNet.OData.Test;
using Microsoft.OData.Edm;
using ModelLibrary;
using Xunit;
using Xunit.Abstractions;

namespace WebApi6xFeaturesTest.NavigationPropertyOnComplexType
{
    public class NavigationPropertyOnComplexTypeTest
    {
        private readonly ITestOutputHelper _output;

        public NavigationPropertyOnComplexTypeTest(ITestOutputHelper output)
        {
            this._output = output;
        }

        [Fact]
        public void HasMany_NavigationPropertyTest()
        {
            ODataModelBuilder builder = new ODataModelBuilder();
            var customer = builder.EntityType<Customer>().HasKey(c => c.Id);
            var city = builder.EntityType<City>().HasKey(c => c.Id);
            var address = builder.ComplexType<Address>();
            address.HasMany(a => a.Cities);

            HttpClient client = GetClient(builder.GetEdmModel());

            string requestUri = "http://localhost/odata/$metadata";
            string result = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" + 
                "<edmx:Edmx Version=\"4.0\" xmlns:edmx=\"http://docs.oasis-open.org/odata/ns/edmx\">" +
                  "<edmx:DataServices><Schema Namespace=\"ModelLibrary\" xmlns=\"http://docs.oasis-open.org/odata/ns/edm\">" +
                    "<EntityType Name=\"Customer\">" +
                      "<Key>" +
                        "<PropertyRef Name=\"Id\" />" +
                      "</Key>"+
                      "<Property Name=\"Id\" Type=\"Edm.Int32\" Nullable=\"false\" />" +
                    "</EntityType>" +
                    "<EntityType Name=\"City\">" +
                      "<Key>" +
                        "<PropertyRef Name=\"Id\" />" +
                      "</Key>" +
                      "<Property Name=\"Id\" Type=\"Edm.Int32\" Nullable=\"false\" />" +
                    "</EntityType>" +
                    "<ComplexType Name=\"Address\">" +
                      "<NavigationProperty Name=\"Cities\" Type=\"Collection(ModelLibrary.City)\" />" +
                    "</ComplexType>" +
                  "</Schema>" +
                  "<Schema Namespace=\"Default\" xmlns=\"http://docs.oasis-open.org/odata/ns/edm\">" +
                    "<EntityContainer Name=\"Container\" />" +
                  "</Schema>" +
                "</edmx:DataServices>" +
              "</edmx:Edmx>";

            HttpRequestMessage request = new HttpRequestMessage(new HttpMethod("Get"), requestUri);
            HttpResponseMessage response = client.SendAsync(request).Result;

            response.EnsureSuccessStatusCode();
            _output.WriteLine(response.Content.ReadAsStringAsync().Result);
            Assert.Equal(result, response.Content.ReadAsStringAsync().Result);
        }

        [Fact]
        public void HasAll_NavigationPropertyTest()
        {
            ODataModelBuilder builder = new ODataModelBuilder();
            builder.EntityType<Customer>().HasKey(c => c.Id);
            builder.EntityType<City>().HasKey(c => c.Id);
            var address = builder.ComplexType<Address>();
            address.HasRequired(a => a.CityInfo);
            address.HasMany(a => a.Cities);

            HttpClient client = GetClient(builder.GetEdmModel());

            string requestUri = "http://localhost/odata/$metadata";
            string result = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                "<edmx:Edmx Version=\"4.0\" xmlns:edmx=\"http://docs.oasis-open.org/odata/ns/edmx\">" +
                  "<edmx:DataServices><Schema Namespace=\"ModelLibrary\" xmlns=\"http://docs.oasis-open.org/odata/ns/edm\">" +
                    "<EntityType Name=\"Customer\">" +
                      "<Key>" +
                        "<PropertyRef Name=\"Id\" />" +
                      "</Key>" +
                      "<Property Name=\"Id\" Type=\"Edm.Int32\" Nullable=\"false\" />" +
                    "</EntityType>" +
                    "<EntityType Name=\"City\">" +
                      "<Key>" +
                        "<PropertyRef Name=\"Id\" />" +
                      "</Key>" +
                      "<Property Name=\"Id\" Type=\"Edm.Int32\" Nullable=\"false\" />" +
                    "</EntityType>" +
                    "<ComplexType Name=\"Address\">" +
                      "<NavigationProperty Name=\"CityInfo\" Type=\"ModelLibrary.City\" Nullable=\"false\" />" +
                      "<NavigationProperty Name=\"Cities\" Type=\"Collection(ModelLibrary.City)\" />" +
                    "</ComplexType>" +
                  "</Schema>" +
                  "<Schema Namespace=\"Default\" xmlns=\"http://docs.oasis-open.org/odata/ns/edm\">" +
                    "<EntityContainer Name=\"Container\" />" +
                  "</Schema>" +
                "</edmx:DataServices>" +
              "</edmx:Edmx>";

            HttpRequestMessage request = new HttpRequestMessage(new HttpMethod("Get"), requestUri);
            HttpResponseMessage response = client.SendAsync(request).Result;

            response.EnsureSuccessStatusCode();
            _output.WriteLine(response.Content.ReadAsStringAsync().Result);
            Assert.Equal(result, response.Content.ReadAsStringAsync().Result);
        }

        [Fact]
        public void HasAll_NavigationPropertyTest_ConventionModelBuilder()
        {
            ODataModelBuilder builder = new ODataConventionModelBuilder();
            builder.ComplexType<Address>();
            //  builder.EntityType<City>();
            HttpClient client = GetClient(builder.GetEdmModel());

            string requestUri = "http://localhost/odata/$metadata";
            string result = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                "<edmx:Edmx Version=\"4.0\" xmlns:edmx=\"http://docs.oasis-open.org/odata/ns/edmx\">" +
                  "<edmx:DataServices><Schema Namespace=\"ModelLibrary\" xmlns=\"http://docs.oasis-open.org/odata/ns/edm\">" +
                    "<ComplexType Name=\"Address\">" +
                      "<Property Name=\"Street\" Type=\"Edm.String\" />" +
                      "<NavigationProperty Name=\"CityInfo\" Type=\"ModelLibrary.City\" />" +
                      "<NavigationProperty Name=\"Cities\" Type=\"Collection(ModelLibrary.City)\" />" +
                    "</ComplexType>" +
                    "<EntityType Name=\"City\">" +
                      "<Key>" +
                        "<PropertyRef Name=\"Id\" />" +
                      "</Key>" +
                      "<Property Name=\"Id\" Type=\"Edm.Int32\" Nullable=\"false\" />" +
                    "</EntityType>" +
                  "</Schema>" +
                  "<Schema Namespace=\"Default\" xmlns=\"http://docs.oasis-open.org/odata/ns/edm\">" +
                    "<EntityContainer Name=\"Container\" />" +
                  "</Schema>" +
                "</edmx:DataServices>" +
              "</edmx:Edmx>";

            HttpRequestMessage request = new HttpRequestMessage(new HttpMethod("Get"), requestUri);
            HttpResponseMessage response = client.SendAsync(request).Result;

            response.EnsureSuccessStatusCode();
            _output.WriteLine(response.Content.ReadAsStringAsync().Result);
            Assert.Equal(result, response.Content.ReadAsStringAsync().Result);
        }

        private static HttpClient GetClient(IEdmModel model)
        {
            var config = new[] { typeof(MetadataController), }.GetHttpConfiguration();
            config.MapODataServiceRoute("odata", "odata", model);
            HttpServer server = new HttpServer(config);
            return new HttpClient(server);
        }
    }
}

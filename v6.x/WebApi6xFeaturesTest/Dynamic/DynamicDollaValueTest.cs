using Microsoft.AspNet.OData.Test;
using Microsoft.OData;
using Microsoft.OData.Edm;
using ModelLibrary;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Web.Http;
using System.Web.OData;
using System.Web.OData.Builder;
using System.Web.OData.Extensions;
using System.Web.OData.Formatter;
using System.Web.OData.Routing;
using System.Web.OData.Routing.Conventions;
using System.Xml;
using Xunit;
using Xunit.Abstractions;

namespace WebApi6xFeaturesTest.Dynamic
{
    public class DynamicDollaValueTest
    {
        private readonly ITestOutputHelper _output;

        public DynamicDollaValueTest(ITestOutputHelper output)
        {
            this._output = output;
        }

        [Fact]
        public void MetadataWorks()
        {
            HttpClient client = GetClient(GetEdmModel());

            string requestUri = "http://localhost/odata/$metadata";
            string result = @"<?xml version=""1.0"" encoding=""utf-8""?>
<edmx:Edmx Version=""4.0"" xmlns:edmx=""http://docs.oasis-open.org/odata/ns/edmx"">
  <edmx:DataServices>
    <Schema Namespace=""ModelLibrary"" xmlns=""http://docs.oasis-open.org/odata/ns/edm"">
      <EntityType Name=""Person"" OpenType=""true"">
        <Key>
          <PropertyRef Name=""Id"" />
        </Key>
        <Property Name=""Id"" Type=""Edm.Int32"" Nullable=""false"" />
      </EntityType>
    </Schema>
    <Schema Namespace=""Default"" xmlns=""http://docs.oasis-open.org/odata/ns/edm"">
      <EntityContainer Name=""Container"">
        <EntitySet Name=""People"" EntityType=""ModelLibrary.Person"" />
      </EntityContainer>
    </Schema>
  </edmx:DataServices>
</edmx:Edmx>";

            HttpRequestMessage request = new HttpRequestMessage(new HttpMethod("Get"), requestUri);
            HttpResponseMessage response = client.SendAsync(request).Result;

            response.EnsureSuccessStatusCode();
            _output.WriteLine(FormatResponseBody(response));
            Assert.Equal(result, FormatResponseBody(response));
        }

        [Fact]
        public void QueryEntity()
        {
            HttpClient client = GetClient(GetEdmModel());

            string requestUri = "http://localhost/odata/People(1)";

            HttpRequestMessage request = new HttpRequestMessage(new HttpMethod("Get"), requestUri);
            HttpResponseMessage response = client.SendAsync(request).Result;

            string result =
                @"{""@odata.context"":""http://localhost/odata/$metadata#People/$entity"",""Id"":0,""foo"":""foo_value"",""bar"":""bar_value""}";

            response.EnsureSuccessStatusCode();
            _output.WriteLine(response.Content.ReadAsStringAsync().Result);
            Assert.Equal(result, response.Content.ReadAsStringAsync().Result);
        }

        [Fact]
        public void QueryDynamicProperties()
        {
            HttpClient client = GetClient(GetEdmModel());

            string requestUri = "http://localhost/odata/People(1)/foo";

            HttpRequestMessage request = new HttpRequestMessage(new HttpMethod("Get"), requestUri);
            HttpResponseMessage response = client.SendAsync(request).Result;

            response.EnsureSuccessStatusCode();
            _output.WriteLine(response.Content.ReadAsStringAsync().Result);
            Assert.Equal(@"{""@odata.context"":""http://localhost/odata/$metadata#People(1)/foo"",""value"":""foo_value""}", response.Content.ReadAsStringAsync().Result);
        }

        [Fact]
        public void QueryDynamicPropertiesDollarValue()
        {
            HttpClient client = GetClient(GetEdmModel());

            string requestUri = "http://localhost/odata/People(1)/foo/$value";

            HttpRequestMessage request = new HttpRequestMessage(new HttpMethod("Get"), requestUri);
            HttpResponseMessage response = client.SendAsync(request).Result;

            response.EnsureSuccessStatusCode();
            _output.WriteLine(response.Content.ReadAsStringAsync().Result);
            Assert.Equal("foo_value", response.Content.ReadAsStringAsync().Result);
        }

        private string FormatResponseBody(HttpResponseMessage response)
        {
            Console.WriteLine();
            string mediaType = response.Content.Headers.ContentType.MediaType;

            if (mediaType.Contains("xml"))
            {

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(response.Content.ReadAsStringAsync().Result);

                StringWriter sw = new StringWriter();
                using (XmlTextWriter writer = new XmlTextWriter(sw))
                {
                    writer.Indentation = 2;  // the Indentation
                    writer.Formatting = Formatting.Indented;
                    doc.WriteContentTo(writer);
                    writer.Close();
                }

                return sw.ToString();
            }
            else
            {
                string value = response.Content.ReadAsStringAsync().Result;
                JObject jobject = JObject.Parse(value);
                return jobject.ToString();
            }
        }

        private static HttpClient GetClient(IEdmModel model)
        {
            var config = new[] { typeof(MetadataController), typeof(PeopleController) }.GetHttpConfiguration();

            var routings = ODataRoutingConventions.CreateDefault();
            routings.Insert(0, new DynamicDollarValueRoutingConvention());

            config.MapODataServiceRoute("odata", "odata", builder =>
                builder.AddService(ServiceLifetime.Singleton, sp => model)
                       .AddService<IEnumerable<IODataRoutingConvention>>(ServiceLifetime.Singleton, sp => routings));

            var formatters = ODataMediaTypeFormatters.Create();
            foreach (var oDataMediaTypeFormatter in formatters)
            {
                oDataMediaTypeFormatter.MediaTypeMappings.Insert(0, new ODataDynamicValueMediaTypeMapping());
            }
            config.Formatters.InsertRange(0, formatters);

            HttpServer server = new HttpServer(config);
            return new HttpClient(server);
        }

        private static IEdmModel GetEdmModel()
        {
            ODataModelBuilder builder = new ODataConventionModelBuilder();
            builder.EntitySet<Person>("People");
            return builder.GetEdmModel();
        }
    }

    public class PeopleController : ODataController
    {
        [EnableQuery]
        [HttpGet]
        public IHttpActionResult Get()
        {
            Person person1 = new Person
            {
                Id = 1,
                DynamicProperties = new Dictionary<string, object>()
            };

            person1.DynamicProperties.Add("foo", "foo1");
            person1.DynamicProperties.Add("bar", "bar1");

            Person person2 = new Person
            {
                Id = 2,
                DynamicProperties = new Dictionary<string, object>()
            };

            person2.DynamicProperties.Add("foo", "foo2");
            person2.DynamicProperties.Add("bar", "bar2");

            IList<Person> persons = new List<Person> { person1, person2 };

            return this.Ok(persons);
        }

        [EnableQuery]
        [HttpGet]
        public IHttpActionResult Get([FromODataUri]int key)
        {
            Person person = new Person
            {
                DynamicProperties = new Dictionary<string, object>()
            };

            person.DynamicProperties.Add("foo", "foo_value");
            person.DynamicProperties.Add("bar", "bar_value");
            return this.Ok(person);
        }

        [HttpGet]
        public IHttpActionResult GetDynamicProperty([FromODataUri]int key, [FromODataUri]string dynamicProperty)
        {
            Person person = new Person
            {
                DynamicProperties = new Dictionary<string, object>()
            };

            person.DynamicProperties.Add("foo", "foo_value");
            person.DynamicProperties.Add("bar", "bar_value");

            // (string) for the example
            return this.Ok((string)person.DynamicProperties[dynamicProperty]);
        }
    }
}

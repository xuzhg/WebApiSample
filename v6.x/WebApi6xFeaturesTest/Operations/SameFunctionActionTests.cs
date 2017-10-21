using Microsoft.AspNet.OData.Test;
using Microsoft.OData;
using Microsoft.OData.Edm;
using Microsoft.OData.Edm.Validation;
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
    public class SameFunctionActionTests
    {
        private readonly ITestOutputHelper _output;

        public SameFunctionActionTests(ITestOutputHelper output)
        {
            this._output = output;
        }

        [Fact]
        public void MetadataWorks()
        {
            HttpClient client = GetClient(GetEdmModel());

            string requestUri = "http://localhost/odata/$metadata";
            HttpRequestMessage request = new HttpRequestMessage(new HttpMethod("Get"), requestUri);
            HttpResponseMessage response = client.SendAsync(request).Result;

            response.EnsureSuccessStatusCode();
            string payload = FormatResponseBody(response);
            _output.WriteLine(payload);
            Assert.Contains(@"<Action Name=""Photo"" IsBound=""true"">", payload);
            Assert.Contains(@"<Function Name=""Photo"" IsBound=""true"">", payload);
        }

        [Fact]
        public void QueryFunction()
        {
            HttpClient client = GetClient(GetEdmModel());

            string requestUri = "http://localhost/odata/Customers2(1)/Default.Photo(hexEncodedPhoto='abc')";

            HttpRequestMessage request = new HttpRequestMessage(new HttpMethod("Get"), requestUri);
            HttpResponseMessage response = client.SendAsync(request).Result;

            string result =@"{}";

            response.EnsureSuccessStatusCode();
            _output.WriteLine(response.Content.ReadAsStringAsync().Result);
            Assert.Equal(result, response.Content.ReadAsStringAsync().Result);
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
            var config = new[] { typeof(MetadataController), typeof(Customers2Controller) }.GetHttpConfiguration();
            config.MapODataServiceRoute("odata", "odata", model);
            HttpServer server = new HttpServer(config);
            return new HttpClient(server);
        }

        private IEdmModel GetEdmModel()
        {
            ODataModelBuilder builder = new ODataConventionModelBuilder();
            builder.EntitySet<Customer>("Customers2");

            var customerType = builder.EntityType<Customer>();

            var function = customerType.Function("Photo");
            function.Returns<byte[]>();
            function.Parameter<string>("hexEncodedPhoto");

            var action = customerType.Action("Photo");
            action.Parameter<string>("hexEncodedPhoto");

            IEdmModel model = builder.GetEdmModel();

            IEnumerable<EdmError> errors;
            model.Validate(out errors);

            if (errors != null)
            {
                foreach(var err in errors)
                {
                    _output.WriteLine(err.ErrorMessage);
                }
            }
            return model;
        }
    }

    public class Customers2Controller : ODataController
    {
        [HttpGet]
        public IHttpActionResult Photo([FromODataUri] int key, [FromODataUri]string hexEncodedPhoto)
        {
            return Ok(key + hexEncodedPhoto);
        }
        /*
        [HttpPost]
        [ODataRoute("Customers2({key})/Default.Photo")]
        public IHttpActionResult MyPhoto([FromODataUri] int key, [FromODataUri]string hexEncodedPhoto)
        {
            return Ok();
        }*/
    }
}

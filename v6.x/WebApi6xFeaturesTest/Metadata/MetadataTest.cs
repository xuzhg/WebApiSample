using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.OData;
using System.Web.OData.Builder;
using System.Web.OData.Extensions;
using Microsoft.AspNet.OData.Test;
using Microsoft.OData.Edm;
using ModelLibrary;
using Xunit;
using Xunit.Abstractions;
using System.Net.Http.Headers;
using System.Web.OData.Formatter.Serialization;
using Microsoft.OData;
using System.Web.OData.Routing.Conventions;
using System.IO;
// using Microsoft.Extensions.DependencyInjection;
using System.Web.OData.Routing;

namespace WebApi6xFeaturesTest.QueryComplexTypeTest
{
    public class MetadataTest
    {
        private readonly ITestOutputHelper _output;

        public MetadataTest(ITestOutputHelper output)
        {
            this._output = output;
        }

        [Fact]
        public void MetadataDocumentTestNormal()
        {
            ODataModelBuilder builder = new ODataConventionModelBuilder();
            builder.EntitySet<Customer>("Customers");
            HttpClient client = GetClient(builder.GetEdmModel(), false);

            string requestUri = "http://localhost/odata/$metadata";

            HttpRequestMessage request = new HttpRequestMessage(new HttpMethod("Get"), requestUri);
            HttpResponseMessage response = client.SendAsync(request).Result;

            response.EnsureSuccessStatusCode();
            string result = response.Content.ReadAsStringAsync().Result;
            _output.WriteLine(result);
            Assert.Contains("<EntitySet Name=\"Customers\"", result);
            Assert.DoesNotContain("<EntitySet Name=\"People\"", result);
        }

        [Fact]
        public void MetadataDocumentTestCustomized()
        {
            ODataModelBuilder builder = new ODataConventionModelBuilder();
            builder.EntitySet<Customer>("Customers");
            HttpClient client = GetClient(builder.GetEdmModel(), true);

            string requestUri = "http://localhost/odata/$metadata";

            HttpRequestMessage request = new HttpRequestMessage(new HttpMethod("Get"), requestUri);
            HttpResponseMessage response = client.SendAsync(request).Result;

            response.EnsureSuccessStatusCode();
            string result = response.Content.ReadAsStringAsync().Result;
            _output.WriteLine(result);
            Assert.DoesNotContain("<EntitySet Name=\"Customers\"", result);
            Assert.Contains("<EntitySet Name=\"People\"", result);
        }

        private static HttpClient GetClient(IEdmModel model, bool customize)
        {
            var config = new[] { typeof(MetadataController) }.GetHttpConfiguration();
            config.Count().Filter().OrderBy().Expand().Select().MaxTop(null);

            if (!customize)
            {
                config.MapODataServiceRoute("odata", "odata", model);
            }
            else
            {
                config.MapODataServiceRoute("odata", "odata", builder =>
                builder.AddService(ServiceLifetime.Singleton, sp => model).
                AddService<ODataSerializerProvider, MySerializerProivder>(ServiceLifetime.Singleton)
                .AddService<IEnumerable<IODataRoutingConvention>>(ServiceLifetime.Singleton, sp =>
               ODataRoutingConventions.CreateDefaultWithAttributeRouting("odata", config)));
            }

            HttpServer server = new HttpServer(config);
            return new HttpClient(server);
        }
    }

    public class MySerializerProivder : DefaultODataSerializerProvider
    {
        private const string RequestContainerKey = "System.Web.OData.RequestContainer";

        public MySerializerProivder(IServiceProvider rootContainer) : base(rootContainer)
        {
        }

        public override ODataSerializer GetODataPayloadSerializer(Type type, HttpRequestMessage request)
        {
            if (typeof(IEdmModel).IsAssignableFrom(type))
            {
                IServiceProvider oldProvider = request.Properties[RequestContainerKey] as IServiceProvider;

                IContainerBuilder builder = new DefaultContainerBuilder();
                builder.AddDefaultODataServices();
                builder.AddService<IEdmModel>(ServiceLifetime.Singleton, sp => CreateEdmModel());

                builder.AddService<IODataPathHandler, DefaultODataPathHandler>(ServiceLifetime.Singleton);

                builder.AddServicePrototype(new ODataMessageWriterSettings
                {
                    EnableMessageStreamDisposal = false,
                    MessageQuotas = new ODataMessageQuotas { MaxReceivedMessageSize = Int64.MaxValue },
                });

                IServiceProvider serviceProvider = builder.BuildContainer();

                request.Properties[RequestContainerKey] = serviceProvider;

                return new MyMetadataSerializer(oldProvider);
            }

            return base.GetODataPayloadSerializer(type, request);
        }

        private static IEdmModel CreateEdmModel()
        {
            EdmModel model = new EdmModel();

            var enumType = new EdmEnumType("NS", "Color");
            var blue = enumType.AddMember("Blue", new EdmEnumMemberValue(0));
            enumType.AddMember("White", new EdmEnumMemberValue(1));
            model.AddElement(enumType);

            var person = new EdmEntityType("NS", "Person");
            var entityId = person.AddStructuralProperty("ID", EdmCoreModel.Instance.GetString(false));
            person.AddKeys(entityId);
            model.AddElement(person);

            var entityContainer = new EdmEntityContainer("Default", "Container");
            model.AddElement(entityContainer);
            var people = new EdmEntitySet(entityContainer, "People", person);
            entityContainer.AddElement(people);
            return model;
        }
    }

    public class MyMetadataSerializer : ODataMetadataSerializer
    {
        private const string RequestContainerKey = "System.Web.OData.RequestContainer";

        private IServiceProvider _oldProvider;

        public MyMetadataSerializer(IServiceProvider serviceProvider)
        {
            _oldProvider = serviceProvider;
        }

        public override void WriteObject(object graph, Type type, ODataMessageWriter messageWriter, ODataSerializerContext writeContext)
        {
            base.WriteObject(graph, type, messageWriter, writeContext);
            writeContext.Request.Properties[RequestContainerKey] = _oldProvider;
        }
    }
}

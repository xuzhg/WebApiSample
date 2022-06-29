using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.OData.Edm;
using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNet.OData.Routing;
using System.ComponentModel.Design;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNet.OData.Formatter.Serialization;
using Microsoft.AspNet.OData.Query.Validators;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.OData.UriParser;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.OData;
using Microsoft.AspNet.OData.Formatter.Deserialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using System.Diagnostics.CodeAnalysis;
using Microsoft.OData.Json;
using Microsoft.AspNet.OData.Formatter;
using System.Net.Http.Headers;
using Microsoft.AspNet.OData.Query;
using Microsoft.AspNet.OData.Query.Expressions;

namespace AzureFuncDemo
{
    public class ODataUriUtils
    {
        public static IServiceProvider BuildODataServiceProvider(IEdmModel model)
        {
            var services = new ServiceCollection();
            services.AddMvcCore(action => action.EnableEndpointRouting = false);
            services.AddOData();

            services.AddSingleton<IODataPathHandler, DefaultODataPathHandler>();

            // ReaderSettings and WriterSettings are registered as prototype services.
            // There will be a copy (if it is accessed) of each prototype for each request.
            services.AddTransient(sp => new ODataMessageReaderSettings
            {
                EnableMessageStreamDisposal = false,
                MessageQuotas = new ODataMessageQuotas { MaxReceivedMessageSize = Int64.MaxValue },
            });
            services.AddTransient(sp => new ODataMessageWriterSettings
            {
                EnableMessageStreamDisposal = false,
                MessageQuotas = new ODataMessageQuotas { MaxReceivedMessageSize = Int64.MaxValue },
            });

            services.AddTransient<IETagHandler, MyETagHandler>();

            services.AddTransient<ODataQuerySettings>();
            //ServiceDescriptor serviceDescriptor = new ServiceDescriptor(typeof(IPerRouteContainer), service =>
            //{
            //    IPerRouteContainer routeContainer = new PerRouteContainer()
            //    {
            //        BuilderFactory = () =>
            //        {
            //            return new DataContainerBuilder();
            //        }
            //    };
            //    return routeContainer;
            //}, ServiceLifetime.Scoped);
            //services.Replace(serviceDescriptor);
            services.AddSingleton<IEdmModel>(sp => model);
            services.AddTransient<ODataUriResolver>(sp => new ODataUriResolver {  EnableCaseInsensitive = true });
            services.AddTransient<ODataQueryValidator>();
            services.AddTransient<TopQueryValidator>();
            services.AddTransient<FilterQueryValidator>();
            services.AddTransient<SkipQueryValidator>();
            services.AddTransient<OrderByQueryValidator>();
            services.AddTransient<CountQueryValidator>();
            services.AddTransient<SelectExpandQueryValidator>();
            services.AddTransient<ODataRawValueSerializer>();
            services.AddTransient<ODataMetadataSerializer>();
            services.AddTransient<ODataErrorSerializer>();
            services.AddTransient<ODataResourceSetSerializer>();
            services.AddTransient<ODataEntityReferenceLinksSerializer>();
            services.AddTransient<ODataEntityReferenceLinkSerializer>();
            services.AddTransient<ODataServiceDocumentSerializer>();

            services.AddSingleton<SkipTokenHandler, DefaultSkipTokenHandler>();
            services.AddTransient<FilterBinder>();

            // SerializerProvider and DeserializerProvider.
            services.AddSingleton<ODataSerializerProvider, DefaultODataSerializerProvider>();
            services.AddSingleton<ODataDeserializerProvider, DefaultODataDeserializerProvider>();

            // Deserializers.
            services.AddSingleton<ODataResourceDeserializer>();
            services.AddSingleton<ODataEnumDeserializer>();
            services.AddSingleton<ODataPrimitiveDeserializer>();
            services.AddSingleton<ODataResourceSetDeserializer>();
            services.AddSingleton<ODataCollectionDeserializer>();
            services.AddSingleton<ODataEntityReferenceLinkDeserializer>();
            services.AddSingleton<ODataActionPayloadDeserializer>();

            // Serializers.
            services.AddSingleton<ODataEnumSerializer>();
            services.AddSingleton<ODataPrimitiveSerializer>();
            services.AddSingleton<ODataResourceValueSerializer>();
            services.AddSingleton<ODataDeltaFeedSerializer>();
            services.AddSingleton<ODataResourceSetSerializer>();
            services.AddSingleton<ODataCollectionSerializer>();
            services.AddSingleton<ODataResourceSerializer>();
            services.AddSingleton<ODataServiceDocumentSerializer>();
            services.AddSingleton<ODataEntityReferenceLinkSerializer>();
            services.AddSingleton<ODataEntityReferenceLinksSerializer>();
            services.AddSingleton<ODataErrorSerializer>();
            services.AddSingleton<ODataMetadataSerializer>();
            services.AddSingleton<ODataRawValueSerializer>();

            services.AddTransient<ODataUriParserSettings>();
            services.AddScoped<IActionContextAccessor, ActionContextAccessor>();
            services.AddTransient(typeof(ODataSimplifiedOptions), sp => new ODataSimplifiedOptions(ODataVersion.V4));
            services.AddTransient<UriPathParser>();

       //     services.AddSingleton<IJsonReaderFactory, DefaultJsonReaderFactory>();
            services.AddSingleton<IJsonWriterFactory>( sp => new DefaultJsonWriterFactory());
            services.AddSingleton<IJsonWriterFactoryAsync>( sp => new DefaultJsonWriterFactory());
            services.AddSingleton(sp => new ODataMediaTypeResolver());
            services.AddTransient<ODataMessageInfo>();
            services.AddSingleton(sp => new ODataPayloadValueConverter());

            return services.BuildServiceProvider();
        }

        public static void InitializeOData(HttpRequest request, IEdmModel model, string source)
        {
            var odataFeatures = request.ODataFeature();

            if (odataFeatures.RequestContainer == null)
            {
                odataFeatures.RequestContainer = BuildODataServiceProvider(model);
            }

            odataFeatures.Path = GetODataPath(request, source);

            odataFeatures.UrlHelper = new MyUriHelper($"{request.Scheme}://{request.Host}{request.PathBase}/api/v1/{source}/",
                new ActionContext { HttpContext = request.HttpContext });

            odataFeatures.RouteName = "any";
        }

        public static Microsoft.AspNet.OData.Routing.ODataPath GetODataPath(HttpRequest request, string source)
        {
            var provider = request.GetRequestContainer();

            var serviceroot = $"{request.Scheme}://{request.Host}{request.PathBase}/api/v1/{source}/";

            int length = $"/api/v1/{source}/".Length;
            var odataPath = request.Path.Value.Substring(length);

            DefaultODataPathHandler handler = new DefaultODataPathHandler();
            return handler.Parse(serviceroot, odataPath, provider);
        }
    }

    public class MyUriHelper : IUrlHelper
    {
        public ActionContext ActionContext { get; }

        private string baseUrl;
        public MyUriHelper(string baseUrl, ActionContext context)
        {
            this.baseUrl = baseUrl;
            ActionContext = context;
        }

        public string Action(UrlActionContext actionContext)
        {
            throw new NotImplementedException();
        }

        [return: NotNullIfNotNull("contentPath")]
        public string Content(string contentPath)
        {
            throw new NotImplementedException();
        }

        public bool IsLocalUrl([NotNullWhen(true)] string url)
        {
            throw new NotImplementedException();
        }

        public string Link(string routeName, object values)
        {
            return baseUrl;
        }

        public string RouteUrl(UrlRouteContext routeContext)
        {
            throw new NotImplementedException();
        }
    }

    public class MyETagHandler : IETagHandler
    {
        public EntityTagHeaderValue CreateETag(IDictionary<string, object> properties)
        {
            return null;
        }

        public IDictionary<string, object> ParseETag(EntityTagHeaderValue etagHeaderValue)
        {
            return null;
        }
    }
}

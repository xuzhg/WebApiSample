using CustomODataRouting.Models;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNet.OData.Formatter;
using Microsoft.AspNet.OData.Formatter.Deserialization;
using Microsoft.AspNet.OData.Routing;
using Microsoft.OData;
using Microsoft.OData.Edm;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;

namespace CustomODataRouting.Extensions
{
    internal static class MyODataMediaTypes
    {
        public static readonly string ApplicationJson = "application/json";
        public static readonly string ApplicationJsonODataFullMetadata = "application/json;odata.metadata=full";
        public static readonly string ApplicationJsonODataFullMetadataStreamingFalse = "application/json;odata.metadata=full;odata.streaming=false";
        public static readonly string ApplicationJsonODataFullMetadataStreamingTrue = "application/json;odata.metadata=full;odata.streaming=true";
        public static readonly string ApplicationJsonODataMinimalMetadata = "application/json;odata.metadata=minimal";
        public static readonly string ApplicationJsonODataMinimalMetadataStreamingFalse = "application/json;odata.metadata=minimal;odata.streaming=false";
        public static readonly string ApplicationJsonODataMinimalMetadataStreamingTrue = "application/json;odata.metadata=minimal;odata.streaming=true";
        public static readonly string ApplicationJsonODataNoMetadata = "application/json;odata.metadata=none";
        public static readonly string ApplicationJsonODataNoMetadataStreamingFalse = "application/json;odata.metadata=none;odata.streaming=false";
        public static readonly string ApplicationJsonODataNoMetadataStreamingTrue = "application/json;odata.metadata=none;odata.streaming=true";
        public static readonly string ApplicationJsonStreamingFalse = "application/json;odata.streaming=false";
        public static readonly string ApplicationJsonStreamingTrue = "application/json;odata.streaming=true";
        public static readonly string ApplicationXml = "application/xml";
    }
    public class MyMediaFormatter : MediaTypeFormatter
    {
        public MyMediaFormatter()
        {
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse(MyODataMediaTypes.ApplicationJsonODataMinimalMetadataStreamingTrue));
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse(MyODataMediaTypes.ApplicationJsonODataMinimalMetadataStreamingFalse));
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse(MyODataMediaTypes.ApplicationJsonODataMinimalMetadata));
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse(MyODataMediaTypes.ApplicationJsonODataFullMetadataStreamingTrue));
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse(MyODataMediaTypes.ApplicationJsonODataFullMetadataStreamingFalse));
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse(MyODataMediaTypes.ApplicationJsonODataFullMetadata));
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse(MyODataMediaTypes.ApplicationJsonODataNoMetadataStreamingTrue));
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse(MyODataMediaTypes.ApplicationJsonODataNoMetadataStreamingFalse));
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse(MyODataMediaTypes.ApplicationJsonODataNoMetadata));
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse(MyODataMediaTypes.ApplicationJsonStreamingTrue));
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse(MyODataMediaTypes.ApplicationJsonStreamingFalse));
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse(MyODataMediaTypes.ApplicationJson));
        }

        public override bool CanReadType(Type type)
        {
            if (!type.IsGenericType)
            {
                return false;
            }

            var arguments = type.GetGenericArguments();
            if (arguments.Count() != 1)
            {
                return false;
            }

            if (arguments[0] != typeof(Like))
            {
                return false;
            }

            return true;
        }

        public override bool CanWriteType(Type type)
        {
            return false;
        }

        public override MediaTypeFormatter GetPerRequestFormatterInstance(Type type, HttpRequestMessage request, MediaTypeHeaderValue mediaType)
        {
            // call base to validate parameters
            base.GetPerRequestFormatterInstance(type, request, mediaType);

            if (Request != null && Request == request)
            {
                // If the request is already set on this formatter, return itself.
                return this;
            }
            else
            {
                return new MyMediaFormatter(this, request);
            }
        }

        public HttpRequestMessage Request { get; set; }

        internal MyMediaFormatter(MyMediaFormatter formatter, HttpRequestMessage request)
            : base(formatter)
        {
            // Parameter 3: request
            Request = request;
        }

        public override Task<object> ReadFromStreamAsync(Type type, Stream readStream, HttpContent content, IFormatterLogger formatterLogger)
        {
            try
            {
                object value = ReadFromStream(type, readStream, content, formatterLogger);
                return Task.FromResult(value);
            }
            catch (Exception ex)
            {
                return FromError<object>(ex);
            }
        }

        public object ReadFromStream(Type type, Stream readStream, HttpContent content, IFormatterLogger formatterLogger)
        {
            ODataMessageReaderSettings oDataReaderSettings = Request.GetReaderSettings();
            ODataPath path = Request.ODataProperties().Path;
            IEdmModel edmModel = Request.GetModel();

            ODataDeserializerContext readContext = new ODataDeserializerContext
            {
                Path = path,
                Model = edmModel,
                Request = this.Request,
                ResourceType = type,
                // ResourceEdmType = edmTypeReference,
            };

            var edmType = edmModel.SchemaElements.OfType<IEdmEntityType>().FirstOrDefault(c => c.Name == "Like");
            var edmTypeReference = new EdmCollectionTypeReference(new EdmCollectionType(new EdmEntityTypeReference(edmType, false)));
            IODataRequestMessage oDataRequestMessage = new ODataMessageWrapper(readStream, null);
            using (
                ODataMessageReader oDataMessageReader = new ODataMessageReader(oDataRequestMessage,
                    oDataReaderSettings, readContext.Model))
            {
                return ConvertResourceSet(oDataMessageReader, edmTypeReference, readContext, this.Request);
            }
        }

        private static readonly MethodInfo CastMethodInfo = typeof(Enumerable).GetMethod("Cast");

        private static object ConvertResourceSet(ODataMessageReader oDataMessageReader,
            IEdmTypeReference edmTypeReference, ODataDeserializerContext readContext, HttpRequestMessage request)
        {
            IEdmCollectionTypeReference collectionType = edmTypeReference.AsCollection();

            EdmEntitySet tempEntitySet = null;
            if (collectionType.ElementType().IsEntity())
            {
                tempEntitySet = new EdmEntitySet(readContext.Model.EntityContainer, "temp",
                    collectionType.ElementType().AsEntity().EntityDefinition());
            }

            // TODO: Sam xu, can we use the parameter-less overload
            ODataReader odataReader = oDataMessageReader.CreateODataUriParameterResourceSetReader(tempEntitySet,
                collectionType.ElementType().AsStructured().StructuredDefinition());
            ODataResourceSetWrapper resourceSet =
                odataReader.ReadResourceOrResourceSet() as ODataResourceSetWrapper;

            ODataDeserializerProvider deserializerProvider = request.GetDeserializerProvider();

            ODataResourceSetDeserializer resourceSetDeserializer =
                (ODataResourceSetDeserializer)deserializerProvider.GetEdmTypeDeserializer(collectionType);

            object result = resourceSetDeserializer.ReadInline(resourceSet, collectionType, readContext);
            IEnumerable enumerable = result as IEnumerable;
            if (enumerable != null)
            {
                IEnumerable newEnumerable = enumerable;

                IEdmTypeReference elementTypeReference = collectionType.ElementType();

                Type elementClrType = typeof(Like);
                IEnumerable castedResult =
                        CastMethodInfo.MakeGenericMethod(elementClrType)
                            .Invoke(null, new object[] { newEnumerable }) as IEnumerable;
                return castedResult;
            }

            return null;
        }

        internal static Task<TResult> FromError<TResult>(Exception exception)
        {
            TaskCompletionSource<TResult> tcs = new TaskCompletionSource<TResult>();
            tcs.SetException(exception);
            return tcs.Task;
        }
    }
}
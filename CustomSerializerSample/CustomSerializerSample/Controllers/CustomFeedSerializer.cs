using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using System.Web.OData.Formatter.Serialization;
using Microsoft.OData.Core;
using Microsoft.OData.Edm;

namespace CustomSerializerSample.Controllers
{
    public class CustomFeedSerializer : ODataFeedSerializer
    {
        public CustomFeedSerializer(ODataSerializerProvider serializerProvider) : base(serializerProvider)
        {
        }

        public override void WriteObjectInline(object graph, IEdmTypeReference expectedType, ODataWriter writer,
            ODataSerializerContext writeContext)
        {
            IEnumerable enumerable = graph as IEnumerable; // Data to serialize
            if (enumerable == null)
            {
                throw new SerializationException("Can't serialize null feed");
            }

            WriteFeed(enumerable, expectedType, writer, writeContext);
        }

        private void WriteFeed(IEnumerable enumerable, IEdmTypeReference feedType, ODataWriter writer,
            ODataSerializerContext writeContext)
        {
            IEdmEntityTypeReference elementType = null;
            if (feedType.IsCollection())
            {
                IEdmTypeReference refer = feedType.AsCollection().ElementType();
                if (refer.IsEntity())
                {
                    elementType = refer.AsEntity();
                }
            }
            Debug.Assert(elementType != null);

            ODataFeed feed = CreateODataFeed(enumerable, feedType.AsCollection(), writeContext);
            Debug.Assert(feed != null);

            ODataEdmTypeSerializer entrySerializer = SerializerProvider.GetEdmTypeSerializer(elementType);
            Debug.Assert(entrySerializer is CustomEntrySerializer);

            // save this for later to support JSON odata.streaming.
            Uri nextPageLink = feed.NextPageLink;
            feed.NextPageLink = null;

            writer.WriteStart(feed);

            foreach (object entry in enumerable)
            {
                entrySerializer.WriteObjectInline(entry, elementType, writer, writeContext);
            }

            // Subtle and suprising behavior: If the NextPageLink property is set before calling WriteStart(feed),
            // the next page link will be written early in a manner not compatible with odata.streaming=true. Instead, if
            // the next page link is not set when calling WriteStart(feed) but is instead set later on that feed
            // object before calling WriteEnd(), the next page link will be written at the end, as required for
            // odata.streaming=true support.

            if (nextPageLink != null)
            {
                feed.NextPageLink = nextPageLink;
            }

            writer.WriteEnd();
        }
    }
}
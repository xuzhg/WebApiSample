using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.OData;
using System.Web.OData.Formatter.Serialization;
using System.Web.OData.Query;
using Microsoft.OData.Core;
using Microsoft.OData.Edm;

namespace CustomSerializerSample.Controllers
{
    internal class CustomEntrySerializer : ODataEntityTypeSerializer
    {
        public CustomEntrySerializer(ODataSerializerProvider serializerProvider)
            : base(serializerProvider)
        {
        }


        public override void WriteObjectInline(object graph, IEdmTypeReference expectedType, ODataWriter writer, ODataSerializerContext writeContext)
        {
            if (graph == null)
            {
                return;
            }

            ISelectExpandWrapper wrapper = graph as ISelectExpandWrapper;
            if (wrapper != null)
            {
                IDictionary<string, object> properties = wrapper.ToDictionary();
                if (properties == null || !properties.Any())
                {
                    return;
                }
            }

            base.WriteObjectInline(graph, expectedType, writer, writeContext);
        }
    }
}
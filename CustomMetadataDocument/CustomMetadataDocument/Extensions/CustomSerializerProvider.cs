using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.OData.Formatter.Serialization;
using Microsoft.OData.Edm;

namespace CustomMetadataDocument.Extensions
{
    public class CustomSerializerProvider : DefaultODataSerializerProvider
    {
        public override ODataSerializer GetODataPayloadSerializer(IEdmModel model, Type type, HttpRequestMessage request)
        {
            if (typeof (IEdmModel).IsAssignableFrom(type))
            {
                return new CustomMetadataSerializer();
            }

            return base.GetODataPayloadSerializer(model, type, request);
        }
    }
}

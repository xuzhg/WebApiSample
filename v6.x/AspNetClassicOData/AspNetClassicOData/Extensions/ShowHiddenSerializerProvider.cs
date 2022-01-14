using Microsoft.OData.Edm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.OData.Formatter.Serialization;

namespace AspNetClassicOData.Extensions
{
    public class ShowHiddenSerializerProvider : DefaultODataSerializerProvider
    {
        public ShowHiddenSerializerProvider(IServiceProvider sp) : base(sp)
        { }

        public override ODataEdmTypeSerializer GetEdmTypeSerializer(IEdmTypeReference edmType)
        {
            if (edmType.IsEntity() || edmType.IsComplex())
            {
                return new ShowHiddenResourceSerializer(this);
            }

            return base.GetEdmTypeSerializer(edmType);

        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.OData.Formatter.Serialization;
using Microsoft.OData.Edm;

namespace CustomSerializerSample.Controllers
{
    public class CustomSerializerProvider : DefaultODataSerializerProvider
    {
        public override ODataEdmTypeSerializer GetEdmTypeSerializer(IEdmTypeReference edmType)
        {
            if (edmType.IsCollection())
            {
                IEdmCollectionTypeReference collection = edmType as IEdmCollectionTypeReference;
                if (collection.ElementType().IsEntity())
                {
                    return new CustomFeedSerializer(this);
                }
            }

            if (edmType.IsEntity())
            {
                return new CustomEntrySerializer(this);
            }

            return base.GetEdmTypeSerializer(edmType);
        }
    }


}
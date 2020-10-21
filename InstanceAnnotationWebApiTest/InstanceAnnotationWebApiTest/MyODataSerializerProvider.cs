using Microsoft.AspNet.OData.Formatter.Serialization;
using Microsoft.OData.Edm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InstanceAnnotationWebApiTest
{
    public class MyODataSerializerProvider : DefaultODataSerializerProvider
    {
        private IServiceProvider _rootProvider;

        public MyODataSerializerProvider(IServiceProvider rootContainer)
            : base(rootContainer)
        {
            _rootProvider = rootContainer;
        }

        public override ODataEdmTypeSerializer GetEdmTypeSerializer(IEdmTypeReference edmType)
        {
            if (edmType.IsCollection())
            {
                IEdmCollectionTypeReference collectionType = edmType.AsCollection();
                if (collectionType.ElementType().IsEntity() || collectionType.ElementType().IsComplex())
                {
                    return new MyResourceSetSerializer(this);
                }
            }

            return base.GetEdmTypeSerializer(edmType);
        }
    }
}

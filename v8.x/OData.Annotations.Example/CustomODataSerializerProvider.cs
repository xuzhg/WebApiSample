using Microsoft.AspNetCore.OData.Formatter.Serialization;
using Microsoft.OData.Edm;

namespace OData.Annotations.Example
{
    public class CustomODataSerializerProvider : ODataSerializerProvider
    {
        private IServiceProvider _rootProvider;

        public CustomODataSerializerProvider(IServiceProvider rootContainer) : base(rootContainer)
        {
            _rootProvider = rootContainer;
        }

        public override IODataEdmTypeSerializer GetEdmTypeSerializer(IEdmTypeReference edmType)
        {
            if (edmType.Definition.TypeKind == EdmTypeKind.Entity)
            {
                return new CustomODataResourceSerializer(this);
            }

            return base.GetEdmTypeSerializer(edmType);
        }
    }
}

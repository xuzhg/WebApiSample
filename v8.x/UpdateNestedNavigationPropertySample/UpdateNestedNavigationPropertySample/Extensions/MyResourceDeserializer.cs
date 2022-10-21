using Microsoft.AspNetCore.OData.Deltas;
using Microsoft.AspNetCore.OData.Edm;
using Microsoft.AspNetCore.OData.Formatter.Deserialization;
using Microsoft.OData.Edm;
using System.Linq;
using System.Reflection;
using System.Security.AccessControl;
using UpdateNestedNavigationPropertySample.Models;

namespace UpdateNestedNavigationPropertySample.Extensions
{
    public class MyResourceDeserializer : ODataResourceDeserializer
    {
        public MyResourceDeserializer(IODataDeserializerProvider deserializerProvider) : base(deserializerProvider)
        {
        }

        public override object CreateResourceInstance(IEdmStructuredTypeReference structuredType, ODataDeserializerContext readContext)
        {
            if (IsDeltaOfT(readContext.ResourceType) && structuredType.FullName().Contains("EducationSettings"))
            {
                IEdmModel model = readContext.Model;
                Type clrType = model.GetTypeMapper().GetClrType(model, structuredType);

                IEnumerable<string> updateableProperties = structuredType
                    .StructuralProperties().OfType<IEdmProperty>().Concat(structuredType.NavigationProperties())
                       .Select(edmProperty => model.GetClrPropertyName(edmProperty));

                return Activator.CreateInstance(readContext.ResourceType, clrType, updateableProperties);
            }

            return base.CreateResourceInstance(structuredType, readContext);
        }

        private bool IsDeltaOfT(Type resourceType)
        {
            return resourceType != null && resourceType.IsGenericType &&
                       resourceType.GetGenericTypeDefinition() == typeof(Delta<>);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.OData.Formatter.Serialization;
using Microsoft.OData.Core;
using Microsoft.OData.Edm;

namespace WebApiComplexTypeSkipPropertyTest
{
    public class MyComplexTypeSerializer : ODataComplexTypeSerializer
    {
        public MyComplexTypeSerializer(ODataSerializerProvider serializerProvider) : base(serializerProvider)
        {
        }

        public override ODataComplexValue CreateODataComplexValue(object graph, IEdmComplexTypeReference complexType, ODataSerializerContext writeContext)
        {
            ODataComplexValue complexValue = base.CreateODataComplexValue(graph, complexType, writeContext);

            IEdmModel model = writeContext.Model;

            SkipPropertyAnnotation skipProperties = model.GetAnnotationValue<SkipPropertyAnnotation>(complexType.Definition);

            IList<ODataProperty> newProperties = new List<ODataProperty>();
            foreach (var property in complexValue.Properties)
            {
                if (skipProperties.Skips.Any(c => c == property.Name))
                {
                    continue;
                }

                newProperties.Add(property);
            }
            
            complexValue.Properties = newProperties;
            return complexValue;
        }
    }

    public class MySerializerProvider : DefaultODataSerializerProvider
    {
        public override ODataEdmTypeSerializer GetEdmTypeSerializer(IEdmTypeReference edmType)
        {
            if (edmType.TypeKind() == EdmTypeKind.Complex)
            {
                return new MyComplexTypeSerializer(this);
            }

            return base.GetEdmTypeSerializer(edmType);
        }
    }
}

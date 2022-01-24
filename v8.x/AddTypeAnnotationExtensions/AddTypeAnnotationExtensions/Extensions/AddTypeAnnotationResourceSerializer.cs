using Microsoft.AspNetCore.OData.Edm;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Formatter.Serialization;
using Microsoft.OData;
using Microsoft.OData.Edm;

namespace AddTypeAnnotationExtensions.Extensions
{
    public class AddTypeAnnotationResourceSerializer : ODataResourceSerializer
    {
        public AddTypeAnnotationResourceSerializer(IODataSerializerProvider p)
            : base(p)
        {  }

        public override ODataResource CreateResource(SelectExpandNode selectExpandNode, ResourceContext resourceContext)
        {
            ODataResource resource = base.CreateResource(selectExpandNode, resourceContext);

            if (resourceContext.SerializerContext.MetadataLevel != ODataMetadataLevel.Full)
            {
                foreach (var prop in resource.Properties)
                {
                    if (!IsDynamicProperty(prop, resourceContext.StructuredType))
                    {
                        continue;
                    }

                    if (prop.TypeAnnotation == null)
                    {
                        string typeName = GetTypeName(prop, resourceContext.EdmModel);
                        if (typeName != null)
                        {
                            prop.TypeAnnotation = new ODataTypeAnnotation(typeName);
                        }
                    }
                }
            }

            return resource;
        }

        private static bool IsDynamicProperty(ODataProperty property, IEdmStructuredType structuredType)
        {
            return structuredType.FindProperty(property.Name, caseInsensitive: true) == null;
        }

        private static string GetTypeName(ODataProperty property, IEdmModel model)
        {
            object value = property.Value;
            if (value == null)
            {
                return null;
            }

            var edmType = model.GetTypeMapper().GetEdmType(model, value.GetType());
            if (edmType == null)
            {
                throw new InvalidOperationException("The dynamic property type is not identified");
            }

            return edmType.FullTypeName();
        }
    }
}

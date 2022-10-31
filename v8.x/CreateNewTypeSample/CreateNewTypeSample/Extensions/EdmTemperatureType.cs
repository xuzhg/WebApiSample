using Microsoft.OData.Edm;

namespace CreateNewTypeSample.Extensions
{
    public class EdmTemperatureType : EdmType, IEdmPrimitiveType, IEdmFullNamedElement
    {
        public override EdmTypeKind TypeKind => EdmTypeKind.Primitive;

        public EdmSchemaElementKind SchemaElementKind => EdmSchemaElementKind.TypeDefinition;

        public string Name => "Temperature";

        public string Namespace => "Edm";

        public string FullName => "Edm.Temperature";

        // It's weird, it's better to add a "EdmPrimitiveTypeKind.Customized"
        public EdmPrimitiveTypeKind PrimitiveKind => EdmPrimitiveTypeKind.String;
    }
}

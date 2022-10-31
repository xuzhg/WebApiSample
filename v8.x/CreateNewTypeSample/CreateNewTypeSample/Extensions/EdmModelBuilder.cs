using CreateNewTypeSample.Models;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;

namespace CreateNewTypeSample.Extensions
{
    public class EdmModelBuilder
    {
        public static IEdmModel GetEdmModel()
        {
            var builder = new ODataModelBuilder();
            builder.EntityType<Person>().HasKey(p => p.Id);
            builder.EntitySet<Person>("People");
            EdmModel model = builder.GetEdmModel() as EdmModel;

            var personType = model.SchemaElements.OfType<IEdmEntityType>().First(c => c.Name == "Person") as EdmEntityType;

            // 1
            var tempTypeRef = new EdmPrimitiveTypeReference(new EdmTemperatureType(), true);
            var normalTempPro = personType.AddStructuralProperty("NormalTemp", tempTypeRef);

            var propertyInfo = typeof(Person).GetProperty("NormalTemp");
            model.SetAnnotationValue(normalTempPro, new ClrPropertyInfoAnnotation(propertyInfo));

            EdmTypeDefinition typeDef = new EdmTypeDefinition("MyNS", "Distance", EdmPrimitiveTypeKind.String);
            model.AddElement(typeDef);

            // 2
            var distProp = personType.AddStructuralProperty("Dist", new EdmTypeDefinitionReference(typeDef, true));
            propertyInfo = typeof(Person).GetProperty("Dist");
            model.SetAnnotationValue(distProp, new ClrPropertyInfoAnnotation(propertyInfo));
            return model;
        }
    }
}

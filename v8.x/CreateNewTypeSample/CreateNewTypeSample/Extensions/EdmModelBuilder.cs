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

            var tempTypeRef = new EdmPrimitiveTypeReference(new EdmTemperatureType(), true);
            var normalTempPro = personType.AddStructuralProperty("NormalTemp", tempTypeRef);

            var propertyInfo = typeof(Person).GetProperty("NormalTemp");
            model.SetAnnotationValue(normalTempPro, new ClrPropertyInfoAnnotation(propertyInfo));
            return model;
        }
    }
}

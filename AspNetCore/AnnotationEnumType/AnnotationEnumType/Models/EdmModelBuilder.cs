using Microsoft.AspNet.OData.Builder;
using Microsoft.OData.Edm.Csdl;
using Microsoft.OData.Edm;
using Microsoft.OData.Edm.Vocabularies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnnotationEnumType.Models
{
    public class EdmModelBuilder
    {
        private static IEdmModel _edmModel;

        public static IEdmModel GetEdmModel()
        {
            if (_edmModel != null)
            {
                return _edmModel;
            }


            var builder = new ODataConventionModelBuilder();
            builder.EntitySet<Customer>("Customers");
            builder.EnumType<Appliance>();
            builder.EnumType<Color>();
            builder.EnumType<Permission>();
            _edmModel = builder.GetEdmModel();

            AppendAnnotation(_edmModel);
            AppendAnnotationForPermission(_edmModel);
            return _edmModel;
        }

        private static void AppendAnnotation(IEdmModel model)
        {
            EdmTerm term = new EdmTerm("NS", "FooBar", EdmCoreModel.Instance.GetString(true));
            ((EdmModel)model).AddElement(term);

            IEdmEnumType enumType = model.SchemaElements.OfType<IEdmEnumType>().First(c => c.Name == "Appliance");

            var member = enumType.Members.First(c => c.Name == "Stove");

            EdmVocabularyAnnotation annotation = new EdmVocabularyAnnotation(member, term, new EdmStringConstant("Stove Top"));

            // Note: OutOfLine can't work for the "EnumMember" because the OutofLine needs the full type of name.
            annotation.SetSerializationLocation(model, EdmVocabularyAnnotationSerializationLocation.Inline);
            ((EdmModel)model).SetVocabularyAnnotation(annotation);
        }

        private static void AppendAnnotationForPermission(IEdmModel model)
        {
            IEdmEnumType enumType = model.SchemaElements.OfType<IEdmEnumType>().First(c => c.Name == "Permission");
            EdmTerm term = new EdmTerm("NS", "PermissionTerm", new EdmEnumTypeReference(enumType, true), appliesTo: "Property");
            ((EdmModel)model).AddElement(term);

            IEdmEntityType entityType = model.SchemaElements.OfType<IEdmEntityType>().First(c => c.Name == "Customer");
            IEdmProperty property = entityType.Properties().First(c => c.Name == "Name");

            EdmVocabularyAnnotation annotation = new EdmVocabularyAnnotation(property, term, new EdmEnumMemberExpression(enumType.Members.First(c => c.Name == "ReadOnly")));
            annotation.SetSerializationLocation(model, EdmVocabularyAnnotationSerializationLocation.Inline);
            ((EdmModel)model).SetVocabularyAnnotation(annotation);
        }
    }
}

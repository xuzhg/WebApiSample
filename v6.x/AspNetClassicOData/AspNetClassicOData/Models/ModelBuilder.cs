using Microsoft.OData.Edm;
using Microsoft.OData.Edm.Csdl;
using Microsoft.OData.Edm.Vocabularies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.OData.Builder;
using System.Web.WebSockets;

namespace AspNetClassicOData.Models
{
    public static class ModelBuilder
    {
        private static IEdmModel _model;
        public static IEdmModel GetEdmModel()
        {
            if (_model == null)
            {
                var builder = new ODataConventionModelBuilder();
                builder.EntitySet<Customer>("Customers");
                _model = builder.GetEdmModel();

                ApplyAnnotation((EdmModel)_model);
            }

            return _model;
        }

        private static void ApplyAnnotation(EdmModel model)
        {
            EdmTerm term = new EdmTerm("NS", "IsShowHidden", EdmPrimitiveTypeKind.Boolean);
            model.AddElement(term);

            EdmEntityType customer = model.SchemaElements.OfType<EdmEntityType>().First(c => c.Name == "Customer");
            foreach (var property in customer.Properties())
            {
                if (HasShowHiddenAttribute<Customer>(property.Name))
                {
                    EdmVocabularyAnnotation ann = new EdmVocabularyAnnotation(property, term, new EdmBooleanConstant(true));
                    ann.SetSerializationLocation(model, EdmVocabularyAnnotationSerializationLocation.Inline);
                    model.SetVocabularyAnnotation(ann);
                }
            }
        }

        private static bool HasShowHiddenAttribute<T>(string propertyName)
        {
            return typeof(T).GetProperty(propertyName).GetCustomAttributes(typeof(ShowHiddenAttribute), false).Any();
        }
    }
}
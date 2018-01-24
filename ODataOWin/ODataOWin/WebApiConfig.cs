using Microsoft.OData.Edm;
using Microsoft.OData.Edm.Csdl;
using Microsoft.OData.Edm.Vocabularies;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.OData.Builder;
using System.Web.OData.Extensions;
using System.Xml.Linq;

namespace ODataOWin
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Count().Filter().OrderBy().Expand().Select().MaxTop(1000);
            IEdmModel model = GetEdmModel();
            config.MapODataServiceRoute("odata", "odata", model);
        }

        private static IEdmModel GetEdmModel()
        {
            var builder = new ODataConventionModelBuilder();
            builder.EntitySet<Product>("Products");
            EdmModel model = builder.GetEdmModel() as EdmModel;

            AddSelectTerm<Product>(model);

            return model;
        }

        private static void AddSelectTerm<T>(EdmModel model)
        {
            EdmComplexType complexType = new EdmComplexType("NS", "SelectType");
            complexType.AddStructuralProperty("DefaultSelect", new EdmCollectionTypeReference(new EdmCollectionType(EdmCoreModel.Instance.GetString(true))));
            complexType.AddStructuralProperty("DefaultHidden", new EdmCollectionTypeReference(new EdmCollectionType(EdmCoreModel.Instance.GetString(true))));
            model.AddElement(complexType);
            EdmTerm term = new EdmTerm("NS", "MyTerm", new EdmComplexTypeReference(complexType, true));
            model.AddElement(term);

            Type type = typeof(T);
            string name = type.Name;
            var entityType = model.SchemaElements.OfType<IEdmEntityType>().FirstOrDefault(c => c.Name == name);
            if (entityType == null)
            {
                return;
            }

            IList<string> defaultSelects = new List<string>();
            IList<string> defaultHiddens = new List<string>();
            var properties = type.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
            foreach (var property in properties)
            {
                var attrs = property.GetCustomAttributes(typeof(DefaultSelectAttribute), false);
                if (attrs != null && attrs.Any())
                {
                    defaultSelects.Add(property.Name);
                    continue;
                }

                attrs = property.GetCustomAttributes(typeof(DefaultHiddenAttribute), false);
                if (attrs != null && attrs.Any())
                {
                    defaultHiddens.Add(property.Name);
                    continue;
                }
            }

            if (defaultSelects.Any() && defaultHiddens.Any())
            {
                List<IEdmPropertyConstructor> edmPropertiesConstructors = new List<IEdmPropertyConstructor>
                {
                    new EdmPropertyConstructor("DefaultSelect", new EdmCollectionExpression(
                        defaultSelects.Select(e => new EdmPropertyPathExpression(e)))),
                    new EdmPropertyConstructor("DefaultHidden", new EdmCollectionExpression(
                        defaultHiddens.Select(e => new EdmPropertyPathExpression(e)))),
                };

                IEdmRecordExpression record = new EdmRecordExpression(edmPropertiesConstructors);
                EdmVocabularyAnnotation annotation = new EdmVocabularyAnnotation(entityType, term, record);
                annotation.SetSerializationLocation(model, EdmVocabularyAnnotationSerializationLocation.Inline);
                model.SetVocabularyAnnotation(annotation);
            }
        }
    }

    public class Product
    {
        [Key]
        [DefaultSelect]
        public int Id { get; set; }

        [DefaultSelect]
        public string Name { get; set; }

        public decimal Price { get; set; }

        [DefaultHidden]
        public DateTimeOffset LastUpdate { get; set; }
    }
}

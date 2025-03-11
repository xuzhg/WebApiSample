using Microsoft.OData;
using Microsoft.OData.Edm;
using Microsoft.OData.Edm.Csdl;
using Microsoft.OData.Edm.Validation;
using Microsoft.OData.Edm.Vocabularies;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
//using Xunit;

namespace ODataReadWrite.Annotations
{
    public class ODataEdmAnnotationTests
    {
       // [Fact]
        public void CreateCustomizedODataAnnotationTest()
        {
            EdmModel model = new EdmModel();

            // Entity Type
            EdmEntityType person = new EdmEntityType("NS", "Person");
            model.AddElement(person);

            // EntityContainer
            EdmEntityContainer container = new EdmEntityContainer("NS", "Default");
            EdmEntitySet persons = container.AddEntitySet("Persons", person);
            model.AddElement(container);

            // Complex Type
            EdmComplexType dataField = new EdmComplexType("UI", "DataField");
            dataField.AddStructuralProperty("Value", EdmCoreModel.Instance.GetAnnotationPath(true));
            model.AddElement(dataField);

            // Term Type
            EdmTerm term = new EdmTerm("UI", "LineItem", new EdmComplexTypeReference(dataField, true));
            model.AddElement(term);

            EdmRecordExpression record = new EdmRecordExpression(new EdmPropertyConstructor("Value", new EdmPathExpression("Name")));
            EdmVocabularyAnnotation annotation = new EdmVocabularyAnnotation(persons, term, record);
            annotation.SetSerializationLocation(model, EdmVocabularyAnnotationSerializationLocation.Inline); // no necessary
            model.SetVocabularyAnnotation(annotation);

            string xml = SerializeAsXml(model);

        //    Assert.Equal(@"", xml);
        }

        private string SerializeAsXml(IEdmModel model)
        {
            using (StringWriter sw = new StringWriter())
            {
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Encoding = System.Text.Encoding.UTF8;
                settings.Indent = true;

                using (XmlWriter xw = XmlWriter.Create(sw, settings))
                {
                    IEnumerable<EdmError> errors;
                    CsdlWriter.TryWriteCsdl(model, xw, CsdlTarget.OData, out errors);
                    xw.Flush();
                }

                return sw.ToString();
            }
        }

    }
}

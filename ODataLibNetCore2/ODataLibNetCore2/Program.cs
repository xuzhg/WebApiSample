using Microsoft.OData.Edm;
using Microsoft.OData.Edm.Csdl;
using Microsoft.OData.Edm.Validation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using MyODataLibForNetCore2;

namespace ODataLibNetCore2
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("In .Net Core 2.1 ");

            IEdmModel model = GetEdmModel();

            Console.WriteLine(GetCsdl(model, CsdlTarget.OData));

            Console.WriteLine("Call .NetStandard library 2.0 ");
            Console.WriteLine(model.SerializeAsJson());

            Console.ReadKey();
        }

        private static IEdmModel GetEdmModel()
        {
            var model = new EdmModel();
            EdmComplexType address = new EdmComplexType("NS", "Address");
            address.AddStructuralProperty("Street", EdmPrimitiveTypeKind.String);
            address.AddStructuralProperty("City", EdmPrimitiveTypeKind.String);
            model.AddElement(address);

            EdmEntityType customer = new EdmEntityType("NS", "Customer");
            customer.AddKeys(customer.AddStructuralProperty("Id", EdmCoreModel.Instance.GetString(false)));
            customer.AddStructuralProperty("Name", EdmPrimitiveTypeKind.String);
            customer.AddStructuralProperty("Location", new EdmComplexTypeReference(address, false));
            model.AddElement(customer);

            var container = new EdmEntityContainer("NS", "Container");
            var customers = new EdmEntitySet(container, "Customers", customer);
            container.AddElement(customers);
            model.AddElement(container);
            return model;
        }

        private static string GetCsdl(IEdmModel model, CsdlTarget target)
        {
            string edmx = string.Empty;

            using (StringWriter sw = new StringWriter())
            {
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;
                settings.Encoding = System.Text.Encoding.UTF8;

                using (XmlWriter xw = XmlWriter.Create(sw, settings))
                {
                    IEnumerable<EdmError> errors;
                    CsdlWriter.TryWriteCsdl(model, xw, target, out errors);
                    xw.Flush();
                }

                edmx = sw.ToString();
            }

            return edmx;
        }
    }
}

using Microsoft.OData.Edm;
using Microsoft.OData.Edm.Library;
using Microsoft.OData.Edm.Library.Values;

namespace UntypeSample.Models
{
    public static class EdmModelBuilder
    {
        private static IEdmModel _edmModel;
        public static IEdmModel EdmModel
        {
            get { return _edmModel ?? (_edmModel = BuildEdmModel()); }
        }

        private static IEdmModel BuildEdmModel()
        {
            var model = new EdmModel();

            // open complex type address
            EdmComplexType address = new EdmComplexType("NS", "Address", null, false, true);
            address.AddStructuralProperty("Street", EdmPrimitiveTypeKind.String);
            model.AddElement(address);

            // enum type color
            EdmEnumType color = new EdmEnumType("NS", "Color");
            color.AddMember(new EdmEnumMember(color, "Red", new EdmIntegerConstant(0)));
            model.AddElement(color);

            // open entity type customer
            EdmEntityType customer = new EdmEntityType("NS", "Customer", null, false, true);
            customer.AddKeys(customer.AddStructuralProperty("CustomerId", EdmPrimitiveTypeKind.Int32));
            customer.AddStructuralProperty("Name", EdmPrimitiveTypeKind.String, false);
            customer.AddStructuralProperty("Color", new EdmEnumTypeReference(color, isNullable: true));
            model.AddElement(customer);

            EdmEntityContainer container = new EdmEntityContainer("NS", "Container");
            container.AddEntitySet("Customers", customer);
            model.AddElement(container);
            return model;
        }
    }
}

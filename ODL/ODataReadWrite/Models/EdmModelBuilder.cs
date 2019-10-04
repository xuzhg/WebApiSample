
using Microsoft.OData.Edm;

namespace ODataReadWrite.Models
{
    class EdmModelBuilder
    {
        private static IEdmModel _edmModel;

        public static IEdmModel GetEdmModel()
        {
            if (_edmModel != null)
            {
                return _edmModel;
            }

            var model = new EdmModel();

            var addressType = BuildAddressType();
            model.AddElement(addressType);

            var customerType = BuildCustomerType(addressType);
            model.AddElement(customerType);

            var container = new EdmEntityContainer("NS", "Container");
            var customers = new EdmEntitySet(container, "Customers", customerType);
            model.AddElement(container);

            _edmModel = model;
            return _edmModel;
        }

        public static EdmComplexType BuildAddressType()
        {
            var address = new EdmComplexType("NS", "Address");
            address.AddStructuralProperty("Street", EdmPrimitiveTypeKind.String);
            address.AddStructuralProperty("City", EdmPrimitiveTypeKind.String);
            return address;
        }

        public static EdmEntityType BuildCustomerType(EdmComplexType address)
        {
            var customer = new EdmEntityType("NS", "Customer");
            customer.AddKeys(customer.AddStructuralProperty("Id", EdmPrimitiveTypeKind.Int32));
            customer.AddStructuralProperty("Name", EdmPrimitiveTypeKind.String);
            customer.AddStructuralProperty("DisplayName", EdmPrimitiveTypeKind.String);
            customer.AddStructuralProperty("MailEnabled", EdmPrimitiveTypeKind.Boolean);
            customer.AddStructuralProperty("AssignedLabels",
                new EdmCollectionTypeReference(new EdmCollectionType(new EdmComplexTypeReference(address, true))));

            return customer;
        }
    }

}

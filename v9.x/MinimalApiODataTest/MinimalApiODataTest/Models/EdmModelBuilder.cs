using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;

namespace MinimalApiODataTest.Models
{
    public static class EdmModelBuilder
    {
        private static IEdmModel? _edmModel;

        public static IEdmModel GetEdmModel()
        {
            if (_edmModel == null)
            {
                var builder = new ODataConventionModelBuilder();
                builder.EntitySet<Customer>("Customers");
                builder.EntitySet<Order>("Orders");
                builder.ComplexType<Address>();
                _edmModel = builder.GetEdmModel();
            }

            return _edmModel;
        }
    }
}

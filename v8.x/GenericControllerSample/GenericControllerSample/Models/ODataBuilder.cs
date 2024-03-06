using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;

namespace GenericControllerSample.Models
{
    public class ODataBuilder
    {
        private static IEdmModel _model;

        public static IEdmModel GetEdmModel()
        {
            if (_model == null)
            {
                var builder = new ODataConventionModelBuilder();
                var customer = builder.EntitySet<Customer>("Customers").EntityType;

                customer.Collection.Function("GetLatestCustomer").Returns<int>().Parameter<int>("key");
                customer.Function("GetLatestCustomer").Returns<int>().Parameter<int>("key");

                builder.EntitySet<Order>("Orders");
                _model = builder.GetEdmModel();
            }

            return _model;
        }
    }
}

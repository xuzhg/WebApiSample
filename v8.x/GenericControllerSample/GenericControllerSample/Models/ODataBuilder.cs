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

                // Edm action BulkTaskOperations
                customer.Action("BulkTaskOperations").Parameter<int>("p1");
                customer.Collection.Action("CollectionActionOperations").Parameter<int>("p1");

                // Edm function EdmFunctionOperations
                customer.Function("EdmFunctionOperations").Returns<int>().Parameter<int>("p1");
                customer.Collection.Function("CollectionFunctionOperations").Returns<string>().Parameter<string>("p1");

                builder.EntitySet<Order>("Orders");
                _model = builder.GetEdmModel();
            }

            return _model;
        }
    }
}

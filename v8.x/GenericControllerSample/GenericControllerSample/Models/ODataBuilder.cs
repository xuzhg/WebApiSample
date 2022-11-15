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
                builder.EntitySet<Customer>("Customers");
                builder.EntitySet<Order>("Orders");
                _model = builder.GetEdmModel();
            }

            return _model;
        }
    }
}

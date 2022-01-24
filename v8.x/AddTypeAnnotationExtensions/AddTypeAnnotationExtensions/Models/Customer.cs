using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;

namespace AddTypeAnnotationExtensions.Models
{
    public class Customer
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public IDictionary<string, object> Dynamics { get; set; }
    }

    public static class ModelBuilder
    {
        private static IEdmModel _model;

        public static IEdmModel GetEdmModel()
        {
            if (_model != null)
            {
                return _model;
            }

            var builder = new ODataConventionModelBuilder();
            builder.EntitySet<Customer>("Customers");
            _model = builder.GetEdmModel();
            return _model;
        }
    }
}

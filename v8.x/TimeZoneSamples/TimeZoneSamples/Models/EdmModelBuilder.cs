using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;

namespace TimeZoneSamples.Models
{
    public static class EdmModelBuilder
    {
        public static IEdmModel GetEdmModel()
        {
            var builder = new ODataConventionModelBuilder();
            builder.EntitySet<Customer>("Customers");
            return builder.GetEdmModel();
        }
    }
}

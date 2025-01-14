using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;

namespace CollectionOfCollectionUsingEdmUntyped.Models
{
    public static class ModelBuilder
    {
        public static IEdmModel GetEdmModel()
        {
            var builder = new ODataConventionModelBuilder();
            builder.EntitySet<PlaneDto>("Planes");
            return builder.GetEdmModel();
        }
    }
}

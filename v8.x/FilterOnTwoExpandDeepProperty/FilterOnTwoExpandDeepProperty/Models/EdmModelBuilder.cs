using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;

namespace FilterOnTwoExpandDeepProperty.Models;

public static class EdmModelBuilder
{
    public static IEdmModel GetEdmModel()
    {
        var builder = new ODataConventionModelBuilder();
        builder.EntitySet<Thing1Thing2RelationTable>("Thing1Thing2RelationTable");
        builder.EntityType<AttributeInfo>();
        return builder.GetEdmModel();
    }
}

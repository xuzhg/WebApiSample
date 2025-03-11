// Configure the HTTP request pipeline.



using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;

public static class ModelBuilder
{
    public static IEdmModel GetEdmModel()
    {
        var builder = new ODataConventionModelBuilder();
        builder.EntitySet<Customer>("Customers");
        return builder.GetEdmModel();
    }
}
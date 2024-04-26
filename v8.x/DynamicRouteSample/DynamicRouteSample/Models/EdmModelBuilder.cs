using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;

namespace DynamicRouteSample.Models;

public static class EdmModelBuilder
{
    public static IEdmModel GetEdmModel1()
    {
        var builder = new ODataConventionModelBuilder();
        builder.EntitySet<Customer>("Customers");
        builder.EntitySet<Person>("People");
        return builder.GetEdmModel();
    }

    public static IEdmModel GetEdmModel2()
    {
        var builder = new ODataConventionModelBuilder();
        builder.EntitySet<Order>("Orders");
        builder.EntitySet<Person>("People");
        return builder.GetEdmModel();
    }
}


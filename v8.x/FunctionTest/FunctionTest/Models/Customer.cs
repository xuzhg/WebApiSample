using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using System.ComponentModel;

namespace FunctionTest.Models;

public static class EdmModelBuilder
{
    private static IEdmModel _edmModel;

    public static IEdmModel GetEdmModel()
    {
        if (_edmModel != null)
        {
            return _edmModel;
        }

        var builder = new ODataConventionModelBuilder();
        builder.EntitySet<Customer>("Customers");

        // only building the function template using the primitive type parameters
        var f = builder.EntityType<Customer>().Collection.Function("MyFunction");
        f.Parameter<Detail>("d");
        f.Parameter<int>("k");
        f.Returns<string>();

        // building the function with full parameter but sending the complex using request body
        f = builder.EntityType<Customer>().Collection.Function("MyFunction2");
        f.Parameter<Detail>("d");
        f.Parameter<int>("k");
        f.Returns<string>();

        // building the function with full parameter but sending the complex using route
        f = builder.EntityType<Customer>().Collection.Function("MyFunction3");
        f.Parameter<Detail>("d");
        f.Parameter<int>("k");
        f.Returns<string>();

        _edmModel = builder.GetEdmModel();
        return _edmModel;
    }
}

public class Customer
{
    public int Id { get; set; }
}

public class Detail
{
    public bool RequiredConfiguration { get; set; }

    public IList<string> Configuration {  get; set; }

    public bool RequiredFullInformation { get; set; }

    public IList<string> FullInformation { get; set; }

    public bool ServerInformation { get; set; }
    public bool RequiredSummary { get; set; }
    public bool RequiredConnectionCheck { get; set; }

    public int KFromFunctionCall { get; set; }
}

using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using OData.Annotations.Example.Controllers;

internal class EdmModelBuilder
{
    public static IEdmModel CreateEdmModel()
    {
        var builder = new ODataConventionModelBuilder();

        var accountConfiguration = builder.EntitySet<Account>("Accounts").EntityType;
        accountConfiguration.Ignore(x => x.OwnerIdName);

        return builder.GetEdmModel();
    }
}
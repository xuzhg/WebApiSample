using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using System.ComponentModel;

namespace Question79295036AttributeRoutingWarning.Models;

public class EdmModelBuilder
{
    public static IEdmModel GetEdmModel()
    {
        var builder = new ODataConventionModelBuilder();
        builder.EntitySet<MessageViewModel>("Message");

        // Define the bound action
        var getTagMessagesAction = builder.EntityType<MessageViewModel>().Collection.Action("GetTagMessages");

        // Optionally define parameters for the action
        getTagMessagesAction.CollectionParameter<int>("tagIds");

        return builder.GetEdmModel();
    }
}

using DynamicRouteSample.Models;
using Microsoft.OData.Edm;

namespace DynamicRouteSample.Extensions;

public class ODataModelProvider : IODataModelProvider
{
    private static IDictionary<string, IEdmModel> Models;
    static ODataModelProvider()
    {
        Models = new Dictionary<string, IEdmModel>
        {
            { "v1", EdmModelBuilder.GetEdmModel1() },
            { "v2", EdmModelBuilder.GetEdmModel2() }
        };
    }

    public IEdmModel GetEdmModel(string prefix)
    {
        if (Models.TryGetValue(prefix, out IEdmModel model))
        {
            return model;
        }

        throw new NotImplementedException($"No EdmModel created for prefix: {prefix}");
    }
}

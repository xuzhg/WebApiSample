using Microsoft.OData.Edm;

namespace DynamicRouteSample.Extensions;

public interface IODataModelProvider
{
    IEdmModel GetEdmModel(string prefix);
}

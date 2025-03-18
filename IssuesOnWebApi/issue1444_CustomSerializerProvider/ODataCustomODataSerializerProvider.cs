using Microsoft.AspNetCore.OData.Formatter.Serialization;
using Microsoft.OData.Edm;

public class ODataCustomODataSerializerProvider : ODataSerializerProvider
{
    public ODataCustomODataSerializerProvider(IServiceProvider serviceProvider) :
        base(serviceProvider)
    {
    }
    public override IODataEdmTypeSerializer GetEdmTypeSerializer(IEdmTypeReference edmType)
    {
        if (edmType.FullName() == typeof(Dictionary<int, List<int>>).FullName)
        {
            // return Do something
        }

        return base.GetEdmTypeSerializer(edmType);
    }
}
using Microsoft.OData.UriParser;

namespace DynamicRouteSample.Extensions;

public interface IODataRouteMatcher
{
    bool Match(string prefix, ODataPath path, RouteValueDictionary values);
}

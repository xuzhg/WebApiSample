using Microsoft.OData.UriParser;

namespace DynamicRouteSample.Extensions;

// We supports:
// {entityset}
// {singleton}
// {key}
// {property}
// {navigation}
// {function}
// {action}
// {cast}
[AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
public class ODataRouteAttribute : Attribute, IODataRouteMatcher
{
    private IList<SegmentInfo> _segments;

    public ODataRouteAttribute(string pattern)
        : this(null, pattern)
    {
    }

    public ODataRouteAttribute(string prefix, string pattern)
    {
        Prefix = prefix;
        Pattern = pattern;
        Initialize();
    }

    /// <summary>
    /// Case-sensitive
    /// without provider prefix means to match all routes
    /// </summary>
    public string Prefix { get; }

    // Be noted: make the match pattern simple.
    // use { and } to match the segment kind: for example {entityset} --> EntitySetSegment
    // {key} -> KeySegment
    // {singleton} -> SingletonSegment
    // without { and } to match the segment value, for example: customers ==> Customers
    public string Pattern { get; }

    public bool Match(string prefix, ODataPath path, RouteValueDictionary values)
    {
        if ((Prefix != null && Prefix != prefix) || path.Count != _segments.Count)
        {
            return false;
        }

        int i = 0;
        foreach(var seg in _segments)
        {
            ODataPathSegment pathSegment = path.ElementAt(i);
            i++;

            if (seg.Kind == SegmentKind.Raw)
            {
                if (!string.Equals(seg.Raw, pathSegment.Identifier))
                {
                    return false;
                }

                continue;
            }
            else if (seg.Kind == SegmentKind.EntitySet && pathSegment is EntitySetSegment entitySet)
            {
                values["entityset"] = entitySet.EntitySet.Name;
                continue;
            }
            else if (seg.Kind == SegmentKind.Singleton && pathSegment is SingletonSegment singleton)
            {
                values["singleton"] = singleton.Singleton.Name;
                continue;
            }
            else if (seg.Kind == SegmentKind.Key && pathSegment is KeySegment key)
            {
                // for multiple keys, use these codes
                //foreach (var keyItem in key.Keys)
                //{
                //    values[keyItem.Key] = keyItem.Value;
                //}

                // for simplicity, assume it's only one key
                var keyItem = key.Keys.First();
                values[keyItem.Key] = keyItem.Value;
                values["key"] = keyItem.Value;
                continue;
            }
            else if (seg.Kind == SegmentKind.Property && pathSegment is PropertySegment property)
            {
                values["property"] = property.Property.Name;
                continue;
            }
            else if (seg.Kind == SegmentKind.Navigation && pathSegment is NavigationPropertySegment nav)
            {
                values["navigation"] = nav.NavigationProperty.Name;
                continue;
            }
            else if ( (seg.Kind == SegmentKind.Function || seg.Kind == SegmentKind.Action) && pathSegment is OperationSegment operation)
            {
                // TODO:
                continue;
            }
            else
            {
                return false;
            }
        }

        return true;
    }

    private void Initialize()
    {
        string[] segments = Pattern.Split('/');

        _segments = new List<SegmentInfo>();

        foreach (var seg in segments)
        {
            int start = seg.IndexOf('(');
            if (start > 0 && seg.EndsWith(")"))
            {
                string firstSegment = seg.Substring(0, start);
                string secondSegment = seg.Substring(start + 1, seg.Length - start - 2);
                _segments.Add(new SegmentInfo(firstSegment));
                _segments.Add(new SegmentInfo(secondSegment));
            }
            else
            {
                _segments.Add(new SegmentInfo(seg));
            }
        }
    }
}

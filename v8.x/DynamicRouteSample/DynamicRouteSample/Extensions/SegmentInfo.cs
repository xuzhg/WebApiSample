namespace DynamicRouteSample.Extensions;

public class SegmentInfo
{
    public static readonly IDictionary<string, SegmentKind> _segmentsDic
        = new Dictionary<string, SegmentKind>(StringComparer.OrdinalIgnoreCase)
        {
            { "entityset", SegmentKind.EntitySet},
            { "singleton", SegmentKind.Singleton},
            { "key", SegmentKind.Key},
            { "id", SegmentKind.Key},
            { "property", SegmentKind.Property},
            { "navigation", SegmentKind.Navigation},
            { "cast", SegmentKind.Cast},
            { "function", SegmentKind.Function},
            { "action", SegmentKind.Action}
        };

    public SegmentInfo(string rawSegment)
    {
        Raw = rawSegment;

        Kind = SegmentKind.Raw;
        if (rawSegment.StartsWith("{") && rawSegment.EndsWith("}"))
        {
            string value = rawSegment.Substring(1, rawSegment.Length - 2);
            if (_segmentsDic.TryGetValue(value, out SegmentKind kind))
            {
                Kind = kind;
            }
            else
            {
                throw new InvalidOperationException($"Segment template '{rawSegment}' is not supported");
            }
        }
    }

    public string Raw { get; }

    public SegmentKind Kind { get; }
}

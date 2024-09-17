using Question78956264AllowDeepNavigation.Controllers;
using System.Text;

namespace Question78956264AllowDeepNavigation.Extensions;

[ODataBrowserPropertyBinding]
public class BrowserInfos
{
    private IList<SegmentInfo> segmentInfos = new List<SegmentInfo>();

    public IList<SegmentInfo> Segments => segmentInfos;

    public void Add(SegmentInfo segmentInfo)
    {
        segmentInfos.Add(segmentInfo);
    }

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        foreach (var segmentInfo in segmentInfos)
        {
            sb.Append(segmentInfo.ToString());
            sb.Append("/");
        }

        return sb.ToString();
    }
}


public abstract class SegmentInfo { }

public class EntitySetSegmentInfo : SegmentInfo
{
    public string EntitySet { get; set; }

    public override string ToString()
    {
        return $"EntitySet: {EntitySet}";
    }
}
public class IntKeySegmentInfo : SegmentInfo
{
    public string KeyName { get; set; }
    public int Value { get; set; }

    public override string ToString()
    {
        return $"Key: {KeyName} = {Value}";
    }
}

public class PropertySegmentInfo : SegmentInfo
{
    public string PropertyName { get; set; }

    public override string ToString()
    {
        return $"Property: {PropertyName}";
    }
}

public class BoundOperationSegmentInfo : SegmentInfo
{
    public string OperationName { get; set; }

    // Without take parameters into consideration for simplicity
    public override string ToString()
    {
        return $"BoundOperation: {OperationName}()";
    }
}

using System.Runtime;
using System.Text;

namespace WKBSample.WBK;

internal class WKBMultiPoint : WKBObject
{
    public override SpatialType SpatialType => SpatialType.MultiPoint;
    public IList<WKBPoint> Points { get; set; } = new List<WKBPoint>();

    public override string ToString()
    {
        if (Points.Count == 0)
        {
            return "MultiPoint (EMPTY)";
        }

        return $"MultiPoint (Count = {Points.Count})";
    }

    public override void GetBits(IList<BitsInfo> bitInfos, WKBConfig config, bool handSrid)
    {
        InsertByteOrder(bitInfos, config.Order);
        byte[] bytes = GetHeader(SpatialType.MultiPoint, bitInfos, config, handSrid);

        InsertSRID(bitInfos, config, handSrid);

        int num = Points.Count;
        string numBytes = BitUtils.GetInt(num, config.Order);
        bitInfos.Add(new BitsInfo { Bytes = numBytes, Info = $"({num}) Points" });

        foreach (WKBPoint point in Points)
        {
            point.GetBits(bitInfos, config, false);
        }
    }

    public override void GetWKB(StringBuilder wkb, WKBConfig config, bool handSrid, bool hasHeader)
    {
        if (hasHeader)
        {
            if (handSrid && config.HasSRID)
            {
                wkb.Append($"SRID={config.SRID};");
            }

            wkb.Append("MULTIPOINT ");
        }

        if (Points.Count == 0)
        {
            wkb.Append("EMPTY");
            return;
        }

        int index = 0;
        wkb.Append("(");
        foreach (WKBPoint point in Points)
        {
            point.GetWKB(wkb, config, false, false);

            if (index != Points.Count - 1)
            {
                wkb.Append(",");
            }

            ++index;
        }
        wkb.Append(")");
    }
}

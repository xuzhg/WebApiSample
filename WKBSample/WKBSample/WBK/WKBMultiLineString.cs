using System.Text;

namespace WKBSample.WBK;

internal class WKBMultiLineString : WKBObject
{
    public override SpatialType SpatialType => SpatialType.MultiLineString;
    public IList<WKBLineString> LineStrings { get; set; } = new List<WKBLineString>();

    public override string ToString()
    {
        if (LineStrings.Count == 0)
        {
            return "MultiLineString (EMPTY)";
        }

        return $"MultiLineString (Count = {LineStrings.Count})";
    }

    public override void GetBits(IList<BitsInfo> bitInfos, WKBConfig config, bool handSrid)
    {
        InsertByteOrder(bitInfos, config.Order);
        byte[] bytes = GetHeader(SpatialType.MultiLineString, bitInfos, config, handSrid);

        InsertSRID(bitInfos, config, handSrid);

        int num = LineStrings.Count;
        string numBytes = BitUtils.GetInt(num, config.Order);
        bitInfos.Add(new BitsInfo { Bytes = numBytes, Info = $"({num}) LineStrings"});

        foreach (WKBLineString lingString in LineStrings)
        {
            lingString.GetBits(bitInfos, config, false);
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

            wkb.Append("MULTILINESTRING ");
        }

        if (LineStrings.Count == 0)
        {
            wkb.Append("EMPTY");
            return;
        }

        wkb.Append("(");
        foreach (WKBLineString lingString in LineStrings)
        {
            lingString.GetWKB(wkb, config, false, false);
        }
        wkb.Append(")");
    }
}

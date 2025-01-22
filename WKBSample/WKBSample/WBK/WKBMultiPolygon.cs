using System;
using System.Text;

namespace WKBSample.WBK;

internal class WKBMultiPolygon : WKBObject
{
    public override SpatialType SpatialType => SpatialType.MultiPolygon;
    public IList<WKBPolygon> Polygons { get; set; } = new List<WKBPolygon>();

    public override string ToString()
    {
        if (Polygons.Count == 0)
        {
            return "MultiPolygon (EMPTY)";
        }

        return $"MultiPolygon (Count = {Polygons.Count})";
    }

    public override void GetBits(IList<BitsInfo> bitInfos, WKBConfig config, bool handSrid)
    {
        InsertByteOrder(bitInfos, config.Order);
        byte[] bytes = GetHeader(SpatialType.MultiPolygon, bitInfos, config, handSrid);

        InsertSRID(bitInfos, config, handSrid);

        int num = Polygons.Count;
        string numBytes = BitUtils.GetInt(num, config.Order);
        bitInfos.Add(new BitsInfo { Bytes = numBytes, Info = $"({num}) Polygons" });

        foreach (WKBPolygon polygon in Polygons)
        {
            polygon.GetBits(bitInfos, config, false);
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

            wkb.Append("MULTIPOLYGON ");
        }

        if (Polygons.Count == 0)
        {
            wkb.Append("EMPTY");
            return;
        }

        int index = 0;
        wkb.Append("(");
        foreach (WKBPolygon polygon in Polygons)
        {
            polygon.GetWKB(wkb, config, false, false);

            if (index != Polygons.Count - 1)
            {
                wkb.Append(",");
            }

            ++index;
        }
        wkb.Append(")");
    }
}

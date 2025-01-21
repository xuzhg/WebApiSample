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

        foreach (WKBPolygon point in Polygons)
        {
            point.GetBits(bitInfos, config, false);
        }
    }
}

namespace WKBSample.WBK;

internal class WKBPolygon : WKBObject
{
    public override SpatialType SpatialType => SpatialType.Polygon;

    public IList<WKBLineString> Rings { get; set; } = new List<WKBLineString>();

    public override string ToString()
    {
        if (Rings.Count == 0)
        {
            return "Polygon (EMPTY)";
        }

        return $"Polygon (Count = {Rings.Count})";
    }

    public override void GetBits(IList<BitsInfo> bitInfos, WKBConfig config, bool handSrid)
    {
        InsertByteOrder(bitInfos, config.Order);
        byte[] bytes = GetHeader(SpatialType.Polygon, bitInfos, config, handSrid);

        InsertSRID(bitInfos, config, handSrid);

        int num = Rings.Count;
        string numBytes = BitUtils.GetInt(num, config.Order);
        bitInfos.Add(new BitsInfo { Bytes = numBytes, Info = $"({num}) Rings" });

        int index = 0;
        foreach (WKBLineString lineString in Rings)
        {
            int pointNum = lineString.Points.Count;
            numBytes = BitUtils.GetInt(pointNum, config.Order);
            bitInfos.Add(new BitsInfo { Bytes = numBytes, Info = $"({pointNum}) Points in [{index}] Ring" });

            int pointIndex = 0;
            foreach (WKBPoint point in lineString.Points)
            {
                string xBits = BitUtils.GetDouble(point.X, config.Order);
                bitInfos.Add(new BitsInfo { Bytes = xBits, Info = $"[{index},{pointIndex}]x ({point.X})" });

                string yBits = BitUtils.GetDouble(point.Y, config.Order);
                bitInfos.Add(new BitsInfo { Bytes = yBits, Info = $"[{index},{pointIndex}]y ({point.Y})" });

                if (config.HasZ)
                {
                    string zBits = BitUtils.GetDouble(point.Z, config.Order);
                    bitInfos.Add(new BitsInfo { Bytes = zBits, Info = $"[{index},{pointIndex}]z ({point.Z})" });
                }

                if (config.HasM)
                {
                    string mBits = BitUtils.GetDouble(point.M, config.Order);
                    bitInfos.Add(new BitsInfo { Bytes = mBits, Info = $"[{index},{pointIndex}]m ({point.M})" });
                }

                pointIndex++;
            }

            index++;
        }
    }
}

namespace WKBSample.WBK
{
    internal class WKBLineString : WKBObject
    {
        public override SpatialType SpatialType => SpatialType.LineString;

        public IList<WKBPoint> Points { get; set; } = new List<WKBPoint>();

        public override string ToString()
        {
            if (Points.Count == 0)
            {
                return "LineString (EMPTY)";
            }

            return $"LineString (Count = {Points.Count})";
        }

        public override void GetBits(IList<BitsInfo> bitInfos, WKBConfig config, bool handSrid)
        {
            InsertByteOrder(bitInfos, config.Order);
            byte[] bytes = GetHeader(SpatialType.LineString, bitInfos, config, handSrid);

            InsertSRID(bitInfos, config, handSrid);

            int num = Points.Count;
            string numBytes = BitUtils.GetInt(num, config.Order);
            bitInfos.Add(new BitsInfo { Bytes = numBytes, Info = $"({num}) Points" });

            int index = 0;
            foreach (WKBPoint point in Points)
            {
                // Directly gets the points
                string xBits = GetDouble(point.X, config.Order);
                bitInfos.Add(new BitsInfo { Bytes = xBits, Info = $"[{index}]:x ({point.X})" });

                string yBits = GetDouble(point.Y, config.Order);
                bitInfos.Add(new BitsInfo { Bytes = yBits, Info = $"[{index}]:y ({point.Y})" });

                if (config.HasZ)
                {
                    string zBits = GetDouble(point.Z, config.Order);
                    bitInfos.Add(new BitsInfo { Bytes = zBits, Info = $"[{index}]:z ({point.Z})" });
                }

                if (config.HasM)
                {
                    string mBits = GetDouble(point.M, config.Order);
                    bitInfos.Add(new BitsInfo { Bytes = mBits, Info = $"[{index}]:m ({point.M})" });
                }

                ++index;
            }
        }
    }
}

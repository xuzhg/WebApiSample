namespace WKBSample.WBK
{
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
            bitInfos.Add(new BitsInfo { Bytes = numBytes, Info = $"Points Number ({num})" });

            foreach (WKBPoint point in Points)
            {
                point.GetBits(bitInfos, config, false);
            }
        }
    }
}

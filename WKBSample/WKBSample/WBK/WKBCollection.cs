namespace WKBSample.WBK
{
    internal class WKBCollection : WKBObject
    {
        public override SpatialType SpatialType => SpatialType.Collection;
        public IList<IWKBObject> Items { get; set; } = new List<IWKBObject>();

        public override string ToString()
        {
            if (Items.Count == 0)
            {
                return "Collection (EMPTY)";
            }

            return $"Collection (Count = {Items.Count})";
        }

        public override void GetBits(IList<BitsInfo> bitInfos, WKBConfig config, bool handSrid)
        {
            InsertByteOrder(bitInfos, config.Order);
            byte[] bytes = GetHeader(SpatialType.Collection, bitInfos, config, handSrid);

            InsertSRID(bitInfos, config, handSrid);

            int num = Items.Count;
            string numBytes = BitUtils.GetInt(num, config.Order);
            bitInfos.Add(new BitsInfo { Bytes = numBytes, Info = $"Item Number ({num})" });

            foreach (IWKBObject item in Items)
            {
                item.GetBits(bitInfos, config, false);
            }
        }
    }
}

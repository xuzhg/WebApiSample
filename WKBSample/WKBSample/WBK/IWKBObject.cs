﻿namespace WKBSample.WBK
{
    internal interface IWKBObject
    {
        SpatialType SpatialType { get; }
        int? SRID { get; set; }

        void GetBits(IList<BitsInfo> bitInfos, WKBConfig config, bool handSrid);
    }
}

using System.Text;

namespace WKBSample.WBK;

internal interface IWKBObject
{
    SpatialType SpatialType { get; }
    int? SRID { get; set; }

    void GetBits(IList<BitsInfo> bitInfos, WKBConfig config, bool handSrid);

    void GetWKB(StringBuilder wkb, WKBConfig config, bool handSrid, bool hasHeader);
}

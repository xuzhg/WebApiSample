using System.Buffers.Binary;

namespace WKBSample.WBK;

public abstract class WKBObject : IWKBObject
{
    public abstract SpatialType SpatialType { get; }

    public int? SRID { get; set; }

    public abstract void GetBits(IList<BitsInfo> bitInfos, WKBConfig config, bool handSrid);

    protected static void InsertByteOrder(IList<BitsInfo> bitInfos, ByteOrder order)
    {
        if (order == ByteOrder.LittleEndian)
        {
            bitInfos.Add(new BitsInfo { Bytes = "01", Info = "LittleEndian" });
        }
        else
        {
            bitInfos.Add(new BitsInfo { Bytes = "00", Info = "BigEndian" });
        }
    }

    protected static byte[] GetHeader(SpatialType type, IList<BitsInfo> bitInfos, WKBConfig config, bool handleSrid)
    {
        uint intSpatialType = (uint)type & 0xff;

        string zHeader = "";
        if (config.HasZ)
        {
            if (config.IsoWKB)
            {
                intSpatialType += 1000;
                zHeader = " Z(ISO)";
            }
            else
            {
                zHeader = " Z";
            }

            intSpatialType |= 0x80000000;
        }

        string mHeader = "";
        if (config.HasM)
        {
            if (config.IsoWKB)
            {
                intSpatialType += 2000;
                mHeader = " M(ISO)";
            }
            else
            {
                mHeader = " M";
            }

            intSpatialType |= 0x40000000;
        }

        string sridHeader = "";
        if (handleSrid)
        {
            if (config.HasSRID)
            {
                intSpatialType |= 0x20000000;
                sridHeader = " SRID";
            }
        }

        string info = type.ToString() + zHeader +  mHeader + sridHeader;

        byte[] bytes = BitConverter.GetBytes(intSpatialType);
        if (config.Order != ByteOrder.LittleEndian)
        {
            uint newValue = BinaryPrimitives.ReverseEndianness(intSpatialType);
            bytes = BitConverter.GetBytes(newValue);
        }

        bitInfos.Add(new BitsInfo { Bytes = BitUtils.ToHex(bytes), Info = info });
        return bytes;
    }

    protected static void InsertSRID(IList<BitsInfo> byteLines, WKBConfig config, bool handleSrid)
    {
        if (handleSrid && config.HasSRID)
        {
            byte[] bytes;
            if (config.Order == ByteOrder.LittleEndian)
            {
                bytes = BitConverter.GetBytes(config.SRID);
            }
            else
            {
                int newValue = BinaryPrimitives.ReverseEndianness(config.SRID);
                bytes = BitConverter.GetBytes(newValue);
            }

            string bytesStr = BitUtils.ToHex(bytes);
            byteLines.Add(new BitsInfo { Bytes = bytesStr, Info = $"SRID({config.SRID})" });
        }
    }
}

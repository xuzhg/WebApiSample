using System.Buffers.Binary;
using System.Text;

namespace WKBSample.WBK
{
    public abstract class WKBObject : IWKBObject
    {
        public abstract SpatialType SpatialType { get; }

        public int? SRID { get; set; }

        public virtual void GetBits(IList<BitsInfo> bitInfos, WKBConfig config, bool handSrid)
        {
        }

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

            bitInfos.Add(new BitsInfo { Bytes = ToHex(bytes), Info = info });
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

                string bytesStr = ToHex(bytes);
                byteLines.Add(new BitsInfo { Bytes = bytesStr, Info = $"SRID({config.SRID})" });
            }
        }



        protected static string GetDouble(double? d, ByteOrder order)
        {
            double dValue = d ?? double.NaN;
            if (order == ByteOrder.LittleEndian)
            {
                return ToHex(BitConverter.GetBytes(dValue));
            }
            else
            {
                double newD = BitUtils.ReverseByteOrder(dValue);
                return ToHex(BitConverter.GetBytes(newD));
            }
        }

        private static string GetDouble(double d, ByteOrder order)
        {
            if (order == ByteOrder.LittleEndian)
            {
                return ToHex(BitConverter.GetBytes(d));
            }
            else
            {
                double newD = BitUtils.ReverseByteOrder(d);
                return ToHex(BitConverter.GetBytes(newD));
            }
        }

        public static string ToHex(byte[] bytes)
        {
            var buf = new StringBuilder(bytes.Length * 2);
            for (int i = 0; i < bytes.Length; i++)
            {
                byte b = bytes[i];
                buf.Append(ToHexDigit((b >> 4) & 0x0F));
                buf.Append(ToHexDigit(b & 0x0F));
            }
            return buf.ToString();
        }

        private static char ToHexDigit(int n)
        {
            if (n < 0 || n > 15)
                throw new ArgumentException("Nibble value out of range: " + n);
            if (n <= 9)
                return (char)('0' + n);
            return (char)('A' + (n - 10));
        }
    }
}

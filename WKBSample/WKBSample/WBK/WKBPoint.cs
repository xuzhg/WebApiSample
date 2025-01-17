using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Runtime;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace WKBSample.WBK
{
    internal class WKBPoint : WKBObject
    {
        public double X { get; set; } = double.NaN;
        public double Y { get; set; } = double.NaN;
        public double? Z { get; set; } = null;
        public double? M { get; set; } = null;

        public bool IsEmpty => double.IsNaN(X) || double.IsNaN(Y);

        public override SpatialType SpatialType => SpatialType.Point;

        public override string ToString()
        {
            if (IsEmpty)
            {
                return "Point (EMPTY)";
            }

            if (Z != null && M != null)
            {
                return $"Point ({X} {Y} {Z.Value} {M.Value})";
            }
            else if (Z == null && M == null)
            {
                return $"Point ({X} {Y})";
            }
            else if (Z == null)
            {
                return $"Point ({X} {Y} null {M.Value})";
            }
            else
            {
                return $"Point ({X} {Y} {Z.Value} null)";
            }
        }

        public override void GetBits(IList<BitsInfo> bitInfos, WKBConfig config, bool handSrid)
        {
            InsertByteOrder(bitInfos, config.Order);
            byte[] bytes = GetHeader(SpatialType.Point, bitInfos, config, handSrid);

            InsertSRID(bitInfos, config, handSrid);

            string xBits = GetDouble(X, config.Order);
            bitInfos.Add(new BitsInfo { Bytes = xBits, Info = $"x ({X})" });

            string yBits = GetDouble(Y, config.Order);
            bitInfos.Add(new BitsInfo { Bytes = yBits, Info = $"y ({Y})" });

            if (config.HasZ)
            {
                string zBits = GetDouble(Z, config.Order);
                bitInfos.Add(new BitsInfo { Bytes = zBits, Info = $"z ({Z})" });
            }

            if (config.HasM)
            {
                string mBits = GetDouble(M, config.Order);
                bitInfos.Add(new BitsInfo { Bytes = mBits, Info = $"m ({M})" });
            }
        }
    };
}

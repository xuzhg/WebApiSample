
namespace WKBSample
{
    public enum ByteOrder : byte
    {
        //
        // Summary:
        //     BigEndian
        BigEndian,
        //
        // Summary:
        //     LittleEndian
        LittleEndian
    }


    public class WKBConfig
    {
        public bool IsoWKB { get; set; }

        public ByteOrder Order { get; set; }

        public int SRID { get; set; }

        public bool HasSRID { get; set; }

        public bool HasZ { get; set; }

        public bool HasM { get; set; }
    }
}
using System.Diagnostics;
using System.Text;

namespace WKBSample.WBK;

internal static class BitUtils
{
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
            throw new ArgumentException("Value out of range: " + n);
        if (n <= 9)
            return (char)('0' + n);
        return (char)('A' + (n - 10));
    }

    private static IDictionary<char, string> HexToBase2Dic = new Dictionary<char, string>
    {
        { '0', "0000" },
        { '1', "0001" },
        { '2', "0010" },
        { '3', "0011" },
        { '4', "0100" },
        { '5', "0101" },
        { '6', "0110" },
        { '7', "0111" },
        { '8', "1000" },
        { '9', "1001" },
        { 'A', "1010" },
        { 'a', "1010" },
        { 'B', "1011" },
        { 'b', "1011" },
        { 'C', "1100" },
        { 'c', "1100" },
        { 'D', "1101" },
        { 'd', "1101" },
        { 'E', "1110" },
        { 'e', "1110" },
        { 'F', "1111" },
        { 'f', "1111" },
    };

    public static string HexToBinary(string hex)
    {
        StringBuilder sb = new StringBuilder();
        int index = 1;
        int count = hex.Length;
        foreach (char c in hex)
        {
            if (!HexToBase2Dic.ContainsKey(c))
            {
                throw new Exception($"Cannot covert {c} to binary");
            }

            sb.Append(HexToBase2Dic[c]);

            if (index < count && index % 2 == 0)
            {
                sb.Append("-");
            }

            index++;
        }

        return sb.ToString();
    }

    public static string ToBinaryString(byte[] byteIn)
    {
        StringBuilder sb = new StringBuilder(8 * byteIn.Length);
        for (int i = 0; i <= byteIn.Length; i++)
        {
            sb.Append(ToBinaryString(byteIn[i]));
        }

        return sb.ToString();
    }


    public static string ToBinaryString(byte b)
    {
        return Convert.ToString(b, 2).PadLeft(8, '0');
    }

    public static string GetIntBinaryString(int n)
    {
        char[] b = new char[32];
        int pos = 31;
        int i = 0;

        while (i < 32)
        {
            if ((n & (1 << i)) != 0)
            {
                b[pos] = '1';
            }
            else
            {
                b[pos] = '0';
            }
            pos--;
            i++;
        }
        return new string(b);
    }

    internal static string GetDouble(double d, ByteOrder order)
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

    //internal static string GetDouble(double? d, ByteOrder order)
    //{
    //    double dValue = d ?? double.NaN;
    //    if (order == ByteOrder.LittleEndian)
    //    {
    //        return ToHex(BitConverter.GetBytes(dValue));
    //    }
    //    else
    //    {
    //        double newD = BitUtils.ReverseByteOrder(dValue);
    //        return ToHex(BitConverter.GetBytes(newD));
    //    }
    //}

    internal static string GetInt(int d, ByteOrder order)
    {
        if (order == ByteOrder.LittleEndian)
        {
            return ToHex(BitConverter.GetBytes(d));
        }
        else
        {
            int newD = ReverseByteOrder(d);
            return ToHex(BitConverter.GetBytes(newD));
        }
    }

    internal static short ReverseByteOrder(short value)
    {
        unchecked
        {
            return (short)ReverseByteOrder((ushort)value);
        }
    }

    internal static int ReverseByteOrder(int value)
    {
        unchecked
        {
            return (int)ReverseByteOrder((uint)value);
        }
    }

    internal static long ReverseByteOrder(long value)
    {
        unchecked
        {
            return (long)ReverseByteOrder((ulong)value);
        }
    }

    internal static float ReverseByteOrder(float value)
    {
        // TODO: BitConverter.SingleToInt32Bits will exist eventually
        // see https://github.com/dotnet/coreclr/pull/833
        byte[] bytes = System.BitConverter.GetBytes(value);
        Debug.Assert(bytes.Length == 4);

        Array.Reverse(bytes, 0, 4);
        return System.BitConverter.ToSingle(bytes, 0);
    }

    internal static double ReverseByteOrder(double value)
    {
        return System.BitConverter.Int64BitsToDouble(ReverseByteOrder(System.BitConverter.DoubleToInt64Bits(value)));
    }

    internal static ushort ReverseByteOrder(ushort value)
    {
        unchecked
        {
            return (ushort)((value & 0x00FF) << 8 |
                            (value & 0xFF00) >> 8);
        }
    }

    internal static uint ReverseByteOrder(uint value)
    {
        return (value & 0x000000FF) << 24 |
               (value & 0x0000FF00) << 8 |
               (value & 0x00FF0000) >> 8 |
               (value & 0xFF000000) >> 24;
    }

    internal static ulong ReverseByteOrder(ulong value)
    {
        return (value & 0x00000000000000FF) << 56 |
               (value & 0x000000000000FF00) << 40 |
               (value & 0x0000000000FF0000) << 24 |
               (value & 0x00000000FF000000) << 8 |
               (value & 0x000000FF00000000) >> 8 |
               (value & 0x0000FF0000000000) >> 24 |
               (value & 0x00FF000000000000) >> 40 |
               (value & 0xFF00000000000000) >> 56;
    }
}

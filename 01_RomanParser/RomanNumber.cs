using System.Text;

namespace _01_RomanParser;

public record RomanNumber(int Value)
{
    public RomanNumber(string input) :
        this(RomanNumberFactory.ParseAsInt(input))
    {
    }


    public override string? ToString()
    {
        if (Value == 0) return "N";
        Dictionary<int, string> parts = new()
        {
            { 1000, "M" },
            { 900, "CM" },
            { 500, "D" },
            { 400, "CD" },
            { 100, "C" },
            { 90, "XC" },
            { 50, "L" },
            { 40, "XL" },
            { 10, "X" },
            { 9, "IX" },
            { 5, "V" },
            { 4, "IV" },
            { 1, "I" }
        };
        var v = Value;
        StringBuilder sb = new();
        foreach (var part in parts)
            while (v >= part.Key)
            {
                v -= part.Key;
                sb.Append(part.Value);
            }

        return sb.ToString();
    }

    public int ToInt()
    {
        return Value;
    }

    public short ToShort()
    {
        return (short)Value;
    }

    public ushort ToUnsignedShort()
    {
        return (ushort)Value;
    }

    public uint ToUnsignedInt()
    {
        return (uint)Value;
    }

    public float ToFloat()
    {
        return (float)Value;
    }

    public double ToDouble()
    {
        return (double)Value;
    }
}
using System.Text;

namespace _01_RomanParser;
public record RomanNumber(int Value)
{
    public RomanNumber(String input) : this(RomanNumberFactory.ParseAsInt(input))
    {}
    
    public override string ToString()
    {
        if (Value == 0) return "N";
        Dictionary<int, String> parts = new()
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
            { 1, "I" },
        };
        int v = Value;
        StringBuilder sb = new();
        foreach (var part in parts)
        {
            while (v >= part.Key)
            {
                v -= part.Key;
                sb.Append(part.Value);
            }
        }
        return sb.ToString();
    }
    public Int32 ToInt() => Value;
    public Int16 ToShort() => (Int16)Value;
    public UInt16 ToUnsignedShort() => (UInt16)Value;
    public UInt32 ToUnsignedInt() => (UInt32)Value;
    public Single ToFloat() => (Single)Value;
    public Double ToDouble() => (Double)Value;
}
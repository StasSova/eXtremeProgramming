using System.Text;

namespace _01_RomanParser;
public record RomanNumber(int Value)
{
    private readonly int _value = Value;  // TODO: Refactoring - exclude
    public int Value { get { return _value; } init => _value = value;  }

    public RomanNumber Plus(RomanNumber other)
    {
        return this with { Value = Value + other.Value };
    }

    public override string? ToString()
    {
        // 3343 -> MMMCCCXLIII
        // M M M
        // D (500) x
        // CD (400) x
        // C C C
        // L x
        // XL 
        // X x
        // V x
        // IV
        // III
        if (_value == 0) return "N";
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
        int v = _value;
        StringBuilder sb = new();
        foreach (var part in parts)
        {
            while(v >= part.Key)
            {
                v -= part.Key;
                sb.Append(part.Value);
            }
        }
        return sb.ToString();
    }
}
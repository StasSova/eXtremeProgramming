using System.Text;

namespace _01_RomanParser;

public record RomanNumber(int Value)
{
    private readonly int _value = Value;  // TODO: Refactoring - exclude
    public int Value { get { return _value; } init => _value = value;  }

    public static RomanNumber Parse(String input)
    {
        int value = 0;
        int prevDigit = 0;   // TODO: rename to ~rightDigit
        int pos = input.Length;
        int maxDigit = 0;
        bool wasLess = false;
        foreach (char c in input.Reverse())
        {
            pos -= 1;
            int digit;
            try
            {
                digit = DigitValue(c.ToString());
            }
            catch
            {
                throw new FormatException(
                    $"Invalid symbol '{c}' in position {pos}");
            }

            if(digit != 0 && prevDigit / digit > 10)
            {
                throw new FormatException(
                    $"Invalid order '{c}' before '{input[pos + 1]}' in position {pos}");
            }

            if(digit < maxDigit)   // if current digit is less than max
            {
                if (wasLess)       // if previously was the less digit
                {
                    throw new FormatException(input);
                }
                wasLess = true;
            }
            else
            {
                maxDigit = digit;
                wasLess = false;
            }

            if (digit >= prevDigit)
            {
                value += digit;
            }
            else
            {
                value -= digit;
            }
            prevDigit = digit;
        }
        return new(value);
    }

    private static int Method1() => 1;
    
    public static int DigitValue(String digit) => digit switch
    {
        "N" => 0,
        "I" => 1,
        "V" => 5,
        "X" => 10,
        "L" => 50,
        "C" => 100,
        "D" => 500,
        "M" => 1000,
        _ => throw new ArgumentException($"{nameof(RomanNumber)}::{nameof(DigitValue)}: 'digit' has invalid value '{digit}'")
    };

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
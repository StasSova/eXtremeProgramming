using System.Text;

namespace _01_RomanParser;

public record RomanNumber(int Value)
{
    private readonly int _value = Value; // TODO: Refactoring - exclude
    public int Value { get => _value; init => _value = value; }

    public static RomanNumber Parse(String input)
    {
        int value = 0;
        int prevDigit = 0;
        int pos = input.Length;
        List<String> errors = new();
        foreach (char c in input.Reverse())
        {
            pos -= 1;
            int digit;
            try
            {
                digit = DigitalValue(c.ToString());
            }
            catch
            {
                errors.Add($"Invalid symbol '{c}' in position {pos}");
                continue;
            }
            if (digit != 0 && prevDigit / digit > 10)
            {
                errors.Add($"Invalid order '{c}' before '{input[pos + 1]}' in position {pos}");
            }
            

            value += digit >= prevDigit ? digit : -digit;
            prevDigit = digit;
        }

        if (errors.Any())
        {
            throw new FormatException(string.Join("; ", errors));
        }

        return new RomanNumber(value);
    }

  
    public static int DigitalValue(String digit)
    {
        return digit switch
        {
            "N" => 0,
            "I" => 1,
            "V" => 5,
            "X" => 10,
            "L" => 50,
            "C" => 100,
            "D" => 500,
            "M" => 1000,
             _  => throw new ArgumentException($" {nameof(RomanNumber)} : {nameof(DigitalValue)}: 'digit' has invalid value '{digit}'")
        };

    }

    public RomanNumber Plus(RomanNumber other)
    {
        return this with { Value = Value + other.Value };
    }
    public RomanNumber Plus(string other)
    {
        return this with { Value = Value + Parse(other).Value };
    }
    public override string? ToString()
    {
        if(_value == 0 ) return "N";
        Dictionary<int, string> parts = new ()
        {
            {1000, "M" },
            {900, "CM" },
            {500, "D" },
            {400, "CD" },
            {100, "C" },
            {90, "XC" },
            {50, "L"},
            {40, "XL"},
            {10, "X"},
            {9, "IX"},
            {5, "V"},
            {4, "IV"},
            {1, "I"},
        };

        int v = _value;

        StringBuilder sb = new ();

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
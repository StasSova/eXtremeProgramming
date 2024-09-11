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

        _CheckValidity(input);

        foreach (char c in input.Reverse())
        {
            int digit = DigitValue(c.ToString());
            value += (digit >= prevDigit) ? digit : -digit;
            prevDigit = digit;
        }
        return new(value);
    }

    private static void _CheckValidity(string input)
    {
        _CheckSymbols(input);
        _CheckPairs(input);
        _CheckFormat(input);
        _CheckSubs(input);
    }

    private static void _CheckSubs(string input)
    {
        // IXIV XCXL  -- одна цифра може відніматись тільки один раз
        HashSet<char> subs = [];
        for (int i = 0; i < input.Length - 1; ++i)
        {
            char c = input[i];
            if(DigitValue(c.ToString()) < DigitValue(input[i + 1].ToString()))
            {
                if (subs.Contains(c))
                {
                    throw new FormatException();
                }
                subs.Add(c);
            }
        }
    }
    /* Д.З. Створити тести для _CheckSubs
     * Розширити тестові кейси для інших методів
     * _CheckSymbols,_CheckPairs,_CheckFormat,_CheckSubs
     */

    private static void _CheckFormat(string input)
    {
        int maxDigit = 0;
        bool wasLess = false;
        bool wasMax = false;
        foreach (char c in input.Reverse())
        {   // IIX   IXIX  IXX
            int digit = DigitValue(c.ToString());
            if (digit < maxDigit)   // if current digit is less than max
            {
                if (wasLess || wasMax)        // if previously was the less digit
                {
                    throw new FormatException(input);
                }
                // if (wasMax)
                // {
                //     throw new FormatException(input);
                // }
                wasLess = true;
            }
            else if (digit == maxDigit)
            {
                wasMax = true;
                wasLess = false;
            }
            else
            {
                maxDigit = digit;
                wasLess = false;
                wasMax = false;
            }
        }
    }

    private static void _CheckPairs(string input)
    {
        for (int i = 0; i < input.Length - 1; ++i)
        {
            int rightDigit = DigitValue(input[i + 1].ToString());
            int leftDigit = DigitValue(input[i].ToString());
            if (leftDigit != 0 && 
                leftDigit < rightDigit &&
                ( rightDigit / leftDigit > 10 ||  // IC IM
                    (leftDigit == 5 || leftDigit == 50 || leftDigit == 500)   // VX
                ))
            {
                throw new FormatException(
                    $"Invalid order '{input[i]}' before '{input[i + 1]}' in position {i}");
            }
        }
    }

    private static void _CheckSymbols(string input)
    {
        int pos = 0;
        foreach (char c in input)
        {
            try
            {
                DigitValue(c.ToString());
            }
            catch
            {
                throw new FormatException(
                    $"Invalid symbol '{c}' in position {pos}");
            }
            pos += 1;
        }
    }

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
    /* Д.З. Скласти тести, що перевіряють роботу методу Plus
     * з використанням римських записів чисел, наприклад,
     * IV + VI = X
     */

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
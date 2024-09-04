namespace _01_RomanParser;

public record RomanNumber(int Value)
{
    public int Value { get; } = Value;

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
            _ => throw new ArgumentException($" {nameof(RomanNumber)} : {nameof(DigitalValue)}'digit' has invalid value '{digit}'")
        };

    }

}
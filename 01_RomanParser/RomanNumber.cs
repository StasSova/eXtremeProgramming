namespace _01_RomanParser;

public record RomanNumber(int Value)
{
    public int Value { get; } = Value;

    public static RomanNumber Parse(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            throw new ArgumentException("Input cannot be null or empty.");

        int totalValue = 0;
        int previousValue = 0;
        int repeatCount = 0;

        for (int i = input.Length - 1; i >= 0; i--)
        {
            char currentChar = input[i];

            if (!RomanDigitValues.TryGetValue(currentChar, out int currentValue))
                throw new ArgumentException($"Invalid Roman numeral character: {currentChar}");

            if (i < input.Length - 1 && input[i] == input[i + 1])
                repeatCount++;
            else
                repeatCount = 1;

            if (repeatCount > 4)
                throw new ArgumentException($"Invalid repetition of Roman numeral character: {currentChar}");

            if (currentValue < previousValue)
                totalValue -= currentValue;
            else
                totalValue += currentValue;

            previousValue = currentValue;
        }

        return new RomanNumber(totalValue);
    }
    
    
    private static string ConvertToRoman(int number)
    {
        var romanNumerals = new[]
        {
            new { Value = 1000, Numeral = "M" },
            new { Value = 900, Numeral = "CM" },
            new { Value = 500, Numeral = "D" },
            new { Value = 400, Numeral = "CD" },
            new { Value = 100, Numeral = "C" },
            new { Value = 90, Numeral = "XC" },
            new { Value = 50, Numeral = "L" },
            new { Value = 40, Numeral = "XL" },
            new { Value = 10, Numeral = "X" },
            new { Value = 9, Numeral = "IX" },
            new { Value = 5, Numeral = "V" },
            new { Value = 4, Numeral = "IV" },
            new { Value = 1, Numeral = "I" }
        };

        var result = string.Empty;
        foreach (var roman in romanNumerals)
        {
            while (number >= roman.Value)
            {
                result += roman.Numeral;
                number -= roman.Value;
            }
        }

        return result;
    }

    
    public static int DigitValue(char digit)
    {
        if (!RomanDigitValues.ContainsKey(digit))
            throw new ArgumentException($"Invalid Roman numeral character: {digit}");

        return RomanDigitValues[digit];
    }
    
    private static readonly Dictionary<char, int> RomanDigitValues = new()
    {
        {'I', 1}, {'V', 5}, {'X', 10}, {'L', 50}, {'C', 100}, {'D', 500}, {'M', 1000}
    };

}
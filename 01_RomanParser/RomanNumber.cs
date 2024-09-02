namespace _01_RomanParser;

public record RomanNumber(int Value)
{
    public int Value { get; } = Value;

    public static RomanNumber Parse(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            throw new ArgumentException("Input cannot be null or empty.");
        }

        input = input.Trim().ToUpper();

        return input switch
        {
            "I" => new RomanNumber(1),
            "V" => new RomanNumber(5),
            "X" => new RomanNumber(10),
            "L" => new RomanNumber(50),
            "C" => new RomanNumber(100),
            "D" => new RomanNumber(500),
            "M" => new RomanNumber(1000),
            _ => throw new ArgumentException("Invalid Roman numeral.")
        };
    }
}
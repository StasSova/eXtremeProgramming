namespace _01_RomanParser.tests;

[TestClass]
public class RomanNumberTest
{
    [TestMethod]
    public void ParseTest()
    {
        var testCases = new Dictionary<string, int>()
    {
        { "N",      0 },
        { "I",      1 },
        { "II",     2 },
        { "III",    3 },
        { "IV",     4 },
        { "V",      5 },
        { "VI",     6 },
        { "VII",    7 },
        { "VIII",   8 },
        { "D",      500 },
        { "CM",     900 },
        { "M",      1000 },
        { "MC",     1100 },
        { "MCM",    1900 },
        { "MM",     2000 },
    };

        foreach (var testCase in testCases)
        {
            RomanNumber rn = RomanNumber.Parse(testCase.Key);
            Assert.IsNotNull(rn);
            Assert.AreEqual(testCase.Value
                ,
                rn.Value,
                $"{testCase.Key} parsing failed. Expected {testCase.Value}, got {rn.Value}."
            );
        }

        Dictionary<string, (char, int)[]> exTestCases = new()
        {
            {"W", new[] {('W', 0)}},
            {"Q", new[] {('Q', 0)}},
            {"s", new[] {('s', 0)}},
            {"Xd", new[] {('d', 1)}},
            {"SWXF", new[] {('S', 0), ('W', 1), ('F', 3)}},
            {"XXFX", new[] {('F', 2)}},
            {"VVVFX", new[] {('F', 3)}},
            {"IVF", new[] {('F', 2)}},
        };

        foreach (var testCase in exTestCases)
        {
            var ex = Assert.ThrowsException<FormatException>(
                () => RomanNumber.Parse(testCase.Key),
                $"{nameof(FormatException)} Parse '{testCase.Key}' must throw");

            foreach (var (symbol, position) in testCase.Value)
            {
                Assert.IsTrue(ex.Message.Contains($"Invalid symbol '{symbol}' in position {position}"),
                    $"{nameof(FormatException)} must contain data about symbol '{symbol}' at position {position}. " +
                    $"TestCase: '{testCase.Key}', ex.Message: '{ex.Message}'");
            }
        }

        Dictionary<String, Object[]> exTestCases2 = new()
        {
            { "IM",  ['I', 'M', 0] },
            { "XIM", ['I', 'M', 1] },
            { "IMX", ['I', 'M', 0] },
            { "XMD", ['X', 'M', 0] },
            { "XID", ['I', 'D', 1] },
            { "ID", ['I', 'D', 0] },
            { "XM", ['X', 'M', 0] },


        };
        foreach (var testCase in exTestCases2)
        {
            var ex = Assert.ThrowsException<FormatException>(
                () => RomanNumber.Parse(testCase.Key),
                $"Parse '{testCase.Key}' must throw FormatException"
            );
            Assert.IsTrue(
                ex.Message.Contains(
                    $"Invalid order '{testCase.Value[0]}' before '{testCase.Value[1]}' in position {testCase.Value[2]}"
                ),
                "FormatException must contain data about mis-ordered symbols and its position"
                + $"testCase: '{testCase.Key}', ex.Message: '{ex.Message}'"
            );
        }
    }

    Dictionary<int, String> _digitValues = new ();

    [TestMethod]
    public void DigitalValueTest()
    {
        var romanToInt = new Dictionary<string, int>()
    {
        { "N", 0 },
        { "I", 1 },
        { "V", 5 },
        { "X", 10 },
        { "L", 50 },
        { "C", 100 },
        { "D", 500 },
        { "M", 1000 },
    };

        foreach (var kvp in romanToInt)
        {

            Assert.AreEqual(
                kvp.Value,
                RomanNumber.DigitalValue(kvp.Key),
                $"{kvp.Value} parsing failed. Expected {kvp}, got {kvp.Value}."
            );
        }


        var random = new Random();
        for (int i = 0; i < 100; i++)
        {
            String invalidDigit = ((char)random.Next(256)).ToString();


            if (romanToInt.ContainsKey(invalidDigit))
            {
                i--;
                continue;
            }


            ArgumentException ex = Assert.ThrowsException<ArgumentException>(
            () => RomanNumber.DigitalValue(invalidDigit),
            $"ArgumentException erxpected for digit = '{invalidDigit}'"
             );

            Assert.IsFalse(
                String.IsNullOrEmpty(ex.Message),
                "ArgumentException must have a message"
                );
            Assert.IsTrue(
                ex.Message.Contains($"'digit' has invalid value '{invalidDigit}'"),
                $"ArgumentException message must contain <'digit' has invalid value '{invalidDigit}'>"
                );
            Assert.IsTrue(
                ex.Message.Contains(nameof(RomanNumber)) &&
                ex.Message.Contains(nameof(RomanNumber.DigitalValue)),
                $"ArgumentException message must contain '{nameof(RomanNumber)}' and '{nameof(RomanNumber.DigitalValue)}'"
                );
        }
    }

    [TestMethod]
    public void ToStringTest()
    {
        Dictionary<int, string> testCases = new()
        {
            { 1, "I" },
            { 2, "II"},
            { 3343, "MMMCCCXLIII" },
            { 4, "IV" },
            { 44, "XLIV" },
            { 9, "IX" },
            { 90, "XC" },
            { 1400, "MCD" },
            { 900, "CM" },
            { 990, "CMXC" },
        };
        _digitValues.Keys.ToList().ForEach(k => testCases.Add(k, _digitValues[k]));
        foreach (var testCase in testCases)
        {
            Assert.AreEqual(
                new RomanNumber(testCase.Key).ToString(),
                testCase.Value,
                $"ToString({testCase.Key})--> {testCase.Value}");
        }
    }


    [TestMethod]
    public void PlusTest()
    {
        RomanNumber rn1 = new(1);
        RomanNumber rn2 = new(2);
        RomanNumber rn3 = rn1.Plus(rn2);
        Assert.IsNotNull(rn3);
        Assert.IsInstanceOfType(rn3, typeof(RomanNumber),
            "Plus result must have RomanNumber type");
        Assert.AreNotSame(rn3, rn1, "Plus result is new instance, neither (v)first, nor second arg");
        Assert.AreNotSame(rn3, rn2, "Plus result is new instance, neither first, nor (v)second arg");
        Assert.AreEqual(rn1.Value + rn2.Value, rn3.Value, "Plus arithmetic");
        RomanNumber rn12 = RomanNumber.Parse("IV");
        String rn22 = "VI";
        RomanNumber rn32 = rn12.Plus(rn22);

        Assert.IsNotNull(rn32);
        Assert.AreNotSame(rn32, rn12, "String result is new instance");
        Assert.AreEqual("X", rn32.ToString(), "Plus X arithmetic");
    }
}
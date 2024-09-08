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
            {"W", [('W', 0)] },
            {"Q", [('Q', 0)] },
            {"s", [('s', 0)] },
            {"Xd", [('d', 1)] },
            {"SWXF", [('S', 0), ('W', 1), ('F', 3)] },
            {"XXFX", [('F', 2)] },
            {"VVVFX", [('F', 3)] },
            {"IVF", [('F', 2)] },
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

        Dictionary<string, (char, int)[]> exTestCases3 = new()
        {
          
            { "XVV", [('V', 2)] },
            { "LL", [('L', 1)] },
            { "LC", [('C', 1)] },
            { "VX", [('X', 1)] },
            { "MM", [('M', 1)] },

        };

        foreach (var testCase in exTestCases3)
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

    }

    Dictionary<int, String> _digitValues = new Dictionary<int, String>();
   


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


        Random random = new Random();
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
        Dictionary<int, string> testCases = new Dictionary<int, string>()
        {
            { 1, "I" },
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
}

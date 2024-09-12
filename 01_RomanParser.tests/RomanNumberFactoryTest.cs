using System.Collections.ObjectModel;
using System.Reflection;

namespace _01_RomanParser.tests;

[TestClass]
public class RomanNumberFactoryTest
{
    public static ReadOnlyDictionary<string, int> DigitValues => new(new Dictionary<String, int>
    {
        { "N", 0    },
        { "I", 1    },
        { "V", 5    },
        { "X", 10   },
        { "L", 50   },
        { "C", 100  },
        { "D", 500  },
        { "M", 1000 },
    });
    


    [TestMethod]
    public void _CheckSymbolsTest()
    {
        Type? rnType = typeof(RomanNumberFactory);
        MethodInfo? m1Info = rnType.GetMethod("_CheckSymbols", 
            BindingFlags.NonPublic | BindingFlags.Static);

        m1Info?.Invoke(null, ["IX"]);

        var ex = Assert.ThrowsException<TargetInvocationException>(
        () => m1Info?.Invoke(null, ["IW"]),
            $"_CheckSymbols 'IW' must throw FormatException"
        );
        Assert.IsInstanceOfType<FormatException>(
            ex.InnerException,
            "FormatException from InnerException"
        );
    }

    [TestMethod]
    public void _CheckPairsTest()
    {
        Type? rnType = typeof(RomanNumberFactory);
        MethodInfo? m1Info = rnType.GetMethod("_CheckPairs",
            BindingFlags.NonPublic | BindingFlags.Static);

        // Assert Not Throws
        m1Info?.Invoke(null, ["IX"]);
        
        var ex = Assert.ThrowsException<TargetInvocationException>(
        () => m1Info?.Invoke(null, ["IM"]),
            $"_CheckPairs 'IM' must throw FormatException"
        );
        Assert.IsInstanceOfType<FormatException>(
            ex.InnerException,
            "FormatException from InnerException"
        );
    }

    [TestMethod]
    public void _CheckFormatTest()
    {
        Type? rnType = typeof(RomanNumberFactory);
        MethodInfo? m1Info = rnType.GetMethod("_CheckFormat",
            BindingFlags.NonPublic | BindingFlags.Static);

        // Assert Not Throws
        m1Info?.Invoke(null, ["IX"]);

        var ex = Assert.ThrowsException<TargetInvocationException>(
        () => m1Info?.Invoke(null, ["IIX"]),
            $"_CheckFormat 'IIX' must throw FormatException"
        );
        Assert.IsInstanceOfType<FormatException>(
            ex.InnerException,
            "_CheckFormat: FormatException from InnerException"
        );
    }

    [TestMethod]
    public void _CheckValidityTest()
    {
        Type? rnType = typeof(RomanNumberFactory);
        MethodInfo? m1Info = rnType.GetMethod("_CheckValidity",
            BindingFlags.NonPublic | BindingFlags.Static);

        // Assert Not Throws
        m1Info?.Invoke(null, ["IX"]);

        String[] testCases = ["IXIX", "IXX", "IVIV", "XCC", "IXIV", "XCXL", "CMCD"];
        foreach (var testCase in testCases)
        {
            var ex = Assert.ThrowsException<TargetInvocationException>(
            () => m1Info?.Invoke(null, [testCase]),
                $"_CheckValidity '{testCase}' must throw FormatException"
            );
            Assert.IsInstanceOfType<FormatException>(
                ex.InnerException,
                "_CheckValidity: FormatException from InnerException"
            );
        }
    }

    [TestMethod]
    public void ParseTest()
    {
        Dictionary<String, int> testCases = new()
        {
            { "N",    0 },
            { "I",    1 },
            { "II",   2 },
            { "III",  3 },
            { "IIII", 4 },   // öèì òåñòîì ìè äîçâîëÿºìî íåîïòèìàëüíó ôîðìó ÷èñëà
            { "IV",   4 },
            { "VI",   6 },
            { "VII",  7 },
            { "VIII", 8 },
            { "IX",   9 },
            { "D",    500 },
            { "M",    1000 },
            { "CM",   900 },
            { "MC",   1100 },
            { "MCM",  1900 },
            { "MM",   2000 },
        };
        foreach (var testCase in testCases)
        {
            RomanNumber rn = RomanNumberFactory.Parse(testCase.Key);
            Assert.IsNotNull(rn);
            Assert.AreEqual(
                testCase.Value, 
                rn.Value, 
                $"{testCase.Key} -> {testCase.Value}"
            );
        }
        Dictionary<String, Object[]> exTestCases = new()
        {
            { "W", ['W', 0] },
            { "Q", ['Q', 0] },
            { "s", ['s', 0] },
            { "sX", ['s', 0] },
            { "Xd", ['d', 1] },
        };
        foreach (var testCase in exTestCases)
        {
            var ex = Assert.ThrowsException<FormatException>(
                () => RomanNumberFactory.Parse(testCase.Key),
                $"Parse '{testCase.Key}' must throw FormatException"
            );
            Assert.IsTrue(
                ex.Message.Contains(
                    $"Invalid symbol '{testCase.Value[0]}' in position {testCase.Value[1]}"
                ),
                "FormatException must contain data about symbol and its position"
                + $"testCase: '{testCase.Key}', ex.Message: '{ex.Message}'"
            );
        }
        Dictionary<String, Object[]> exTestCases2 = new()
        {
            { "IM",  ['I', 'M', 0] },
            { "XIM", ['I', 'M', 1] },
            { "IMX", ['I', 'M', 0] },
            { "XMD", ['X', 'M', 0] },
            { "XID", ['I', 'D', 1] },
            { "VX",  ['V', 'X', 0] },
            { "VL",  ['V', 'L', 0] },
            { "LC",  ['L', 'C', 0] },
            { "DM",  ['D', 'M', 0] },
        };
        foreach (var testCase in exTestCases2)
        {
            var ex = Assert.ThrowsException<FormatException>(
                () => RomanNumberFactory.Parse(testCase.Key),
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

        String[] exTestCases3 =
        {
            "IXC", "IIX", "VIX",
            "CIIX", "IIIX", "VIIX",
            "VIXC", "IVIX", "CVIIX",  // XIX+ CIX+ IIX- VIX-
            "CIXC", "IXCM", "IXXC",
        };
        foreach (var testCase in exTestCases3)
        {
            var ex = Assert.ThrowsException<FormatException>(
                () => RomanNumberFactory.Parse(testCase),
                $"Parse '{testCase}' must throw FormatException"
            );
        }
    }

    [TestMethod]
    public void DigitValueTest()
    {           
        foreach (var testCase in DigitValues)
        {
            Assert.AreEqual(
                testCase.Value, 
                RomanNumberFactory.DigitValue(testCase.Key),
                $"{testCase.Key} -> {testCase.Value}"
            );
        }
        Random random = new();
        for (int i = 0; i < 100; ++i)
        {
            String invalidDigit = ((char) random.Next(256)).ToString();
            if(DigitValues.ContainsKey(invalidDigit))
            {
                --i;
                continue;
            }
            ArgumentException ex =
                Assert.ThrowsException<ArgumentException>(
                    () => RomanNumberFactory.DigitValue(invalidDigit),
                    $"ArgumentException expected for digit = '{invalidDigit}'"
                );
          
            Assert.IsFalse(
                String.IsNullOrEmpty(ex.Message),
                "ArgumentException must have a message"
            );
            Assert.IsTrue(
                ex.Message.Contains($"'digit' has invalid value '{invalidDigit}'"),
                "ArgumentException message must contain <'digit' has invalid value ''>"
            );
            Assert.IsTrue(
                ex.Message.Contains(nameof(RomanNumberFactory)) &&
                ex.Message.Contains(nameof(RomanNumberFactory.DigitValue)),
                $"ArgumentException message must contain '{nameof(RomanNumberFactory)}' and '{nameof(RomanNumberFactory.DigitValue)}' "
            );
        }
    }
    
}
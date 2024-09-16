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
        Type rnType = typeof(RomanNumberFactory);
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
        Type rnType = typeof(RomanNumberFactory);
        MethodInfo? m1Info = rnType.GetMethod("_CheckPairs",
            BindingFlags.NonPublic | BindingFlags.Static);

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
        Type rnType = typeof(RomanNumberFactory);
        MethodInfo? m1Info = rnType.GetMethod("_CheckFormat",
            BindingFlags.NonPublic | BindingFlags.Static);

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
        Type rnType = typeof(RomanNumberFactory);
        MethodInfo? m1Info = rnType.GetMethod("_CheckValidity",
            BindingFlags.NonPublic | BindingFlags.Static);

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

        var assertThrowsExceptionMethods = typeof(Assert).GetMethods()
        .Where(x => x.Name == "ThrowsException")
        .Where(x => x.IsGenericMethod);

        var assertThrowsExceptionFuncStringMethod = assertThrowsExceptionMethods.Skip(3).FirstOrDefault();

        TestCase[] testCases1 = [
            new() { Source = "N",    Value = 0 },
            new() { Source = "I",    Value = 1 },
            new() { Source = "II",   Value = 2 },
            new() { Source = "III",  Value = 3 },
            new() { Source = "IIII", Value = 4 },
            new() { Source = "IV",   Value = 4 },
            new() { Source = "VI",   Value = 6 },
            new() { Source = "VII",  Value = 7 },
            new() { Source = "VIII", Value = 8 },
            new() { Source = "IX",   Value = 9 },
            new() { Source = "D",    Value = 500 },
            new() { Source = "M",    Value = 1000 },
            new() { Source = "CM",   Value = 900 },
            new() { Source = "MC",   Value = 1100 },
            new() { Source = "MCM",  Value = 1900 },
            new() { Source = "MM",   Value = 2000 },
        ];

        foreach (var testCase in testCases1)
        {
            RomanNumber rn = RomanNumberFactory.Parse(testCase.Source);
            Assert.IsNotNull(rn);
            Assert.AreEqual(
                testCase.Value,
                rn.Value,
                $"{testCase.Source} -> {testCase.Value}"
            );
        }

        var formatExceptionType = typeof(FormatException);
        String partTemplate = "Invalid symbol '{0}' in position {1}";
        TestCase[] exTestCases1 = [
            new() { Source = "W",   ExceptionMessageParts = [String.Format(partTemplate, "W",  0)], ExceptionType = formatExceptionType},
            new() { Source = "Q",   ExceptionMessageParts = [String.Format(partTemplate, "Q",  0)], ExceptionType = formatExceptionType},
            new() { Source = "s",   ExceptionMessageParts = [String.Format(partTemplate, "s",  0)], ExceptionType = formatExceptionType},
            new() { Source = "sX",  ExceptionMessageParts = [String.Format(partTemplate, "s",  0)], ExceptionType = formatExceptionType},
            new() { Source = "Xd",  ExceptionMessageParts = [String.Format(partTemplate, "d",  1)], ExceptionType = formatExceptionType},

        ];

        foreach (var testCase in exTestCases1)
        {


            dynamic? ex = assertThrowsExceptionFuncStringMethod?
                .MakeGenericMethod(testCase.ExceptionType!)
                .Invoke(null,
                [() => RomanNumberFactory.Parse(testCase.Source),
                $"Parse '{testCase.Source}' must throw FormatException"]);

            Assert.IsTrue(
                ex!.Message.Contains(
                    testCase.ExceptionMessageParts!.First()
                    ),
                $"FormatException must contain '{testCase.ExceptionMessageParts!.First()}'"
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
            "VIXC", "IVIX", "CVIIX",  
            "CIXC", "IXCM", "IXXC",
        };
        foreach (var testCase in exTestCases3)
        {
            var throwsException = Assert.ThrowsException<FormatException>(
                () => RomanNumberFactory.Parse(testCase),
                $"Parse '{testCase}' must throw FormatException"
            );
        }


        String[] exTestCases4 =
        {
            "IXIX", "IXX", "IXIV",
            "XCXC", "CMM", "CMCD",
            "XCXL", "XCC", "XCCI",
        };

        foreach (var testCase in exTestCases4)
        {
            var ex = Assert.ThrowsException<FormatException>(
                () => RomanNumberFactory.Parse(testCase),
                $"Parse '{testCase}' must throw FormatException"
            );
            Assert.IsTrue(ex.Message.Contains("Invalid"),
                $"Exception message for '{testCase}' must contain 'Invalid'");
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
            String invalidDigit = ((char)random.Next(256)).ToString();
            if (DigitValues.ContainsKey(invalidDigit))
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
    class TestCase
    {
        public String Source { get; set; }
        public int? Value { get; set; }
        public Type? ExceptionType { get; set; }
        public IEnumerable<String>? ExceptionMessageParts { get; set; }

    }
}
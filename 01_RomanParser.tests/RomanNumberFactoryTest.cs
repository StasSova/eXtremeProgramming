using System.Collections.ObjectModel;
using System.Reflection;

namespace _01_RomanParser.tests;

[TestClass]
public class RomanNumberFactoryTest
{
    public static ReadOnlyDictionary<string, int> DigitValues => new(new Dictionary<string, int>
    {
        { "N", 0 },
        { "I", 1 },
        { "V", 5 },
        { "X", 10 },
        { "L", 50 },
        { "C", 100 },
        { "D", 500 },
        { "M", 1000 }
    });


    [TestMethod]
    public void _CheckSymbolsTest()
    {
        string[] testCases = ["IW"];
        CheckPrivateMethod("_CheckSymbols", testCases);
    }

    [TestMethod]
    public void _CheckPairsTest()
    {
        string[] testCases = ["IM"];
        CheckPrivateMethod("_CheckPairs", testCases);
    }

    [TestMethod]
    public void _CheckFormatTest()
    {
        string[] testCases = ["IIX"];
        CheckPrivateMethod("_CheckFormat", testCases);
    }

    [TestMethod]
    public void _CheckValidityTest()
    {
        string[] testCases = ["IXIX", "IXX", "IVIV", "XCC", "IXIV", "XCXL", "CMCD"];
        CheckPrivateMethod("_CheckValidity", testCases);
    }

    private void CheckPrivateMethod(string methodName, string[] testCases)
    {
        var rnType = typeof(RomanNumberFactory);
        var m1Info = rnType.GetMethod(methodName,
            BindingFlags.NonPublic | BindingFlags.Static);

        foreach (var testCase in testCases)
        {
            var ex = Assert.ThrowsException<TargetInvocationException>(
                () => m1Info?.Invoke(null, [testCase]),
                $"{methodName} '{testCase}' must throw FormatException"
            );
            Assert.IsInstanceOfType<FormatException>(
                ex.InnerException,
                $"{methodName}: FormatException from InnerException"
            );
        }
    }

    [TestMethod]
    public void ParseTest()
    {
        var assertThrowsExceptionMethods =
            typeof(Assert).GetMethods()
                .Where(x => x.Name == "ThrowsException")
                .Where(x => x.IsGenericMethod);

        var assertThrowsExceptionFuncStringMethod =
            assertThrowsExceptionMethods
                .Skip(3)
                .FirstOrDefault();

        var ex1Template = "Invalid symbol '{0}' in position {1}";
        var exSrcTemplate = "Parse('{0}')";
        var ex2Template = "Invalid order '{0}' before '{1}' in position {2}";
        var formatExceptionType = typeof(FormatException);

        TestCase[] testCases =
        [
            new TestCase { Source = "N", Value = 0 },
            new TestCase { Source = "I", Value = 1 },
            new TestCase { Source = "II", Value = 2 },
            new TestCase { Source = "III", Value = 3 },
            new TestCase { Source = "IIII", Value = 4 },
            new TestCase { Source = "IV", Value = 4 },
            new TestCase { Source = "VI", Value = 6 },
            new TestCase { Source = "VII", Value = 7 },
            new TestCase { Source = "VIII", Value = 8 },
            new TestCase { Source = "IX", Value = 9 },
            new TestCase { Source = "D", Value = 500 },
            new TestCase { Source = "M", Value = 1000 },
            new TestCase { Source = "CM", Value = 900 },
            new TestCase { Source = "MC", Value = 1100 },
            new TestCase { Source = "MCM", Value = 1900 },
            new TestCase { Source = "MM", Value = 2000 },

            new TestCase
            {
                Source = "W",
                ExceptionMessageParts = [string.Format(exSrcTemplate, "W"), string.Format(ex1Template, 'W', 0)],
                ExceptionType = formatExceptionType
            },
            new TestCase
            {
                Source = "Q",
                ExceptionMessageParts = [string.Format(exSrcTemplate, "Q"), string.Format(ex1Template, 'Q', 0)],
                ExceptionType = formatExceptionType
            },
            new TestCase
            {
                Source = "s",
                ExceptionMessageParts = [string.Format(exSrcTemplate, "s"), string.Format(ex1Template, 's', 0)],
                ExceptionType = formatExceptionType
            },
            new TestCase
            {
                Source = "sX",
                ExceptionMessageParts = [string.Format(exSrcTemplate, "sX"), string.Format(ex1Template, 's', 0)],
                ExceptionType = formatExceptionType
            },
            new TestCase
            {
                Source = "Xd",
                ExceptionMessageParts = [string.Format(exSrcTemplate, "Xd"), string.Format(ex1Template, 'd', 1)],
                ExceptionType = formatExceptionType
            },

            new TestCase
            {
                Source = "IM", ExceptionMessageParts = [string.Format(ex2Template, 'I', 'M', 0)],
                ExceptionType = formatExceptionType
            },
            new TestCase
            {
                Source = "XIM", ExceptionMessageParts = [string.Format(ex2Template, 'I', 'M', 1)],
                ExceptionType = formatExceptionType
            },
            new TestCase
            {
                Source = "IMX", ExceptionMessageParts = [string.Format(ex2Template, 'I', 'M', 0)],
                ExceptionType = formatExceptionType
            },
            new TestCase
            {
                Source = "XMD", ExceptionMessageParts = [string.Format(ex2Template, 'X', 'M', 0)],
                ExceptionType = formatExceptionType
            },
            new TestCase
            {
                Source = "XID", ExceptionMessageParts = [string.Format(ex2Template, 'I', 'D', 1)],
                ExceptionType = formatExceptionType
            },
            new TestCase
            {
                Source = "VX", ExceptionMessageParts = [string.Format(ex2Template, 'V', 'X', 0)],
                ExceptionType = formatExceptionType
            },
            new TestCase
            {
                Source = "VL", ExceptionMessageParts = [string.Format(ex2Template, 'V', 'L', 0)],
                ExceptionType = formatExceptionType
            },
            new TestCase
            {
                Source = "LC", ExceptionMessageParts = [string.Format(ex2Template, 'L', 'C', 0)],
                ExceptionType = formatExceptionType
            },
            new TestCase
            {
                Source = "DM", ExceptionMessageParts = [string.Format(ex2Template, 'D', 'M', 0)],
                ExceptionType = formatExceptionType
            },

            new TestCase { Source = "IXC", ExceptionMessageParts = [], ExceptionType = formatExceptionType },
            new TestCase { Source = "IIX", ExceptionMessageParts = [], ExceptionType = formatExceptionType },
            new TestCase { Source = "VIX", ExceptionMessageParts = [], ExceptionType = formatExceptionType },
            new TestCase { Source = "CIIX", ExceptionMessageParts = [], ExceptionType = formatExceptionType },
            new TestCase { Source = "IIIX", ExceptionMessageParts = [], ExceptionType = formatExceptionType },
            new TestCase { Source = "VIIX", ExceptionMessageParts = [], ExceptionType = formatExceptionType },
            new TestCase { Source = "IVIX", ExceptionMessageParts = [], ExceptionType = formatExceptionType },
            new TestCase { Source = "IXXC", ExceptionMessageParts = [], ExceptionType = formatExceptionType },
            new TestCase { Source = "IXCM", ExceptionMessageParts = [], ExceptionType = formatExceptionType },
            new TestCase { Source = "CVIIX", ExceptionMessageParts = [], ExceptionType = formatExceptionType },
            new TestCase { Source = "VIXC", ExceptionMessageParts = [], ExceptionType = formatExceptionType },
            new TestCase { Source = "CIXC", ExceptionMessageParts = [], ExceptionType = formatExceptionType },

            new TestCase
                { Source = "IXIX", ExceptionMessageParts = new[] { "Invalid" }, ExceptionType = formatExceptionType },
            new TestCase
                { Source = "IXX", ExceptionMessageParts = new[] { "Invalid" }, ExceptionType = formatExceptionType },
            new TestCase
                { Source = "IXIV", ExceptionMessageParts = new[] { "Invalid" }, ExceptionType = formatExceptionType },
            new TestCase
                { Source = "XCXC", ExceptionMessageParts = new[] { "Invalid" }, ExceptionType = formatExceptionType },
            new TestCase
                { Source = "CMM", ExceptionMessageParts = new[] { "Invalid" }, ExceptionType = formatExceptionType },
            new TestCase
                { Source = "CMCD", ExceptionMessageParts = new[] { "Invalid" }, ExceptionType = formatExceptionType },
            new TestCase
                { Source = "XCXL", ExceptionMessageParts = new[] { "Invalid" }, ExceptionType = formatExceptionType },
            new TestCase
                { Source = "XCC", ExceptionMessageParts = new[] { "Invalid" }, ExceptionType = formatExceptionType },
            new TestCase
                { Source = "XCCI", ExceptionMessageParts = new[] { "Invalid" }, ExceptionType = formatExceptionType }
        ];

        foreach (var testCase in testCases)
            if (testCase.Value is not null)
            {
                var rn = RomanNumberFactory.Parse(testCase.Source);
                Assert.IsNotNull(rn);
                Assert.AreEqual(
                    testCase.Value,
                    rn.Value,
                    $"{testCase.Source} -> {testCase.Value}"
                );
            }
            else
            {
                dynamic? ex = assertThrowsExceptionFuncStringMethod?
                    .MakeGenericMethod(testCase.ExceptionType!)
                    .Invoke(null,
                    [
                        () => RomanNumberFactory.Parse(testCase.Source),
                        $"Parse('{testCase.Source}') must throw FormatException"
                    ]);

                foreach (var exPart in testCase.ExceptionMessageParts ?? [])
                    Assert.IsTrue(
                        ex!.Message.Contains(exPart),
                        $"Parse('{testCase.Source}') FormatException must contain '{exPart}'"
                    );
            }
    }


    [TestMethod]
    public void DigitValueTest()
    {
        foreach (var testCase in DigitValues)
            Assert.AreEqual(
                testCase.Value,
                RomanNumberFactory.DigitValue(testCase.Key),
                $"{testCase.Key} -> {testCase.Value}"
            );
        Random random = new();
        for (var i = 0; i < 100; ++i)
        {
            var invalidDigit = ((char)random.Next(256)).ToString();
            if (DigitValues.ContainsKey(invalidDigit))
            {
                --i;
                continue;
            }

            var ex =
                Assert.ThrowsException<ArgumentException>(
                    () => RomanNumberFactory.DigitValue(invalidDigit),
                    $"ArgumentException expected for digit = '{invalidDigit}'"
                );

            Assert.IsFalse(
                string.IsNullOrEmpty(ex.Message),
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

    private class TestCase
    {
        public string Source { get; set; }
        public int? Value { get; set; }
        public Type? ExceptionType { get; set; }
        public IEnumerable<string>? ExceptionMessageParts { get; set; }
    }
}
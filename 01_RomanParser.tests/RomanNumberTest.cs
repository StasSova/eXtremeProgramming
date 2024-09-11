using System.Reflection;

namespace _01_RomanParser.tests;

[TestClass]
public class RomanNumberTest
{
    private readonly Dictionary<String, int> _digitValues = new()
    {
        { "N", 0    },
        { "I", 1    },
        { "V", 5    },
        { "X", 10   },
        { "L", 50   },
        { "C", 100  },
        { "D", 500  },
        { "M", 1000 },
    };

    [TestMethod]
    public void _CheckSymbolsTest()
    {
        Type? rnType = typeof(RomanNumber);
        MethodInfo? m1Info = rnType.GetMethod("_CheckSymbols", 
            BindingFlags.NonPublic | BindingFlags.Static);

        // Assert Not Throws
        m1Info?.Invoke(null, ["IX"]);

        var ex = Assert.ThrowsException<TargetInvocationException>(
        () => m1Info?.Invoke(null, ["IW"]),
            "_CheckSymbols 'IW' must throw FormatException"
        );
        Assert.IsInstanceOfType(
            ex.InnerException,
            typeof(FormatException),
            "FormatException from InnerException"
        );
    }

    [TestMethod]
    public void _CheckPairsTest()
    {
        Type? rnType = typeof(RomanNumber);
        MethodInfo? m1Info = rnType.GetMethod("_CheckPairs",
            BindingFlags.NonPublic | BindingFlags.Static);

        // Assert Not Throws
        m1Info?.Invoke(null, ["IX"]);
        
        var ex = Assert.ThrowsException<TargetInvocationException>(
        () => m1Info?.Invoke(null, ["IM"]),
            "_CheckPairs 'IM' must throw FormatException"
        );
        Assert.IsInstanceOfType(
            ex.InnerException,
            typeof(FormatException),
            "FormatException from InnerException"
        );
    }

    [TestMethod]
    public void _CheckFormatTest()
    {
        Type? rnType = typeof(RomanNumber);
        MethodInfo? m1Info = rnType.GetMethod("_CheckFormat",
            BindingFlags.NonPublic | BindingFlags.Static);

        // Assert Not Throws
        m1Info?.Invoke(null, ["IX"]);

        var ex = Assert.ThrowsException<TargetInvocationException>(
        () => m1Info?.Invoke(null, ["IIX"]),
            "_CheckFormat 'IIX' must throw FormatException"
        );
        
        Assert.IsInstanceOfType(
            ex.InnerException,
            typeof(FormatException),
            "_CheckFormat: FormatException from InnerException"
        );
    }

    [TestMethod]
    public void _CheckValidityTest()
    {
        Type? rnType = typeof(RomanNumber);
        MethodInfo? m1Info = rnType.GetMethod("_CheckValidity",
            BindingFlags.NonPublic | BindingFlags.Static);

        // Assert Not Throws
        m1Info?.Invoke(null, ["IX"]);

        string[] testCases = ["IXIX", "IXX", "IVIV", "XCC", "IXIV", "XCXL", "CMCD"];
        foreach (var testCase in testCases)
        {
            var ex = Assert.ThrowsException<TargetInvocationException>(
                () => m1Info?.Invoke(null, [testCase]),
                "_CheckValidity '{testCase}' must throw FormatException"
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
            { "IIII", 4 },   
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
            RomanNumber rn = RomanNumber.Parse(testCase.Key);
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
                () => RomanNumber.Parse(testCase.Key),
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

        String[] exTestCases3 =
        [
            "IXC", "IIX", "VIX",
            "CIIX", "IIIX", "VIIX",
            "VIXC", "IVIX", "CVIIX",
            "CIXC", "IXCM", "IXXC"
        ];
        foreach (var testCase in exTestCases3)
        {
            var ex = Assert.ThrowsException<FormatException>(
                () => RomanNumber.Parse(testCase),
                $"Parse '{testCase}' must throw FormatException"
            );
            // Assert.IsTrue(
            //     ex.Message.Contains(nameof(RomanNumber)) &&
            //     ex.Message.Contains(nameof(RomanNumber.Parse)) &&
            //     ex.Message.Contains(
            //         $"invalid sequence: more than 1 less digit before '{testCase[^1]}'"),
            //     $"ex.Message must contain info about origin, cause and data. {ex.Message}"
            // );
        }
    }

    [TestMethod]
    public void DigitValueTest()
    {           
        foreach (var testCase in _digitValues)
        {
            Assert.AreEqual(
                testCase.Value, 
                RomanNumber.DigitValue(testCase.Key),
                $"{testCase.Key} -> {testCase.Value}"
            );
        }
        Random random = new();
        for (int i = 0; i < 100; ++i)
        {
            String invalidDigit = ((char) random.Next(256)).ToString();
            if(_digitValues.ContainsKey(invalidDigit))
            {
                --i;
                continue;
            }
            ArgumentException ex =
                Assert.ThrowsException<ArgumentException>(
                    () => RomanNumber.DigitValue(invalidDigit),
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
                ex.Message.Contains(nameof(RomanNumber)) &&
                ex.Message.Contains(nameof(RomanNumber.DigitValue)),
                $"ArgumentException message must contain '{nameof(RomanNumber)}' and '{nameof(RomanNumber.DigitValue)}' "
            );
        }
    }

    [TestMethod]
    public void ToStringTest()
    {
        Dictionary<int, String> testCases = new() {
            { 2, "II" },
            { 3343, "MMMCCCXLIII" },
            { 4, "IV" },
            { 44, "XLIV" },
            { 9, "IX" },
            { 90, "XC" },
            { 1400, "MCD" },
            { 999, "CMXCIX" },  
            { 444, "CDXLIV" },
            { 990, "CMXC" }, 
        };

        _digitValues.Keys.ToList().ForEach(k => testCases.Add(_digitValues[k], k));
        
        foreach (var testCase in testCases)
        {
            Assert.AreEqual(
                testCase.Value,
                new RomanNumber(testCase.Key).ToString(),
                $"ToString({testCase.Key}) --> {testCase.Value}"
            );
        }
    }


    [TestMethod]
    public void PlusTest()
    {
        RomanNumber rn1 = new(1);
        RomanNumber rn2 = new(2);
        var rn3 = rn1.Plus(rn2);
        Assert.IsNotNull(rn3);
        Assert.IsInstanceOfType(rn3, typeof(RomanNumber), 
            "Plus result mast have RomanNumber type");
        Assert.AreNotSame(rn3, rn1, 
            "Plus result is new instance, neither (v)first, nor second arg");
        Assert.AreNotSame(rn3, rn2, 
            "Plus result is new instance, neither first, nor (v)second arg");
        Assert.AreEqual(rn1.Value + rn2.Value, rn3.Value, 
            "Plus arithmetic");
    }
    
    
    private static MethodInfo? GetMethod(string methodName)
    {
        Type? rnType = typeof(RomanNumber);
        return rnType.GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Static);
    }
    
    [TestMethod]
    public void CheckSymbols_ValidSymbols_DoesNotThrow()
    {
        // Arrange
        var validInputs = new[]
        {
            "I", "V", "X", "L", "C", "D", "M", "IX", "XL", "XC", "CD", "CM"
        };
        MethodInfo? method = GetMethod("_CheckSymbols");

        // Act & Assert
        foreach (var input in validInputs)
        {
            try
            {
                method?.Invoke(null, [input]);
            }
            catch (TargetInvocationException ex)
            {
                Assert.Fail($"_CheckSymbols should not throw for valid input '{input}'. Exception: {ex.InnerException?.Message}");
            }
        }
    }

    [TestMethod]
    public void CheckSymbols_InvalidSymbols_ThrowsFormatException()
    {
        // Arrange
        var invalidInputs = new[]
        {
            "A", "G", "Z", "IK", "C5", "M1"
        };
        MethodInfo? method = GetMethod("_CheckSymbols");

        // Act & Assert
        foreach (var input in invalidInputs)
        {
            var ex = Assert.ThrowsException<TargetInvocationException>(() => 
                method?.Invoke(null, [input]));
            Assert.IsTrue(ex.InnerException is FormatException);
            Assert.IsTrue(ex.InnerException?.Message.Contains("Invalid symbol"));
        }
    }

    [TestMethod]
    public void CheckPairs_ValidPairs_DoesNotThrow()
    {
        // Arrange
        var validInputs = new[]
        {
            "IV", "IX", "XL", "XC", "CD", "CM"
        };
        MethodInfo? method = GetMethod("_CheckPairs");

        // Act & Assert
        foreach (var input in validInputs)
        {
            try
            {
                method?.Invoke(null, [input]);
            }
            catch (TargetInvocationException ex)
            {
                Assert.Fail($"_CheckPairs should not throw for valid input '{input}'. Exception: {ex.InnerException?.Message}");
            }
        }
    }

    [TestMethod]
    public void CheckPairs_InvalidPairs_ThrowsFormatException()
    {
        // Arrange
        var invalidInputs = new[]
        {
            "IC", "IM", "VX", "VL", "VC", "VM", "XD", "XM"
        };
        MethodInfo? method = GetMethod("_CheckPairs");

        // Act & Assert
        foreach (var input in invalidInputs)
        {
            var ex = Assert.ThrowsException<TargetInvocationException>(() => 
                method?.Invoke(null, [input]));
            Assert.IsTrue(ex.InnerException is FormatException);
            Assert.IsTrue(ex.InnerException?.Message.Contains("Invalid order"));
        }
    }

    [TestMethod]
    public void CheckFormat_ValidFormats_DoesNotThrow()
    {
        // Arrange
        var validInputs = new[]
        {
            "I", "IX", "XL", "XC", "CD", "CM", "MCMXC"
        };
        MethodInfo? method = GetMethod("_CheckFormat");

        // Act & Assert
        foreach (var input in validInputs)
        {
            try
            {
                method?.Invoke(null, [input]);
            }
            catch (TargetInvocationException ex)
            {
                Assert.Fail($"_CheckFormat should not throw for valid input '{input}'. Exception: {ex.InnerException?.Message}");
            }
        }
    }
    
    [TestMethod]
    public void CheckFormat_InvalidFormats_ThrowsFormatException()
    {
        // Arrange
        var invalidInputs = new[]
        {
            "IIX", "IXIX", "IXX", "VIX", "LXL", "DCD", "MMMM"
        };
        MethodInfo? method = GetMethod("_CheckFormat");

        // Act & Assert
        foreach (var input in invalidInputs)
        {
            var ex = Assert.ThrowsException<TargetInvocationException>(() => 
                method?.Invoke(null, new object[] { input }));
            Assert.IsTrue(ex.InnerException is FormatException, $"Expected FormatException for input '{input}'");
            Assert.IsTrue(ex.InnerException?.Message.Contains(input), $"Expected message to contain '{input}' for input '{input}'");
        }
    }

    [TestMethod]
    public void CheckSubs_ValidSubs_DoesNotThrow()
    {
        // Arrange
        var validInputs = new[]
        {
            "IX", "XC", "CD", "MCM", "XLV"
        };
        MethodInfo? method = GetMethod("_CheckSubs");

        // Act & Assert
        foreach (var input in validInputs)
        {
            try
            {
                method?.Invoke(null, [input]);
            }
            catch (TargetInvocationException ex)
            {
                Assert.Fail($"_CheckSubs should not throw for valid input '{input}'. Exception: {ex.InnerException?.Message}");
            }
        }
    }

    [TestMethod]
    public void CheckSubs_InvalidSubs_ThrowsFormatException()
    {
        // Arrange
        var invalidInputs = new[]
        {
            "IXIV", "XCXL", "CDCD", "IM", "XM"
        };
        MethodInfo? method = GetMethod("_CheckSubs");

        // Act & Assert
        foreach (var input in invalidInputs)
        {
            var ex = Assert.ThrowsException<TargetInvocationException>(() => 
                method?.Invoke(null, [input]));
            Assert.IsTrue(ex.InnerException is FormatException, $"Expected FormatException for input '{input}'");
        }
    }
}
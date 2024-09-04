using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace _01_RomanParser.tests;

[TestClass]
public class RomanNumberTest
{
    [TestMethod]
    public void ParseTest()
    {
        var testCases = new Dictionary<string, int>()
    {
        {"I", 1},
        {"II", 2},
        {"III", 3},
        {"IV", 4},
        {"V", 5},
        {"VI", 6},
        {"VII", 7},
        {"VIII", 8},
        {"IX", 9},
        {"X", 10},
        {"XL", 40},
        {"L", 50},
        {"XC", 90},
        {"C", 100},
        {"CD", 400},
        {"D", 500},
        {"CM", 900},
        {"M", 1000},
        {"MC", 1100},
        {"MCM", 1900},
        {"MM", 2000},
        {"MMM", 3000},

        //Not optimal
        {"IIII", 4},  
        {"VIIII", 9}, 
        {"XXXX", 40}, 
        {"LXXXX", 90},
        {"CCCC", 400},
        {"DCCCC", 900},
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
    }

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

            // вимагатимемо від винятку повідомлення, що містить назву аргументу (digit)
            // містить значення аргументу, що призвело до винятку
            // назву класу та метододу, що викинуло виняток

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

}
using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace _01_RomanParser.tests;

[TestClass]
public class RomanNumberTest
{
    [TestMethod]
    public void ParseTest_ValidSingleRomanNumbers()
    {
        var useCases = new Dictionary<string, int>()
        {
            // Optimal forms
            // {"N", 0},
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
            
            // Non-optimal forms (incorrect usage)
            {"IIII", 4},  
            {"VIIII", 9}, 
            {"XXXX", 40}, 
            {"LXXXX", 90},
            {"CCCC", 400},
            {"DCCCC", 900},
        };

        foreach (var testCase in useCases)
        {
            Assert.AreEqual(
                testCase.Value, 
                RomanNumber.Parse(testCase.Key).Value, 
                $"{testCase.Key} -> {testCase.Value}");
        }
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void ParseTest_InvalidRomanNumber()
    {
        // Cases that should throw an exception
        RomanNumber.Parse("A");
        RomanNumber.Parse("IIIIII");
        RomanNumber.Parse("VV"); 
        RomanNumber.Parse("IC"); 
        RomanNumber.Parse("XM"); 
        RomanNumber.Parse("VL"); 
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void ParseTest_EmptyInput()
    {
        RomanNumber.Parse(""); // Exception
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void ParseTest_NullInput()
    {
        RomanNumber.Parse(null); // Exception
    }
}

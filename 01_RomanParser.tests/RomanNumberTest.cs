using System.Reflection;

namespace _01_RomanParser.tests;
[TestClass]
public class RomanNumberTest
{
    [TestMethod]
    public void ToStringTest()
    {
        Dictionary<int, String> testCases = new() {   // Append / Concat
            { 2, "II" },
            { 3343, "MMMCCCXLIII" },
            { 4, "IV" },
            { 44, "XLIV" },
            { 9, "IX" },
            { 90, "XC" },
            { 1400, "MCD" },
            { 999, "CMXCIX" },  
            { 444, "CDXLIV" },
            { 990, "CMXC" }
        };

        foreach (var (k,v) in RomanNumberFactoryTest.DigitValues)
        {
            testCases.Add(v,k);
        }
        
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
}
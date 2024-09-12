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
}
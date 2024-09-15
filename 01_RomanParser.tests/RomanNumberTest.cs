namespace _01_RomanParser.tests;

[TestClass]
public class RomanNumberTest
{
    [TestMethod]
    public void ConstructorTest()
    {
        var rn = new RomanNumber("IX");
        Assert.IsInstanceOfType<int>(rn.ToInt());
        Assert.IsInstanceOfType<uint>(rn.ToUnsignedInt());
        Assert.IsInstanceOfType<short>(rn.ToShort());
        Assert.IsInstanceOfType<ushort>(rn.ToUnsignedShort());
        Assert.IsInstanceOfType<float>(rn.ToFloat());
        Assert.IsInstanceOfType<double>(rn.ToDouble());

        rn = new RomanNumber(3);
        Assert.IsNotNull(rn);
    }

    [TestMethod]
    public void ToStringTest()
    {
        Dictionary<int, string> testCases = new()
        {
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

        foreach (var (k, v) in RomanNumberFactoryTest.DigitValues) testCases.Add(v, k);

        foreach (var testCase in testCases)
            Assert.AreEqual(
                testCase.Value,
                new RomanNumber(testCase.Key).ToString(),
                $"ToString({testCase.Key}) --> {testCase.Value}"
            );
    }
}
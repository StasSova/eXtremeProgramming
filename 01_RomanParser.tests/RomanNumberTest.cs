namespace _01_RomanParser.tests;

[TestClass]
public class RomanNumberTest
{
    [TestMethod]
    public void ParseTest_ValidSingleRomanNumbers()
    {
        Assert.AreEqual(1, RomanNumber.Parse("I").Value);
        Assert.AreEqual(5, RomanNumber.Parse("V").Value);
        Assert.AreEqual(10, RomanNumber.Parse("X").Value);
        Assert.AreEqual(50, RomanNumber.Parse("L").Value);
        Assert.AreEqual(100, RomanNumber.Parse("C").Value);
        Assert.AreEqual(500, RomanNumber.Parse("D").Value);
        Assert.AreEqual(1000, RomanNumber.Parse("M").Value);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void ParseTest_InvalidRomanNumber()
    {
        RomanNumber.Parse("A"); // Exception
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
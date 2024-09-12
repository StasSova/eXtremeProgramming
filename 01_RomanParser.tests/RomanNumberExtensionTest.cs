namespace _01_RomanParser.tests;

[TestClass]
public class RomanNumberExtensionTest
{
    [TestMethod]
    public void PlusTest()
    {
        RomanNumber
            rn1 = new(1),
            rn2 = new(2),
            rn0 = new(0);

        Assert.AreEqual(3,
            rn1.Plus(rn2).Value);

        Assert.AreEqual(4,
            rn1.Plus(rn1).Plus(rn2).Value);
        Assert.AreEqual(4,
            rn1.Plus(rn1).Plus(rn2).Plus(rn0).Value);

        Assert.AreEqual(5,
            rn1.Plus(rn2).Plus(rn2).Value);

        Assert.AreNotSame(rn1, rn1.Plus(rn1));
        Assert.AreNotSame(rn1, rn1.Plus(new RomanNumber(0)));

    }
}
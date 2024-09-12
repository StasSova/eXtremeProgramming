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
            rn3 = new(3),
            rn10 = new(10),
            rn50 = new(50),
            rn100 = new(100);

        Assert.AreEqual(3, rn1.Plus(rn2).Value);
        Assert.AreEqual(6, rn1.Plus(rn1).Plus(rn2).Plus(rn2).Value);

        Assert.AreEqual(13, rn10.Plus(rn3).Value);
        Assert.AreEqual(60, rn10.Plus(rn50).Value);

        Assert.AreEqual(106, rn1.Plus(rn2).Plus(rn3).Plus(rn100).Value);
        Assert.AreEqual(163, rn50.Plus(rn100).Plus(rn10).Plus(rn3).Value);

        Assert.AreEqual(300, rn100.Plus(rn100, rn100).Value);
        Assert.AreEqual(300, rn100.Plus(rn100).Plus(rn100).Value);
    }
}
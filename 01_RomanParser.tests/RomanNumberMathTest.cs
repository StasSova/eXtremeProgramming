namespace _01_RomanParser.tests
{
    [TestClass]
    public class RomanNumberMathTest
    {

        [TestMethod]
        public void PlusTest()
        {
            RomanNumber 
                rn1 = new(1),
                rn2 = new(2),
                rn3 = new(3);

            Assert.AreEqual(6, 
                RomanNumberMath.Plus(rn1, rn2, rn3).Value);
        }
    }
}

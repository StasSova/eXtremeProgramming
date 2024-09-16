namespace _01_RomanParser
{
    public static class RomanNumberExtension
    {
        public static RomanNumber Plus(this RomanNumber romanNumber, params RomanNumber[] other)
        {
            return RomanNumberMath.Plus([romanNumber, .. other]);
        }
    }
}

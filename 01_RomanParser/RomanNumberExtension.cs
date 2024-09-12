namespace _01_RomanParser;

public static class RomanNumberExtension
{
    public static RomanNumber Plus(this RomanNumber rn, params RomanNumber[] others)
    {
        return RomanNumberMath.Plus([rn, ..others]);
    }
}
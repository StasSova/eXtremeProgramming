namespace _01_RomanParser;

public static class RomanNumberMath
{
    // public static RomanNumber Plus(this RomanNumber rn, RomanNumber other)
    // {
    //     return rn with { Value = rn.Value + other.Value };
    // }

    public static RomanNumber Plus(params RomanNumber[] args)
    {
        return new(args.Sum(r=> r.Value));
    }
}
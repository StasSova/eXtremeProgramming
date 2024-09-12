namespace _01_RomanParser;

public class RomanNumberMath
{
    public static RomanNumber Plus(params RomanNumber[] args)
    {
        return new(args.Sum(r=> r.Value));
    }
}
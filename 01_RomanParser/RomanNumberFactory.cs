namespace _01_RomanParser
{

    public class RomanNumberFactory
    {
        public static RomanNumber Parse(String input)
        {
            return new(ParseAsInt(input));
        }

        public static int ParseAsInt(String input)
        {
            int value = 0;
            int rightDigit = 0;

            _CheckValidity(input);

            foreach (char c in input.Reverse())
            {
                int digit = DigitValue(c.ToString());
                value += (digit >= rightDigit) ? digit : -digit;
                rightDigit = digit;
            }
            return value;
        }

        private static void _CheckValidity(string input)
        {
            _CheckSymbols(input);
            _CheckPairs(input);
            _CheckFormat(input);
            _CheckSubs(input);
        }

        private static void _CheckSubs(string input)
        {
            // IXIV XCXL  -- одна цифра може відніматись тільки один раз

            HashSet<char> subs = new();
            for (int i = 0; i < input.Length - 1; ++i)
            {
                char c = input[i];
                if (DigitValue(c.ToString()) < DigitValue(input[i + 1].ToString()))
                {
                    if (subs.Contains(c))
                    {
                        throw new FormatException($"Invalid subtraction repetition: '{c}' in '{input}'");
                    }
                    subs.Add(c);
                }
            }
        }


        private static void _CheckFormat(string input)
        {
            int maxDigit = 0;
            bool wasLess = false;
            bool wasMax = false;
            foreach (char c in input.Reverse())
            {
                int digit = DigitValue(c.ToString());
                if (digit < maxDigit)
                {
                    if (wasLess || wasMax)
                    {
                        throw new FormatException($"Invalid repetition or subtraction pattern in '{input}'");
                    }
                    wasLess = true;
                }
                else if (digit == maxDigit)
                {
                    wasMax = true;
                    wasLess = false;
                }
                else
                {
                    maxDigit = digit;
                    wasLess = false;
                    wasMax = false;
                }
            }
        }


        private static void _CheckPairs(string input)
        {
            for (int i = 0; i < input.Length - 1; ++i)
            {
                int rightDigit = DigitValue(input[i + 1].ToString());
                int leftDigit = DigitValue(input[i].ToString());
                if (leftDigit != 0 &&
                    leftDigit < rightDigit &&
                    (rightDigit / leftDigit > 10 ||  // IC IM
                        (leftDigit == 5 || leftDigit == 50 || leftDigit == 500)   // VX
                    ))
                {
                    throw new FormatException(
                        $"Invalid order '{input[i]}' before '{input[i + 1]}' in position {i}");
                }
            }
        }

        private static void _CheckSymbols(string input)
        {
            int pos = 0;
            foreach (char c in input)
            {
                try
                {
                    DigitValue(c.ToString());
                }
                catch
                {
                    throw new FormatException(
                        $"Invalid symbol '{c}' in position {pos}");
                }
                pos += 1;
            }
        }

        public static int DigitValue(String digit) => digit switch
        {
            "N" => 0,
            "I" => 1,
            "V" => 5,
            "X" => 10,
            "L" => 50,
            "C" => 100,
            "D" => 500,
            "M" => 1000,
            _ => throw new ArgumentException($"{nameof(RomanNumberFactory)}::{nameof(DigitValue)}: 'digit' has invalid value '{digit}'")
        };
    }

}

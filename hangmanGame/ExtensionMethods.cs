using System.Linq;

namespace hangmanGame
{
    internal static class ExtensionMethods
    {
        public static bool IsNotInt(this string value)
        {
            return !int.TryParse(value, out _);
        }

        public static bool IsInt(this string value)
        {
            return int.TryParse(value, out _);
        }

        public static bool ContainsInt(this string value)
        {
            return value.Any(char.IsDigit);
        }

        public static bool IsNotInRange(this int value, int maxValue, int minValue)
        {
            return !Enumerable.Range(minValue, maxValue).Contains(value);
        }
    }
}
